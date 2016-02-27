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
        public int TokenId { get; set; }

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
    ///     token group for quoted strings
    /// </summary>
    public class QuotedStringTokenValue : TokenGroupValue {

        /// <summary>
        ///     tokenize a quoted string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {
            while (!input.AtEof) {
                char currentChar = input.NextChar();

                if (!input.AtEof && currentChar == '\'') {
                    char nextChar = input.NextChar();
                    if (nextChar == '\'') {
                        prefix.Append("'");
                    }
                    else {
                        prefix.Append("'");
                        input.Putback(nextChar);
                        return new PascalToken(PascalToken.QuotedString, prefix.ToString());
                    }
                }

                prefix.Append(currentChar);
            }

            return new PascalToken(PascalToken.QuotedString, prefix.ToString());
        }
    }

    /// <summary>
    ///     token group which moves the input to eof
    /// </summary>
    public class SoftEofTokenValue : TokenGroupValue {

        /// <summary>
        ///     read until eof, discard token value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {
            while (!input.AtEof)
                input.NextChar();
            return new PascalToken(PascalToken.Eof, string.Empty);
        }
    }

    /// <summary>
    ///     double-quoted string
    /// </summary>
    public class DoubleQuoteStringGroupTokenValue : TokenGroupValue {

        /// <summary>
        ///     tokenize a quoted string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {
            while (!input.AtEof) {
                char currentChar = input.NextChar();

                if (!input.AtEof && currentChar == '"') {
                    char nextChar = input.NextChar();
                    if (nextChar == '"') {
                        prefix.Append("\"");
                    }
                    else {
                        prefix.Append("\"");
                        input.Putback(nextChar);
                        return new PascalToken(PascalToken.DoubleQuotedString, prefix.ToString());
                    }
                }

                prefix.Append(currentChar);
            }

            return new PascalToken(PascalToken.DoubleQuotedString, prefix.ToString());
        }
    }

    /// <summary>
    ///     token group for strings
    /// </summary>
    public class StringGroupTokenValue : TokenGroupValue {

        private QuotedStringTokenValue quotedString
            = new QuotedStringTokenValue();

        private DigitTokenGroupValue digits
            = new DigitTokenGroupValue();

        private HexNumberTokenValue hexDigits
            = new HexNumberTokenValue();

        /// <summary>
        ///     parse a string literal
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {
            input.Putback(prefix[0]);
            prefix.Length = 0;

            while (!input.AtEof) {
                char currentChar = input.NextChar();
                if (currentChar == '#') {
                    char nextChar = input.NextChar();
                    prefix.Append(currentChar);
                    if (nextChar == '$') {
                        prefix.Append(nextChar);
                        hexDigits.WithPrefix(input, prefix);
                    }
                    else {
                        input.Putback(nextChar);
                        digits.WithPrefix(input, prefix);
                    }
                }
                else if (currentChar == '\'') {
                    prefix.Append(currentChar);
                    quotedString.WithPrefix(input, prefix);
                }
                else {
                    input.Putback(currentChar);
                    break;
                }
            }
            return new PascalToken(PascalToken.QuotedString, prefix.ToString());
        }
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
    ///     token group value in curly braces
    /// </summary>
    public abstract class AlternativeCurlyBracedTokenValue : SequenceGroupTokenValue {

        /// <summary>
        ///     get the end of the sequence
        /// </summary>
        protected override string EndSequence
            => "*)";
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
    ///     token group for curly brace comments
    /// </summary>
    public class AlternativeCurlyBraceCommenTokenValue : AlternativeCurlyBracedTokenValue {

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
    ///     token group for preprocessor commands
    /// </summary>
    public class AlternativePreprocessorTokenValue : AlternativeCurlyBracedTokenValue {

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
        /// <param name="input">char to test</param>
        /// <returns></returns>
        protected abstract bool MatchesClass(char input);

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
    ///     token group value for control characters
    /// </summary>
    public class ControlTokenGroupValue : CharacterClassTokenGroupValue {

        /// <summary>
        ///     token id: ControlChar
        /// </summary>
        protected override int TokenId
            => PascalToken.ControlChar;


        /// <summary>
        ///     test if the charcater is a control char
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => (!char.IsWhiteSpace(input)) && (char.IsControl(input));

    }

    /// <summary>
    ///     token group for whitespace
    /// </summary>
    public class WhiteSpaceTokenGroupValue : CharacterClassTokenGroupValue {

        /// <summary>
        ///     get the token id
        /// </summary>
        protected override int TokenId
            => PascalToken.WhiteSpace;

        /// <summary>
        ///     test if the character is whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => char.IsWhiteSpace(input);
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
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => ('0' <= input) && (input <= '9') ||
               ('a' <= input) && (input <= 'f') ||
               ('A' <= input) && (input <= 'F');
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
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => ('0' <= input) && (input <= '9');
    }

    /// <summary>
    ///     token group for identifiers
    /// </summary>
    public class IdentifierTokenGroupValue : TokenGroupValue {

        private IdentifierCharacterClass identifierCharClass
            = new IdentifierCharacterClass() { AllowAmpersand = false, AllowDigits = true };

        /// <summary>
        ///     tokenize an identifier
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {
            bool ignoreKeywords = prefix[0] == '&';
            if (ignoreKeywords)
                prefix.Clear();

            while (!input.AtEof) {
                var currentChar = input.NextChar();
                if (!identifierCharClass.Matches(currentChar)) {
                    input.Putback(currentChar);
                    break;
                }
                prefix.Append(currentChar);
            }

            string value = prefix.ToString();
            int tokenKind;

            if ((!ignoreKeywords) && (StandardTokenizer.TryGetKeyword(value, out tokenKind)))
                return new PascalToken() { Value = value, Kind = tokenKind };
            else
                return new PascalToken() { Value = value, Kind = PascalToken.Identifier };

        }
    }

    /// <summary>
    ///     token group for end of line comments
    /// </summary>
    public class EondOfLineCommentTokenGroupValue : TokenGroupValue {

        /// <summary>
        ///     tokenize end-of-line comments
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public override PascalToken WithPrefix(IParserInput input, StringBuilder prefix) {

            while (!input.AtEof) {
                char currentChar = input.NextChar();
                prefix.Append(currentChar);

                if (currentChar == 0x0D) {
                    if (!input.AtEof) {
                        char nextChar = input.NextChar();
                        if (nextChar == 0x0A) {
                            prefix.Append(nextChar);
                        }
                        else {
                            input.Putback(nextChar);
                        }
                    }
                    break;
                }

                if (currentChar == 0x0A) {
                    break;
                }
            }
            return new PascalToken(PascalToken.Comment, prefix.ToString());
        }
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

        /// <summary>
        ///     tokenizer a number
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
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