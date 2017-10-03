using PasPasPas.Parsing.SyntaxTree;
using System;
using System.Collections.Generic;
using System.Linq;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.TokenGroups;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Parsing.Tokenizer.Patterns {

    /// <summary>
    ///     group input patterns with a common prefix
    /// </summary>
    public class InputPattern {

        /// <summary>
        ///     length (cache)
        /// </summary>
        private int length;

        /// <summary>
        ///     add a string as single prefixes
        /// </summary>
        /// <param name="template"></param>
        /// <param name="tokenKind"></param>
        public void Add(string template, int tokenKind) {
            var group = this;

            if (string.IsNullOrEmpty(template))
                new ArgumentNullException(nameof(template));

            for (var index = 0; index < template.Length; index++) {
                var match = template[index];
                var lastChar = index + 1 == template.Length;
                if (!group.Tokens.Value.TryGetValue(match, out group)) {
                    if (lastChar) {
                        group = Add(match, tokenKind);
                    }
                    else {
                        group = Add(match, TokenKind.Undefined);
                    }
                }
                else if (lastChar) {
                    var tokenValue = group.TokenValue as SimpleTokenGroupValue;
                    if (tokenValue == null || tokenValue.TokenId != TokenKind.Undefined)
                        throw new InvalidOperationException();
                    else
                        tokenValue.TokenId = tokenKind;
                }
            }
        }

        /// <summary>
        ///     prefix
        /// </summary>
        /// <param name="prefix">prefix</param>
        /// <param name="tokenValue">token value</param>
        public InputPattern(CharacterClass prefix, PatternContinuation tokenValue, string completePrefix) {
            Prefix = prefix ?? throw new ArgumentNullException(nameof(prefix));
            TokenValue = tokenValue ?? throw new ArgumentNullException(nameof(tokenValue));
            CompletePrefix = StringPool.PoolString(completePrefix);
        }

        /// <summary>
        ///     create a new punctuator group
        /// </summary>
        /// <param name="prefix">prefix</param>
        /// <param name="tokenValue">token value</param>
        public InputPattern(char prefix, int tokenValue, string completePrefix)
            : this(new SingleCharClass(prefix), new SimpleTokenGroupValue(tokenValue, completePrefix), completePrefix) {
            //..
        }

        /// <summary>
        ///     token group length
        /// </summary>
        public int Length {
            get {
                if (length > 0)
                    return length;

                if (!Tokens.IsValueCreated)
                    return 1;

                length = 1 + Tokens.Value.Values.Select(t => t.Length).Max();
                return length;
            }
        }

        /// <summary>
        ///     prefix
        /// </summary>
        public CharacterClass Prefix { get; }

        /// <summary>
        ///     token value
        /// </summary>
        public PatternContinuation TokenValue { get; }

        /// <summary>
        ///     complete prefix
        /// </summary>
        public string CompletePrefix { get; }

        /// <summary>
        ///     tokens
        /// </summary>
        private Lazy<IDictionary<char, InputPattern>> Tokens { get; }
            = new Lazy<IDictionary<char, InputPattern>>(() => new Dictionary<char, InputPattern>());

        /// <summary>
        ///     add a token to this token group
        /// </summary>
        /// <param name="nextPunct">next punctuator part</param>
        /// <param name="token"></param>
        /// <returns>new token</returns>
        public InputPattern Add(char nextPunct, int token) {
            var result = new InputPattern(nextPunct, token, CompletePrefix + nextPunct);
            Tokens.Value.Add(nextPunct, result);
            length = -1;
            return result;
        }

        /// <summary>
        ///     add a token to this token group
        /// </summary>
        /// <param name="nextPunct">next punctuator part</param>
        /// <param name="token"></param>
        /// <returns>new token</returns>
        public InputPattern Add(char nextPunct, PatternContinuation token) {

            if (token == null)
                throw new ArgumentNullException(nameof(token));

            var result = new InputPattern(new SingleCharClass(nextPunct), token, CompletePrefix + nextPunct);
            Tokens.Value.Add(nextPunct, result);
            length = -1;
            return result;
        }

        /// <summary>
        ///     match an input with this pattern group
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="tokenLength">token length</param>
        /// <returns>matched token value</returns>
        public PatternContinuation Match(TokenizerState state, out int tokenLength) {
            var subgroup = this;
            var index = 1;

            while (index < Length && index < state.Length) {
                var oldSubgroup = subgroup;
                if (subgroup.Tokens.IsValueCreated && subgroup.Tokens.Value.TryGetValue(state.GetBufferCharAt(index), out subgroup))
                    index++;
                else {
                    subgroup = oldSubgroup;
                    break;
                }
            }

            tokenLength = index;
            return subgroup.TokenValue;
        }
    }

}
