﻿using System.Linq;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.Tokenizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasPasPasTests {

    public class ParserTestBase {

        protected OptionSet TestOptions
            = new OptionSet(new StandardFileAccess());

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

            ClearOptions();

            var logManager = new LogManager();
            var environment = new ParserServices(logManager);
            var log = new LogTarget();
            environment.Options = TestOptions;

            var parser = new StandardParser(environment);
            using (var inputFile = new StringInput(input, new FileReference("test.pas")))
            using (var reader = new StackedFileReader()) {
                reader.AddFile(inputFile);
                parser.BaseTokenizer = new StandardTokenizer(environment, reader);
                var hasError = false;
                var errorText = string.Empty;

                log.ProcessMessage += (x, y) => {
                    errorText += y.Message.MessageID.ToString() + Environment.NewLine;
                    hasError = hasError ||
                    y.Message.Severity == MessageSeverity.Error ||
                    y.Message.Severity == MessageSeverity.FatalError;
                };


                ISyntaxPart result = parser.Parse();
                var visitor = new TerminalVisitor();
                result.Accept(visitor.AsVisitor(), visitor.AsVisitor());
                Assert.AreEqual(output, visitor.ResultBuilder.ToString());
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);
            }
        }

        private class AstVisitor<T> : SyntaxPartVisitorBase<AstVisitorOptions<T>> {

            public override bool BeginVisit(ISyntaxPart part, AstVisitorOptions<T> options) {
                options.Result = options.SearchFunction(part);
                return EqualityComparer<T>.Default.Equals(default(T), options.Result);
            }

        }

        private class AstVisitorOptions<T> {
            public T Result { get; internal set; }
            public Func<object, T> SearchFunction { get; set; }

        }

        protected void RunAstTest<T>(string completeInput, Func<object, T> searchFunction, T expectedResult, params Guid[] errorMessages) {
            var msgs = new List<ILogMessage>();
            var logMgr = new LogManager();
            var log = new LogTarget();
            logMgr.RegisterTarget(log);

            var hasError = false;
            var errorText = string.Empty;

            log.ProcessMessage += (x, y) => {
                msgs.Add(y.Message);
                errorText += y.Message.MessageID.ToString() + Environment.NewLine;
                hasError = hasError ||
                y.Message.Severity == MessageSeverity.Error ||
                y.Message.Severity == MessageSeverity.FatalError;
            };

            foreach (var input in completeInput.Split('§')) {

                ISyntaxPart tree = RunAstTest(input, logMgr, msgs);
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);


                var visitor = new TreeTransformer() { LogManager = logMgr };
                tree.Accept(visitor.AsVisitor(), visitor.AsVisitor());

                var astVisitor = new AstVisitor<T>();
                var astOptions = new AstVisitorOptions<T>() { SearchFunction = searchFunction };
                VisitorHelper.AcceptVisitor(visitor.Project, astVisitor, astOptions);

                var validator = new StructureValidator() { Manager = logMgr };
                visitor.Project.Accept(validator.AsVisitor(), validator.AsVisitor());

                Assert.AreEqual(expectedResult, astOptions.Result);
            }

            if (errorMessages.Length < 1) {
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);
            }
            Assert.AreEqual(errorMessages.Length, msgs.Count);
            foreach (Guid guid in errorMessages)
                Assert.IsTrue(msgs.Where(t => t.MessageID == guid).Any());
        }


        protected ISyntaxPart RunAstTest(string input, LogManager logManager, IList<ILogMessage> messages) {
            ClearOptions();

            var environment = new ParserServices(logManager) {
                Options = TestOptions
            };

            var parser = new StandardParser(environment);
            using (var inputFile = new StringInput(input, new FileReference("z.x.pas")))
            using (var reader = new StackedFileReader()) {
                reader.AddFile(inputFile);
                parser.BaseTokenizer = new StandardTokenizer(environment, reader);
                return parser.Parse();
            }
        }


        protected void RunCompilerDirective(string directive, object expected, Func<object> actual, params Guid[] messages) {
            var fileAccess = (StandardFileAccess)TestOptions.Files;
            var fileCounter = 0;
            var incFile = new FileReference("dummy.inc");
            var resFile1 = new FileReference("res.res");
            var resFile2 = new FileReference("test_0.res");
            var linkDll = new FileReference("link.dll");

            fileAccess.ClearMockups();
            fileAccess.AddOneTimeMockup(new StringInput("DEFINE DUMMY_INC", incFile));
            fileAccess.AddOneTimeMockup(new StringInput("RES RES RES", resFile1));
            fileAccess.AddOneTimeMockup(new StringInput("RES RES RES", resFile2));
            fileAccess.AddOneTimeMockup(new StringInput("MZE!", linkDll));

            var log = new LogManager();
            var environment = new ParserServices(log) {
                Options = TestOptions
            };
            var visitor = new CompilerDirectiveVisitor() { Environment = environment };

            var terminals = new TerminalVisitor();

            var msgs = new ListLogTarget();
            log.RegisterTarget(msgs);

            ClearOptions();

            string[] directives = directive.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var directivePart in directives) {
                TestOptions.ResetOnNewUnit(environment.Log);
                string[] subParts = directivePart.Split(new[] { '§' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var subPart in subParts) {
                    var hasFoundInput = false;
                    using (var input = new StringInput(subPart, new FileReference("test_" + fileCounter.ToString() + ".pas")))
                    using (var reader = new StackedFileReader()) {
                        reader.AddFile(input);
                        var parser = new CompilerDirectiveParser(environment, reader) {
                            IncludeInput = reader
                        };
                        while (!reader.AtEof) {
                            ISyntaxPart result = parser.Parse();

                            if (!hasFoundInput) {
                                terminals.ResultBuilder.Clear();
                                if (result != null)
                                    result.Accept(terminals.AsVisitor(), terminals.AsVisitor());

                                Assert.AreEqual(subPart, terminals.ResultBuilder.ToString());
                            }
                            hasFoundInput = reader.AtEof || hasFoundInput;

                            visitor.IncludeInput = reader;
                            if (result != null)
                                result.Accept(visitor.AsVisitor(), visitor.AsVisitor());

                        }
                        fileCounter++;
                    }
                }
            }

            Assert.AreEqual(expected, actual());

            log.UnregisterTarget(msgs);

            Assert.AreEqual(messages.Length, msgs.Messages.Count);

            var m = new HashSet<Guid>(msgs.Messages.Select(t => t.MessageID));
            foreach (Guid guid in messages)
                Assert.IsTrue(m.Contains(guid));

            m = new HashSet<Guid>(messages);
            foreach (Guid guid in msgs.Messages.Select(t => t.MessageID))
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
