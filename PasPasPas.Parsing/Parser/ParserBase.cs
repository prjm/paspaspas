﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;

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

        private readonly TokenizerWithLookahead tokenizer;
        private readonly LogSource logSource;

        /// <summary>
        ///     creates a new parser
        /// </summary>
        /// <param name="tokenizerWithLookAhead"></param>
        /// <param name="environment">environment</param>
        protected ParserBase(ParserServices environment, TokenizerWithLookahead tokenizerWithLookAhead) {
            Environment = environment;
            tokenizer = tokenizerWithLookAhead;
            logSource = new LogSource(environment.Log, ParserLogMessage);
        }

        /// <summary>
        ///     tokenizer to use
        /// </summary>        
        public ITokenizer BaseTokenizer
        {
            get
            {
                return tokenizer.BaseTokenizer;
            }

            set
            {
                tokenizer.BaseTokenizer = value;
            }
        }

        /// <summary>
        ///     wrapper tokenizer
        /// </summary>
        public TokenizerWithLookahead Tokenizer
            => tokenizer;


        /// <summary>
        ///     basic working environment
        /// </summary>
        public ParserServices Environment { get; }


        /// <summary>
        ///     unexpected input token
        /// </summary>
        /// <returns></returns>
        protected ISyntaxPart Unexpected() {
            logSource.Error(UnexpectedToken, CurrentToken().Kind, CurrentToken().Value);
            return null;
        }

        /// <summary>
        ///     parser syntax error
        /// </summary>
        /// <param name="message">message id</param>
        /// <param name="parent">parent syntax tree node</param>
        /// <param name="expectedTokens">expected tokens (or)</param>
        /// <returns></returns>
        protected void ErrorAndSkip(ISyntaxPart parent, Guid message, int[] expectedTokens) {
            logSource.Error(message);
            CreateByError(parent);
        }

        /// <summary>
        ///     mark an already parsed terminal as error
        /// </summary>
        /// <param name="message">message id</param>
        /// <param name="values">error values</param>
        /// <param name="parent">parent syntax tree node</param>
        /// <returns></returns>
        protected void ErrorLastPart(ISyntaxPart parent, Guid message, params object[] values) {
            var lastSymbol = parent.Parts.Last();
            parent.Parts.Remove(lastSymbol);
            lastSymbol.Parent = null;

            foreach (Terminal t in SyntaxPartBase.FindAllTerminals(lastSymbol)) {
                var invalid = new InvalidToken(t.Token);
                invalid.Parent = parent;
                parent.Parts.Add(invalid);
            }

            logSource.Error(message, values);
        }

        /// <summary>
        ///     Require a token kind
        /// </summary>
        /// <param name="tokenKind">required kind of tokem</param>
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
            return Tokenizer.CreatePseudoToken(TokenKind.Undefined);
        }

        /// <summary>
        ///     assume an optional token kind
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <returns>optional token kind</returns>
        protected bool Optional(params int[] tokenKind) {
            if (!Match(tokenKind))
                return false;

            FetchNextToken();
            return true;
        }

        /// <summary>
        ///     fetch the next token
        /// </summary>
        protected void FetchNextToken() {
            tokenizer.FetchNextToken();
        }

        /// <summary>
        ///     assume an optional token kind
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <param name="matchingType">matched token</param>
        /// <returns>optional token kind</returns>
        protected bool Optional(out int matchingType, params int[] tokenKind) {
            if (!Match(tokenKind)) {
                matchingType = TokenKind.Undefined;
                return false;
            }

            matchingType = CurrentToken().Kind;
            FetchNextToken();
            return true;
        }

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="numberOfTokens">number of tokens to look ahead</param>
        /// <param name="tokenKind">token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected virtual bool LookAhead(int numberOfTokens, params int[] tokenKind) {
            var token = tokenizer.LookAhead(numberOfTokens);

            if (token == null)
                return false;

            return tokenKind.Any(t => t == token.Kind);
        }

        /// <summary>
        ///     tests if a token is currently availiable
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <returns></returns>
        protected bool Match(int tokenKind) {
            var token = CurrentToken();
            if (token == null)
                return false;

            return token.Kind == tokenKind;
        }


        /// <summary>
        ///     get the current token
        /// </summary>
        /// <returns></returns>
        protected Token CurrentToken()
            => tokenizer.CurrentToken();

        /// <summary>
        ///     mach any token
        /// </summary>
        /// <param name="tokens">token list</param>
        /// <returns><c>true</c> if matched</returns>
        protected bool Match(params int[] tokens) {
            foreach (var token in tokens) {
                if (Match(token))
                    return true;
            }

            return false;
        }


        /// <summary>
        ///     print a parser grammar
        /// </summary>
        /// <param name="type">parser type</param>
        /// <param name="result">result</param>
        protected static void PrintGrammar(Type type, StringBuilder result) {
            var rules = GetRules(type);

            foreach (var rule in rules) {

                if (!rule.Incomplete)
                    continue;

                if (rule.Incomplete) {
                    result.Append(" X ");
                }
                else {
                    result.Append("   ");
                }
                result.Append(rule.RuleName);
                result.Append(" := ");
                result.AppendLine(rule.Rule);
            }
        }

        /// <summary>
        ///     get grammar rules
        /// </summary>
        /// <returns></returns>
        private static IList<RuleAttribute> GetRules(Type type) {
            var result = new List<RuleAttribute>();
            var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);

            foreach (var method in methods) {
                var attr = method.GetCustomAttribute<RuleAttribute>(false);
                if (attr != null) {
                    result.Add(attr);
                }
            }

            return result;
        }

        /// <summary>
        ///     test if the searched tokens is located before token to stop
        /// </summary>
        /// <param name="tokenToSearch">search token</param>
        /// <param name="tokenToStop">stop token</param>
        /// <returns></returns>
        protected bool HasTokenBeforeToken(int tokenToSearch, params int[] tokenToStop) {
            var lookahead = 1;
            var stopArray = tokenToStop.Concat(new[] { TokenKind.Eof, TokenKind.Undefined }).ToArray();
            while (!LookAhead(lookahead, stopArray)) {
                if (LookAhead(lookahead, tokenToSearch))
                    return true;
                lookahead++;
            }

            return false;
        }

        /// <summary>
        ///     create an syntax part element if the token kind matches
        /// </summary>
        /// <typeparam name="T">syntax part type</typeparam>
        /// <param name="tokenKind">token kind</param>
        /// <param name="result">created syntax part</param>
        /// <param name="parent">parent node</param>
        /// <returns><c>true</c> if match</returns>
        protected bool OptionalPart<T>(ISyntaxPart parent, out T result, int tokenKind)
            where T : ISyntaxPart, new() {

            if (!Match(tokenKind)) {
                result = default(T);
                return false;
            }

            result = CreateByTerminal<T>(parent, tokenKind);
            return true;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(ISyntaxPart part, params int[] tokenKind) {

            var requiresIdentifier = tokenKind.Length == 1 && tokenKind[0] == TokenKind.Identifier;

            if (!Tokenizer.HasNextToken()) {
                return false;
            }

            if (!Match(tokenKind) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(CurrentToken());
            terminal.Parent = part;
            part.Parts.Add(terminal);
            FetchNextToken();
            return true;

        }

        /// <summary>
        ///     override: allow another symbol instead of an identifier
        /// </summary>
        /// <returns></returns>
        protected virtual bool AllowIdentifier()
            => false;

        /// <summary>
        ///     optionally continue a syntax part by a terminal symbol
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <param name="parent">parent node</param>
        /// <param name="tokenKind">expected token kind</param>
        /// <returns>syntax tree node</returns>
        protected T CreateByTerminal<T>(ISyntaxPart parent, params int[] tokenKind)
            where T : ISyntaxPart, new() {
            var result = CreateChild<T>(parent);
            if (Match(tokenKind)) {
                var terminal = new Terminal(CurrentToken());
                terminal.Parent = result;
                result.Parts.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind);
            }

            FetchNextToken();
            return result;
        }

        /// <summary>
        ///     add a terminal node marked as invalid
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected InvalidToken CreateByError(ISyntaxPart parent) {
            var invalid = new InvalidToken(CurrentToken());
            invalid.Parent = parent;
            parent.Parts.Add(invalid);
            FetchNextToken();
            return invalid;
        }

        /// <summary>
        ///     create a syntax part
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <returns></returns>
        protected static T CreateChild<T>(ISyntaxPart parent)
            where T : ISyntaxPart, new() {
            var result = new T();
            if (parent != null) {
                result.Parent = parent;
                parent.Parts.Add(result);
            }
            return result;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind"></param>
        protected void ContinueWithOrMissing(ISyntaxPart result, params int[] tokenKind) {
            if (!ContinueWith(result, tokenKind)) {
                // add missing token / todo
            }
        }



        /// <summary>
        ///     parse input
        /// </summary>
        /// <returns>parsed input</returns>
        public abstract ISyntaxPart Parse();
    }
}