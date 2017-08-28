using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {


    /// <summary>
    ///     token group for numbers
    /// </summary>
    public class NumberTokenGroupValue : PatternContinuation {

        private CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.Integer, new DigitCharClass(false));

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

        private static bool CurrentCharMatches(ITokenizerState state, CharacterClass c) {
            if (state.AtEof)
                return false;

            var currentChar = state.CurrentCharacter;

            if (c.Matches(currentChar)) {
                state.Append(currentChar);
                return true;
            }

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

            if (CurrentCharMatches(state, dot)) {
                state.NextChar(false);
                withDot = true;

                if (CurrentCharMatches(state, digitTokenizer.CharClass)) {
                    digitTokenizer.Tokenize(state);
                }

                if (state.BufferEndsWith(".")) {
                    state.PreviousChar();
                    withDot = false;
                    state.Length -= 1;
                }

            }

            if (CurrentCharMatches(state, exponent)) {
                state.NextChar(false);
                if (state.AtEof) {
                    state.Error(TokenizerBase.UnexpectedEndOfToken);
                }
                else {
                    if (CurrentCharMatches(state, plusminus))
                        state.NextChar(false);

                    if (CurrentCharMatches(state, digitTokenizer.CharClass)) {
                        digitTokenizer.Tokenize(state);
                    }
                    else {
                        state.Error(TokenizerBase.UnexpectedEndOfToken);
                    }
                }

                withExponent = true;
            }

            if (AllowIdents && CurrentCharMatches(state, allIdents)) {
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