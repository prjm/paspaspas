﻿using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     token group for numbers
    /// </summary>
    public sealed class NumberTokenGroupValue : PatternContinuation {

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

        public static readonly Guid ParsedIntegers =
            new Guid(new byte[] { 0x85, 0x19, 0x39, 0xd3, 0x70, 0xa7, 0x98, 0x47, 0xa1, 0x83, 0x8b, 0xdf, 0xd5, 0x48, 0x2a, 0x90 });
        /* {d3391985-a770-4798-a183-8bdfd5482a90} */

        public static readonly Guid ParsedHexNumbers =
            new Guid(new byte[] { 0x5b, 0x5, 0x2b, 0xc4, 0xa0, 0xd2, 0x34, 0x46, 0x9d, 0x85, 0x67, 0xc, 0x9f, 0x60, 0xfc, 0x22 });
        /* {c42b055b-d2a0-4634-9d85-670c9f60fc22} */

        private readonly IIntegerParser parser
            = StaticEnvironment.Require<IIntegerParser>(ParsedIntegers);


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
            if (state.AtEof)
                return false;

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
                var value = state.GetBufferContent().Pool();
                return new Token(TokenKind.Integer, state.CurrentPosition, value, parser.ParseInt(value));
            }

        }

    }

}