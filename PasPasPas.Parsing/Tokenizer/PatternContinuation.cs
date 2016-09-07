using System;
using System.Text;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;

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
    ///     encapsulation of current continuation state
    /// </summary>
    public class ContinuationState {

        private bool switchedFile = false;

        /// <summary>
        ///     input
        /// </summary>
        public StackedFileReader Input { get; set; }

        /// <summary>
        ///     parsed token
        /// </summary>
        public PascalToken Result { get; set; }

        /// <summary>
        ///     input buffer
        /// </summary>
        public StringBuilder Buffer { get; set; }

        /// <summary>
        ///     test if reading from the buffer is invalid
        /// </summary>
        public bool IsValid
            => (!Input.AtEof) && (!switchedFile);

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource Log { get; set; }

        /// <summary>
        ///     current input length
        /// </summary>
        public int Length
             => Buffer.Length;

        /// <summary>
        ///     tests if the buffer ends with a given string value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool EndsWith(string value)
            => Buffer.EndsWith(value);

        /// <summary>
        ///     finish tokenizing
        /// </summary>
        /// <param name="tokenId">generated token id</param>
        public void Finish(int tokenId) {
            Result.Kind = tokenId;
            Result.Value = Buffer.ToString();
        }

        /// <summary>
        ///     append a character
        /// </summary>
        public char FetchAndAppendChar() {
            char result = Input.FetchChar(out switchedFile);
            Buffer.Append(result);
            return result;
        }

        /// <summary>
        ///     fetch a single character without appending it
        /// </summary>
        /// <returns></returns>
        public char FetchChar()
            => Input.FetchChar(out switchedFile);

        /// <summary>
        ///     putback a character
        /// </summary>
        /// <param name="nextChar">character to putback</param>
        public void Putback(char nextChar) {
            Input.PutbackChar(Result.FilePath, nextChar);
        }

        /// <summary>
        ///     append a character
        /// </summary>
        /// <param name="currentChar"></param>
        internal void AppendChar(char currentChar) {
            Buffer.Append(currentChar);
        }

        internal void PutbackBuffer() {
            Input.PutbackStringBuffer(Result.FilePath, Buffer);
            Buffer.Clear();
        }

        internal void Finish(int tokenKind, string value) {
            Result.Kind = tokenKind;
            Result.Value = value;
        }

        internal void Error(Guid messageId, params object[] messageData) {
            Log.ProcessMessage(new LogMessage(MessageSeverity.Error, TokenizerBase.TokenizerLogMessage, messageId, messageData));
        }
    }

    /// <summary>
    ///     token group value
    /// </summary>
    public abstract class PatternContinuation {

        /// <summary>
        ///     create a new result token
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>created token</returns>
        protected virtual PascalToken CreateResult(IFileReference path)
            => new PascalToken() { FilePath = path };

        /// <summary>
        ///     generate a token
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="prefix">prefix</param>
        /// <param name="log">log source</param>
        /// <returns>tokent</returns>
        public PascalToken Tokenize(StackedFileReader input, StringBuilder prefix, ILogSource log) {
            var state = new ContinuationState() {
                Result = CreateResult(input.CurrentInputFile),
                Input = input,
                Buffer = prefix,
                Log = log
            };

            ParseByPrefix(state);
            return state.Result;
        }


        /// <summary>
        ///     complete the parsing of an input pattern with a given prefix
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public abstract void ParseByPrefix(ContinuationState state);

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
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {
            state.Finish(TokenId);
        }
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
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {
            var found = false;

            while (state.IsValid && (!found)) {
                found = state.EndsWith(EndSequence);

                if (!found)
                    state.FetchAndAppendChar();
            }

            found = state.EndsWith(EndSequence);

            if (!found)
                state.Error(TokenizerBase.UnexpectedEndOfToken, EndSequence);

            state.Finish(TokenId);
        }

        /// <summary>
        ///     end sequence
        /// </summary>
        protected abstract string EndSequence { get; }
    }

    /// <summary>
    ///     base class for string literal based token values
    /// </summary>
    public abstract class StringBasedTokenValue : PatternContinuation {

        private StringBuilder parsedString = new StringBuilder();

        /// <summary>
        ///     create a literal result token
        /// </summary>
        /// <param name="path">token path</param>
        /// <returns></returns>
        protected override PascalToken CreateResult(IFileReference path) {
            ResetParsedString();
            return new StringLiteralToken() {
                FilePath = path
            };
        }

        /// <summary>
        ///     reset the parsed string
        /// </summary>
        protected void ResetParsedString() {
            parsedString.Clear();
        }

        /// <summary>
        ///     append char to buffer
        /// </summary>
        /// <param name="charToAppend"></param>
        protected void AppendToBuffer(char charToAppend) {
            parsedString.Append(charToAppend);
        }

        /// <summary>
        ///     append string to buffer
        /// </summary>
        /// <param name="stringToAppend"></param>
        protected void AppendToBuffer(string stringToAppend) {
            parsedString.Append(stringToAppend);
        }

        /// <summary>
        ///     finish result
        /// </summary>
        /// <param name="state"></param>
        /// <param name="tokenKind"></param>
        protected void FinishResult(ContinuationState state, int tokenKind) {
            var literal = ((StringLiteralToken)state.Result);


            if (literal.LiteralValue == null)
                literal.LiteralValue = parsedString.ToString();
            else
                literal.LiteralValue = literal.LiteralValue + parsedString.ToString();

            parsedString.Clear();
            state.Finish(tokenKind);
        }

    }

    /// <summary>
    ///     token group for quoted strings
    /// </summary>
    public class QuotedStringTokenValue : StringBasedTokenValue {

        /// <summary>
        ///     parse a quoted string
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {
            var found = false;

            while (state.IsValid && (!found)) {
                var currentChar = state.FetchChar();

                if (state.IsValid && currentChar == '\'') {
                    var nextChar = state.FetchChar();
                    found = nextChar != '\'';
                    if (found)
                        state.Putback(nextChar);
                    else
                        AppendToBuffer(nextChar);
                }
                else if (currentChar == '\'') {
                    found = true;
                }
                else {
                    AppendToBuffer(currentChar);
                }

                state.AppendChar(currentChar);
            }

            if (!found) {
                state.Error(TokenizerBase.UnexpectedEndOfToken, "'");
            }

            FinishResult(state, TokenKind.QuotedString);
        }

        /// <summary>
        ///     removes quotes from the text
        /// </summary>
        /// <param name="quotedText"></param>
        /// <returns></returns>
        public static string Unwrap(PascalToken quotedText) {
            var literal = quotedText as StringLiteralToken;
            return literal != null ? literal.LiteralValue : string.Empty;
        }
    }

    /// <summary>
    ///     token group which moves the input to eof
    /// </summary>
    public class SoftEofTokenValue : PatternContinuation {

        /// <summary>    
        ///     read the file until eof in one token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {

            while (state.IsValid)
                state.FetchChar();

            state.Finish(TokenKind.Eof);
        }
    }

    /// <summary>
    ///     double-quoted string
    /// </summary>
    public class DoubleQuoteStringGroupTokenValue : StringBasedTokenValue {

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {
            var found = false;

            while (state.IsValid && (!found)) {
                char currentChar = state.FetchChar();

                if (currentChar == '"' && state.IsValid) {
                    char nextChar = state.FetchChar();
                    found = nextChar != '"';

                    if (found)
                        state.Putback(nextChar);
                    else
                        AppendToBuffer(nextChar);
                }
                else if (currentChar == '"') {
                    found = true;
                }
                else {
                    AppendToBuffer(currentChar);
                }

                state.AppendChar(currentChar);
            }

            if (!found) {
                state.Error(TokenizerBase.UnexpectedEndOfToken, "\"");
            }

            FinishResult(state, TokenKind.DoubleQuotedString);
        }
    }

    /// <summary>
    ///     token group for strings
    /// </summary>
    public class StringGroupTokenValue : StringBasedTokenValue {

        private QuotedStringTokenValue quotedString
            = new QuotedStringTokenValue();

        private DigitTokenGroupValue digits
            = new DigitTokenGroupValue();

        private HexNumberTokenValue hexDigits
            = new HexNumberTokenValue();

        private StringBuilder controlBuffer
            = new StringBuilder();

        /// <summary>
        ///     parse a string literal
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {
            state.PutbackBuffer();

            while (state.IsValid) {
                char currentChar = state.FetchChar();
                if (currentChar == '#' && !state.IsValid) {
                    state.Error(TokenizerBase.UnexpectedEndOfToken);
                }
                else if (currentChar == '#') {
                    char nextChar = state.FetchChar();
                    state.AppendChar(currentChar);
                    if (nextChar == '$' && !state.IsValid) {
                        state.Error(TokenizerBase.UnexpectedEndOfToken);
                    }
                    else if (nextChar == '$') {
                        controlBuffer.Clear();
                        controlBuffer.Append('$');
                        var controlChar = hexDigits.Tokenize(state.Input, controlBuffer, state.Log);
                        if (controlChar.Kind != TokenKind.HexNumber) {
                            state.Error(TokenizerBase.UnexpectedCharacter);
                        }
                        else {
                            state.Buffer.Append(controlBuffer);
                            AppendToBuffer((char)HexNumberTokenValue.Unwrap(controlChar));
                        }
                    }
                    else {
                        state.Putback(nextChar);
                        controlBuffer.Clear();
                        var controlChar = digits.Tokenize(state.Input, controlBuffer, state.Log);
                        if (controlChar.Kind != TokenKind.Integer) {
                            state.Error(TokenizerBase.UnexpectedCharacter);
                        }
                        else {
                            state.Buffer.Append(controlBuffer);
                            AppendToBuffer((char)DigitTokenGroupValue.Unwrap(controlChar));
                        }
                    }
                }
                else if (currentChar == '\'') {
                    state.AppendChar(currentChar);
                    AppendToBuffer(QuotedStringTokenValue.Unwrap(quotedString.Tokenize(state.Input, controlBuffer, state.Log)));
                }
                else {
                    state.Putback(currentChar);
                    break;
                }
            }
            FinishResult(state, TokenKind.QuotedString);
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
    public class CurlyBraceCommentTokenValue : CurlyBracedTokenValue {

        /// <summary>
        ///     get the token id
        /// </summary>
        protected override int TokenId
            => TokenKind.Comment;
    }

    /// <summary>
    ///     token group for curly brace comments
    /// </summary>
    public class AlternativeCurlyBraceCommentTokenValue : AlternativeCurlyBracedTokenValue {

        /// <summary>
        ///     get the token id
        /// </summary>
        protected override int TokenId
            => TokenKind.Comment;
    }

    /// <summary>
    ///     token group for preprocessor commands
    /// </summary>
    public class PreprocessorTokenValue : CurlyBracedTokenValue {

        /// <summary>
        ///     token kind
        /// </summary>
        protected override int TokenId
            => TokenKind.Preprocessor;
    }

    /// <summary>
    ///     token group for preprocessor commands
    /// </summary>
    public class AlternativePreprocessorTokenValue : AlternativeCurlyBracedTokenValue {

        /// <summary>
        ///     token kind
        /// </summary>
        protected override int TokenId
            => TokenKind.Preprocessor;
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
        ///     minimal length
        /// </summary>
        protected virtual int MinLength { get; } = 0;

        /// <summary>
        ///     test if a character matches the given class
        /// </summary>
        /// <param name="input">char to test</param>
        /// <returns></returns>
        protected abstract bool MatchesClass(char input);

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {

            if (state.IsValid) {

                var currentChar = state.FetchChar();

                while (state.IsValid && MatchesClass(currentChar)) {
                    state.AppendChar(currentChar);
                    currentChar = state.FetchChar();
                }

                if (!MatchesClass(currentChar))
                    state.Putback(currentChar);
                else
                    state.AppendChar(currentChar);
            }

            if (MinLength > 0 && state.Length < MinLength)
                state.Error(TokenizerBase.UnexpectedEndOfToken, state.Buffer.ToString());

            Finish(state);
        }


        /// <summary>
        ///     finish parsing
        /// </summary>
        /// <param name="state">state</param>
        protected virtual void Finish(ContinuationState state) {
            state.Finish(TokenId);
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
            => TokenKind.ControlChar;


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
            => TokenKind.WhiteSpace;

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
        ///     create a new integer literal token
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override PascalToken CreateResult(IFileReference path)
            => new IntegerLiteralToken() {
                FilePath = path
            };

        /// <summary>
        ///     token kind: hex number
        /// </summary>
        protected override int TokenId
            => TokenKind.HexNumber;

        /// <summary>
        ///     minimal length: "$" + hexdigit
        /// </summary>
        protected override int MinLength
            => 2;

        /// <summary>
        ///     test if a char matches a hex number
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => ('0' <= input) && (input <= '9') ||
               ('a' <= input) && (input <= 'f') ||
               ('A' <= input) && (input <= 'F');

        /// <summary>
        ///     unwrap hexadecimal number
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int Unwrap(PascalToken token) {
            var literal = token as IntegerLiteralToken;
            return literal != null ? literal.LiteralValue : 0;
        }

        /// <summary>
        ///     finish parsing
        /// </summary>
        /// <param name="state"></param>
        protected override void Finish(ContinuationState state) {
            int value;

            if (int.TryParse(state.Buffer.ToString(1, state.Buffer.Length - 1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out value))
                ((IntegerLiteralToken)state.Result).LiteralValue = value;

            base.Finish(state);
        }
    }

    /// <summary>
    ///     token group value for digits
    /// </summary>
    public class DigitTokenGroupValue : CharacterClassTokenGroupValue {

        /// <summary>
        ///     create a new integer literal token
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override PascalToken CreateResult(IFileReference path)
            => new IntegerLiteralToken() {
                FilePath = path
            };


        /// <summary>
        ///     token id
        /// </summary>
        protected override int TokenId
            => TokenKind.Integer;

        /// <summary>
        ///     matches a digit
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override bool MatchesClass(char input)
            => ('0' <= input) && (input <= '9');


        /// <summary>
        ///     finish parsing
        /// </summary>
        /// <param name="state"></param>
        protected override void Finish(ContinuationState state) {
            int value;

            if (int.TryParse(state.Buffer.ToString(), out value))
                ((IntegerLiteralToken)state.Result).LiteralValue = value;

            base.Finish(state);
        }

        /// <summary>
        ///     unwrap number
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int Unwrap(PascalToken token) {
            var literal = token as IntegerLiteralToken;
            if (literal != null)
                return literal.LiteralValue;

            var numberLiteral = token as NumberLiteralToken;
            if (numberLiteral != null)
                return numberLiteral.LiteralValue;

            return 0;
        }

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
        public bool AllowDots
        {
            get { return identifierCharClass.AllowDots; }
            set { identifierCharClass.AllowDots = value; }
        }

        /// <summary>
        ///     allow digits in identifiers
        /// </summary>
        public bool AllowDigits
        {
            get { return identifierCharClass.AllowDigits; }
            set { identifierCharClass.AllowDigits = value; }
        }

        /// <summary>
        ///     allow ampersand in identifiers
        /// </summary>
        public bool AllowAmpersand
        {
            get { return identifierCharClass.AllowAmpersand; }
            set { identifierCharClass.AllowAmpersand = value; }
        }

        private readonly IDictionary<string, int> knownKeywords;

        /// <summary>
        ///     create a new token group for ids and keywords
        /// </summary>
        /// <param name="keywords"></param>
        public IdentifierTokenGroupValue(IDictionary<string, int> keywords) {

            if (keywords == null)
                throw new ArgumentNullException(nameof(keywords));

            knownKeywords = keywords;
        }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {
            bool hasAmpersand = state.Buffer[0] == '&';
            bool ignoreKeywords = AllowAmpersand && hasAmpersand;

            if (ignoreKeywords)
                state.PutbackBuffer();
            else if (hasAmpersand) {
                state.Finish(TokenKind.Undefined);
                state.Error(TokenizerBase.UnexpectedCharacter, "&");
                return;
            }

            while (state.IsValid) {
                var currentChar = state.FetchChar();
                if (!identifierCharClass.Matches(currentChar)) {
                    state.Putback(currentChar);
                    break;
                }
                state.AppendChar(currentChar);
            }

            int tokenKind;
            string value = state.Buffer.ToString();
            if ((!ignoreKeywords) && (knownKeywords.TryGetValue(value, out tokenKind)))
                state.Finish(tokenKind, value);
            else
                state.Finish(TokenKind.Identifier, value);

        }
    }

    /// <summary>
    ///     token group for end of line comments
    /// </summary>
    public class EndOfLineCommentTokenGroupValue : PatternContinuation {

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {

            while (state.IsValid) {
                char currentChar = state.FetchAndAppendChar();

                if (!LineCounter.IsNewLineChar(currentChar)) {
                    state.AppendChar(currentChar);
                }
                else {
                    state.Putback(currentChar);
                    break;
                }
            }

            state.Finish(TokenKind.Comment);
        }
    }

    /// <summary>
    ///     token group for numbers
    /// </summary>
    public class NumberTokenGroupValue : PatternContinuation {

        /// <summary>
        ///     create a new result token
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>created token</returns>
        protected override PascalToken CreateResult(IFileReference path)
            => new NumberLiteralToken() { FilePath = path };

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

        private static bool NextCharMatches(ContinuationState state, CharacterClass c) {
            if (!state.IsValid)
                return false;

            char nextChar = state.FetchChar();

            if (c.Matches(nextChar)) {
                state.AppendChar(nextChar);
                return true;
            }

            state.Putback(nextChar);
            return false;

        }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override void ParseByPrefix(ContinuationState state) {
            var intPrefix = DigitTokenGroupValue.Unwrap(digitTokenizer.Tokenize(state.Input, state.Buffer, state.Log));
            var result = state.Result as NumberLiteralToken;
            var withDot = false;
            var withExponent = false;

            if (!state.IsValid) {
                result.LiteralValue = intPrefix;
                state.Finish(TokenKind.Integer);
                return;
            }

            if (NextCharMatches(state, dot)) {
                if (NextCharMatches(state, numbers)) {
                    digitTokenizer.Tokenize(state.Input, state.Buffer, state.Log);
                    withDot = true;
                }

                if (state.EndsWith(".")) {
                    state.Putback('.');
                    state.Buffer.Length -= 1;
                }
            }

            if (NextCharMatches(state, exponent)) {
                if (!state.IsValid || !NextCharMatches(state, plusminus)) {
                    state.Error(TokenizerBase.UnexpectedEndOfToken, "+", "-");
                }
                else if (!state.IsValid || digitTokenizer.Tokenize(state.Input, state.Buffer, state.Log).Kind != TokenKind.Integer) {
                    state.Error(TokenizerBase.UnexpectedEndOfToken);
                }

                withExponent = true;
            }

            if (withDot || withExponent) {
                state.Finish(TokenKind.Real);
            }
            else {
                result.LiteralValue = intPrefix;
                state.Finish(TokenKind.Integer);
            }

        }



    }
}