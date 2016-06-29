using PasPasPas.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     group punctuators with the same prefix
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

            for (int index = 0; index < template.Length; index++) {
                var match = template[index];
                var lastChar = index + 1 == template.Length;
                if (!group.Tokens.Value.TryGetValue(match, out group)) {
                    if (lastChar) {
                        group = Add(match, tokenKind);
                    }
                    else {
                        group = Add(match, PascalToken.Undefined);
                    }
                }
                else if (lastChar) {
                    var tokenValue = group.TokenValue as SimpleTokenGroupValue;
                    if (tokenValue == null || tokenValue.TokenId != PascalToken.Undefined)
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
        public InputPattern(CharacterClass prefix, PatternContinuation tokenValue) {
            Prefix = prefix;
            TokenValue = tokenValue;
        }

        /// <summary>
        ///     create a new punctuator group
        /// </summary>
        /// <param name="prefix">prefix</param>
        /// <param name="tokenValue">token value</param>
        public InputPattern(char prefix, int tokenValue) : this(new SingleCharClass(prefix), new SimpleTokenGroupValue(tokenValue)) {
            //..
        }

        /// <summary>
        ///     token group length
        /// </summary>
        public int Length
        {
            get
            {
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
            var result = new InputPattern(nextPunct, token);
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
            var result = new InputPattern(new SingleCharClass(nextPunct), token);
            Tokens.Value.Add(nextPunct, result);
            length = -1;
            return result;
        }

        /// <summary>
        ///     match an input with this punctuator group
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="tokenLength">token length</param>
        /// <returns>matched token value</returns>
        public PatternContinuation Match(StringBuilder input, out int tokenLength) {
            var subgroup = this;
            int index = 1;
            while (index < Length && index < input.Length) {
                var oldSubgroup = subgroup;
                if (subgroup.Tokens.IsValueCreated && subgroup.Tokens.Value.TryGetValue(input[index], out subgroup))
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
