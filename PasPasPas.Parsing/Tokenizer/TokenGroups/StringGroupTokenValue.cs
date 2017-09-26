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
    public sealed class StringGroupTokenValue : PatternContinuation {

        private QuotedStringTokenValue quotedString
            = new QuotedStringTokenValue(TokenKind.QuotedString, '\'');

        private CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.Integer, new DigitCharClass(false), 0, StaticDependency.ParsedIntegers, Guid.Empty);

        private CharacterClassTokenGroupValue hexDigits
            = new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, StaticDependency.ParsedHexNumbers, Tokenizer.IncompleteHexNumber);

        /// <summary>
        ///     parse a string literal
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(TokenizerState state) {
            using (var resultBuilder = PoolFactory.FetchStringBuilder()) {
                state.PreviousChar();
                state.Clear();

                do {
                    var currentChar = state.NextChar(false);
                    if (currentChar == '#') {
                        state.Append('#');
                        if (state.AtEof)
                            state.Error(Tokenizer.IncompleteString);
                        else {
                            var nextChar = state.NextChar(false);
                            if (nextChar == '$') {
                                state.Append('$');
                                var controlChar = hexDigits.Tokenize(state);
                                if (controlChar.Kind != TokenKind.HexNumber || !LiteralValues.Literals.IsValidInteger(controlChar.ParsedValue))
                                    state.Error(Tokenizer.IncompleteString);
                                else
                                    resultBuilder.Data.Append(LiteralValues.Literals.ConvertCharLiteral(controlChar.ParsedValue));
                            }
                            else {
                                state.PreviousChar();
                                var controlChar = digitTokenizer.Tokenize(state);
                                if (controlChar.Kind != TokenKind.Integer || !LiteralValues.Literals.IsValidInteger(controlChar.ParsedValue))
                                    state.Error(Tokenizer.UnexpectedCharacter);
                                else
                                    resultBuilder.Data.Append(LiteralValues.Literals.ConvertCharLiteral(controlChar.ParsedValue));
                            }
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
                } while (!state.AtEof);

                var data = StringPool.PoolString(resultBuilder.Data.ToString());
                return new Token(TokenKind.QuotedString, state, data);
            }
        }
    }


}
