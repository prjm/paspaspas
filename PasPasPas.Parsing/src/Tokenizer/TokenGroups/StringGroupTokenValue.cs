using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
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
            = new CharacterClassTokenGroupValue(TokenKind.IntegralNumber, new DigitCharClass(false), 0, LiteralParserKind.IntegerNumbers, Guid.Empty);

        private CharacterClassTokenGroupValue hexDigits
            = new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, LiteralParserKind.HexNumbers, TokenizerBase.IncompleteHexNumber);

        /// <summary>
        ///     parse a string literal
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(TokenizerState state) {
            var parsedValue = state.Environment.Runtime.Strings.EmptyString;

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
                                if (controlChar.Kind == TokenKind.HexNumber && controlChar.ParsedValue is IIntegerValue hexValue && !hexValue.IsNegative) {
                                    var charValue = state.Environment.Runtime.Chars.ToWideCharValue(KnownTypeIds.WideCharType, (char)hexValue.UnsignedValue) as ICharValue;
                                    parsedValue = state.Environment.Runtime.Strings.Concat(parsedValue, charValue);
                                    resultBuilder.Item.Append(charValue.AsWideChar);
                                }
                                else
                                    state.Error(TokenizerBase.IncompleteString);
                            }
                            else {
                                var controlChar = digitTokenizer.Tokenize(state);
                                if (controlChar.Kind == TokenKind.IntegralNumber && controlChar.ParsedValue is IIntegerValue intValue && !intValue.IsNegative) {
                                    var charValue = state.Environment.Runtime.Chars.ToWideCharValue(KnownTypeIds.WideCharType, (char)intValue.UnsignedValue) as ICharValue;
                                    parsedValue = state.Environment.Runtime.Strings.Concat(parsedValue, charValue);
                                    resultBuilder.Item.Append(charValue.AsWideChar);
                                }
                                else
                                    state.Error(TokenizerBase.UnexpectedCharacter);
                            }
                        }
                    }
                    else if (state.LookAhead() == '\'') {
                        state.NextChar(true);
                        var qs = quotedString.Tokenize(state);
                        parsedValue = state.RuntimeValues.Strings.Concat(parsedValue, qs.ParsedValue);
                    }
                    else {
                        break;
                    }
                } while (!state.AtEof);

                return new Token(TokenKind.QuotedString, state, parsedValue);
            }
        }
    }
}
