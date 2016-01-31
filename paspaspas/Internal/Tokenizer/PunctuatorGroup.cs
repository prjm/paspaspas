using System;
using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     group punctuators with the same prefix
    /// </summary>
    public class PunctuatorGroup {

        /// <summary>
        ///     length (cache)
        /// </summary>
        private int length;

        /// <summary>
        ///     prefix
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="tokenValue">token value</param>
        public PunctuatorGroup(char prefix, int tokenValue) {
            Prefix = prefix;
            TokenValue = tokenValue;
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
        public char Prefix { get; }

        /// <summary>
        ///     token value
        /// </summary>
        public int TokenValue { get; }

        /// <summary>
        ///     tokens
        /// </summary>
        private Lazy<IDictionary<char, PunctuatorGroup>> Tokens { get; }
            = new Lazy<IDictionary<char, PunctuatorGroup>>(() => new Dictionary<char, PunctuatorGroup>());

        /// <summary>
        ///     add a token to this token group
        /// </summary>
        /// <param name="nextPunct">next punctuator part</param>
        /// <param name="token"></param>
        /// <returns>new token</returns>
        public PunctuatorGroup Add(char nextPunct, int token) {
            var result = new PunctuatorGroup(nextPunct, token);
            Tokens.Value.Add(nextPunct, result);
            length = -1;
            return result;
        }

        /// <summary>
        ///     match an input with this punctuator group
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="tokenValue">token value</param>
        /// <returns>matched token value</returns>
        public int Match(string input, out string tokenValue) {
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

            tokenValue = input.Substring(0, index);
            return subgroup.TokenValue;
        }
    }
}
