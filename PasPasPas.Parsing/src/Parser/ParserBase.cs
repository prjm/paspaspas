﻿using System;
using System.Collections.Generic;
using System.Text;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree;
using System.Linq;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Options.Bundles;

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
            logSource.Error(UnexpectedToken, token.Kind, token.Value);
            FetchNextToken();
        }

        /// <summary>
        ///     parser syntax error
        /// </summary>
        /// <param name="message">message id</param>
        /// <param name="parent">parent syntax tree node</param>
        /// <param name="expectedTokens">expected tokens (or)</param>
        /// <returns></returns>
        protected void ErrorAndSkip(IExtendableSyntaxPart parent, Guid message, int[] expectedTokens) {
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
        protected void ErrorLastPart(IExtendableSyntaxPart parent, Guid message, params object[] values)
            => logSource.Error(message, values);

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
        protected virtual bool LookAhead(int numberOfTokens, params int[] tokenKind) {
            var token = tokenizer.LookAhead(numberOfTokens);
            return token.MatchesKind(tokenKind);
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
            /*
            MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);

            foreach (MethodInfo method in methods) {
                RuleAttribute attr = method.GetCustomAttribute<RuleAttribute>(false);
                if (attr != null) {
                    result.Add(attr);
                }
            }
            */
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
            var stopArray = tokenToStop.Concat(new[] { TokenKind.Undefined }).ToArray();
            while (!LookAhead(lookahead, stopArray)) {
                if (LookAhead(lookahead, tokenToSearch))
                    return true;
                lookahead++;
            }

            return false;
        }

        /// <summary>
        ///     test if the searched tokens match until a given token is reached
        /// </summary>
        /// <param name="tokenToStop">search token</param>
        /// <param name="allowedTokens">stop token</param>
        /// <returns></returns>
        protected Tuple<bool, int> HasTokenUntilToken(int[] tokenToStop, params int[] allowedTokens) {
            var lookahead = 1;
            var stopArray = new[] { TokenKind.Undefined };

            while (!LookAhead(lookahead, tokenToStop) && !LookAhead(lookahead, stopArray)) {
                if (!LookAhead(lookahead, allowedTokens))
                    return new Tuple<bool, int>(false, lookahead);
                lookahead++;
            }

            return new Tuple<bool, int>(true, lookahead);
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

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            FetchNextToken();
            return terminal;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        [Obsolete("Changed syntax tree")]
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind) {
            var requiresIdentifier = tokenKind == TokenKind.Identifier;

            if (!Match(tokenKind) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind1, int tokenKind2) {
            var requiresIdentifier = (tokenKind1 == TokenKind.Identifier) || (tokenKind2 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return false;
            }

            if (!Match(tokenKind1, tokenKind2) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
        }


        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind1, int tokenKind2, int tokenKind3) {
            var requiresIdentifier = (tokenKind1 == TokenKind.Identifier) || (tokenKind2 == TokenKind.Identifier) || (tokenKind3 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return false;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            var requiresIdentifier =
                (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return false;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
        }


        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) {
            var requiresIdentifier = (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier) ||
                (tokenKind5 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return false;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
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
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) {
            var requiresIdentifier = (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier) ||
                (tokenKind5 == TokenKind.Identifier) ||
                (tokenKind6 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return false;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
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
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7) {
            var requiresIdentifier = (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier) ||
                (tokenKind5 == TokenKind.Identifier) ||
                (tokenKind6 == TokenKind.Identifier) ||
                (tokenKind7 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return false;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
        }

        /// <summary>
        ///     continue with token
        /// </summary>
        /// <param name="part"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        /// <param name="tokenKind7"></param>
        /// <param name="tokenKind8"></param>
        /// <returns></returns>
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8) {
            var requiresIdentifier = (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier) ||
                (tokenKind5 == TokenKind.Identifier) ||
                (tokenKind6 == TokenKind.Identifier) ||
                (tokenKind7 == TokenKind.Identifier) ||
                (tokenKind8 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return false;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
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
        /// <param name="tokenKind9"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(IExtendableSyntaxPart part, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8, int tokenKind9) {
            var requiresIdentifier = (tokenKind1 == TokenKind.Identifier) ||
                (tokenKind2 == TokenKind.Identifier) ||
                (tokenKind3 == TokenKind.Identifier) ||
                (tokenKind4 == TokenKind.Identifier) ||
                (tokenKind5 == TokenKind.Identifier) ||
                (tokenKind6 == TokenKind.Identifier) ||
                (tokenKind8 == TokenKind.Identifier) ||
                (tokenKind7 == TokenKind.Identifier) ||
                (tokenKind9 == TokenKind.Identifier);

            if (!Tokenizer.HasNextToken) {
                return false;
            }

            if (!Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8, tokenKind9) &&
                (!requiresIdentifier ||
                (requiresIdentifier && !AllowIdentifier()))) {
                return false;
            }

            var terminal = new Terminal(tokenizer.CurrentTokenSequence);
            part.Add(terminal);
            FetchNextToken();
            return true;
        }

        /// <summary>
        ///     create a syntax tree node by terminal
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parent"></param>
        /// <param name="tokenKind"></param>
        protected void InitByTerminal(IExtendableSyntaxPart result, IExtendableSyntaxPart parent, int tokenKind) {

            if (parent != null)
                parent.Add(result);

            if (Match(tokenKind)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind);
            }

            FetchNextToken();
        }

        /// <summary>
        ///     init syntax node by terminal node
        /// </summary>
        /// <param name="result">result</param>
        /// <param name="parent">parent node</param>
        /// <param name="tokenKind1">token kind 1</param>
        /// <param name="tokenKind2">token kind 2</param>
        protected void InitByTerminal(IExtendableSyntaxPart result, IExtendableSyntaxPart parent, int tokenKind1, int tokenKind2) {
            parent.Add(result);

            if (Match(tokenKind1, tokenKind2)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2);
            }

            FetchNextToken();
        }

        /// <summary>
        ///     create a syntax tree node based on a terminal
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parent"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        protected void InitByTerminal(IExtendableSyntaxPart result, IExtendableSyntaxPart parent, int tokenKind1, int tokenKind2, int tokenKind3) {
            parent.Add(result);

            if (Match(tokenKind1, tokenKind2, tokenKind3)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3);
            }

            FetchNextToken();
        }

        /// <summary>
        ///     init syntax node by terminal node
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parent"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        protected void InitByTerminal(IExtendableSyntaxPart result, IExtendableSyntaxPart parent, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            parent.Add(result);

            if (Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4);
            }

            FetchNextToken();
        }

        /// <summary>
        ///     create a syntax tree node based on a termina
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parent"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        protected void InitByTerminal(IExtendableSyntaxPart result, IExtendableSyntaxPart parent, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) {
            parent.Add(result);

            if (Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5);
            }

            FetchNextToken();
        }

        /// <summary>
        ///     init syntax node by terminal node
        /// </summary>
        /// <param name="result"></param>
        /// <param name="parent"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        protected void InitByTerminal(IExtendableSyntaxPart result, IExtendableSyntaxPart parent, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) {
            parent.Add(result);

            if (Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6);
            }

            FetchNextToken();
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
        /// <param name="result">result node</param>
        /// <param name="tokenKind">expected token kind</param>
        /// <returns>syntax tree node</returns>
        protected T CreateByTerminal<T>(T result, int tokenKind) where T : IExtendableSyntaxPart {

            if (Match(tokenKind)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind);
            }

            FetchNextToken();
            return result;
        }

        /// <summary>
        ///     optionally continue a syntax part by a terminal symbol
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <param name="result">result</param>
        /// <param name="tokenKind1">expected token kind</param>
        /// <param name="tokenKind2">expected token kind</param>///
        /// <returns>syntax tree node</returns>
        protected T CreateByTerminal<T>(T result, int tokenKind1, int tokenKind2) where T : IExtendableSyntaxPart {

            if (Match(tokenKind1, tokenKind2)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2);
            }

            FetchNextToken();
            return result;
        }

        /// <summary>
        ///     optionally continue a syntax part by a terminal symbol
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <param name="result">result node</param>
        /// <param name="tokenKind1">expected token kind</param>
        /// <param name="tokenKind2">expected token kind</param>
        /// <param name="tokenKind3">expected token kind</param>
        /// <returns>syntax tree node</returns>
        protected T CreateByTerminal<T>(T result, int tokenKind1, int tokenKind2, int tokenKind3) where T : IExtendableSyntaxPart {

            if (Match(tokenKind1, tokenKind2, tokenKind3)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3);
            }

            FetchNextToken();
            return result;
        }

        /// <summary>
        ///     optionally continue a syntax part by a terminal symbol
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <param name="result">result</param>
        /// <param name="tokenKind1">expected token kind</param>
        /// <param name="tokenKind2">expected token kind</param>
        /// <param name="tokenKind3">expected token kind</param>
        /// <param name="tokenKind4">expected token kind</param>
        /// <returns>syntax tree node</returns>
        protected T CreateByTerminal<T>(T result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) where T : IExtendableSyntaxPart {

            if (Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4);
            }

            FetchNextToken();
            return result;
        }


        /// <summary>
        ///     optionally continue a syntax part by a terminal symbol
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <param name="result">result</param>
        /// <param name="tokenKind1">expected token kind</param>
        /// <param name="tokenKind2">expected token kind</param>
        /// <param name="tokenKind3">expected token kind</param>
        /// <param name="tokenKind4">expected token kind</param>
        /// <param name="tokenKind5">expected token kind</param>
        /// <returns>syntax tree node</returns>
        protected T CreateByTerminal<T>(T result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) where T : IExtendableSyntaxPart {

            if (Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5);
            }

            FetchNextToken();
            return result;
        }


        /// <summary>
        ///     optionally continue a syntax part by a terminal symbol
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <param name="result">result</param>
        /// <param name="tokenKind1">expected token kind</param>
        /// <param name="tokenKind2">expected token kind</param>
        /// <param name="tokenKind3">expected token kind</param>
        /// <param name="tokenKind4">expected token kind</param>
        /// <param name="tokenKind5">expected token kind</param>
        /// <param name="tokenKind6">expected token kind</param>
        /// <returns>syntax tree node</returns>
        protected T CreateByTerminal<T>(T result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) where T : IExtendableSyntaxPart {

            if (Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6);
            }

            FetchNextToken();
            return result;
        }

        /// <summary>
        ///     optionally continue a syntax part by a terminal symbol
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <param name="result">result</param>
        /// <param name="tokenKind1">expected token kind</param>
        /// <param name="tokenKind2">expected token kind</param>
        /// <param name="tokenKind3">expected token kind</param>
        /// <param name="tokenKind4">expected token kind</param>
        /// <param name="tokenKind5">expected token kind</param>
        /// <param name="tokenKind6">expected token kind</param>
        /// <param name="tokenKind7">expected token kind</param>
        /// <param name="tokenKind8">expected token kind</param>
        /// <returns>syntax tree node</returns>
        protected T CreateByTerminal<T>(T result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8) where T : IExtendableSyntaxPart, new() {

            if (Match(tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8)) {
                var terminal = new Terminal(tokenizer.CurrentTokenSequence);
                result.Add(terminal);
            }
            else {
                ContinueWithOrMissing(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8);
            }

            FetchNextToken();
            return result;
        }


        /// <summary>
        ///     add a terminal node marked as invalid
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        protected Terminal CreateByError(IExtendableSyntaxPart parent) {
            var invalid = new Terminal(tokenizer.CurrentTokenSequence) {
                ParentItem = parent
            };
            parent.Add(invalid);
            FetchNextToken();
            return invalid;
        }

        /// <summary>
        ///     create a syntax part
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <returns></returns>
        [Obsolete]
        protected static T CreateChild<T>(IExtendableSyntaxPart parent)
            where T : IExtendableSyntaxPart, new() {
            var result = new T();
            if (parent != null) {
                result.ParentItem = parent;
                parent.Add(result);
            }
            return result;
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind"></param>
        [Obsolete]
        protected void ContinueWithOrMissing(IExtendableSyntaxPart result, int tokenKind) {
            if (!ContinueWith(result, tokenKind)) {
                ErrorMissingToken(tokenKind);
            }
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="tokenKind"></param>
        protected Terminal ContinueWithOrMissing(int tokenKind) {
            var terminal = ContinueWith(tokenKind);

            if (terminal == null) {
                ErrorMissingToken(tokenKind);
            }
            return terminal;
        }

        /// <summary>
        ///     report a missing token
        /// </summary>
        /// <param name="tokenKind"></param>
        protected void ErrorMissingToken(int tokenKind)
            => logSource.Error(MissingToken, tokenKind);

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        protected void ContinueWithOrMissing(IExtendableSyntaxPart result, int tokenKind1, int tokenKind2) {
            if (!ContinueWith(result, tokenKind1, tokenKind2)) {
                logSource.Error(MissingToken, tokenKind1, tokenKind2);
                // add missing token / todo
            }
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        protected void ContinueWithOrMissing(IExtendableSyntaxPart result, int tokenKind1, int tokenKind2, int tokenKind3) {
            if (!ContinueWith(result, tokenKind1, tokenKind2, tokenKind3)) {
                logSource.Error(MissingToken, tokenKind1, tokenKind2, tokenKind3);
                // add missing token / todo
            }
        }

        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        protected void ContinueWithOrMissing(IExtendableSyntaxPart result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4) {
            if (!ContinueWith(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4)) {
                logSource.Error(MissingToken, tokenKind1, tokenKind2, tokenKind3, tokenKind4);
                // add missing token / todo
            }
        }


        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        protected void ContinueWithOrMissing(IExtendableSyntaxPart result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5) {
            if (!ContinueWith(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5)) {
                logSource.Error(MissingToken, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5);
                // add missing token / todo
            }
        }



        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        protected void ContinueWithOrMissing(IExtendableSyntaxPart result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6) {
            if (!ContinueWith(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6)) {
                logSource.Error(MissingToken, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6);
                // add missing token / todo
            }
        }



        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        /// <param name="tokenKind7"></param>
        protected void ContinueWithOrMissing(IExtendableSyntaxPart result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7) {
            if (!ContinueWith(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7)) {
                logSource.Error(MissingToken, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7);
                // add missing token / todo
            }
        }


        /// <summary>
        ///     continue with a specific token and mark it as missing
        /// </summary>
        /// <param name="result"></param>
        /// <param name="tokenKind1"></param>
        /// <param name="tokenKind2"></param>
        /// <param name="tokenKind3"></param>
        /// <param name="tokenKind4"></param>
        /// <param name="tokenKind5"></param>
        /// <param name="tokenKind6"></param>
        /// <param name="tokenKind7"></param>
        /// <param name="tokenKind8"></param>
        protected void ContinueWithOrMissing(IExtendableSyntaxPart result, int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8) {
            if (!ContinueWith(result, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8)) {
                logSource.Error(MissingToken, tokenKind1, tokenKind2, tokenKind3, tokenKind4, tokenKind5, tokenKind6, tokenKind7, tokenKind8);
                // add missing token / todo
            }
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
            if (Tokenizer != null) {
                tokenizer.Dispose();
                tokenizer = null;
            }
        }

    }
}