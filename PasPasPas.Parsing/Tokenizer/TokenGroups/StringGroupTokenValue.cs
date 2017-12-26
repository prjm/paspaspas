using System;
using PasPasPas.Infrastructure.Common;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
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
                                if (controlChar.Kind != TokenKind.HexNumber || !LiteralValues.Literals.IsValidInteger(controlChar.ParsedValue))
                                    state.Error(Tokenizer.IncompleteString);
                                else
                                    resultBuilder.Data.Append(state.ConvertCharLiteral(controlChar.ParsedValue));
                            }
                            else {
                                var controlChar = digitTokenizer.Tokenize(state);
                                if (controlChar.Kind != TokenKind.Integer || !LiteralValues.Literals.IsValidInteger(controlChar.ParsedValue))
                                    state.Error(Tokenizer.UnexpectedCharacter);
                                else
                                    resultBuilder.Data.Append(state.ConvertCharLiteral(controlChar.ParsedValue));
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

                IValue data;
                if (resultBuilder.Data.Length == 1)
                    data = state.Constants.ToValue(resultBuilder.Data[0]);
                else
                    data = state.Constants.ToValue(state.Environment.StringPool.PoolString(resultBuilder.Data.ToString()));

                return new Token(TokenKind.QuotedString, state, data);
            }
        }
    }
}
