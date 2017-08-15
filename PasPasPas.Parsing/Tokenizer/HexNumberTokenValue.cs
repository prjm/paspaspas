using System;
using System.Text;
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

    }

}
