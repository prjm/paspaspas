#nullable disable
using System;
using System.Collections.Generic;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.TokenGroups;

namespace PasPasPas.Parsing.Tokenizer.Patterns {

    /// <summary>
    ///     storage for input patterns, used by the tokenizer
    /// </summary>
    public class InputPatterns {

        /// <summary>
        ///     simple input patterns
        /// </summary>
        private readonly IDictionary<char, InputPattern> simplePatterns =
            new Dictionary<char, InputPattern>();

        /// <summary>
        ///     list of complex input patterns
        /// </summary>
        private readonly IList<InputPatternAndClass> complexPatterns =
            new List<InputPatternAndClass>();

        /// <summary>
        ///     keywords
        /// </summary>
        public IDictionary<string, int> Keywords { get; }

        /// <summary>
        ///     create a new set of input patterns
        /// </summary>
        /// <param name="keywords">supported keywords, if any</param>
        public InputPatterns(IDictionary<string, int> keywords)
            => Keywords = keywords ?? new Dictionary<string, int>();

        /// <summary>
        ///     add a simple pattern based upon one character
        /// </summary>
        /// <param name="prefix">prefix</param>
        /// <param name="tokenValue">resulting token value</param>
        /// <returns></returns>
        public InputPattern AddPattern(char prefix, int tokenValue) {
            var result = new InputPattern(prefix, tokenValue, string.Empty);
            simplePatterns.Add(prefix, result);
            return result;
        }

        /// <summary>
        ///     add a complex pattern
        /// </summary>
        /// <param name="prefix">character prefix</param>
        /// <param name="tokenValue">tokenizer</param>
        /// <returns></returns>
        public InputPattern AddPattern(CharacterClass prefix, PatternContinuation tokenValue) {

            if (prefix == null)
                throw new ArgumentNullException(nameof(prefix));

            if (tokenValue == null)
                throw new ArgumentNullException(nameof(tokenValue));

            var result = new InputPattern(prefix, tokenValue, string.Empty);

            if (!(prefix is SingleCharClass prefixedCharecterClass))
                complexPatterns.Add(new InputPatternAndClass(prefix, result));
            else
                simplePatterns.Add(prefixedCharecterClass.Match, result);

            return result;
        }

        /// <summary>
        ///     add a simple pattern
        /// </summary>
        /// <param name="prefix">pattern prefix</param>
        /// <param name="tokenValue">pattern continuation</param>
        /// <returns>created pattern</returns>
        public InputPattern AddPattern(char prefix, PatternContinuation tokenValue) {

            if (tokenValue == null)
                throw new ArgumentNullException(nameof(tokenValue));

            var result = new InputPattern(new SingleCharClass(prefix), tokenValue, string.Empty);
            simplePatterns.Add(prefix, result);
            return result;
        }

        /// <summary>
        ///     test if a input pattern matches
        /// </summary>
        /// <param name="valueToMatch">char to match</param>
        /// <param name="tokenGroup">pattern</param>
        /// <returns></returns>
        public bool Match(char valueToMatch, out InputPattern tokenGroup) {

            if (simplePatterns.TryGetValue(valueToMatch, out tokenGroup))
                return true;

            for (var i = 0; i < complexPatterns.Count; i++) {
                var pattern = complexPatterns[i];
                if (pattern.CharClass.Matches(valueToMatch)) {
                    tokenGroup = pattern.GroupValue;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     fetch the next token from the input
        /// </summary>
        /// <returns></returns>
        public Token FetchNextToken(TokenizerState state) {
            state.Clear();
            var startValue = state.NextChar(true);
            state.StartPosition = state.CurrentPosition;

            if (Match(startValue, out var tokenGroup)) {
                return FetchTokenByGroup(state, tokenGroup);
            }

            state.Error(MessageNumbers.UnexpectedCharacter);
            return new Token(TokenKind.Invalid, state.GetBufferContent());
        }

        /// <summary>
        ///     fetch a token for this group
        /// </summary>
        /// <returns></returns>
        public static Token FetchTokenByGroup(TokenizerState state, InputPattern tokenGroup) {

            var len = tokenGroup.Length;

            while (state.Length < len && !state.AtEof) {
                state.NextChar(true);
            }

            var tokenKind = tokenGroup.Match(state, out var tokenLength);

            for (var inputIndex = state.Length - 1; inputIndex >= tokenLength; inputIndex--) {
                state.PreviousChar();
            }
            state.Length = tokenLength;

            return tokenKind.Tokenize(state);
        }
    }

}
