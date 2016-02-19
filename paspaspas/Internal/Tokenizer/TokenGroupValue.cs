﻿using System;
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
        ///     complete the parsing
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <returns>completed token</returns>
        public abstract PascalToken WithPrefix(IParserInput input, StringBuilder prefix);
    }

    /// <summary>
    ///     simple token group value: no more characters
    /// </summary>
    public class SimpleTokenGroupValue : TokenGroupValue {


        /// <summary>
        ///     creates a new simple token without suffix
        /// </summary>
        /// <param name="tokenValue"></param>
        public SimpleTokenGroupValue(int tokenValue) {
            TokenId = tokenValue;
        }

        /// <summary>
        ///     token kind
        /// </summary>
        public int TokenId { get; }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix)
            => new PascalToken(TokenId, prefix.ToString());
    }

    /// <summary>
    ///     token group value based on a sequence
    /// </summary>
    public abstract class SequenceGroupTokenValue : TokenGroupValue {

        /// <summary>
        ///     token id
        /// </summary>
        protected abstract int TokenId { get; }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {
            var endSeq = EndSequence;

            while (!input.AtEof) {
                if (prefix.EndsWith(endSeq))
                    return new PascalToken(TokenId, prefix.ToString());
                prefix = prefix.Append(input.NextChar());
            }

            return new PascalToken(TokenId, prefix.ToString());
        }

        /// <summary>
        ///     end sequence
        /// </summary>
        protected abstract string EndSequence { get; }
    }

    /// <summary>
    ///     token group value in curly braces
    /// </summary>
    public abstract class CurlyBracedTokenValue : SequenceGroupTokenValue {

        /// <summary>
        ///     get the end of the sequence
        /// </summary>
        protected override string EndSequence
            => "}";
    }

    /// <summary>
    ///     token group for curly brace comments
    /// </summary>
    public class CurlyBraceCommenTokenValue : CurlyBracedTokenValue {

        /// <summary>
        ///     get the token id
        /// </summary>
        protected override int TokenId
            => PascalToken.Comment;
    }

    /// <summary>
    ///     token group for preprocessor commands
    /// </summary>
    public class PreprocessorTokenValue : CurlyBracedTokenValue {

        /// <summary>
        ///     token kind
        /// </summary>
        protected override int TokenId
            => PascalToken.Preprocessor;
    }

    /// <summary>
    ///     tokenizer based on a character class
    /// </summary>
    public abstract class CharacterClassTokenGroupValue : TokenGroupValue {


        /// <summary>
        ///     token kind
        /// </summary>
        protected abstract int TokenId { get; }

        /// <summary>
        ///     test if a character macthes the given class
        /// </summary>
        /// <param name="c">char to test</param>
        /// <returns></returns>
        protected abstract bool MatchesClass(char c);

        /// <summary>
        ///     read whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {
            if (!input.AtEof) {
                var currentChar = input.NextChar();

                while (!input.AtEof && MatchesClass(currentChar)) {
                    prefix.Append(currentChar);
                    currentChar = input.NextChar();
                }

                if (!MatchesClass(currentChar))
                    input.Putback(currentChar);
                else
                    prefix.Append(currentChar);
            }

            return new PascalToken(TokenId, prefix.ToString());
        }

    }

    /// <summary>
    ///     token group for whitespace
    /// </summary>
    public class WhitespaceTokenGroupValue : CharacterClassTokenGroupValue {

        /// <summary>
        ///     get the token id
        /// </summary>
        protected override int TokenId
            => PascalToken.WhiteSpace;

        /// <summary>
        ///     test if the character is whitespace
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char c)
            => char.IsWhiteSpace(c);
    }

    /// <summary>
    ///     tokenizer for hex numbers
    /// </summary>
    public class HexNumberTokenValue : CharacterClassTokenGroupValue {

        /// <summary>
        ///     token kind: hex number
        /// </summary>
        protected override int TokenId
            => PascalToken.HexNumber;

        /// <summary>
        ///     test if a char matches a hex number
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char c)
            => ('0' <= c) && (c <= '9') ||
               ('a' <= c) && (c <= 'f') ||
               ('A' <= c) && (c <= 'F');
    }

    /// <summary>
    ///     token group value for digits
    /// </summary>
    public class DigitTokenGroupValue : CharacterClassTokenGroupValue {

        /// <summary>
        ///     token id
        /// </summary>
        protected override int TokenId
            => PascalToken.Integer;

        /// <summary>
        ///     matches a digit
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char c)
            => ('0' <= c) && (c <= '9');
    }

    /// <summary>
    ///     token group for numbers
    /// </summary>
    public class NumberTokenGroupValue : TokenGroupValue {

        private DigitTokenGroupValue digitTokenizer
            = new DigitTokenGroupValue();

        private NumberCharacterClass numbers
            = new NumberCharacterClass();

        private SingleCharClass dot
            = new SingleCharClass('.');

        private ExponentCharacterClass exponent
            = new ExponentCharacterClass();

        private PlusMinusCharacterClass plusminus
            = new PlusMinusCharacterClass();

        private bool NextCharMatches(IParserInput input, StringBuilder builder, CharacterClass c) {
            if (input.AtEof)
                return false;

            char n = input.NextChar();
            if (c.Matches(n)) {
                builder.Append(n);
                return true;
            }
            else {
                input.Putback(n);
                return false;
            }
        }

        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {
            var token = digitTokenizer.WithPrefix(input, prefix);
            var withDot = false;
            var withExponent = false;
            if (input.AtEof)
                return token;

            if (NextCharMatches(input, prefix, dot)) {
                if (NextCharMatches(input, prefix, numbers)) {
                    digitTokenizer.WithPrefix(input, prefix);
                    withDot = true;
                }
                else if (prefix.EndsWith(".")) {
                    input.Putback(".");
                    prefix.Length -= 1;
                }
            }

            if (NextCharMatches(input, prefix, exponent)) {
                NextCharMatches(input, prefix, plusminus);
                digitTokenizer.WithPrefix(input, prefix);
                withExponent = true;
            }

            if (withDot || withExponent) {
                return new PascalToken(PascalToken.Real, prefix.ToString());
            }
            else {
                return new PascalToken(PascalToken.Integer, prefix.ToString());
            }

        }
    }

}