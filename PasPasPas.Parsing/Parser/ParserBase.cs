using PasPasPas.Api;
using System;
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
            = new Guid("{5EFB031C-8B43-4749-9C2E-6670F91D6B4C}");

        /// <summary>
        ///     message: unexpected token
        /// </summary>    
        public static readonly Guid UnexpectedToken
            = new Guid("{62E2B740-13A1-46B6-A3CA-298318753C39}");

        private readonly TokenizerWithLookahead tokenizer;
        private readonly LogSource logSource;

        /// <summary>
        ///     creates a new parser
        /// </summary>
        /// <param name="tokenizerWithLookahead"></param>
        /// <param name="environment">environment</param>
        protected ParserBase(ParserServices environment, TokenizerWithLookahead tokenizerWithLookahead) {
            Environment = environment;
            tokenizer = tokenizerWithLookahead;
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
        ///     Require a token kind
        /// </summary>
        /// <param name="tokenKind">required kind of tokem</param>
        /// <returns>parser token</returns>
        protected PascalToken Require(int tokenKind) {
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
        protected PascalToken Require(params int[] tokenKind) {
            foreach (var token in tokenKind) {
                if (Match(token)) {
                    return Require(token);
                }
            }

            Unexpected();
            return Tokenizer.CreatePseudoToken(PascalToken.Undefined);
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
                matchingType = PascalToken.Undefined;
                return false;
            }

            matchingType = CurrentToken().Kind;
            FetchNextToken();
            return true;
        }

        /// <summary>
        ///     look ahead a few tokens
        /// </summary>
        /// <param name="num">number of tokens to look ahead</param>
        /// <param name="tokenKind">token kind to test for</param>
        /// <returns><c>true</c> if the token kind matches</returns>
        protected virtual bool LookAhead(int num, params int[] tokenKind) {
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
        protected PascalToken CurrentToken()
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
            var stopArray = tokenToStop.Concat(new[] { PascalToken.Eof, PascalToken.Undefined }).ToArray();
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

            result = CreateByTerminal<T>(parent);
            return true;
        }

        /// <summary>
        ///     continue syntax part
        /// </summary>
        /// <param name="tokenKind"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        protected bool ContinueWith(ISyntaxPart part, int tokenKind) {
            if (!Match(tokenKind)) {
                return false;
            }

            var terminal = new Terminal(CurrentToken());
            terminal.Parent = part;
            part.Parts.Add(terminal);
            FetchNextToken();
            return true;

        }

        /// <summary>
        ///     optionally continue a syntax part by a terminal symbol
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <returns></returns>
        protected T CreateByTerminal<T>(ISyntaxPart parent)
            where T : ISyntaxPart, new() {
            var result = CreateChild<T>(parent);
            var terminal = new Terminal(CurrentToken());
            terminal.Parent = result;
            result.Parts.Add(terminal);
            FetchNextToken();
            return result;
        }

        /// <summary>
        ///     create a syntax part
        /// </summary>
        /// <typeparam name="T">parent object</typeparam>
        /// <returns></returns>
        protected T CreateChild<T>(ISyntaxPart parent)
            where T : ISyntaxPart, new() {
            var result = new T();
            if (parent != null) {
                result.Parent = parent;
                parent.Parts.Add(result);
            }
            return result;
        }


        /// <summary>
        ///     parse input
        /// </summary>
        /// <returns>parsed input</returns>
        public abstract ISyntaxPart Parse();
    }
}