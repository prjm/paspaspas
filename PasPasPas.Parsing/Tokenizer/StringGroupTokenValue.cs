using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     token group for strings
    /// </summary>
    public class StringGroupTokenValue : PatternContinuation {

        private QuotedStringTokenValue quotedString
            = new QuotedStringTokenValue(TokenKind.QuotedString, '\'');

        private CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.Integer, new DigitCharClass(false));

        private HexNumberTokenValue hexDigits
            = new HexNumberTokenValue();

        /// <summary>
        ///     parse a string literal
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(TokenizerState state) {
            var resultBuilder = new StringBuilder();
            state.PreviousChar();
            state.Clear();

            var currentChar = state.NextChar(false);
            while (!state.AtEof) {
                currentChar = state.CurrentCharacter;
                if (currentChar == '#') {
                    state.Append('#');
                    var nextChar = state.NextChar(true);
                    if (state.AtEof) {
                        state.Error(TokenizerBase.UnexpectedEndOfToken);
                    }
                    else if (nextChar == '$') {
                        var controlChar = hexDigits.Tokenize(state);
                        if (controlChar.Kind != TokenKind.HexNumber) {
                            state.Error(TokenizerBase.UnexpectedCharacter);
                        }
                        else {
                            resultBuilder.Append(Convert.ToChar(Literals.ParseHexNumberLiteral(controlChar.Value)));
                        }
                    }
                    else {
                        var controlChar = digitTokenizer.Tokenize(state);
                        if (controlChar.Kind != TokenKind.Integer) {
                            state.Error(TokenizerBase.UnexpectedCharacter);
                        }
                        else {
                            resultBuilder.Append(Convert.ToChar(Literals.ParseIntegerLiteral(controlChar.Value)));
                        }
                    }
                }
                else if (currentChar == '\'') {
                    state.Append('\'');
                    var qs = quotedString.Tokenize(state);
                    resultBuilder.Append(qs.ParsedValue);
                }
                else {
                    break;
                }
            }

            var value = state.GetBufferContent().Pool();
            var data = resultBuilder.ToString().Pool();
            return new Token(TokenKind.QuotedString, state.CurrentPosition, value, data);
        }
    }


}
