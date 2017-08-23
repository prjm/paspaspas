using System.Text;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {
    /*


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
            */

    /// <summary>
    ///     token group for quoted strings
    /// </summary>
    public class QuotedStringTokenValue : PatternContinuation {

        public char QuoteChar { get; private set; }

        public int TokenId { get; private set; }

        /// <summary>
        ///     create a new quoted string token value
        /// </summary>
        /// <param name="quoteChar"></param>
        public QuotedStringTokenValue(int tokenId, char quoteChar) {
            QuoteChar = quoteChar;
            TokenId = tokenId;
        }

        /// <summary>
        ///     parse a quoted string
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(ITokenizerState state) {
            var found = false;
            var quote = QuoteChar;

            while ((!found) && (!state.AtEof)) {
                var currentChar = state.NextChar(false);

                if (currentChar == quote) {
                    var nextChar = state.NextChar(false);
                    found = nextChar != quote;

                    if (found)
                    {
                        state.Append(quote);
                    }
                    else
                    {
                        state.Append(quote);
                        state.Append(nextChar);
                    }
                }
                else {
                    state.Append(currentChar);
                }

            }

            if (!found) {
                state.Error(TokenizerBase.UnexpectedEndOfToken);
            }

            found = state.BufferEndsWith(QuoteChar);

            if (!found)
                state.Error(TokenizerBase.UnexpectedEndOfToken);

            return new Token(TokenId, state);
        }

    }

    /*
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