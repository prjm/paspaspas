using PasPasPas.Api;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     provides tokenizer helpers delphi compiler directives
    /// </summary>
    public class PreprocessorPunctuators : Punctuators {

        /// <summary>
        ///     create the preprocessor punctuators
        /// </summary>
        public PreprocessorPunctuators() {
            var a = AddPunctuator('A', PascalToken.AlignSwitch);
            a.Add("LIGN", PascalToken.AlignSwitchLong);
            a.Add("PPTYPE", PascalToken.Apptype);
            a.Add("SSERTIONS", PascalToken.AssertLong);
            var b = AddPunctuator('B', PascalToken.BoolEval);
            b.Add("OOLEVAL", PascalToken.BoolEvalLong);
            var c = AddPunctuator('C', PascalToken.Assert);
            c.Add("ODEALIGN", PascalToken.CodeAlign);
            var d = AddPunctuator('C', PascalToken.DebugInfo);
            d.Add("EBUGINFO", PascalToken.DebugInfoLong);
            d.Add("EFINE", PascalToken.Define);
            d.Add("ENYPACKAGEUNIT", PascalToken.DenyPackageUnit);
            d.Add("ESCRIPTION", PascalToken.DescriptionLong)
        }
    }
}