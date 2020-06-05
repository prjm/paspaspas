#nullable disable
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     missing token details
    /// </summary>
    public class MissingTokenInfo {

        /// <summary>
        ///     create a new missing token inf
        /// </summary>
        /// <param name="position"></param>
        /// <param name="tokenKinds"></param>
        public MissingTokenInfo(int position, params int[] tokenKinds) {
            Position = position;
            TokenKinds = tokenKinds.ToImmutableArray();
        }

        /// <summary>
        ///     position offset
        /// </summary>
        public int Position { get; }

        /// <summary>
        ///     token kinds
        /// </summary>
        public ImmutableArray<int> TokenKinds { get; }

        /// <summary>
        ///     convert this info to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"{Position}: " + string.Join(", ", TokenKinds);

    }

    /// <summary>
    ///     base class for parsers
    /// </summary>
    public abstract class ParserBase : IParser {

        /// <summary>
        ///     flag, if <c>true</c> units are resolved
        /// </summary>
        public bool ResolveIncludedFiles { get; set; } = true;

        private TokenizerWithLookahead tokenizer;
        private readonly IOptionSet options;
        private readonly IParserEnvironment environment;

        /// <summary>
        ///     creates a new parser
        /// </summary>
        /// <param name="parserOptions">parser options</param>
        /// <param name="tokenizerWithLookAhead">used tokenizer</param>
        /// <param name="env">environment</param>
        protected ParserBase(IParserEnvironment env, IOptionSet parserOptions, TokenizerWithLookahead tokenizerWithLookAhead) {
            environment = env;
            tokenizer = tokenizerWithLookAhead;
            LogSource = env.Log.CreateLogSource(MessageGroups.Parser);
            options = parserOptions;
        }

        /// <summary>
        ///     tokenizer to use
        /// </summary>
        public ITokenizer BaseTokenizer
            => tokenizer.BaseTokenizer;

        /// <summary>
        ///     parser environment
        /// </summary>
        public IParserEnvironment Environment
            => environment;

        /// <summary>
        ///     option set
        /// </summary>
        public IOptionSet Options
            => options;

        /// <summary>
        ///     wrapper tokenizer
        /// </summary>
        public TokenizerWithLookahead Tokenizer
            => tokenizer;

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource LogSource { get; }

        /// <summary>
        ///     unexpected input token
        /// </summary>
        /// <returns></returns>
        protected void Unexpected() {
            var token = CurrentToken();
            LogSource.LogError(MessageNumbers.UnexpectedToken, token.Kind, token.Value);
            FetchNextToken();
        }

        /// <summary>
        ///     parser syntax error
        /// </summary>
        /// <param name="message">message id</param>
        /// <param name="expectedTokens">expected tokens (or)</param>
        /// <returns></returns>
        protected Terminal ErrorAndSkip(uint message, int[] expectedTokens) {
            LogSource.LogError(message, expectedTokens);
            return CreateByError();
        }

        /// <summary>
        ///     mark an already parsed terminal as error
        /// </summary>
        /// <param name="message">message id</param>
        /// <param name="values">error values</param>
        /// <returns></returns>
        protected void ErrorLastPart(uint message, params object[] values)
            => LogSource.LogError(message, values);

        /// <summary>
        ///     Require a token kind
        /// </summary>
        /// <param name="tokenKind">required kind of token</param>
        /// <returns>parser token</returns>
        protected Token Require(int tokenKind) {
            if (!Match(tokenKind))
                Unexpected();

            var result = CurrentToken();
            FetchNextToken();
            return result;
        }

        /// <summary>
        ///     require a specific token kind
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <returns></returns>
        protected bool RequireTokenKind(int tokenKind)
            => Require(tokenKind).Kind == tokenKind;

        /// <summary>
        ///     require one token of the token list
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <returns></returns>
        protected Token Require(params int[] tokenKind) {
            foreach (var token in tokenKind) {
                if (Match(token)) {
                    return Require(token);
                }
            }

            Unexpected();
            return Token.Empty;
        }

        /// <summary>
        ///     fetch the next token
        /// </summary>
        protected void FetchNextToken()
            => tokenizer.FetchNextToken();

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind">token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind);
        }

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind1">first token kind to test for</param>
        /// <param name="tokenKind2">second token kind to test for</param>
        /// <param name="tokenKind3">third token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind1, int tokenKind2, int tokenKind3) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind1, tokenKind2, tokenKind3);
        }

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind1">first token kind to test for</param>
        /// <param name="tokenKind2">second token kind to test for</param>
        /// <param name="tokenKind3">third token kind to test for</param>
        /// <param name="tokenKind4"></param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind1, tokenKind2, tokenKind3, tokenKind4);
        }

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind1">first token kind to test for</param>
        /// <param name="tokenKind2">second token kind to test for</param>
        /// <param name="tokenKind3">third token kind to test for</param>
        /// <param name="tokenKind4">forth token kind to test for</param>
        /// <param name="tokenKind5">fifth token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5);
        }

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind1">first token kind to test for</param>
        /// <param name="tokenKind2">second token kind to test for</param>
        /// <param name="tokenKind3">third token kind to test for</param>
        /// <param name="tokenKind4">forth token kind to test for</param>
        /// <param name="tokenKind5">fifth token kind to test for</param>
        /// <param name="tokenKind6">sixth token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6);
        }


        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind1">first token kind to test for</param>
        /// <param name="tokenKind2">second token kind to test for</param>
        /// <param name="tokenKind3">third token kind to test for</param>
        /// <param name="tokenKind4">forth token kind to test for</param>
        /// <param name="tokenKind5">fifth token kind to test for</param>
        /// <param name="tokenKind6">sixth token kind to test for</param>
        /// <param name="tokenKind7">sevenths token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7);
        }

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind1">first token kind to test for</param>
        /// <param name="tokenKind2">second token kind to test for</param>
        /// <param name="tokenKind3">third token kind to test for</param>
        /// <param name="tokenKind4">forth token kind to test for</param>
        /// <param name="tokenKind5">fifth token kind to test for</param>
        /// <param name="tokenKind6">sixth token kind to test for</param>
        /// <param name="tokenKind7">sevenths token kind to test for</param>
        /// <param name="tokenKind8"></param>
        /// <param name="tokenKind9"></param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8, int tokenKind9) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8, tokenKind9);
        }



        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind1">first token kind to test for</param>
        /// <param name="tokenKind2">second token kind to test for</param>
        /// <param name="tokenKind3">third token kind to test for</param>
        /// <param name="tokenKind4">forth token kind to test for</param>
        /// <param name="tokenKind5">fifth token kind to test for</param>
        /// <param name="tokenKind6">sixth token kind to test for</param>
        /// <param name="tokenKind7">sevenths token kind to test for</param>
        /// <param name="tokenKind8"></param>
        /// <param name="tokenKind9"></param>
        /// <param name="tokenKind10"></param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8, int tokenKind9, int tokenKind10) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8, tokenKind9, tokenKind10);
        }

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind1">first token kind to test for</param>
        /// <param name="tokenKind2">second token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int numberOfTokens, int tokenKind1, int tokenKind2) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind1, tokenKind2);
        }

        /// <summary>
        ///     tests if the current token matches the selected token
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <returns></returns>
        protected bool Match(int tokenKind) {
            var token = CurrentToken();
            return token.Kind == tokenKind;
        }

        /// <summary>
        ///     tests if the current token matches one of the selected tokens
        /// </summary>
        /// <param name="tokenKind1">first token</param>
        /// <param name="tokenKind2"></param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2) {
            var token = CurrentToken();
            return
                token.Kind == tokenKind1 ||
                token.Kind == tokenKind2;
        }

        /// <summary>
        ///     tests if the current token matches one of the selected tokens
        /// </summary>
        /// <param name="tokenKind1">first token</param>
        /// <param name="tokenKind2">second token</param>
        /// <param name="tokenKind3">third token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3) {
            var token = CurrentToken();
            return
                token.Kind == tokenKind1 ||
                token.Kind == tokenKind2 ||
                token.Kind == tokenKind3;
        }

        /// <summary>
        ///     tests if the current token matches one of the selected tokens
        /// </summary>
        /// <param name="tokenKind1">first token</param>
        /// <param name="tokenKind2">second token</param>
        /// <param name="tokenKind3">third token</param>
        /// <param name="tokenKind4">fourth token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            var token = CurrentToken();
            return
                token.Kind == tokenKind1 ||
                token.Kind == tokenKind2 ||
                token.Kind == tokenKind3 ||
                token.Kind == tokenKind4;
        }

        /// <summary>
        ///     tests if the current token matches one of the selected tokens
        /// </summary>
        /// <param name="tokenKind1">first token</param>
        /// <param name="tokenKind2">second token</param>
        /// <param name="tokenKind3">third token</param>
        /// <param name="tokenKind4">fourth token</param>
        /// <param name="tokenKind5">fifth token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) {
            var token = CurrentToken();
            return
                token.Kind == tokenKind1 ||
                token.Kind == tokenKind2 ||
                token.Kind == tokenKind3 ||
                token.Kind == tokenKind4 ||
                token.Kind == tokenKind5;
        }

        /// <summary>
        ///     tests if the current token matches one of the selected tokens
        /// </summary>
        /// <param name="tokenKind1">first token</param>
        /// <param name="tokenKind2">second token</param>
        /// <param name="tokenKind3">third token</param>
        /// <param name="tokenKind4">fourth token</param>
        /// <param name="tokenKind5">fifth token</param>
        /// <param name="tokenKind6">sixth token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) {
            var token = CurrentToken();
            return
                token.Kind == tokenKind1 ||
                token.Kind == tokenKind2 ||
                token.Kind == tokenKind3 ||
                token.Kind == tokenKind4 ||
                token.Kind == tokenKind5 ||
                token.Kind == tokenKind6;
        }

        /// <summary>
        ///     tests if the current token matches one of the selected tokens
        /// </summary>
        /// <param name="tokenKind1">first token</param>
        /// <param name="tokenKind2">second token</param>
        /// <param name="tokenKind3">third token</param>
        /// <param name="tokenKind4">fourth token</param>
        /// <param name="tokenKind5">fifth token</param>
        /// <param name="tokenKind6">sixth token</param>
        /// <param name="tokenKind7">seventh token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7) {
            var token = CurrentToken();
            return
                token.Kind == tokenKind1 ||
                token.Kind == tokenKind2 ||
                token.Kind == tokenKind3 ||
                token.Kind == tokenKind4 ||
                token.Kind == tokenKind5 ||
                token.Kind == tokenKind6 ||
                token.Kind == tokenKind7;
        }

        /// <summary>
        ///     tests if the current token matches one of the selected tokens
        /// </summary>
        /// <param name="tokenKind1">first token</param>
        /// <param name="tokenKind2">second token</param>
        /// <param name="tokenKind3">third token</param>
        /// <param name="tokenKind4">fourth token</param>
        /// <param name="tokenKind5">fifth token</param>
        /// <param name="tokenKind6">sixth token</param>
        /// <param name="tokenKind7">seventh token</param>
        /// <param name="tokenKind8">eighth token</param>
        /// <param name="tokenKind9">ninth token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8, int tokenKind9) {
            var token = CurrentToken();
            return
                token.Kind == tokenKind1 ||
                token.Kind == tokenKind2 ||
                token.Kind == tokenKind3 ||
                token.Kind == tokenKind4 ||
                token.Kind == tokenKind5 ||
                token.Kind == tokenKind6 ||
                token.Kind == tokenKind7 ||
                token.Kind == tokenKind8 ||
                token.Kind == tokenKind9;
        }



        /// <summary>
        ///     tests if the current token matches one of the selected tokens
        /// </summary>
        /// <param name="tokenKind1">first token</param>
        /// <param name="tokenKind2">second token</param>
        /// <param name="tokenKind3">third token</param>
        /// <param name="tokenKind4">fourth token</param>
        /// <param name="tokenKind5">fifth token</param>
        /// <param name="tokenKind6">sixth token</param>
        /// <param name="tokenKind7">seventh token</param>
        /// <param name="tokenKind8">eighth token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8) {
            var token = CurrentToken();
            return
                token.Kind == tokenKind1 ||
                token.Kind == tokenKind2 ||
                token.Kind == tokenKind3 ||
                token.Kind == tokenKind4 ||
                token.Kind == tokenKind5 ||
                token.Kind == tokenKind6 ||
                token.Kind == tokenKind7 ||
                token.Kind == tokenKind8;
        }


        /// <summary>
        ///     get the current token
        /// </summary>
        /// <returns></returns>
        protected Token CurrentToken()
            => tokenizer.CurrentToken;

        /// <summary>
        ///     token sequence
        /// </summary>
        /// <returns></returns>
        protected TokenSequence CurrentTokenSequences()
            => tokenizer.CurrentTokenSequence;

        /// <summary>
        ///     test if the searched tokens is located before token to stop
        /// </summary>
        /// <param name="tokenToSearch">search token</param>
        /// <param name="tokenToStop">stop token</param>
        /// <returns></returns>
        protected bool HasTokenBeforeToken(int tokenToSearch, int tokenToStop) {
            var lookahead = 1;

            while (!LookAhead(lookahead, tokenToStop, TokenKind.Undefined)) {

                if (LookAhead(lookahead, tokenToSearch))
                    return true;

                if (Tokenizer.BaseTokenizer.AtEof)
                    return false;

                lookahead++;
            }

            return false;
        }

        /// <summary>
        ///     test if the searched tokens is located before token to stop
        /// </summary>
        /// <param name="tokenToSearch">search token</param>
        /// <param name="tokenToStop1"></param>
        /// <param name="tokenToStop2"></param>
        /// <returns></returns>
        protected bool HasTokenBeforeToken(int tokenToSearch, int tokenToStop1, int tokenToStop2) {
            var lookahead = 1;

            while (!LookAhead(lookahead, tokenToStop1, tokenToStop2, TokenKind.Undefined)) {

                if (LookAhead(lookahead, tokenToSearch))
                    return true;

                if (Tokenizer.BaseTokenizer.AtEof)
                    return false;

                lookahead++;
            }

            return false;
        }

        /// <summary>
        ///     test if the searched tokens is located before token to stop
        /// </summary>
        /// <param name="tokenToSearch">search token</param>
        /// <param name="tokenToStop1"></param>
        /// <param name="tokenToStop2"></param>
        /// <param name="tokenToStop3"></param>
        /// <returns></returns>
        protected bool HasTokenBeforeToken(int tokenToSearch, int tokenToStop1, int tokenToStop2, int tokenToStop3) {
            var lookahead = 1;

            while (!LookAhead(lookahead, tokenToStop1, tokenToStop2, tokenToStop3, TokenKind.Undefined)) {

                if (LookAhead(lookahead, tokenToSearch))
                    return true;

                if (Tokenizer.BaseTokenizer.AtEof)
                    return false;

                lookahead++;
            }

            return false;
        }

        /// <summary>
        ///     test if the searched tokens is located before token to stop
        /// </summary>
        /// <param name="tokenToSearch">search token</param>
        /// <param name="tokenToStop1"></param>
        /// <param name="tokenToStop2"></param>
        /// <param name="tokenToStop3"></param>
        /// <param name="tokenToStop4"></param>
        /// <param name="tokenToStop5"></param>
        /// <param name="tokenToStop6"></param>
        /// <returns></returns>
        protected bool HasTokenBeforeToken(int tokenToSearch, int tokenToStop1, int tokenToStop2, int tokenToStop3, int tokenToStop4, int tokenToStop5, int tokenToStop6) {
            var lookahead = 1;

            while (!LookAhead(lookahead, tokenToStop1, tokenToStop2, tokenToStop3, tokenToStop4, tokenToStop5, tokenToStop6, TokenKind.Undefined)) {

                if (LookAhead(lookahead, tokenToSearch))
                    return true;

                if (Tokenizer.BaseTokenizer.AtEof)
                    return false;

                lookahead++;
            }

            return false;
        }

        /// <summary>
        ///     test if the searched tokens match until a given token is reached
        /// </summary>
        /// <returns></returns>
        protected bool FindCloseBrackets(out int position) {
            var lookahead = 1;

            while (!LookAhead(lookahead, TokenKind.AngleBracketsClose) && !LookAhead(lookahead, TokenKind.Undefined)) {
                if (!LookAhead(lookahead, TokenKind.Identifier, TokenKind.Dot, TokenKind.Comma, TokenKind.AngleBracketsOpen, TokenKind.StringKeyword, TokenKind.ShortString, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString, TokenKind.PointerKeyword)) {
                    position = lookahead;
                    return false;
                }
                lookahead++;
            }

            position = lookahead;
            return true;
        }

        /// <summary>
        ///     continue with syntax part
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <param name="hasSymbol"></param>
        /// <returns></returns>
        protected Terminal ContinueWith(int tokenKind, out bool hasSymbol) {
            var result = ContinueWith(tokenKind);
            hasSymbol = result != default;
            return result;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <returns></returns>
        protected Terminal ContinueWith(int tokenKind) {
            var requiresIdentifier = tokenKind == TokenKind.Identifier;

            if (!Match(tokenKind) &&
                (!requiresIdentifier || requiresIdentifier && !AllowIdentifier())) {
                return default;
            }

            var terminal = ((Terminals)environment.TerminalPool).GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return terminal;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <returns></returns>
        protected Terminal ContinueWith(int tokenKind1, int tokenKind2) {
            var requiresIdentifier = tokenKind1 == TokenKind.Identifier || tokenKind2 == TokenKind.Identifier;

            if (!Tokenizer.HasNextToken)
                return default;

            if (!Match(tokenKind1, tokenKind2) &&
                (!requiresIdentifier || requiresIdentifier && !AllowIdentifier()))
                return default;

            var terminal = ((Terminals)environment.TerminalPool).GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return terminal;
        }


        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <returns></returns>
        protected Terminal ContinueWith(int tokenKind1, int tokenKind2, int tokenKind3) {
            var requiresIdentifier =
                tokenKind1 == TokenKind.Identifier ||
                tokenKind2 == TokenKind.Identifier ||
                tokenKind3 == TokenKind.Identifier;

            if (!Tokenizer.HasNextToken) {
                return default;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3) &&
                (!requiresIdentifier || requiresIdentifier && !AllowIdentifier()))
                return default;

            var terminal = ((Terminals)environment.TerminalPool).GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return terminal;
        }



        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <returns></returns>
        protected Terminal ContinueWith(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            var requiresIdentifier =
                tokenKind1 == TokenKind.Identifier ||
                tokenKind2 == TokenKind.Identifier ||
                tokenKind3 == TokenKind.Identifier ||
                tokenKind4 == TokenKind.Identifier;

            if (!Tokenizer.HasNextToken)
                return default;

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4) &&
                (!requiresIdentifier || requiresIdentifier && !AllowIdentifier()))
                return default;


            var terminal = ((Terminals)environment.TerminalPool).GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return terminal;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <returns></returns>
        protected Terminal ContinueWith(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) {
            var requiresIdentifier =
                tokenKind1 == TokenKind.Identifier ||
                tokenKind2 == TokenKind.Identifier ||
                tokenKind3 == TokenKind.Identifier ||
                tokenKind4 == TokenKind.Identifier ||
                tokenKind5 == TokenKind.Identifier;

            if (!Tokenizer.HasNextToken)
                return default;

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5) &&
                (!requiresIdentifier || requiresIdentifier && !AllowIdentifier()))
                return default;

            var terminal = ((Terminals)environment.TerminalPool).GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return terminal;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        /// <returns></returns>
        protected Terminal ContinueWith(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) {
            var requiresIdentifier =
                tokenKind2 == TokenKind.Identifier ||
                tokenKind1 == TokenKind.Identifier ||
                tokenKind3 == TokenKind.Identifier ||
                tokenKind4 == TokenKind.Identifier ||
                tokenKind5 == TokenKind.Identifier ||
                tokenKind6 == TokenKind.Identifier;

            if (!Tokenizer.HasNextToken)
                return default;

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6) &&
                (!requiresIdentifier || requiresIdentifier && !AllowIdentifier()))
                return default;

            var terminal = ((Terminals)environment.TerminalPool).GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return terminal;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        /// <param name="tokenKind7"></param>
        /// <param name="tokenKind8"></param>
        /// <returns></returns>
        protected Terminal ContinueWith(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8) {
            var requiresIdentifier =
                tokenKind1 == TokenKind.Identifier ||
                tokenKind2 == TokenKind.Identifier ||
                tokenKind3 == TokenKind.Identifier ||
                tokenKind4 == TokenKind.Identifier ||
                tokenKind5 == TokenKind.Identifier ||
                tokenKind6 == TokenKind.Identifier ||
                tokenKind7 == TokenKind.Identifier ||
                tokenKind8 == TokenKind.Identifier;

            if (!Tokenizer.HasNextToken)
                return default;

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8) &&
                (!requiresIdentifier || requiresIdentifier && !AllowIdentifier()))
                return default;

            var terminal = ((Terminals)environment.TerminalPool).GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return terminal;
        }

        /// <summary>
        ///     override: allow another symbol instead of an identifier
        /// </summary>
        /// <returns></returns>
        protected virtual bool AllowIdentifier()
            => false;



        /// <summary>
        ///     add a terminal node marked as invalid
        /// </summary>
        /// <returns></returns>
        protected Terminal CreateByError() {
            var invalid = ((Terminals)environment.TerminalPool).GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return invalid;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind) {
            var terminal = ContinueWith(tokenKind);

            if (terminal == default)
                return ErrorMissingToken(tokenKind);

            return terminal;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind"></param>
        protected Terminal ErrorMissingToken(int tokenKind) {
            var position = tokenizer.CurrentTokenSequence.Position;
            var data = new MissingTokenInfo(position, tokenKind);
            LogSource.LogError(MessageNumbers.MissingToken, data);
            return default;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        protected Terminal ErrorMissingToken(int tokenKind1, int tokenKind2) {
            var position = tokenizer.CurrentTokenSequence.Position;
            var data = new MissingTokenInfo(position, tokenKind1, tokenKind2);
            LogSource.LogError(MessageNumbers.MissingToken, data);
            return default;
        }


        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        protected Terminal ErrorMissingToken(int tokenKind1, int tokenKind2, int tokenKind3) {
            var position = tokenizer.CurrentTokenSequence.Position;
            var data = new MissingTokenInfo(position, tokenKind1, tokenKind2, tokenKind3);
            LogSource.LogError(MessageNumbers.MissingToken, data);
            return default;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        protected Terminal ErrorMissingToken(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            var position = tokenizer.CurrentTokenSequence.Position;
            var data = new MissingTokenInfo(position, tokenKind1, tokenKind2, tokenKind3, tokenKind4);
            LogSource.LogError(MessageNumbers.MissingToken, data);
            return default;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        protected Terminal ErrorMissingToken(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) {
            var position = tokenizer.CurrentTokenSequence.Position;
            var data = new MissingTokenInfo(position, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5);
            LogSource.LogError(MessageNumbers.MissingToken, data);
            return default;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        protected Terminal ErrorMissingToken(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) {
            var position = tokenizer.CurrentTokenSequence.Position;
            var data = new MissingTokenInfo(position, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6);
            LogSource.LogError(MessageNumbers.MissingToken, data);
            return default;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind1, int tokenKind2) {
            var terminal = ContinueWith(tokenKind1, tokenKind2);

            if (terminal == default)
                return ErrorMissingToken(tokenKind1, tokenKind2);

            return terminal;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            var terminal = ContinueWith(tokenKind1, tokenKind2, tokenKind3, tokenKind4);

            if (terminal == default)
                return ErrorMissingToken(tokenKind1, tokenKind2, tokenKind3, tokenKind4);

            return terminal;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) {
            var terminal = ContinueWith(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5);

            if (terminal == default)
                return ErrorMissingToken(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5);

            return terminal;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind1, int tokenKind2, int tokenKind3) {
            var terminal = ContinueWith(tokenKind1, tokenKind2, tokenKind3);

            if (terminal == default)
                return ErrorMissingToken(tokenKind1, tokenKind2, tokenKind3);

            return terminal;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) {
            var terminal = ContinueWith(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6);

            if (terminal == default)
                return ErrorMissingToken(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6);

            return terminal;
        }

        /// <summary>
        ///     parse input
        /// </summary>
        /// <returns>parsed input</returns>
        public abstract ISyntaxPart Parse();

        /// <summary>
        ///     dispose tokenizer
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     dispose tokenizer
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (Tokenizer != default) {
                tokenizer.Dispose();
                tokenizer = default;
            }
        }

        /// <summary>
        ///     get a list from the list pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IPoolItem<List<T>> GetList<T>() where T : class
            => Environment.ListPools.GetList<T>();

        /// <summary>
        ///     add one item to the list
        /// </summary>
        /// <typeparam name="TList"></typeparam>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected static TItem AddToList<TList, TItem>(IPoolItem<List<TList>> list, TItem item) where TList : class where TItem : class, TList {
            if (item != default)
                list.Item.Add(item);
            return item;
        }

        /// <summary>
        ///     get a fixed size array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        protected ImmutableArray<T> GetFixedArray<T>(IPoolItem<List<T>> list) where T : class
            => Environment.ListPools.GetFixedArray(list);

    }
}