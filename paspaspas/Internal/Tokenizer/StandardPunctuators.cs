using PasPasPas.Api;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     standard pascal punctuators
    /// </summary>
    public class StandardPunctuators : Punctuators {

        /// <summary>
        ///     register punctuators
        /// </summary>
        public StandardPunctuators() {
            var dot = AddPunctuator('.', PascalToken.Dot);
            dot.Add('.', PascalToken.DotDot);
            dot.Add(')', PascalToken.CloseBraces);

            var lparen = AddPunctuator('(', PascalToken.OpenParen);
            lparen.Add('.', PascalToken.OpenBraces);
            lparen.Add('*', new AlternativeCurlyBraceCommenTokenValue()).Add('$', new AlternativePreprocessorTokenValue());

            AddPunctuator(',', PascalToken.Comma);
            AddPunctuator(')', PascalToken.CloseParen);
            AddPunctuator(';', PascalToken.Semicolon);
            AddPunctuator('=', PascalToken.EqualsSign);
            AddPunctuator('[', PascalToken.OpenBraces);
            AddPunctuator(']', PascalToken.CloseBraces);
            AddPunctuator(':', PascalToken.Colon).Add('=', PascalToken.Assignment);
            AddPunctuator('^', PascalToken.Circumflex);
            AddPunctuator('+', PascalToken.Plus);
            AddPunctuator('-', PascalToken.Minus);
            AddPunctuator('*', PascalToken.Times);
            AddPunctuator('/', PascalToken.Slash).Add('/', new EondOfLineCommentTokenGroupValue());
            AddPunctuator('@', PascalToken.At);
            AddPunctuator('>', PascalToken.GreaterThen).Add('=', PascalToken.GreaterThenEquals);
            var lt = AddPunctuator('<', PascalToken.LessThen);
            lt.Add('=', PascalToken.LessThenEquals);
            lt.Add('>', PascalToken.NotEquals);

            AddPunctuator('{', new CurlyBraceCommenTokenValue()).Add('$', new PreprocessorTokenValue());
            AddPunctuator('$', new HexNumberTokenValue());
            AddPunctuator(new WhitspaceCharacterClass(), new WhiteSpaceTokenGroupValue());
            AddPunctuator(new IdentifierCharacterClass(), new IdentifierTokenGroupValue());
            AddPunctuator(new NumberCharacterClass(), new NumberTokenGroupValue());
            AddPunctuator(new ControlCharacterClass(), new ControlTokenGroupValue());
            AddPunctuator('\'', new StringGroupTokenValue());
            AddPunctuator('"', new DoubleQuoteStringGroupTokenValue());
            AddPunctuator('#', new StringGroupTokenValue());
            AddPunctuator('\x001A', new SoftEofTokenValue());
        }


    }
}
