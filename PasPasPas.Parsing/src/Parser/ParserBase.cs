using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     base class for parsers
    /// </summary>
    public abstract class ParserBase : IParser {

        /// <summary>
        ///     message group for parser logs
        /// </summary>
        public static readonly Guid ParserLogMessage
            = new Guid(new byte[] { 0x22, 0xc3, 0x76, 0x5e, 0x97, 0x6c, 0xe8, 0x49, 0xb7, 0x65, 0x1d, 0xa7, 0x31, 0xf4, 0x5b, 0x33 });
        /* {5e76c322-6c97-49e8-b765-1da731f45b33} */

        /// <summary>
        ///     message: unexpected token
        /// </summary>
        public static readonly Guid UnexpectedToken
            = new Guid(new byte[] { 0xe0, 0xbb, 0xd3, 0x5, 0xe2, 0x32, 0xeb, 0x43, 0x85, 0xb, 0xeb, 0xdb, 0x2b, 0x7e, 0xf2, 0x72 });
        /* {05d3bbe0-32e2-43eb-850b-ebdb2b7ef272} */

        /// <summary>
        ///     message: user generated message
        /// </summary>
        public static readonly Guid UserGeneratedMessage
            = new Guid(new byte[] { 0xac, 0xe2, 0xd4, 0x74, 0xdd, 0x54, 0xcc, 0x4a, 0x90, 0x8a, 0x9f, 0x2e, 0x8a, 0x2d, 0x91, 0x65 });
        /* {74d4e2ac-54dd-4acc-908a-9f2e8a2d9165} */

        /// <summary>
        ///     missing token
        /// </summary>
        public static readonly Guid MissingToken
            = new Guid(new byte[] { 0x84, 0x1f, 0xb1, 0x9f, 0xe3, 0x2f, 0x9c, 0x48, 0xb7, 0x15, 0xdd, 0x29, 0x45, 0x8e, 0x43, 0x4e });
        /* {9fb11f84-2fe3-489c-b715-dd29458e434e} */

        private TokenizerWithLookahead tokenizer;
        private readonly LogSource logSource;
        private readonly OptionSet options;
        private readonly IParserEnvironment environment;

        /// <summary>
        ///     creates a new parser
        /// </summary>
        /// <param name="parserOptions">parser options</param>
        /// <param name="tokenizerWithLookAhead">used tokenizer</param>
        /// <param name="env">environment</param>
        protected ParserBase(IParserEnvironment env, OptionSet parserOptions, TokenizerWithLookahead tokenizerWithLookAhead) {
            environment = env;
            tokenizer = tokenizerWithLookAhead;
            logSource = new LogSource(env.Log, ParserLogMessage);
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
        public OptionSet Options
            => options;

        /// <summary>
        ///     wrapper tokenizer
        /// </summary>
        public TokenizerWithLookahead Tokenizer
            => tokenizer;

        /// <summary>
        ///     unexpected input token
        /// </summary>
        /// <returns></returns>
        protected void Unexpected() {
            var token = CurrentToken();
            logSource.LogError(UnexpectedToken, token.Kind, token.Value);
            FetchNextToken();
        }

        /// <summary>
        ///     parser syntax error
        /// </summary>
        /// <param name="message">message id</param>
        /// <param name="expectedTokens">expected tokens (or)</param>
        /// <returns></returns>
        protected Terminal ErrorAndSkip(Guid message, int[] expectedTokens) {
            logSource.LogError(message, expectedTokens);
            return CreateByError();
        }

        /// <summary>
        ///     mark an already parsed terminal as error
        /// </summary>
        /// <param name="message">message id</param>
        /// <param name="values">error values</param>
        /// <returns></returns>
        protected void ErrorLastPart(Guid message, params object[] values)
            => logSource.LogError(message, values);

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
                (token.Kind == tokenKind1) ||
                (token.Kind == tokenKind2);
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
                (token.Kind == tokenKind1) ||
                (token.Kind == tokenKind2) ||
                (token.Kind == tokenKind3);
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
                (token.Kind == tokenKind1) ||
                (token.Kind == tokenKind2) ||
                (token.Kind == tokenKind3) ||
                (token.Kind == tokenKind4);
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
                (token.Kind == tokenKind1) ||
                (token.Kind == tokenKind2) ||
                (token.Kind == tokenKind3) ||
                (token.Kind == tokenKind4) ||
                (token.Kind == tokenKind5);
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
                (token.Kind == tokenKind1) ||
                (token.Kind == tokenKind2) ||
                (token.Kind == tokenKind3) ||
                (token.Kind == tokenKind4) ||
                (token.Kind == tokenKind5) ||
                (token.Kind == tokenKind6);
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
                (token.Kind == tokenKind1) ||
                (token.Kind == tokenKind2) ||
                (token.Kind == tokenKind3) ||
                (token.Kind == tokenKind4) ||
                (token.Kind == tokenKind5) ||
                (token.Kind == tokenKind6) ||
                (token.Kind == tokenKind7);
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
        /// <param name="tokenKind8">eigth token</param>
        /// <param name="tokenKind9">eigth token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8, int tokenKind9) {
            var token = CurrentToken();
            return
                (token.Kind == tokenKind1) ||
                (token.Kind == tokenKind2) ||
                (token.Kind == tokenKind3) ||
                (token.Kind == tokenKind4) ||
                (token.Kind == tokenKind5) ||
                (token.Kind == tokenKind6) ||
                (token.Kind == tokenKind7) ||
                (token.Kind == tokenKind8) ||
                (token.Kind == tokenKind9);
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
        /// <param name="tokenKind8">eigth token</param>
        /// <returns></returns>
        protected bool Match(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8) {
            var token = CurrentToken();
            return
                (token.Kind == tokenKind1) ||
                (token.Kind == tokenKind2) ||
                (token.Kind == tokenKind3) ||
                (token.Kind == tokenKind4) ||
                (token.Kind == tokenKind5) ||
                (token.Kind == tokenKind6) ||
                (token.Kind == tokenKind7) ||
                (token.Kind == tokenKind8);
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
            hasSymbol = result != null;
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
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return null;
            }

            var terminal = environment.TerminalPool.GetTerminal(tokenizer.CurrentTokenSequence);
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
            var requiresIdentifier = (tokenKind1 == TokenKind.Identifier) || (tokenKind2 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return null;
            }

            if (!Match(tokenKind1, tokenKind2) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return null;
            }

            var terminal = environment.TerminalPool.GetTerminal(tokenizer.CurrentTokenSequence);
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
                (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return null;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return null;
            }

            var terminal = environment.TerminalPool.GetTerminal(tokenizer.CurrentTokenSequence);
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
                (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return null;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return null;
            }

            var terminal = environment.TerminalPool.GetTerminal(tokenizer.CurrentTokenSequence);
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
                (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier) ||
                (tokenKind5 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return null;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return null;
            }

            var terminal = environment.TerminalPool.GetTerminal(tokenizer.CurrentTokenSequence);
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
                (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier) ||
                (tokenKind5 == TokenKind.Identifier) ||
                (tokenKind6 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken)
                return null;

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return null;
            }

            var terminal = environment.TerminalPool.GetTerminal(tokenizer.CurrentTokenSequence);
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
                (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier) ||
                (tokenKind5 == TokenKind.Identifier) ||
                (tokenKind6 == TokenKind.Identifier) ||
                (tokenKind7 == TokenKind.Identifier) ||
                (tokenKind8 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken)
                return null;

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return null;
            }

            var terminal = environment.TerminalPool.GetTerminal(tokenizer.CurrentTokenSequence);
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
            var invalid = environment.TerminalPool.GetTerminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return invalid;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind) {
            var terminal = ContinueWith(tokenKind);

            if (terminal == null)
                return ErrorMissingToken(tokenKind);

            return terminal;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind"></param>
        protected Terminal ErrorMissingToken(int tokenKind) {
            logSource.LogError(MissingToken, tokenKind);
            return null;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        protected Terminal ErrorMissingToken(int tokenKind1, int tokenKind2) {
            logSource.LogError(MissingToken, tokenKind1, tokenKind2);
            return null;
        }


        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        protected Terminal ErrorMissingToken(int tokenKind1, int tokenKind2, int tokenKind3) {
            logSource.LogError(MissingToken, tokenKind1, tokenKind2, tokenKind3);
            return null;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        protected Terminal ErrorMissingToken(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            logSource.LogError(MissingToken, tokenKind1, tokenKind2, tokenKind3, tokenKind4);
            return null;
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
            logSource.LogError(MissingToken, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5);
            return null;
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
            logSource.LogError(MissingToken, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6);
            return null;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind1, int tokenKind2) {
            var terminal = ContinueWith(tokenKind1, tokenKind2);

            if (terminal == null)
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

            if (terminal == null)
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

            if (terminal == null)
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

            if (terminal == null)
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

            if (terminal == null)
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
            if (Tokenizer != null) {
                tokenizer.Dispose();
                tokenizer = null;
            }
        }

        /// <summary>
        ///     get a list from the list pool
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected PoolItem<List<T>> GetList<T>() where T : class
            => Environment.ListPools.GetList<T>();

        /// <summary>
        ///     add one item to the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Q"></typeparam>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected static Q AddToList<T, Q>(PoolItem<List<T>> list, Q item) where T : class where Q : T {
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
        protected static ImmutableArray<T> GetFixedArray<T>(PoolItem<List<T>> list) where T : class {

            switch (list.Item.Count) {
                case 0:
                    return ImmutableArray<T>.Empty;

                case 1:
                    return ImmutableArray.Create(list.Item[0]);

                case 2:
                    return ImmutableArray.Create(list.Item[0], list.Item[1]);

                case 3:
                    return ImmutableArray.Create(list.Item[0], list.Item[1], list.Item[2]);

                case 4:
                    return ImmutableArray.Create(list.Item[0], list.Item[1], list.Item[2], list.Item[3]);

            };

            var builder = ListPools.GetImmutableArrayBuilder(list);
            for (var index = 0; index < list.Item.Count; index++)
                builder.Add(list.Item[index]);

            LogHistogram(builder);
            return builder.MoveToImmutable();
        }

        [Conditional("DEBUG")]
        private static void LogHistogram<T>(ImmutableArray<T>.Builder builder) where T : class {
            if (Histograms.Enable)
                Histograms.Value(HistogramKeys.SyntaxLists, string.Concat(typeof(T).Name, builder.Count));
        }
    }
}