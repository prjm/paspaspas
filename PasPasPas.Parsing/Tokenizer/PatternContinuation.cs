using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     a pattern continination is used as a base class to parse some type of token
    /// </summary>
    public abstract class PatternContinuation {

        /// <summary>
        ///     generate the next token
        /// </summary>
        public abstract Token Tokenize(ITokenizerState state);

        /// <summary>
        ///     test if a given character can trigger a new line
        /// </summary>
        /// <param name="value">char to test</param>
        /// <returns><c>true</c> if the character can trigger a new line</returns>
        public static bool IsNewLineChar(char value)
            => value == '\r' || value == '\n';

        /// <summary>
        ///     test if a string contains a newline character
        /// </summary>
        /// <param name="value">string to tet</param>
        /// <returns><c>true</c> if it contains a new line character</returns>
        public static bool ContainsNewLineChar(string value) {
            for (var index = 0; index < value.Length; index++) {
                if (IsNewLineChar(value[index]))
                    return true;
            }
            return false;
        }

    }

    /*

    /// <summary>
    ///     semicolon or asm comment
    /// </summary>
    public class SemicolonOrAsmTokenValue : PatternContinuation {

        /// <summary>
        ///     allow a comment
        /// </summary>
        public bool AllowComment { get; set; }

        /// <summary>
        ///     parse until eol or semicolon
        /// </summary>
        /// <param name="state"></param>
        public override void ParseByPrefix(ContinuationState state) {
            if (!AllowComment) {
                state.Finish(TokenKind.Semicolon);
                return;
            }

            var found = false;
            while (state.IsValid && (!found)) {
                var currentChar = state.FetchAndAppendChar();
                found = LineCounter.IsNewLineChar(currentChar);

                if (found && state.IsValid) {
                    var nextChar = state.FetchChar();

                    if (LineCounter.IsNewLineChar(nextChar))
                        state.AppendChar(nextChar);
                    else
                        state.Putback(nextChar, state.SwitchedFile);
                }
            }

            if (!found)
                state.Error(TokenizerBase.UnexpectedEndOfToken);

            state.Finish(TokenKind.Comment);
        }
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
        protected override Token CreateResult(IFileReference path) {
            ResetParsedString();
            return Token.Empty;
            //return new StringLiteralToken() {
            //FilePath = path
            //};
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
            var literal = ((Token)state.Result);

            /*

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
                    var switchState = state.SwitchedFile;
                    found = nextChar != '\'';
                    if (found)
                        state.Putback(nextChar, switchState);
                    else {
                        AppendToBuffer(nextChar);
                        state.AppendChar(nextChar);
                    }
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
        public static string Unwrap(Token quotedText) {
            return null;
            //var literal = quotedText as StringLiteralToken;
            //return literal != null ? literal.LiteralValue : string.Empty;
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
                var currentChar = state.FetchChar();

                if (currentChar == '"' && state.IsValid) {
                    var switchState = state.SwitchedFile;
                    var nextChar = state.FetchChar();
                    found = nextChar != '"';

                    if (found)
                        state.Putback(nextChar, switchState);
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
                var switchState = state.SwitchedFile;
                var currentChar = state.FetchChar();
                if (currentChar == '#' && !state.IsValid) {
                    state.Error(TokenizerBase.UnexpectedEndOfToken);
                }
                else if (currentChar == '#') {
                    switchState = state.SwitchedFile;
                    var nextChar = state.FetchChar();
                    state.AppendChar(currentChar);
                    if (nextChar == '$' && !state.IsValid) {
                        state.Error(TokenizerBase.UnexpectedEndOfToken);
                    }
                    else if (nextChar == '$') {
                        controlBuffer.Clear();
                        controlBuffer.Append('$');
                        Token controlChar = hexDigits.Tokenize(state);
                        if (controlChar.Kind != TokenKind.HexNumber) {
                            state.Error(TokenizerBase.UnexpectedCharacter);
                        }
                        else {
                            state.Buffer.Append(controlBuffer);
                            AppendToBuffer((char)HexNumberTokenValue.Unwrap(controlChar));
                        }
                    }
                    else {
                        state.Putback(nextChar, switchState);
                        controlBuffer.Clear();
                        Token controlChar = digits.Tokenize(state);
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
                    AppendToBuffer(QuotedStringTokenValue.Unwrap(quotedString.Tokenize(state)));
                }
                else {
                    state.Putback(currentChar, switchState);
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
    ///     tokenizer for hex numbers
    /// </summary>
    public class HexNumberTokenValue : CharacterClassTokenGroupValue {

        /// <summary>
        ///     create a new integer literal token
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected override Token CreateResult(IFileReference path)
            => Token.Empty;

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
        ///     error message id
        /// </summary>
        protected override Guid MinLengthMessage
            => StandardTokenizer.IncompleteHexNumber;

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
        public static int Unwrap(Token token) {
            return 0;
            //var literal = token as IntegerLiteralToken;
            //return literal != null ? literal.LiteralValue : 0;
        }

        /// <summary>
        ///     finish parsing
        /// </summary>
        /// <param name="state"></param>
        protected override void Finish(ContinuationState state) {
            int value;

            //if (int.TryParse(state.Buffer.ToString(1, state.Buffer.Length - 1), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out value))
            //((Token)state.Result).LiteralValue = value;

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
        protected override Token CreateResult(IFileReference path)
            => Token.Empty;


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

            //if (int.TryParse(state.Buffer.ToString(), out value))
            //    ((IntegerLiteralToken)state.Result).LiteralValue = value;

            base.Finish(state);
        }

        /// <summary>
        ///     unwrap number
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static int Unwrap(Token token) {
            return 0;
            /*
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
    ///     if <c>true</c> asm blocks are parsed as a token
    /// </summary>
    public bool ParseAsm { get; set; }
        = false;

    /// <summary>
    ///     allow dots in identifiers
    /// </summary>
    public bool AllowDots {
        get { return identifierCharClass.AllowDots; }
        set { identifierCharClass.AllowDots = value; }
    }

    /// <summary>
    ///     allow digits in identifiers
    /// </summary>
    public bool AllowDigits {
        get { return identifierCharClass.AllowDigits; }
        set { identifierCharClass.AllowDigits = value; }
    }

    /// <summary>
    ///     allow ampersand in identifiers
    /// </summary>
    public bool AllowAmpersand {
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
        var hasAmpersand = state.Buffer[0] == '&';
        var ignoreKeywords = AllowAmpersand && hasAmpersand;

        if (ignoreKeywords)
            state.PutbackBuffer();
        else if (hasAmpersand) {
            state.Finish(TokenKind.Undefined);
            state.Error(TokenizerBase.UnexpectedCharacter, "&");
            return;
        }

        while (state.IsValid) {
            var switchState = state.SwitchedFile;
            var currentChar = state.FetchChar();
            if (!identifierCharClass.Matches(currentChar)) {
                state.Putback(currentChar, switchState);
                break;
            }
            state.AppendChar(currentChar);
        }

        int tokenKind;
        var value = state.Buffer.ToString();

        if (hasAmpersand && state.Buffer.Length < 2)
            state.Error(StandardTokenizer.IncompleteIdentifier, state.Buffer.ToString());

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
            var switchState = state.SwitchedFile;
            var currentChar = state.FetchChar();

            if (!LineCounter.IsNewLineChar(currentChar)) {
                state.AppendChar(currentChar);
            }
            else {
                state.Putback(currentChar, switchState);
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
    protected override Token CreateResult(IFileReference path)
        => Token.Empty;

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

    private IdentifierCharacterClass allIdents
        = new IdentifierCharacterClass() { AllowAmpersand = true, AllowDigits = true, AllowDots = true, };

    private IdentifierTokenGroupValue identTokenizer
        = new IdentifierTokenGroupValue(new Dictionary<string, int>());

    /// <summary>
    ///     flag, if <c>true</c> idents are generated if possible
    /// </summary>
    public bool AllowIdents { get; set; }
        = false;

    private static bool NextCharMatches(ContinuationState state, CharacterClass c) {
        if (!state.IsValid)
            return false;

        var switchState = state.SwitchedFile;
        var nextChar = state.FetchChar();

        if (c.Matches(nextChar)) {
            state.AppendChar(nextChar);
            return true;
        }

        state.Putback(nextChar, switchState);
        return false;

    }

    public override void ParseByPrefix(ContinuationState state) {
        int intPrefix = DigitTokenGroupValue.Unwrap(digitTokenizer.Tokenize(state));

        var result = state.Result;
        var withDot = false;
        var withExponent = false;

        if (!state.IsValid) {
            //result.LiteralValue = intPrefix;
            state.Finish(TokenKind.Integer);
            return;
        }

        if (NextCharMatches(state, dot)) {
            withDot = true;

            if (NextCharMatches(state, numbers)) {
                digitTokenizer.Tokenize(state);
            }

            var switchState = state.SwitchedFile;
            if (state.EndsWith(".")) {
                withDot = false;
                state.Putback('.', switchState);
                state.Buffer.Length -= 1;
            }

        }

        if (NextCharMatches(state, exponent)) {
            if (!state.IsValid) {
                state.Error(TokenizerBase.UnexpectedEndOfToken, "+", "-");
            }
            else {
                NextCharMatches(state, plusminus);
                var currentLen = state.Buffer.Length;
                if (!state.IsValid || digitTokenizer.Tokenize(state).Kind != TokenKind.Integer || state.Buffer.Length == currentLen) {
                    state.Error(TokenizerBase.UnexpectedEndOfToken);
                }
            }

            withExponent = true;
        }

        if (AllowIdents && NextCharMatches(state, allIdents)) {
            identTokenizer.Tokenize(state);
            state.Finish(TokenKind.Identifier);
            return;
        }

        if (withDot || withExponent) {
            state.Finish(TokenKind.Real);
        }
        else {
            //result.LiteralValue = intPrefix;
            state.Finish(TokenKind.Integer);
        }

    }

        */

}