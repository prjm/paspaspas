using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using PasPasPas.Api;
using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.Parser.Standard;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Common;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    public class ParserTestBase : CommonTest {

        protected const string CstPath = "z.x.pas";

        protected static string CompactWhitespace(string input) {
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

            var env = CreateEnvironment();
            var testOptions = Factory.CreateOptions(env, default);
            ClearOptions(testOptions);

            var log = new LogTarget();
            var api = Factory.CreateParserApi(env, testOptions);
            var path = api.Tokenizer.Readers.CreateFileRef("test.pas");

            env.Log.RegisterTarget(log);

            using (var parser = api.CreateParser(CommonApi.CreateResolverForSingleString(api.Tokenizer.Readers, path, input), path)) {
                var hasError = false;
                var errorText = string.Empty;

                log.ProcessMessage += (x, y) => {
                    errorText += y.Message.MessageID.ToString(MessageNumbers.NumberFormat, CultureInfo.InvariantCulture) + Environment.NewLine;
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

        protected ISyntaxPart RunAstTest<T>(string completeInput, Func<object, T> searchFunction, T expectedResult, bool withTypes = false, params uint[] errorMessages) {
            var env = CreateEnvironment();
            var msgs = new List<ILogMessage>();
            var log = new LogTarget();
            env.Log.RegisterTarget(log);

            var hasError = false;
            var errorText = string.Empty;

            log.ProcessMessage += (x, y) => {
                msgs.Add(y.Message);
                errorText += y.Message.MessageID.ToString(MessageNumbers.NumberFormat, CultureInfo.InvariantCulture) + Environment.NewLine;
                hasError = hasError ||
                y.Message.Severity == MessageSeverity.Error ||
                y.Message.Severity == MessageSeverity.FatalError;
            };

            var project = new ProjectItemCollection();
            ISyntaxPart tree = null;

            foreach (var input in completeInput.Split('§')) {

                tree = RunAstTest(input, env);
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

        protected T RunCstTest<T>(Func<StandardParser, T> tester, string tokens = "", params uint[] errorMessages) {
            var env = CreateEnvironment();
            var msgs = new List<ILogMessage>();
            var log = new LogTarget();
            env.Log.RegisterTarget(log);

            var hasError = false;
            var errorText = string.Empty;

            log.ProcessMessage += (x, y) => {
                msgs.Add(y.Message);
                errorText += y.Message.MessageID.ToString(MessageNumbers.NumberFormat, CultureInfo.InvariantCulture) + Environment.NewLine;
                hasError = hasError ||
                y.Message.Severity == MessageSeverity.Error ||
                y.Message.Severity == MessageSeverity.FatalError;
            };

            var testOptions = Factory.CreateOptions(env, default);
            var api = Factory.CreateParserApi(env, testOptions);
            var path = api.Tokenizer.Readers.CreateFileRef(CstPath);
            var data = CreateResolver(api.Tokenizer.Readers, path, tokens);

            ClearOptions(testOptions);
            using (var parser = api.CreateParser(data, path)) {
                var standard = parser as StandardParser;
                Assert.IsNotNull(standard);
                var value = tester(standard);
                Assert.IsNotNull(value);

                if (errorMessages.Length < 1) {
                    Assert.AreEqual(string.Empty, errorText);
                    Assert.IsFalse(hasError);
                }
                Assert.AreEqual(errorMessages.Length, msgs.Count);
                foreach (var guid in errorMessages)
                    Assert.IsTrue(msgs.Where(t => t.MessageID == guid).Any());

                return value;
            }
        }

        protected ISyntaxPart RunAstTest(string input, ITypedEnvironment env) {
            var testOptions = Factory.CreateOptions(env, default);
            var api = Factory.CreateParserApi(env, testOptions);
            var path = api.Tokenizer.Readers.CreateFileRef("z.x.pas");
            var resolver = CommonApi.CreateResolverForSingleString(api.Tokenizer.Readers, path, input);

            ClearOptions(testOptions);

            using (var parser = api.CreateParser(resolver, path)) {
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

                    return decl.Value.TypeInfo.IsConstant();
                }
                return null;
            }

            RunAstTest(statement, search, true, true);
        }

        protected IInputResolver CreateResolver(IReaderApi api, FileReference path, string content) {
            IReaderInput doResolve(FileReference file, IReaderApi a) {
                var incFile = new FileReference(Path.GetFullPath("dummy.inc"));
                var resFile1 = new FileReference(Path.GetFullPath("res.res"));
                var resFile2 = new FileReference(Path.GetFullPath("test_0.res"));
                var linkDll = new FileReference(Path.GetFullPath("link.dll"));

                if (file.Equals(incFile))
                    return a.CreateInputForString(incFile, "DEFINE DUMMY_INC");

                if (file.Equals(resFile1))
                    return a.CreateInputForString(resFile1, "RES RES RES");

                if (file.Equals(resFile2))
                    return a.CreateInputForString(resFile2, "RES RES RES");

                if (file.Equals(linkDll))
                    return a.CreateInputForString(linkDll, "MZE!");

                if (file.Equals(path))
                    return a.CreateInputForString(path, content);

                return default;
            }

            return api.CreateInputResolver(doResolve);
        }

        protected void RunCompilerDirective(string directive, object expected, Func<IOptionSet, object> actual, params uint[] messages) {

            var env = CreateEnvironment();
            var fileCounter = 0;
            var incFile = new FileReference(Path.GetFullPath("dummy.inc"));
            var resFile1 = new FileReference(Path.GetFullPath("res.res"));
            var resFile2 = new FileReference(Path.GetFullPath("test_0.res"));
            var linkDll = new FileReference(Path.GetFullPath("link.dll"));
            var testOptions = Factory.CreateOptions(env, default);
            var api = Factory.CreateParserApi(env, testOptions);

            var msgs = new ListLogTarget();
            env.Log.RegisterTarget(msgs);

            ClearOptions(testOptions);

            var directives = directive.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var directivePart in directives) {
                testOptions.ResetOnNewUnit();
                var subParts = directivePart.Split(new[] { '§' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var subPart in subParts) {
                    var hasFoundInput = false;
                    var path = new FileReference("test_" + fileCounter.ToString(CultureInfo.InvariantCulture) + ".pas");
                    using (var reader = api.Tokenizer.Readers.CreateReader(CreateResolver(api.Tokenizer.Readers, path, subPart), path)) {
                        var visitor = new CompilerDirectiveVisitor(testOptions, path, env.Log);
                        var terminals = new TerminalVisitor();


                        var parser = new CompilerDirectiveParser(api.Tokenizer, testOptions, reader) {
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
                            hasFoundInput = reader.CurrentFile == null || reader.AtEof || hasFoundInput;

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

            var m = new HashSet<uint>(msgs.Messages.Select(t => t.MessageID));
            foreach (var guid in messages)
                Assert.IsTrue(m.Contains(guid));

            m = new HashSet<uint>(messages);
            foreach (var guid in msgs.Messages.Select(t => t.MessageID))
                Assert.IsTrue(m.Contains(guid));

        }

        private void ClearOptions(IOptionSet testOptions) {
            testOptions.Clear();
            testOptions.ConditionalCompilation.Conditionals.OwnValues.Add(new ConditionalSymbol() {
                Name = "PASPASPAS_TEST"
            });
        }
    }
}
