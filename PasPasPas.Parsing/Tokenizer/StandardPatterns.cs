using PasPasPas.Api;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     standard pascal punctuators
    /// </summary>
    public class StandardPatterns : InputPatterns {

        /// <summary>
        ///     register punctuators
        /// </summary>
        public StandardPatterns() {
            var dot = AddPattern('.', PascalToken.Dot);
            dot.Add('.', PascalToken.DotDot);
            dot.Add(')', PascalToken.CloseBraces);

            var lparen = AddPattern('(', PascalToken.OpenParen);
            lparen.Add('.', PascalToken.OpenBraces);
            lparen.Add('*', new AlternativeCurlyBraceCommenTokenValue()).Add('$', new AlternativePreprocessorTokenValue());

            AddPattern(',', PascalToken.Comma);
            AddPattern(')', PascalToken.CloseParen);
            AddPattern(';', PascalToken.Semicolon);
            AddPattern('=', PascalToken.EqualsSign);
            AddPattern('[', PascalToken.OpenBraces);
            AddPattern(']', PascalToken.CloseBraces);
            AddPattern(':', PascalToken.Colon).Add('=', PascalToken.Assignment);
            AddPattern('^', PascalToken.Circumflex);
            AddPattern('+', PascalToken.Plus);
            AddPattern('-', PascalToken.Minus);
            AddPattern('*', PascalToken.Times);
            AddPattern('/', PascalToken.Slash).Add('/', new EondOfLineCommentTokenGroupValue());
            AddPattern('@', PascalToken.At);
            AddPattern('>', PascalToken.GreaterThen).Add('=', PascalToken.GreaterThenEquals);
            var lt = AddPattern('<', PascalToken.LessThen);
            lt.Add('=', PascalToken.LessThenEquals);
            lt.Add('>', PascalToken.NotEquals);

            AddPattern('{', new CurlyBraceCommentTokenValue()).Add('$', new PreprocessorTokenValue());
            AddPattern('$', new HexNumberTokenValue());
            AddPattern(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            AddPattern(new IdentifierCharacterClass(), new IdentifierTokenGroupValue(StandardTokenizer.Keywords));
            AddPattern(new NumberCharacterClass(), new NumberTokenGroupValue());
            AddPattern(new ControlCharacterClass(), new ControlTokenGroupValue());
            AddPattern('\'', new StringGroupTokenValue());
            AddPattern('"', new DoubleQuoteStringGroupTokenValue());
            AddPattern('#', new StringGroupTokenValue());
            AddPattern('\x001A', new SoftEofTokenValue());
        }


    }
}
