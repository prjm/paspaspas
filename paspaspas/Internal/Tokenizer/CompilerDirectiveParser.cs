using PasPasPas.Api;
using PasPasPas.Api.Options;
using PasPasPas.Internal.Parser;
using System.Collections.Generic;
using System;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     helper parser for compiler directives
    /// </summary>
    public class CompilerDirectiveParser : ParserBase {

        /// <summary>
        ///     create a new compilre directive parser
        /// </summary>
        public CompilerDirectiveParser() : base(new CompilerDirectiveTokenizerWithLookahead()) { }

        /// <summary>
        ///     compiler opions
        /// </summary>
        public OptionSet Options { get; set; }

        /// <summary>
        ///     compiler options
        /// </summary>
        protected CompileOptions CompilerOptions
             => Options.CompilerOptions;

        private static HashSet<int> switches
            = new HashSet<int>() {
                PascalToken.AlignSwitch, PascalToken.AlignSwitch1,PascalToken.AlignSwitch2,PascalToken.AlignSwitch4,PascalToken.AlignSwitch8,PascalToken.AlignSwitch16,
                PascalToken.BoolEvalSwitch,
            };

        private static HashSet<int> longSwitches
            = new HashSet<int>() {
                PascalToken.AlignSwitchLong,
                PascalToken.BoolEvalSwitchLong
            };

        private static HashSet<int> parameters
            = new HashSet<int>() {
                PascalToken.Apptype,
                PascalToken.CodeAlign
            };

        /// <summary>
        ///     parse a compiler directive
        /// </summary>
        public void ParseCompilerDirective() {
            var kind = CurrentToken().Kind;
            if (switches.Contains(kind)) {
                ParseSwitch();
                return;
            }

            if (longSwitches.Contains(kind)) {
                ParseLongSwitch();
                return;
            }

            if (parameters.Contains(kind)) {
                ParseParameter();
                return;
            }
        }

        private void ParseParameter() {
            if (Match(PascalToken.Apptype)) {
                ParseApptypeParameter();
                return;
            }

            if (Match(PascalToken.CodeAlign)) {
                ParseCodeAlignParameter();
                return;
            }
        }

        private void ParseCodeAlignParameter() {
            Require(PascalToken.CodeAlign);
            var value = Require(PascalToken.Integer).Value;
            int align;

            if (!int.TryParse(value, out align)) {
                Unexpected();
                return;
            }


            switch (align) {
                case 1:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.OneByte;
                    return;
                case 2:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.TwoByte;
                    return;
                case 4:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.FourByte;
                    return;
                case 8:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.EightByte;
                    return;
                case 16:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.SixteenByte;
                    return;
            }

            Unexpected();
        }

        private void ParseApptypeParameter() {
            Require(PascalToken.Apptype);
            var value = Require(PascalToken.Identifier).Value;

            if (string.Equals(value, "CONSOLE", StringComparison.OrdinalIgnoreCase)) {
                CompilerOptions.ApplicationType.Value = AppType.Console;
            }
            else if (string.Equals(value, "GUI", StringComparison.OrdinalIgnoreCase)) {
                CompilerOptions.ApplicationType.Value = AppType.Gui;
            }
        }

        /// <summary>
        ///     parse a long switch
        /// </summary>
        private void ParseLongSwitch() {
            if (Optional(PascalToken.AlignSwitchLong)) {
                ParseAlignLongSwitch();
                return;
            }

            if (Optional(PascalToken.BoolEvalSwitchLong)) {
                ParseLongBoolEvalSwitch();
                return;
            }
        }

        private void ParseLongBoolEvalSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.BoolEval.Value = BooleanEvaluation.CompleteEvaluation;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.BoolEval.Value = BooleanEvaluation.ShortEvaluation;
                return;
            }
        }

        /// <summary>
        ///     
        /// </summary>
        private void ParseAlignLongSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.Align.Value = Alignment.QuadWord;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.Align.Value = Alignment.Unaligned;
                return;
            }

            int value;
            if (Match(PascalToken.Integer) && int.TryParse(CurrentToken().Value, out value)) {
                switch (value) {
                    case 1:
                        CompilerOptions.Align.Value = Alignment.Unaligned;
                        return;
                    case 2:
                        CompilerOptions.Align.Value = Alignment.Word;
                        return;
                    case 4:
                        CompilerOptions.Align.Value = Alignment.DoubleWord;
                        return;
                    case 8:
                        CompilerOptions.Align.Value = Alignment.QuadWord;
                        return;
                    case 16:
                        CompilerOptions.Align.Value = Alignment.DoubleQuadWord;
                        return;
                }
            }

            Unexpected();
        }

        /// <summary>
        ///     parse a switch
        /// </summary>
        private void ParseSwitch() {
            if (Match(PascalToken.AlignSwitch, PascalToken.AlignSwitch1, PascalToken.AlignSwitch2, PascalToken.AlignSwitch4, PascalToken.AlignSwitch8, PascalToken.AlignSwitch16)) {
                ParseAlignSwitch();
                return;
            }

            if (Match(PascalToken.BoolEvalSwitch)) {
                ParseBoolEvalSwitch();
                return;
            }
        }

        private void ParseBoolEvalSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.BoolEval.Value = BooleanEvaluation.CompleteEvaluation;
                return;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.BoolEval.Value = BooleanEvaluation.ShortEvaluation;
                return;
            }

            Unexpected();
        }

        private void ParseAlignSwitch() {
            switch (CurrentToken().Kind) {

                case PascalToken.AlignSwitch1:
                    CompilerOptions.Align.Value = Alignment.Unaligned;
                    return;

                case PascalToken.AlignSwitch2:
                    CompilerOptions.Align.Value = Alignment.Word;
                    return;

                case PascalToken.AlignSwitch4:
                    CompilerOptions.Align.Value = Alignment.DoubleWord;
                    return;

                case PascalToken.AlignSwitch8:
                    CompilerOptions.Align.Value = Alignment.QuadWord;
                    return;

                case PascalToken.AlignSwitch16:
                    CompilerOptions.Align.Value = Alignment.DoubleQuadWord;
                    return;
            }

            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.Align.Value = Alignment.QuadWord;
                return;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.Align.Value = Alignment.Unaligned;
                return;
            }

            Unexpected();
        }
    }
}
