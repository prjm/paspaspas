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
            AddPunctuator('.', PascalToken.Dot).Add('.', PascalToken.DotDot);
            AddPunctuator(',', PascalToken.Comma);
            AddPunctuator('(', PascalToken.OpenParen);
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
            AddPunctuator('/', PascalToken.Slash);
            AddPunctuator('@', PascalToken.At);
            AddPunctuator('>', PascalToken.GreaterThen).Add('=', PascalToken.GreaterThenEquals);
            var lt = AddPunctuator('<', PascalToken.LessThen);
            lt.Add('=', PascalToken.LessThenEquals);
            lt.Add('>', PascalToken.NotEquals);

            AddPunctuator('{', new CurlyBraceCommenTokenValue()).Add('$', new PreprocessorTokenValue());
            AddPunctuator('$', new HexNumberTokenValue());
            AddPunctuator(new WhitspaceCharacterClass(), new WhitespaceTokenGroupValue());
            AddPunctuator(new NumberCharacterClass(), new NumberTokenGroupValue());
        }


    }
}
