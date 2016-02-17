using System;
using System.Text;
using PasPasPas.Api.Input;
using PasPasPas.Api;

namespace PasPasPas.Internal.Tokenizer {

    static class StringBuilderHelper {

        /// <summary>
        ///     test if a string builder ends with a given string
        /// </summary>
        /// <param name="stringBuilder">string builder to look at</param>
        /// <param name="test">search string</param>
        /// <returns><c>true</c> if the string builder ends with that string</returns>
        public static bool EndsWith(this StringBuilder stringBuilder, string test) {
            if (stringBuilder.Length < test.Length)
                return false;

            string end = stringBuilder.ToString(stringBuilder.Length - test.Length, test.Length);
            return end.Equals(test, StringComparison.Ordinal);
        }
    }

    /// <summary>
    ///     token group value
    /// </summary>
    public abstract class TokenGroupValue {

        /// <summary>
        ///     creates a new token value 
        /// </summary>
        /// <param name="tokenValue">token id to use</param>
        protected TokenGroupValue(int tokenValue) {
            Token = tokenValue;
        }

        /// <summary>
        ///     token id
        /// </summary>
        public int Token { get; }

        /// <summary>
        ///     complete the parsing
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <returns></returns>
        public abstract string WithPrefix(IParserInput input, StringBuilder prefix);
    }

    /// <summary>
    ///     simple token group value: no more characters
    /// </summary>
    public class SimpleTokenGroupValue : TokenGroupValue {


        /// <summary>
        ///     creates a new simple token without suffix
        /// </summary>
        /// <param name="tokenValue"></param>
        public SimpleTokenGroupValue(int tokenValue) : base(tokenValue) { }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <returns></returns>
        public override string WithPrefix(IParserInput input, StringBuilder prefix)
            => prefix.ToString();
    }

    /// <summary>
    ///     token group value based on a sequence
    /// </summary>
    public abstract class SequenceGroupTokenValue : TokenGroupValue {

        /// <summary>
        ///     creates a new token value based on a sequence
        /// </summary>
        /// <param name="tokenValue"></param>
        protected SequenceGroupTokenValue(int tokenValue) : base(tokenValue) { }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <returns></returns>
        public override string WithPrefix(IParserInput input, StringBuilder prefix) {
            var endSeq = EndSequence;

            while (!input.AtEof) {
                if (prefix.EndsWith(endSeq))
                    return prefix.ToString();
                prefix = prefix.Append(input.NextChar());
            }

            return prefix.ToString();
        }

        /// <summary>
        ///     end sequence
        /// </summary>
        protected abstract string EndSequence { get; }
    }

    /// <summary>
    ///     token group value in curly braces
    /// </summary>
    public class CurlyBracedTokenValue : SequenceGroupTokenValue {

        /// <summary>
        ///     creates a new token group for tokensin curly braces
        /// </summary>
        /// <param name="tokenValue"></param>
        public CurlyBracedTokenValue(int tokenValue) : base(tokenValue) { }

        /// <summary>
        ///     get the end of the sequence
        /// </summary>
        protected override string EndSequence
            => "}";
    }

    /// <summary>
    ///     token group for whitespace
    /// </summary>
    public class WhitespaceTokenGroupValue : TokenGroupValue {

        /// <summary>
        ///     create a new tokengroup for whitespace
        /// </summary>
        public WhitespaceTokenGroupValue() : base(PascalToken.WhiteSpace) { }

        /// <summary>
        ///     read whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override string WithPrefix(IParserInput input, StringBuilder prefix) {
            var currentChar = input.NextChar();
            while (!input.AtEof && char.IsWhiteSpace(currentChar)) {
                prefix.Append(input);
                currentChar = input.NextChar();
            }

            if (!input.AtEof)
                input.Putback(currentChar);

            return prefix.ToString();
        }
    }

}