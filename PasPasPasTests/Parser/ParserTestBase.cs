using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using PasPasPas.Api;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.Parser.Standard;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    public class ParserTestBase : CommonTest {

        protected const string CstPath = "z.x.pas";

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

            var testOptions = new OptionSet(CreateEnvironment());
            ClearOptions(testOptions);

            var log = new LogTarget();
            var env = new DefaultEnvironment();
            var api = new ParserApi(env, testOptions);

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

        protected ISyntaxPart RunAstTest<T>(string completeInput, Func<object, T> searchFunction, T expectedResult, bool withTypes = false, params Guid[] errorMessages) {
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
            ISyntaxPart tree = null;

            foreach (var input in completeInput.Split('§')) {

                tree = RunAstTest(input, env, msgs);
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);


                var visitor = new TreeTransformer(env, project);
                tree.Accept(visitor.AsVisitor());

                if (withTypes) {
                    var ta = new TypeAnnotator(env);
                    project.Accept(ta.AsVisitor());
                }

                var astVisitor = new AstVisitor<T>() { SearchFunction = searchFunction };
                visitor.Project.Accept(astVisitor.AsVisitor());

                var validator = new StructureValidator() { Manager = env.Log };
                visitor.Project.Accept(validator.AsVisitor());

                Assert.IsNotNull(astVisitor.Result);
                Assert.AreEqual(expectedResult, astVisitor.Result);
            }

            if (errorMessages.Length < 1) {
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);
            }
            Assert.AreEqual(errorMessages.Length, msgs.Count);
            foreach (var guid in errorMessages)
                Assert.IsTrue(msgs.Where(t => t.MessageID == guid).Any());

            return tree;
        }

        protected T RunCstTest<T>(Func<StandardParser, T> tester, string tokens = "") {
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

            var testOptions = new OptionSet(env);
            var api = new ParserApi(env, testOptions);

            ClearOptions(testOptions);
            using (var parser = api.CreateParserForString(CstPath, tokens)) {
                var standard = parser as StandardParser;
                Assert.IsNotNull(standard);
                var value = tester(standard);
                Assert.IsNotNull(value);
                return value;
            }
        }

        protected ISyntaxPart RunAstTest(string input, ITypedEnvironment env, IList<ILogMessage> messages) {
            var testOptions = new OptionSet(env);
            var api = new ParserApi(env, testOptions);

            ClearOptions(testOptions);

            using (var parser = api.CreateParserForString("z.x.pas", input)) {
                return parser.Parse();
            }
        }

        protected void TestConstant(string expr, string constName = "x", int typeId = -1) {
            var statement = $"program z.x; const x = {expr}; .";

            bool? search(object t) {

                if (t is ConstantDeclaration decl && string.Equals(constName, decl.Name.CompleteName, StringComparison.Ordinal)) {
                    if (decl == null || decl.Value == null)
                        return null;

                    if (typeId >= 0)
                        Assert.AreEqual(typeId, decl.Value.TypeInfo.TypeId);

                    return decl.Value.TypeInfo.IsConstant;
                }
                return null;
            }

            RunAstTest<bool?>(statement, search, true, true);
        }

        protected void RunCompilerDirective(string directive, object expected, Func<OptionSet, object> actual, params Guid[] messages) {

            var env = CreateEnvironment();
            var fileCounter = 0;
            var incFile = new FileReference(Path.GetFullPath("dummy.inc"));
            var resFile1 = new FileReference(Path.GetFullPath("res.res"));
            var resFile2 = new FileReference(Path.GetFullPath("test_0.res"));
            var linkDll = new FileReference(Path.GetFullPath("link.dll"));
            var testOptions = new OptionSet(env);

            var msgs = new ListLogTarget();
            env.Log.RegisterTarget(msgs);

            ClearOptions(testOptions);

            var directives = directive.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var directivePart in directives) {
                testOptions.ResetOnNewUnit(env.Log);
                var subParts = directivePart.Split(new[] { '§' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var subPart in subParts) {
                    var hasFoundInput = false;
                    var path = new FileReference("test_" + fileCounter.ToString(CultureInfo.InvariantCulture) + ".pas");
                    using (var reader = new StackedFileReader()) {
                        var visitor = new CompilerDirectiveVisitor(testOptions, path, env.Log);
                        var terminals = new TerminalVisitor();

                        reader.AddMockupFile(incFile, "DEFINE DUMMY_INC");
                        reader.AddMockupFile(resFile1, "RES RES RES");
                        reader.AddMockupFile(resFile2, "RES RES RES");
                        reader.AddMockupFile(linkDll, "MZE!");

                        reader.AddStringToRead(path, subPart);
                        var parser = new CompilerDirectiveParser(env, testOptions, reader) {
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
            }

            Assert.AreEqual(expected, actual(testOptions));
            env.Log.UnregisterTarget(msgs);
            Assert.AreEqual(messages.Length, msgs.Messages.Count);

            var m = new HashSet<Guid>(msgs.Messages.Select(t => t.MessageID));
            foreach (var guid in messages)
                Assert.IsTrue(m.Contains(guid));

            m = new HashSet<Guid>(messages);
            foreach (var guid in msgs.Messages.Select(t => t.MessageID))
                Assert.IsTrue(m.Contains(guid));

        }

        private void ClearOptions(OptionSet testOptions) {
            testOptions.Clear();
            testOptions.ConditionalCompilation.Conditionals.OwnValues.Add(new ConditionalSymbol() {
                Name = "PASPASPAS_TEST"
            });
        }
    }
}
