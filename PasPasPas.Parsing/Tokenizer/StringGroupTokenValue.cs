using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    }
                    else {
                        var controlChar = digitTokenizer.Tokenize(state);
                        if (controlChar.Kind != TokenKind.Integer) {
                            state.Error(TokenizerBase.UnexpectedCharacter);
                        }
                    }
                }
                else if (currentChar == '\'') {
                    state.Append('\'');
                    quotedString.Tokenize(state);
                }
                else {
                    break;
                }
            }

            return new Token(TokenKind.QuotedString, state);
        }
    }


}
