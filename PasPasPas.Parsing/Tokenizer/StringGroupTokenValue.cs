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

        private DigitTokenGroupValue digits
            = new DigitTokenGroupValue();

        private HexNumberTokenValue hexDigits
            = new HexNumberTokenValue();

        /// <summary>
        ///     parse a string literal
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(ITokenizerState state) {
            state.PreviousChar();
            state.Clear();

            while (!state.AtEof) {
                var currentChar = state.NextChar(true);
                if (currentChar == '#' && state.AtEof) {
                    state.Error(TokenizerBase.UnexpectedEndOfToken);
                }
                else if (currentChar == '#') {
                    var nextChar = state.NextChar(true);
                    if (nextChar == '$' && state.AtEof) {
                        state.Error(TokenizerBase.UnexpectedEndOfToken);
                    }
                    else if (nextChar == '$') {
                        var controlChar = hexDigits.Tokenize(state);
                        if (controlChar.Kind != TokenKind.HexNumber) {
                            state.Error(TokenizerBase.UnexpectedCharacter);
                        }
                    }
                    else {
                        state.PreviousChar();
                        var controlChar = digits.Tokenize(state);
                        if (controlChar.Kind != TokenKind.Integer) {
                            state.Error(TokenizerBase.UnexpectedCharacter);
                        }
                    }
                }
                else if (currentChar == '\'') {
                    quotedString.Tokenize(state);
                }
                else {
                    state.PreviousChar();
                    break;
                }
            }

            return new Token(TokenKind.QuotedString, state);
        }
    }


}
