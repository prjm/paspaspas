using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     provides tokenizer helpers delphi compiler directives
    /// </summary>
    public class CompilerDirectivePatterns : InputPatterns {

        /// <summary>
        ///     create the preprocessor punctuators
        /// </summary>
        public CompilerDirectivePatterns() {
            AddPattern('+', PascalToken.Plus);
            AddPattern('-', PascalToken.Minus);
            AddPattern('(', TokenKind.OpenParen);
            AddPattern(')', TokenKind.CloseParen);
            AddPattern('[', TokenKind.OpenBraces);
            AddPattern(']', TokenKind.CloseBraces);
            AddPattern(',', TokenKind.Comma);
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