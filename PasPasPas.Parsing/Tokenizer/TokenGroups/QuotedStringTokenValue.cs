using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for quoted strings
    /// </summary>
    public sealed class QuotedStringTokenValue : PatternContinuation {

        /// <summary>
        ///     quote char
        /// </summary>
        public char QuoteChar { get; }

        /// <summary>
        ///     token id
        /// </summary>
        public int TokenId { get; }

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
        public override Token Tokenize(TokenizerState state) {
            var found = false;
            var quote = QuoteChar;
            using (var resultBuilder = PoolFactory.FetchStringBuilder()) {

                while ((!found) && (!state.AtEof)) {
                    var nextChar1 = state.LookAhead(1);
                    var nextChar2 = state.LookAhead(2);

                    found = (nextChar1 == quote) && (nextChar2 != quote);
                    var escapedQuote = (nextChar1 == quote) && (nextChar2 == quote);

                    if (!found)
                        resultBuilder.Data.Append(state.NextChar(true));
                    else
                        state.NextChar(true);

                    if (escapedQuote)
                        state.NextChar(true);
                }

                found = state.BufferEndsWith(QuoteChar) && state.Length > 1;

                if (!found)
                    state.Error(Tokenizer.IncompleteString);

                return new Token(TokenId, state, StringPool.PoolString(resultBuilder.Data.ToString()));
            }
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