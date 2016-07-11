using PasPasPas.Api;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     provides tokenizer helpers delphi compiler directives
    /// </summary>
    public class PreprocessorPunctuators : InputPatterns {

        /// <summary>
        ///     create the preprocessor punctuators
        /// </summary>
        public PreprocessorPunctuators() {
            AddPattern('+', PascalToken.Plus);
            AddPattern('-', PascalToken.Minus);
            AddPattern('(', PascalToken.OpenParen);
            AddPattern(')', PascalToken.CloseParen);
            AddPattern('[', PascalToken.OpenBraces);
            AddPattern(']', PascalToken.CloseBraces);
            AddPattern(',', PascalToken.Comma);
            AddPattern('*', PascalToken.Times);
            AddPattern(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            AddPattern(new IdentifierCharacterClass() { AllowDots = true }, new IdentifierTokenGroupValue(CompilerDirectiveTokenizer.Keywords) { AllowDots = true });
            AddPattern(new NumberCharacterClass(), new NumberTokenGroupValue());
            AddPattern('$', new HexNumberTokenValue());
            AddPattern(new ControlCharacterClass(), new ControlTokenGroupValue());
            AddPattern('\'', new QuotedStringTokenValue());
        }
    }
}