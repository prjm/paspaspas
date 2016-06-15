﻿using PasPasPas.Api;
using PasPasPas.DesktopPlatform;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.Tokenizer;
using System;
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
            StringBuilder result = new StringBuilder();
            bool wasWhitespace = false;

            for (int charIndex = 0; charIndex < input.Length; charIndex++) {
                char currentChar = input[charIndex];

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

            StandardParser parser = new StandardParser(environment);
            using (var inputFile = new StringInput(input, new FileReference("test.pas")))
            using (var reader = new StackedFileReader()) {
                reader.AddFile(inputFile);
                parser.BaseTokenizer = new StandardTokenizer(environment) { Input = reader };
                var hasError = false;
                string errorText = string.Empty;

                log.ProcessMessage += (x, y) => {
                    errorText += y.Message.FormattedMessage + Environment.NewLine;
                    hasError = hasError ||
                    y.Message.Severity == MessageSeverity.Error ||
                    y.Message.Severity == MessageSeverity.FatalError;
                };

                var result = parser.Parse();
                var formatter = new PascalFormatter();
                result.ToFormatter(formatter);
                Assert.AreEqual(CompactWhitespace(output), CompactWhitespace(formatter.Result));
                Assert.AreEqual(string.Empty, errorText);
                Assert.IsFalse(hasError);
            }
        }

        protected void RunCompilerDirective(string directive, object expected, Func<object> actual) {
            var fileAccess = (StandardFileAccess)TestOptions.Files;
            var fileCounter = 0;

            fileAccess.ClearMockups();
            fileAccess.AddOneTimeMockup("dummy.inc", new StringInput("DEFINE DUMMY_INC", new FileReference("dummy.inc")));
            fileAccess.AddOneTimeMockup("res.res", new StringInput("RES RES RES", new FileReference("res.res")));
            fileAccess.AddOneTimeMockup("test_0.res", new StringInput("RES RES RES", new FileReference("test_0.res")));
            fileAccess.AddOneTimeMockup("link.dll", new StringInput("MZE!", new FileReference("link.dll")));

            var log = new LogManager();
            var environment = new ParserServices(log);
            environment.Options = TestOptions;
            //environment.Register(new CommonConfiguration());
            //environment.Register(TestOptions);
            //environment.Register(fileAccess);

            ClearOptions();

            var directives = directive.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var directivePart in directives) {
                TestOptions.ResetOnNewUnit();
                var subParts = directivePart.Split(new[] { '§' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var subPart in subParts) {
                    var parser = new CompilerDirectiveParser(environment);
                    using (var input = new StringInput(subPart, new FileReference("test_" + fileCounter.ToString() + ".pas")))
                    using (var reader = new StackedFileReader()) {
                        reader.AddFile(input);
                        parser.BaseTokenizer.Input = reader;
                        parser.IncludeInput = reader;
                        while (!reader.AtEof) {
                            parser.ParseCompilerDirective();
                        }
                        fileCounter++;
                    }
                }
            }

            Assert.AreEqual(expected, actual());
        }

        private void ClearOptions() {
            TestOptions.Clear();
            TestOptions.ConditionalCompilation.Conditionals.OwnValues.Add(new ConditionalSymbol() {
                Name = "PASPASPAS_TEST"
            });
        }
    }
}
