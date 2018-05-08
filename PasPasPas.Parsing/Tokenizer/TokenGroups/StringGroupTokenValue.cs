using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for strings
    /// </summary>
    public sealed class StringGroupTokenValue : PatternContinuation {

        private QuotedStringTokenValue quotedString
            = new QuotedStringTokenValue(TokenKind.QuotedString, '\'');

        private CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.Integer, new DigitCharClass(false), 0, LiteralParserKind.IntegerNumbers, Guid.Empty);

        private CharacterClassTokenGroupValue hexDigits
            = new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, LiteralParserKind.HexNumbers, Tokenizer.IncompleteHexNumber);

        /// <summary>
        ///     parse a string literal
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(TokenizerState state) {
            using (var resultBuilder = state.FetchStringBuilder()) {
                state.PreviousChar();
                state.Clear();

                do {
                    if (state.LookAhead() == '#') {
                        state.NextChar(true);
                        if (state.AtEof)
                            state.Error(Tokenizer.IncompleteString);
                        else {
                            if (state.LookAhead() == '$') {
                                state.NextChar(true);
                                var controlChar = hexDigits.Tokenize(state);
                                if (controlChar.Kind == TokenKind.HexNumber && controlChar.ParsedValue is IIntegerValue hexValue && !hexValue.IsNegative)
                                    resultBuilder.Data.Append(state.ConvertCharLiteral(hexValue.UnsignedValue));
                                else
                                    state.Error(Tokenizer.IncompleteString);
                            }
                            else {
                                var controlChar = digitTokenizer.Tokenize(state);
                                if (controlChar.Kind == TokenKind.Integer && controlChar.ParsedValue is IIntegerValue intValue && !intValue.IsNegative)
                                    resultBuilder.Data.Append(state.ConvertCharLiteral(intValue.UnsignedValue));
                                else
                                    state.Error(Tokenizer.UnexpectedCharacter);
                            }
                        }
                    }
                    else if (state.LookAhead() == '\'') {
                        state.NextChar(true);
                        var qs = quotedString.Tokenize(state);
                        resultBuilder.Data.Append(qs.ParsedValue);
                    }
                    else {
                        break;
                    }
                } while (!state.AtEof);

                ITypeReference data;
                if (resultBuilder.Data.Length == 1)
                    data = state.Constants.Chars.ToWideCharValue(resultBuilder.Data[0]);
                else
                    data = state.Constants.Strings.ToUnicodeString(state.Environment.StringPool.PoolString(resultBuilder.Data.ToString()));

                return new Token(TokenKind.QuotedString, state, data);
            }
        }
    }
}
