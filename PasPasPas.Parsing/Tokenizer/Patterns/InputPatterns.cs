﻿using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Infrastructure.Utils;
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
                ExceptionHelper.ArgumentIsNull(nameof(prefix));

            if (tokenValue == null)
                ExceptionHelper.ArgumentIsNull(nameof(tokenValue));

            var result = new InputPattern(prefix, tokenValue, string.Empty);
            var prefixedCharecterClass = prefix as SingleCharClass;

            if (prefixedCharecterClass == null)
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

            if (Match(startValue, out InputPattern tokenGroup)) {
                return FetchTokenByGroup(state, tokenGroup);
            }

            state.Error(Tokenizer.UnexpectedCharacter);
            return new Token(TokenKind.Invalid, state);
        }

        /// <summary>
        ///     fetch a token for this group
        /// </summary>
        /// <returns></returns>
        public Token FetchTokenByGroup(TokenizerState state, InputPattern tokenGroup) {

            var position = state.CurrentPosition;
            var len = tokenGroup.Length;

            while (state.Length < len && (!state.AtEof)) {
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
