using System;
using System.Collections.Generic;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.Tokenizer.CharClass;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for identifiers
    /// </summary>
    public sealed class IdentifierTokenGroupValue : PatternContinuation {

        private readonly IdentifierCharacterClass identifierCharClass;
        private readonly IDictionary<string, int> knownKeywords;
        private readonly bool allowAmpersands;

        /// <summary>
        ///     create a new tokenizer for identifiers
        /// </summary>
        /// <param name="keywords">defined keywords</param>
        /// <param name="allowAmpersand">allow ampersands</param>
        /// <param name="allowDigits">allow digits</param>
        /// <param name="allowDots">allow dots</param>
        public IdentifierTokenGroupValue(IDictionary<string, int> keywords, bool allowAmpersand = false, bool allowDigits = true, bool allowDots = false) {
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
                state.Error(MessageNumbers.UnexpectedCharacter);
                return new Token(TokenKind.Invalid, state.GetBufferContent());
            }

            while (!state.AtEof) {
                if (!identifierCharClass.Matches(state.LookAhead()))
                    break;
                state.NextChar(true);
            }

            if (hasAmpersand && state.Length < 2)
                state.Error(MessageNumbers.IncompleteIdentifier);

            var value = state.GetBufferContent();

            if (!ignoreKeywords && knownKeywords.TryGetValue(value, out var tokenKind))
                return new Token(tokenKind, state.GetBufferContent());
            else
                return new Token(TokenKind.Identifier, state.GetBufferContent());

        }
    }

}
