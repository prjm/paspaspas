﻿using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for identifiers
    /// </summary>
    public sealed class IdentifierTokenGroupValue : PatternContinuation {

        private readonly IdentifierCharacterClass identifierCharClass;
        private readonly IDictionary<string, int> knownKeywords;
        private readonly bool allowAmpersands;

        public IdentifierTokenGroupValue(IDictionary<string, int> keywords, bool allowAmpersand = false, bool allowDigits = false, bool allowDots = false) {
            allowAmpersands = allowAmpersand;
            knownKeywords = keywords ?? throw new ArgumentNullException(nameof(keywords));
            identifierCharClass = new IdentifierCharacterClass(allowAmpersand, allowDigits, allowDots);
        }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(TokenizerState state) {
            var hasAmpersand = state.GetBufferCharAt(0) == '&';
            var ignoreKeywords = allowAmpersands && hasAmpersand;

            if (!allowAmpersands && hasAmpersand) {
                state.Error(Tokenizer.UnexpectedCharacter);
                return new Token(TokenKind.Invalid, state);
            }

            while (!state.AtEof) {
                if (!identifierCharClass.Matches(state.LookAhead()))
                    break;
                state.NextChar(true);
            }

            if (hasAmpersand && state.Length < 2)
                state.Error(Tokenizer.IncompleteIdentifier);

            var value = state.GetBufferContent();

            if ((!ignoreKeywords) && (knownKeywords.TryGetValue(value, out var tokenKind)))
                return new Token(tokenKind, state);
            else
                return new Token(TokenKind.Identifier, state);

        }
    }

}
