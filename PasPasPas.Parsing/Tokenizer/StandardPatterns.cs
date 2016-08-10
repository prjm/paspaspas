﻿using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     standard pascal punctuators
    /// </summary>
    public class StandardPatterns : InputPatterns {

        /// <summary>
        ///     register punctuators
        /// </summary>
        public StandardPatterns() {
            var dot = AddPattern('.', TokenKind.Dot);
            dot.Add('.', PascalToken.DotDot);
            dot.Add(')', TokenKind.CloseBraces);

            var lparen = AddPattern('(', TokenKind.OpenParen);
            lparen.Add('.', TokenKind.OpenBraces);
            lparen.Add('*', new AlternativeCurlyBraceCommentTokenValue()).Add('$', new AlternativePreprocessorTokenValue());

            AddPattern(',', TokenKind.Comma);
            AddPattern(')', TokenKind.CloseParen);
            AddPattern(';', TokenKind.Semicolon);
            AddPattern('=', TokenKind.EqualsSign);
            AddPattern('[', TokenKind.OpenBraces);
            AddPattern(']', TokenKind.CloseBraces);
            AddPattern(':', TokenKind.Colon).Add('=', PascalToken.Assignment);
            AddPattern('^', TokenKind.Circumflex);
            AddPattern('+', PascalToken.Plus);
            AddPattern('-', PascalToken.Minus);
            AddPattern('*', PascalToken.Times);
            AddPattern('/', PascalToken.Slash).Add('/', new EndOfLineCommentTokenGroupValue());
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
