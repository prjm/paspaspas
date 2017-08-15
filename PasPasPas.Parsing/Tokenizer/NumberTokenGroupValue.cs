using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {


    /// <summary>
    ///     token group for numbers
    /// </summary>
    public class NumberTokenGroupValue : PatternContinuation {

        private DigitTokenGroupValue digitTokenizer
            = new DigitTokenGroupValue();

        private NumberCharacterClass numbers
            = new NumberCharacterClass();

        private SingleCharClass dot
            = new SingleCharClass('.');

        private ExponentCharacterClass exponent
            = new ExponentCharacterClass();

        private PlusMinusCharacterClass plusminus
            = new PlusMinusCharacterClass();

        private IdentifierCharacterClass allIdents
            = new IdentifierCharacterClass() { AllowAmpersand = true, AllowDigits = true, AllowDots = true, };

        private IdentifierTokenGroupValue identTokenizer
            = new IdentifierTokenGroupValue(new Dictionary<string, int>());

        /// <summary>
        ///     flag, if <c>true</c> idents are generated if possible
        /// </summary>
        public bool AllowIdents { get; set; }
            = false;

        private static bool NextCharMatches(ITokenizerState state, CharacterClass c) {
            if (state.AtEof)
                return false;

            var nextChar = state.NextChar(false);

            if (c.Matches(nextChar)) {
                state.Append(nextChar);
                return true;
            }

            state.PreviousChar();
            return false;
        }

        public override Token Tokenize(ITokenizerState state) {
            var withDot = false;
            var withExponent = false;

            if (state.AtEof) {
                return new Token(TokenKind.Integer, state);
            }
            else {
                digitTokenizer.Tokenize(state);
            }

            if (NextCharMatches(state, dot)) {
                withDot = true;

                if (NextCharMatches(state, numbers)) {
                    digitTokenizer.Tokenize(state);
                }

                if (state.BufferEndsWith(".")) {
                    withDot = false;
                    state.PreviousChar();
                    state.Length -= 1;
                }

            }

            if (NextCharMatches(state, exponent)) {
                if (state.AtEof) {
                    state.Error(TokenizerBase.UnexpectedEndOfToken);
                }
                else {
                    NextCharMatches(state, plusminus);
                    var currentLen = state.Length;
                    if (state.AtEof || digitTokenizer.Tokenize(state).Kind != TokenKind.Integer || state.Length == currentLen) {
                        state.Error(TokenizerBase.UnexpectedEndOfToken);
                    }
                }

                withExponent = true;
            }

            if (AllowIdents && NextCharMatches(state, allIdents)) {
                identTokenizer.Tokenize(state);
                return new Token(TokenKind.Identifier, state);
            }

            if (withDot || withExponent) {
                return new Token(TokenKind.Real, state);
            }
            else {
                return new Token(TokenKind.Integer, state);
            }

        }

    }

}