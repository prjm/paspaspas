using System;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

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

    /*

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