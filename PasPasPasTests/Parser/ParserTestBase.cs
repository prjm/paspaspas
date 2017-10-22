using System.Linq;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using System;
using System.Collections.Generic;
using System.Text;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Api;
using PasPasPas.Parsing;
using PasPasPasTests.Common;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.Parser;
using System.IO;

namespace PasPasPasTests.Parser {

    public class ParserTestBase : CommonTest {

        protected OptionSet TestOptions
            = null;

        protected CompileOptions CompilerOptions
            => TestOptions.CompilerOptions;

        protected WarningOptions Warnings
            => TestOptions.Warnings;

        protected ConditionalCompilationOptions ConditionalCompilation
            => TestOptions.ConditionalCompilation;

        protected MetaInformation Meta
            => TestOptions.Meta;

        protected string CompactWhitespace(string input) {
            var result = new StringBuilder();
            var wasWhitespace = false;

            for (var charIndex = 0; charIndex < input.Length; charIndex++) {
                var currentChar = input[charIndex];

                if (char.IsWhiteSpace(currentChar)) {
                    if (!wasWhitespace) {
                        result.Append(" ");
                        wasWhitespace = true;
                    }
                }
                else {
                    result.Append(currentChar);
                    wasWhitespace = false;
                }
            }

            return result.ToString().Trim();
        }

        protected void ParseString(string input, string output = null) {
            if (string.IsNullOrEmpty(output))
                output = input;

            TestOptions = new OptionSet(CreateEnvironment());
            ClearOptions();

            var log = new LogTarget();
            var fileAccess = new StandardFileAccess();
            var env = new DefaultEnvironment();
            var api = new ParserApi(env, TestOptions);

            env.Log.RegisterTarget(log);

            using (var parser = api.CreateParserForString("test.pas", input)) {
                var hasError = false;
                var errorText = string.Empty;

                log.ProcessMessage += (x, y) => {
                    errorText += y.Message.MessageID.ToString() + Environment.NewLine;
                    hasError = hasError ||
                    y.Message.Severity == MessageSeverity.Error ||
                    y.Message.Severity == MessageSeverity.FatalError;
                };

                var result = parser.Parse();
                var visitor = new TerminalVisitor();
                result.Accept(visitor.AsVisitor());
                Assert.AreEqual(output, visitor.ResultBuilder.ToString());
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);
            }
        }

        private class AstVisitor<T> : IStartEndVisitor {

            public T Result { get; internal set; }
            public Func<object, T> SearchFunction { get; set; }

            public IStartEndVisitor AsVisitor() => this;
            public void EndVisit<VisitorType>(VisitorType element) { }

            public void StartVisit<ISyntaxPart>(ISyntaxPart part) {
                var data = SearchFunction(part);
                if (EqualityComparer<T>.Default.Equals(default, Result))
                    Result = data;
            }

        }

        protected void RunAstTest<T>(string completeInput, Func<object, T> searchFunction, T expectedResult, params Guid[] errorMessages) {
            var env = CreateEnvironment();
            var msgs = new List<ILogMessage>();
            var log = new LogTarget();
            env.Log.RegisterTarget(log);

            var hasError = false;
            var errorText = string.Empty;

            log.ProcessMessage += (x, y) => {
                msgs.Add(y.Message);
                errorText += y.Message.MessageID.ToString() + Environment.NewLine;
                hasError = hasError ||
                y.Message.Severity == MessageSeverity.Error ||
                y.Message.Severity == MessageSeverity.FatalError;
            };

            var project = new ProjectRoot();

            foreach (var input in completeInput.Split('§')) {

                var tree = RunAstTest(input, env, msgs);
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);


                var visitor = new TreeTransformer(project) { LogManager = env.Log };
                tree.Accept(visitor.AsVisitor());

                var astVisitor = new AstVisitor<T>() { SearchFunction = searchFunction };
                visitor.Project.Accept(astVisitor.AsVisitor());

                var validator = new StructureValidator() { Manager = env.Log };
                visitor.Project.Accept(validator.AsVisitor());

                Assert.AreEqual(expectedResult, astVisitor.Result);
            }

            if (errorMessages.Length < 1) {
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);
            }
            Assert.AreEqual(errorMessages.Length, msgs.Count);
            foreach (var guid in errorMessages)
                Assert.IsTrue(msgs.Where(t => t.MessageID == guid).Any());
        }


        protected ISyntaxPart RunAstTest(string input, IParserEnvironment env, IList<ILogMessage> messages) {
            TestOptions = new OptionSet(env);
            var api = new ParserApi(env, TestOptions);

            ClearOptions();

            using (var parser = api.CreateParserForString("z.x.pas", input)) {
                return parser.Parse();
            }
        }


        protected void RunCompilerDirective(string directive, object expected, Func<object> actual, params Guid[] messages) {

            var env = CreateEnvironment();
            var fileAccess = env.Files;
            var fileCounter = 0;
            var incFile = new FileReference(Path.GetFullPath("dummy.inc"));
            var resFile1 = new FileReference(Path.GetFullPath("res.res"));
            var resFile2 = new FileReference(Path.GetFullPath("test_0.res"));
            var linkDll = new FileReference(Path.GetFullPath("link.dll"));

            fileAccess.AddMockupFile(incFile, new StringBufferReadable("DEFINE DUMMY_INC"));
            fileAccess.AddMockupFile(resFile1, new StringBufferReadable("RES RES RES"));
            fileAccess.AddMockupFile(resFile2, new StringBufferReadable("RES RES RES"));
            fileAccess.AddMockupFile(linkDll, new StringBufferReadable("MZE!"));

            TestOptions = new OptionSet(env);

            var msgs = new ListLogTarget();
            env.Log.RegisterTarget(msgs);

            ClearOptions();

            var directives = directive.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var directivePart in directives) {
                TestOptions.ResetOnNewUnit(env.Log);
                var subParts = directivePart.Split(new[] { '§' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var subPart in subParts) {
                    var hasFoundInput = false;
                    var path = new FileReference("test_" + fileCounter.ToString() + ".pas");
                    var input = new StringBufferReadable(subPart);
                    var buffer = new FileBuffer();
                    var reader = new StackedFileReader(buffer);
                    var visitor = new CompilerDirectiveVisitor(TestOptions, path, env.Log);
                    var terminals = new TerminalVisitor();

                    buffer.Add(path, input);

                    reader.AddFileToRead(path);
                    var parser = new CompilerDirectiveParser(env, TestOptions, reader) {
                        IncludeInput = reader
                    };

                    while (reader.CurrentFile != null && !reader.AtEof) {
                        var result = parser.Parse();

                        if (!hasFoundInput) {
                            terminals.ResultBuilder.Clear();
                            if (result != null)
                                result.Accept(terminals.AsVisitor());

                            Assert.AreEqual(subPart, terminals.ResultBuilder.ToString());
                        }
                        hasFoundInput = (reader.CurrentFile == null || reader.AtEof) || hasFoundInput;

                        visitor.IncludeInput = reader;
                        if (result != null)
                            result.Accept(visitor.AsVisitor());

                    }
                    fileCounter++;
                }
            }

            Assert.AreEqual(expected, actual());
            env.Log.UnregisterTarget(msgs);
            Assert.AreEqual(messages.Length, msgs.Messages.Count);

            var m = new HashSet<Guid>(msgs.Messages.Select(t => t.MessageID));
            foreach (var guid in messages)
                Assert.IsTrue(m.Contains(guid));

            m = new HashSet<Guid>(messages);
            foreach (var guid in msgs.Messages.Select(t => t.MessageID))
                Assert.IsTrue(m.Contains(guid));

        }

        private void ClearOptions() {
            TestOptions.Clear();
            TestOptions.ConditionalCompilation.Conditionals.OwnValues.Add(new ConditionalSymbol() {
                Name = "PASPASPAS_TEST"
            });
        }
    }
}
