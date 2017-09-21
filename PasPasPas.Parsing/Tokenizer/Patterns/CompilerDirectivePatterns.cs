using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.TokenGroups;

namespace PasPasPas.Parsing.Tokenizer.Patterns {

    /// <summary>
    ///     provides tokenizer helpers delphi compiler directives
    /// </summary>
    public class CompilerDirectivePatterns : InputPatterns {

        /// <summary>
        ///     create the preprocessor punctuators
        /// </summary>
        public CompilerDirectivePatterns() {

            AddPattern('+', TokenKind.Plus);
            AddPattern('-', TokenKind.Minus);
            AddPattern('(', TokenKind.OpenParen);
            AddPattern(')', TokenKind.CloseParen);
            AddPattern('[', TokenKind.OpenBraces);
            AddPattern(']', TokenKind.CloseBraces);
            AddPattern(',', TokenKind.Comma);
            AddPattern('*', TokenKind.Times);
            AddPattern(new WhiteSpaceCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.WhiteSpace, new WhiteSpaceCharacterClass()));
            AddPattern(new IdentifierCharacterClass() { AllowDots = true }, new IdentifierTokenGroupValue(CompilerDirectiveTokenizer.Keywords) { AllowDots = true });
            /*
            AddPattern(new NumberCharacterClass(), new NumberTokenGroupValue() { AllowIdents = true });
            AddPattern('$', new HexNumberTokenValue());
            AddPattern(new ControlCharacterClass(), new ControlTokenGroupValue());
            AddPattern('\'', new QuotedStringTokenValue());
            */
        }
    }
}