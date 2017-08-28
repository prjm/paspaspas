using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     standard patterns for the tokenizer
    /// </summary>
    public class StandardPatterns : InputPatterns {

        /// <summary>
        ///     register tokenizer patters
        /// </summary>
        public StandardPatterns() {
            AddPattern(',', TokenKind.Comma);

            var dot = AddPattern('.', TokenKind.Dot);
            dot.Add('.', TokenKind.DotDot);
            dot.Add(')', TokenKind.CloseBraces);

            var lparen = AddPattern('(', TokenKind.OpenParen);
            lparen.Add('.', TokenKind.OpenBraces);
            lparen.Add('*', new SequenceGroupTokenValue(TokenKind.Comment, "*)")).Add('$', new SequenceGroupTokenValue(TokenKind.Preprocessor, "*)"));

            AddPattern(')', TokenKind.CloseParen);
            AddPattern(';', TokenKind.Semicolon);
            AddPattern('=', TokenKind.EqualsSign);
            AddPattern('[', TokenKind.OpenBraces);
            AddPattern(']', TokenKind.CloseBraces);
            AddPattern(':', TokenKind.Colon).Add('=', TokenKind.Assignment);
            AddPattern('^', TokenKind.Circumflex);
            AddPattern('+', TokenKind.Plus);
            AddPattern('-', TokenKind.Minus);
            AddPattern('*', TokenKind.Times);

            AddPattern('/', TokenKind.Slash).Add('/', new EndOfLineCommentTokenGroupValue());

            AddPattern('@', TokenKind.At);
            AddPattern('>', TokenKind.GreaterThen).Add('=', TokenKind.GreaterThenEquals);

            var lt = AddPattern('<', TokenKind.LessThen);
            lt.Add('=', TokenKind.LessThenEquals);
            lt.Add('>', TokenKind.NotEquals);

            AddPattern('{', new SequenceGroupTokenValue(TokenKind.Comment, "}")).Add('$', new SequenceGroupTokenValue(TokenKind.Preprocessor, "}"));
            AddPattern('$', new HexNumberTokenValue());
            AddPattern(new WhiteSpaceCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.WhiteSpace, new WhiteSpaceCharacterClass()));
            AddPattern(new IdentifierCharacterClass(), new IdentifierTokenGroupValue(StandardTokenizer.Keywords) { AllowAmpersand = true, ParseAsm = true });

            AddPattern(new DigitCharClass(false), new NumberTokenGroupValue());
            AddPattern(new ControlCharacterClass(), new ControlTokenGroupValue());
            AddPattern('\'', new StringGroupTokenValue());
            AddPattern('"', new QuotedStringTokenValue(TokenKind.DoubleQuotedString, '"'));
            AddPattern('#', new StringGroupTokenValue());
            AddPattern('\x001A', new SoftEofTokenValue());

        }


    }
}
