using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for strings
    /// </summary>
    public class StringGroupTokenValue : PatternContinuation {

        private QuotedStringTokenValue quotedString
            = new QuotedStringTokenValue(TokenKind.QuotedString, '\'');

        private CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.Integer, new DigitCharClass(false), 0, Literals.ParsedIntegers, Guid.Empty);

        private CharacterClassTokenGroupValue hexDigits
            = new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, Literals.ParsedHexNumbers, StandardTokenizer.IncompleteHexNumber);

        /// <summary>
        ///     parse a string literal
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(TokenizerState state) {
            using (var resultBuilder = PoolFactory.FetchStringBuilder()) {
                state.PreviousChar();
                state.Clear();

                var currentChar = state.NextChar(false);
                while (!state.AtEof) {
                    currentChar = state.CurrentCharacter;
                    if (currentChar == '#') {
                        state.Append('#');
                        var nextChar = state.NextChar(false);
                        if (state.AtEof)
                            state.Error(TokenizerBase.UnexpectedEndOfToken);
                        else if (nextChar == '$') {
                            state.Append('$');
                            var controlChar = hexDigits.Tokenize(state);
                            if (controlChar.Kind != TokenKind.HexNumber)
                                state.Error(TokenizerBase.UnexpectedCharacter);
                            else
                                resultBuilder.Data.Append(Convert.ToChar(controlChar.ParsedValue));
                        }
                        else {
                            state.PreviousChar();
                            var controlChar = digitTokenizer.Tokenize(state);
                            if (controlChar.Kind != TokenKind.Integer)
                                state.Error(TokenizerBase.UnexpectedCharacter);
                            else
                                resultBuilder.Data.Append(Convert.ToChar(controlChar.ParsedValue));
                        }
                    }
                    else if (currentChar == '\'') {
                        state.Append('\'');
                        var qs = quotedString.Tokenize(state);
                        resultBuilder.Data.Append(qs.ParsedValue);
                    }
                    else {
                        break;
                    }
                }

                var value = state.GetBufferContent().Pool();
                var data = resultBuilder.Data.ToString().Pool();
                return new Token(TokenKind.QuotedString, state.CurrentPosition, value, data);
            }
        }
    }


}
