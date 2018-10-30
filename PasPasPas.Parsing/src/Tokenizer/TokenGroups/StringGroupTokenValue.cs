using System;
using PasPasPas.Globals.Runtime;
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
            = new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, LiteralParserKind.HexNumbers, TokenizerBase.IncompleteHexNumber);

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
                            state.Error(TokenizerBase.IncompleteString);
                        else {
                            if (state.LookAhead() == '$') {
                                state.NextChar(true);
                                var controlChar = hexDigits.Tokenize(state);
                                if (controlChar.Kind == TokenKind.HexNumber && controlChar.ParsedValue is IIntegerValue hexValue && !hexValue.IsNegative)
                                    resultBuilder.Item.Append(state.ConvertCharLiteral(hexValue.UnsignedValue).ToString()[0]);
                                else
                                    state.Error(TokenizerBase.IncompleteString);
                            }
                            else {
                                var controlChar = digitTokenizer.Tokenize(state);
                                if (controlChar.Kind == TokenKind.Integer && controlChar.ParsedValue is IIntegerValue intValue && !intValue.IsNegative)
                                    resultBuilder.Item.Append(state.ConvertCharLiteral(intValue.UnsignedValue).ToString()[0]);
                                else
                                    state.Error(TokenizerBase.UnexpectedCharacter);
                            }
                        }
                    }
                    else if (state.LookAhead() == '\'') {
                        state.NextChar(true);
                        var qs = quotedString.Tokenize(state);
                        foreach (var c in qs.ParsedValue.ToString())
                            resultBuilder.Item.Append(c);
                    }
                    else {
                        break;
                    }
                } while (!state.AtEof);

                ITypeReference data;
                if (resultBuilder.Item.Length == 1)
                    data = state.RuntimeValues.Chars.ToWideCharValue(resultBuilder.Item[0]);
                else
                    data = state.RuntimeValues.Strings.ToUnicodeString(state.Environment.StringPool.PoolString(resultBuilder.Item));

                return new Token(TokenKind.QuotedString, state, data);
            }
        }
    }
}
