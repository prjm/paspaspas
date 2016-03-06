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

        /// <summary>
        ///     conditional compilation options
        /// </summary>
        protected ConditionalCompilationOptions ConditionalCompilation
            => Options.ConditionalCompilation;

        private static HashSet<int> switches
            = new HashSet<int>() {
                PascalToken.AlignSwitch, PascalToken.AlignSwitch1,PascalToken.AlignSwitch2,PascalToken.AlignSwitch4,PascalToken.AlignSwitch8,PascalToken.AlignSwitch16,
                PascalToken.BoolEvalSwitch,
                PascalToken.AssertSwitch,
                PascalToken.DebugInfoOrDescriptionSwitch,
            };

        private static HashSet<int> longSwitches
            = new HashSet<int>() {
                PascalToken.AlignSwitchLong,
                PascalToken.BoolEvalSwitchLong,
                PascalToken.AssertSwitchLong,
                PascalToken.DebugInfoSwitchLong,
            };

        private static HashSet<int> parameters
            = new HashSet<int>() {
                PascalToken.Apptype,
                PascalToken.CodeAlign,
                PascalToken.Define,
                PascalToken.Undef,
                PascalToken.IfDef,
                PascalToken.EndIf
            };

        /// <summary>
        ///     parse a compiler directive
        /// </summary>
        public bool ParseCompilerDirective() {
            var kind = CurrentToken().Kind;
            var result = false;

            if (switches.Contains(kind)) {
                result = ParseSwitch();
            }
            else if (longSwitches.Contains(kind)) {
                result = ParseLongSwitch();
            }
            else if (parameters.Contains(kind)) {
                result = ParseParameter();
            }

            return result;
        }

        private bool ParseParameter() {

            if (Match(PascalToken.IfDef)) {
                ParseIfDef();
                return true;
            }


            if (Match(PascalToken.EndIf)) {
                ParseEndIf();
                return true;
            }

            if (ConditionalCompilation.Skip)
                return false;

            if (Match(PascalToken.Apptype)) {
                ParseApptypeParameter();
                return true;
            }

            if (Match(PascalToken.CodeAlign)) {
                ParseCodeAlignParameter();
                return true;
            }

            if (Match(PascalToken.Define)) {
                ParseDefine();
                return true;
            }

            if (Match(PascalToken.Undef)) {
                ParseUndef();
                return true;
            }

            return false;
        }

        private void ParseEndIf() {
            Require(PascalToken.EndIf);
            ConditionalCompilation.RemoveIfDefCondition();
        }

        private void ParseIfDef() {
            Require(PascalToken.IfDef);
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                ConditionalCompilation.AddIfDefCondition(value);
            }
        }

        private void ParseUndef() {
            Require(PascalToken.Undef);
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                ConditionalCompilation.UndefineSymbol(value);
            }
        }

        private void ParseDefine() {
            Require(PascalToken.Define);
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                ConditionalCompilation.DefineSymbol(value);
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
        private bool ParseLongSwitch() {

            if (ConditionalCompilation.Skip)
                return false;

            if (Optional(PascalToken.AlignSwitchLong)) {
                ParseAlignLongSwitch();
                return true;
            }

            if (Optional(PascalToken.BoolEvalSwitchLong)) {
                ParseLongBoolEvalSwitch();
                return true;
            }

            if (Optional(PascalToken.AssertSwitchLong)) {
                ParseLongAssertSwitch();
                return true;
            }

            if (Optional(PascalToken.DebugInfoSwitchLong)) {
                ParseLongDebugInfoSwitch();
                return true;
            }

            return false;
        }

        private void ParseLongDebugInfoSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.DebugInfo.Value = DebugInformation.IncludeDebugInformation;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.DebugInfo.Value = DebugInformation.NoDebugInfo;
                return;
            }

            Unexpected();
        }

        private void ParseLongAssertSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.Assertions.Value = AssertionMode.EnableAssertions;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.Assertions.Value = AssertionMode.DisableAssertions;
                return;
            }

            Unexpected();
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

            Unexpected();
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
        private bool ParseSwitch() {

            if (ConditionalCompilation.Skip)
                return false;

            if (Match(PascalToken.AlignSwitch, PascalToken.AlignSwitch1, PascalToken.AlignSwitch2, PascalToken.AlignSwitch4, PascalToken.AlignSwitch8, PascalToken.AlignSwitch16)) {
                ParseAlignSwitch();
                return true;
            }

            if (Match(PascalToken.BoolEvalSwitch)) {
                ParseBoolEvalSwitch();
                return true;
            }

            if (Match(PascalToken.AssertSwitch)) {
                ParseAssertSwitch();
                return true;
            }

            if (Match(PascalToken.DebugInfoOrDescriptionSwitch)) {
                return ParseDebugInfoOrDescriptionSwitch();
            }

            return false;
        }


        private bool ParseDebugInfoOrDescriptionSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.DebugInfo.Value = DebugInformation.IncludeDebugInformation;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.DebugInfo.Value = DebugInformation.NoDebugInfo;
                return true;
            }

            Unexpected();
            return false;
        }

        private void ParseAssertSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.Assertions.Value = AssertionMode.EnableAssertions;
                return;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.Assertions.Value = AssertionMode.DisableAssertions;
                return;
            }

            Unexpected();
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
