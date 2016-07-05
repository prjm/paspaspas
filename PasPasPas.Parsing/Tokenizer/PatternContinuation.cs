using System;
using System.Text;
using PasPasPas.Api;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     helper class for a string builder
    /// </summary>
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
    public abstract class PatternContinuation {

        /// <summary>
        ///     generate a token
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <returns>tokent</returns>
        public PascalToken Tokenize(StackedFileReader input, StringBuilder prefix) {
            var result = new PascalToken();
            result.FilePath = input.CurrentInputFile;
            //return WithPrefix(input, prefix, currentFile);
            return result;
        }



        /// <summary>
        ///     complete the parsing
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <returns>completed token</returns>
        protected abstract PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference currentFile);
    }

    /// <summary>
    ///     simple token group value: no more characters
    /// </summary>
    public class SimpleTokenGroupValue : PatternContinuation {


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
        /// <param name="file">file references</param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file)
            => null; // new PascalToken(TokenId, prefix.ToString(), file);
    }

    /// <summary>
    ///     token group value based on a sequence
    /// </summary>
    public abstract class SequenceGroupTokenValue : PatternContinuation {

        /// <summary>
        ///     token id
        /// </summary>
        protected abstract int TokenId { get; }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <param name="file">input file</param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            var endSeq = EndSequence;
            bool switchedInput = false;

            while ((!input.AtEof) && (!switchedInput)) {
                if (prefix.EndsWith(endSeq))
                    return null; // new PascalToken(TokenId, prefix.ToString(), file);
                prefix = prefix.Append(input.FetchChar(out switchedInput));
            }

            return null; // PascalToken(TokenId, prefix.ToString(), file);
        }

        /// <summary>
        ///     end sequence
        /// </summary>
        protected abstract string EndSequence { get; }
    }

    /// <summary>
    ///     token group for quoted strings
    /// </summary>
    public class QuotedStringTokenValue : PatternContinuation {

        /// <summary>
        ///     tokenize a quoted string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <param name="file">input file</param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            bool switchedInput = false;
            while (!input.AtEof) {
                char currentChar = input.FetchChar(out switchedInput);

                if (!input.AtEof && currentChar == '\'' && !switchedInput) {
                    char nextChar = input.FetchChar(out switchedInput);
                    if (nextChar == '\'') {
                        prefix.Append("'");
                    }
                    else {
                        prefix.Append("'");
                        input.PutbackChar(file, nextChar);
                        return null; // new PascalToken(PascalToken.QuotedString, prefix.ToString(), file);
                    }
                }

                prefix.Append(currentChar);
            }

            return null; // new PascalToken(PascalToken.QuotedString, prefix.ToString(), file);
        }

        /// <summary>
        ///     removes quotes from the text
        /// </summary>
        /// <param name="quotedText"></param>
        /// <returns></returns>
        public static string Unwrap(string quotedText) {
            var result = quotedText ?? string.Empty;

            // TODO: be more strict here..

            if (result.StartsWith("'", StringComparison.Ordinal)) {
                result = result.Substring(1, result.Length - 1);
            }

            if (result.EndsWith("'", StringComparison.Ordinal)) {
                result = result.Substring(0, result.Length - 1);
            }

            return result;
        }
    }

    /// <summary>
    ///     token group which moves the input to eof
    /// </summary>
    public class SoftEofTokenValue : PatternContinuation {

        /// <summary>
        ///     read until eof, discard token value
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <param name="file">input file</param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            bool switchedInput = false;
            while (!input.AtEof && !switchedInput)
                input.FetchChar(out switchedInput);
            return null; // new PascalToken(PascalToken.Eof, string.Empty, file);
        }
    }

    /// <summary>
    ///     double-quoted string
    /// </summary>
    public class DoubleQuoteStringGroupTokenValue : PatternContinuation {

        /// <summary>
        ///     tokenize a quoted string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <param name="file">file reference</param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            bool switchedInput = false;
            while (!input.AtEof && !switchedInput) {
                char currentChar = input.FetchChar(out switchedInput);

                if (!input.AtEof && currentChar == '"' && !switchedInput) {
                    char nextChar = input.FetchChar(out switchedInput);
                    if (nextChar == '"') {
                        prefix.Append("\"");
                    }
                    else {
                        prefix.Append("\"");
                        input.PutbackChar(file, nextChar);
                        return null; //  new PascalToken(PascalToken.DoubleQuotedString, prefix.ToString(), file);
                    }
                }

                prefix.Append(currentChar);
            }

            return null; // new PascalToken(PascalToken.DoubleQuotedString, prefix.ToString(), file);
        }
    }

    /// <summary>
    ///     token group for strings
    /// </summary>
    public class StringGroupTokenValue : PatternContinuation {

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
        /// <param name="file">file reference</param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            input.PutbackChar(file, prefix[0]);
            prefix.Length = 0;
            bool switchedInput = false;

            while (!input.AtEof) {
                char currentChar = input.FetchChar(out switchedInput);
                if (currentChar == '#') {
                    char nextChar = input.FetchChar(out switchedInput);
                    prefix.Append(currentChar);
                    if (nextChar == '$') {
                        prefix.Append(nextChar);
                        hexDigits.Tokenize(input, prefix);
                    }
                    else {
                        input.PutbackChar(file, nextChar);
                        digits.Tokenize(input, prefix);
                    }
                }
                else if (currentChar == '\'') {
                    prefix.Append(currentChar);
                    quotedString.Tokenize(input, prefix);
                }
                else {
                    input.PutbackChar(file, currentChar);
                    break;
                }
            }
            return null; // new PascalToken(PascalToken.QuotedString, prefix.ToString(), file);
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
    public abstract class CharacterClassTokenGroupValue : PatternContinuation {


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
        /// <param name="file">input file</param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            bool switchedInput = false;
            if (!input.AtEof && !switchedInput) {
                var currentChar = input.FetchChar(out switchedInput);

                while (!input.AtEof && MatchesClass(currentChar) && !switchedInput) {
                    prefix.Append(currentChar);
                    currentChar = input.FetchChar(out switchedInput);
                }

                if (!MatchesClass(currentChar))
                    input.PutbackChar(file, currentChar);
                else
                    prefix.Append(currentChar);
            }

            return null; // new PascalToken(TokenId, prefix.ToString(), file);
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
    public class IdentifierTokenGroupValue : PatternContinuation {

        private IdentifierCharacterClass identifierCharClass
            = new IdentifierCharacterClass() { AllowAmpersand = false, AllowDigits = true };

        /// <summary>
        ///     allow dots in identifiers
        /// </summary>
        public bool AllowDots { get { return identifierCharClass.AllowDots; } set { identifierCharClass.AllowDots = value; } }

        private readonly IDictionary<string, int> knownKeywords;

        /// <summary>
        ///     create a new token group for ids and keywords
        /// </summary>
        /// <param name="keywords"></param>
        public IdentifierTokenGroupValue(IDictionary<string, int> keywords) {
            knownKeywords = keywords;
        }

        /// <summary>
        ///     tokenize an identifier
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            bool ignoreKeywords = prefix[0] == '&';
            bool switchedInput = false;

            if (ignoreKeywords)
                prefix.Clear();

            while (!input.AtEof && !switchedInput) {
                var currentChar = input.FetchChar(out switchedInput);
                if (!identifierCharClass.Matches(currentChar)) {
                    input.PutbackChar(file, currentChar);
                    break;
                }
                prefix.Append(currentChar);
            }

            string value = prefix.ToString();
            int tokenKind;

            if ((!ignoreKeywords) && (knownKeywords.TryGetValue(value, out tokenKind)))
                return null; // new PascalToken(tokenKind, value, file);
            else
                return null; // new PascalToken(PascalToken.Identifier, value, file);

        }
    }

    /// <summary>
    ///     token group for end of line comments
    /// </summary>
    public class EondOfLineCommentTokenGroupValue : PatternContinuation {

        /// <summary>
        ///     tokenize end-of-line comments
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <param name="file">file</param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            bool switchedInput = false;

            while (!input.AtEof && !switchedInput) {
                char currentChar = input.FetchChar(out switchedInput);
                prefix.Append(currentChar);

                if (currentChar == 0x0D) {
                    if (!input.AtEof && !switchedInput) {
                        char nextChar = input.FetchChar(out switchedInput);
                        if (nextChar == 0x0A) {
                            prefix.Append(nextChar);
                        }
                        else {
                            input.PutbackChar(file, nextChar);
                        }
                    }
                    break;
                }

                if (currentChar == 0x0A) {
                    break;
                }
            }
            return null; // PascalToken(PascalToken.Comment, prefix.ToString(), file);
        }
    }

    /// <summary>
    ///     token group for numbers
    /// </summary>
    public class NumberTokenGroupValue : PatternContinuation {

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

        private static bool NextCharMatches(StackedFileReader input, StringBuilder builder, CharacterClass c) {
            if (input.AtEof)
                return false;

            var file = input.CurrentInputFile;
            bool switchedInput = false;
            char n = input.FetchChar(out switchedInput);
            if (c.Matches(n)) {
                builder.Append(n);
                return true;
            }
            else {
                input.PutbackChar(file, n);
                return false;
            }
        }

        /// <summary>
        ///     tokenizer a number
        /// </summary>
        /// <param name="input"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        protected override PascalToken WithPrefix(StackedFileReader input, StringBuilder prefix, IFileReference file) {
            var token = digitTokenizer.Tokenize(input, prefix);
            var withDot = false;
            var withExponent = false;
            if (input.AtEof)
                return token;

            if (NextCharMatches(input, prefix, dot)) {
                if (NextCharMatches(input, prefix, numbers)) {
                    digitTokenizer.Tokenize(input, prefix);
                    withDot = true;
                }
                else if (prefix.EndsWith(".")) {
                    input.PutbackString(file, ".");
                    prefix.Length -= 1;
                }
            }

            if (NextCharMatches(input, prefix, exponent)) {
                NextCharMatches(input, prefix, plusminus);
                digitTokenizer.Tokenize(input, prefix);
                withExponent = true;
            }

            if (withDot || withExponent) {
                return null; // new PascalToken(PascalToken.Real, prefix.ToString(), file);
            }
            else {
                return null; // new PascalToken(PascalToken.Integer, prefix.ToString(), file);
            }

        }



    }

}