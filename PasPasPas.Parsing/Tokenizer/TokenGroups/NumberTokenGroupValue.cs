using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for numbers
    /// </summary>
    public sealed class NumberTokenGroupValue : PatternContinuation {

        private readonly CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.Integer, new DigitCharClass(false), 0, StaticDependency.ParsedIntegers, Guid.Empty);

        private readonly IdentifierCharacterClass allIdents
            = new IdentifierCharacterClass(ampersands: true, digits: true, dots: true);

        private readonly IdentifierTokenGroupValue identTokenizer
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
                var number = LiteralValues.Literals.ParseIntegerLiteral(state.Environment, state.GetBufferContent());
                return new Token(TokenKind.Integer, state, number);
            }

            state.Clear();
            state.PreviousChar();
            var digits = digitTokenizer.Tokenize(state).ParsedValue;
            object decimals = null;
            object exp = null;
            var minus = false;

            if (state.LookAhead(1) == '.') {
                if (digitTokenizer.CharClass.Matches(state.LookAhead(2))) {
                    state.NextChar(true);
                    withDot = true;
                    decimals = digitTokenizer.Tokenize(state).ParsedValue;
                }
            }

            var nextChar = state.LookAhead();

            if (nextChar == 'E' || nextChar == 'e') {
                state.NextChar(true);
                if (state.AtEof) {
                    state.Error(Tokenizer.UnexpectedEndOfToken);
                }
                else {
                    nextChar = state.LookAhead();
                    minus = nextChar == '-';
                    if (nextChar == '-' || nextChar == '+')
                        state.NextChar(true);

                    if (!state.AtEof) {
                        if (digitTokenizer.CharClass.Matches(state.LookAhead())) {
                            exp = digitTokenizer.Tokenize(state).ParsedValue;
                        }
                        else {
                            state.Error(Tokenizer.UnexpectedEndOfToken);
                        }
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
                return new Token(TokenKind.Real, state, LiteralValues.Literals.ConvertRealLiteral(state.Environment, digits, decimals, minus, exp));
            }
            else {
                return new Token(TokenKind.Integer, state, digits);
            }
        }

    }

}