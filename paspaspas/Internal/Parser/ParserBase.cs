using PasPasPas.Api;
using PasPasPas.Internal.Tokenizer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace PasPasPas.Internal.Parser {

    /// <summary>
    ///     base class for parsers
    /// </summary>
    public class ParserBase : MessageGenerator, IParserInformationProvider {

        private TokenizerWithLookahead tokenizer
            = new TokenizerWithLookahead();

        /// <summary>
        ///     tokenizer to use
        /// </summary>        
        public IPascalTokenizer BaseTokenizer
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
        ///     unexpected input token
        /// </summary>
        /// <returns></returns>
        protected ISyntaxPart Unexpected() {
            LogError(MessageData.UnexpectedToken, CurrentToken().Kind, CurrentToken().Value);
            return null;
        }

        /// <summary>
        ///     Require a token kind
        /// </summary>
        /// <param name="tokenKind">required kind of tokem</param>
        /// <returns>parser token</returns>
        protected PascalToken Require(int tokenKind) {
            if (!Match(tokenKind))
                Unexpected();

            var result = CurrentToken();
            tokenizer.FetchNextToken();
            return result;
        }

        /// <summary>
        ///     require one token of the token list
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <returns></returns>
        protected PascalToken Require(params int[] tokenKind) {
            foreach (var token in tokenKind) {
                if (Match(token)) {
                    return Require(token);
                }
            }

            Unexpected();
            return new PascalToken() { Kind = PascalToken.Undefined, Value = string.Empty };
        }

        /// <summary>
        ///     assume an optional token kind
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <returns>optional token kind</returns>
        protected bool Optional(int tokenKind) {
            if (!Match(tokenKind))
                return false;

            tokenizer.FetchNextToken();
            return true;
        }

        /// <summary>
        ///     assume an optional token kind
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <param name="matchingType">matched token</param>
        /// <returns>optional token kind</returns>
        protected bool Optional(out int matchingType, params int[] tokenKind) {
            if (!Match(tokenKind)) {
                matchingType = PascalToken.Undefined;
                return false;
            }

            matchingType = CurrentToken().Kind;
            tokenizer.FetchNextToken();
            return true;
        }



        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="num">number of tokens to look ahead</param>
        /// <param name="tokenKind">token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected bool LookAhead(int num, params int[] tokenKind) {
            var token = tokenizer.LookAhead(num);
            return tokenKind.Any(t => t == token.Kind);
        }

        /// <summary>
        ///     tests if a token is currently availiable
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <returns></returns>
        protected bool Match(int tokenKind)
            => CurrentToken().Kind == tokenKind;

        /// <summary>
        ///     get the current token
        /// </summary>
        /// <returns></returns>
        protected PascalToken CurrentToken() {
            if (!tokenizer.HasNextToken())
                return new PascalToken() { Kind = PascalToken.Eof, Value = string.Empty };
            return tokenizer.CurrentToken();
        }

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
            var stopArray = tokenToStop.Concat(new[] { PascalToken.Eof, PascalToken.Undefined }).ToArray();
            while (!LookAhead(lookahead, stopArray)) {
                if (LookAhead(lookahead, tokenToSearch))
                    return true;
                lookahead++;
            }

            return false;
        }
    }
}