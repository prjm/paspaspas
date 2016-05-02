using PasPasPas.Api;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     provides tokenizer helpers delphi compiler directives
    /// </summary>
    public class PreprocessorPunctuators : Punctuators {

        /// <summary>
        ///     create the preprocessor punctuators
        /// </summary>
        public PreprocessorPunctuators() {
            AddPunctuator('+', PascalToken.Plus);
            AddPunctuator('-', PascalToken.Minus);
            AddPunctuator('(', PascalToken.OpenParen);
            AddPunctuator(')', PascalToken.CloseParen);
            AddPunctuator('[', PascalToken.OpenBraces);
            AddPunctuator(']', PascalToken.CloseBraces);
            AddPunctuator(',', PascalToken.Comma);
            AddPunctuator('*', PascalToken.Times);
            AddPunctuator(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            AddPunctuator(new IdentifierCharacterClass() { AllowDots = true }, new IdentifierTokenGroupValue(CompilerDirectiveTokenizer.Keywords) { AllowDots = true });
            AddPunctuator(new NumberCharacterClass(), new NumberTokenGroupValue());
            AddPunctuator('$', new HexNumberTokenValue());
            AddPunctuator(new ControlCharacterClass(), new ControlTokenGroupValue());
            AddPunctuator('\'', new QuotedStringTokenValue());
        }
    }
}