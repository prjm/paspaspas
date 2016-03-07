using PasPasPas.Api;
using PasPasPas.Api.Options;
using PasPasPas.Internal.Input;
using PasPasPas.Internal.Log;
using PasPasPas.Internal.Parser;
using PasPasPas.Internal.Tokenizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPasTests {
    public class ParserTestBase {

        protected OptionSet TestOptions
            = new OptionSet();

        protected CompileOptions CompilerOptions
            => TestOptions.CompilerOptions;

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

            StandardParser parser = new StandardParser();
            parser.BaseTokenizer = new StandardTokenizer() { Input = new StringInput(input) };
            var hasError = false;
            string errorText = string.Empty;

            parser.LogMessage += (x, y) => {
                errorText += y.Message.ToSimpleString() + Environment.NewLine;
                hasError = hasError || y.Message.Level == LogLevel.Error;
            };

            var result = parser.Parse();
            var formatter = new PascalFormatter();
            result.ToFormatter(formatter);
            Assert.AreEqual(CompactWhitespace(output), CompactWhitespace(formatter.Result));
            Assert.AreEqual(string.Empty, errorText);
            Assert.IsFalse(hasError);
        }

        protected void RunCompilerDirective(string directive, object expected, Func<object> actual) {
            TestOptions.Clear();
            TestOptions.ConditionalCompilation.Conditionals.OwnValues.Add(new ConditionalSymbol() {
                Name = "PASPASPAS_TEST"
            });

            var directives = directive.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var directivePart in directives) {
                TestOptions.ResetOnNewUnit();
                var subParts = directivePart.Split(new[] { '§' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var subPart in subParts) {
                    var parser = new CompilerDirectiveParser();
                    var tokenizer = new CompilerDirectiveTokenizer();
                    var input = new StringInput(subPart);
                    tokenizer.Input = input;
                    parser.BaseTokenizer = tokenizer;
                    parser.Options = TestOptions;
                    parser.ParseCompilerDirective();
                }
            }

            Assert.AreEqual(expected, actual());
        }

    }
}
