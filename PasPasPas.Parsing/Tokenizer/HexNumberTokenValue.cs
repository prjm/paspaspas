using System;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

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

        */
    /*
/// <summary>
///     token group for quoted strings
/// </summary>
public class QuotedStringTokenValue : PatternContinuation {

public override Token Tokenize(StringBuilder buffer, int position, ITokenizer tokenizer) {
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

}

/*


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

*/

    /// <summary>
    ///     tokenizer for hex numbers
    /// </summary>
    public class HexNumberTokenValue : CharacterClassTokenGroupValue {


        /// <summary>
        ///     creates a new token group for hex numbers
        /// </summary>
        public HexNumberTokenValue() : base(TokenKind.HexNumber, new DigitCharClass(true), 2) {
            //..
        }

        /// <summary>
        ///     error message id
        /// </summary>
        protected override Guid MinLengthMessage
            => StandardTokenizer.IncompleteHexNumber;


    }

}
