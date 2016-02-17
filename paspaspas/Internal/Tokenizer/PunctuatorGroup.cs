﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="prefix">prefix</param>
        /// <param name="tokenValue">token value</param>
        public PunctuatorGroup(CharacterClass prefix, TokenGroupValue tokenValue) {
            Prefix = prefix;
            TokenValue = tokenValue;
        }

        /// <summary>
        ///     create a new punctuator group
        /// </summary>
        /// <param name="prefix">prefix</param>
        /// <param name="tokenValue">token value</param>
        public PunctuatorGroup(char prefix, int tokenValue) : this(new SingleCharClass(prefix), new SimpleTokenGroupValue(tokenValue)) {
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
        public TokenGroupValue TokenValue { get; }

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
        ///     add a token to this token group
        /// </summary>
        /// <param name="nextPunct">next punctuator part</param>
        /// <param name="token"></param>
        /// <returns>new token</returns>
        public PunctuatorGroup Add(char nextPunct, TokenGroupValue token) {
            var result = new PunctuatorGroup(new SingleCharClass(nextPunct), token);
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
        public TokenGroupValue Match(StringBuilder input, out int tokenLength) {
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
