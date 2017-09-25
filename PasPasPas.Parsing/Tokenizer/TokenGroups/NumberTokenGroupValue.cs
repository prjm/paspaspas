using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for numbers
    /// </summary>
    public sealed class NumberTokenGroupValue : PatternContinuation {

        private CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.Integer, new DigitCharClass(false), 0, StaticDependency.ParsedIntegers, Guid.Empty);

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

        /// <summary>
        ///     test if the current char matches a given char class
        /// </summary>
        /// <param name="state">current state</param>
        /// <param name="charClass"></param>
        /// <returns>character class to match</returns>
        private static bool CurrentCharMatches(TokenizerState state, CharacterClass charClass) {
            var currentChar = state.CurrentCharacter;

            if (charClass.Matches(currentChar)) {
                state.Append(currentChar);
                return true;
            }

            return false;
        }

        public override Token Tokenize(TokenizerState state) {
            var withDot = false;
            var withExponent = false;

            if (state.AtEof) {
                var number = LiteralValues.Literals.ParseIntegerLiteral(state.GetBufferContent());
                return new Token(TokenKind.Integer, state, number);
            }

            state.Clear();
            state.PreviousChar();
            var digits = digitTokenizer.Tokenize(state).ParsedValue;
            object decimals = null;
            object exp = null;
            var minus = false;

            if (!state.AtEof)
                state.NextChar(false);

            if (CurrentCharMatches(state, dot)) {
                withDot = true;

                if (!state.AtEof)
                    state.NextChar(false);

                if (digitTokenizer.CharClass.Matches(state.CurrentCharacter)) {
                    state.PreviousChar();
                    decimals = digitTokenizer.Tokenize(state).ParsedValue;
                    if (!state.AtEof)
                        state.NextChar(false);
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
                    state.Error(Tokenizer.UnexpectedEndOfToken);
                }
                else {
                    minus = state.CurrentCharacter == '-';
                    if (CurrentCharMatches(state, plusminus))
                        state.NextChar(false);

                    if (digitTokenizer.CharClass.Matches(state.CurrentCharacter)) {
                        state.PreviousChar();
                        exp = digitTokenizer.Tokenize(state).ParsedValue;
                    }
                    else {
                        state.Error(Tokenizer.UnexpectedEndOfToken);
                    }
                }

                withExponent = true;
            }

            if (AllowIdents && CurrentCharMatches(state, allIdents)) {
                identTokenizer.Tokenize(state);
                return new Token(TokenKind.Identifier, state);
            }

            if (withDot || withExponent) {
                return new Token(TokenKind.Real, state, LiteralValues.Literals.ConvertRealLiteral(digits, decimals, minus, exp));
            }
            else {
                return new Token(TokenKind.Integer, state, digits);
            }
        }

    }

}