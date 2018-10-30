using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     tokenizer mode
    /// </summary>
    public enum TokenizerMode {

        /// <summary>
        ///     undefined tokenizer mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     standard tokenizer
        /// </summary>
        Standard = 1,

        /// <summary>
        ///     compiler directive tokenizer
        /// </summary>
        CompilerDirective = 2
    };

    /// <summary>
    ///     pool of token sequences
    /// </summary>
    public class TokenSequences : ObjectPool<TokenizerWithLookahead.TokenSequence> {

        /// <summary>
        ///     prepare a token sequence
        /// </summary>
        /// <param name="entry"></param>
        protected override void Prepare(TokenizerWithLookahead.TokenSequence entry)
            => entry.Clear();

    }

    /// <summary>
    ///     base class for a tokenizer with a lookahead list
    /// </summary>
    public sealed class TokenizerWithLookahead : ITokenizer, IDisposable {

        /// <summary>
        ///     a sequence of fetched tokens
        /// </summary>
        public class TokenSequence {

            /// <summary>
            ///     token value
            /// </summary>
            public Token Value { get; set; }

            /// <summary>
            ///     token prefix
            /// </summary>
            public ImmutableArray<Token> Prefix
                => prefix;

            private ImmutableArray<Token> prefix;

            /// <summary>
            ///     token suffix (invalid to parser)
            /// </summary>
            public ImmutableArray<Token> Suffix
                => suffix;

            private ImmutableArray<Token> suffix;

            private void BuildArray(Queue<Token> tokens, ref ImmutableArray<Token> target) {
                switch (tokens.Count) {

                    case 0:
                        target = ImmutableArray<Token>.Empty;
                        break;

                    case 1:
                        target = ImmutableArray.Create(tokens.Dequeue());
                        break;

                    case 2:
                        target = ImmutableArray.Create(tokens.Dequeue(), tokens.Dequeue());
                        break;

                    case 3:
                        target = ImmutableArray.Create(tokens.Dequeue(), tokens.Dequeue(), tokens.Dequeue());
                        break;

                    case 4:
                        target = ImmutableArray.Create(tokens.Dequeue(), tokens.Dequeue(), tokens.Dequeue(), tokens.Dequeue());
                        break;

                    default:
                        var builder = ListPools.GetImmutableArrayBuilder(tokens);

                        while (tokens.Count > 0)
                            builder.Add(tokens.Dequeue());

                        target = builder.MoveToImmutable();
                        break;
                };
            }

            /// <summary>
            ///     gets the buffer current prefix of invalid tokens
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="environment"></param>
            public void AssignPrefix(Queue<Token> tokens, IParserEnvironment environment)
                => BuildArray(tokens, ref prefix);

            /// <summary>
            ///     get the current suffix of invalid tokens
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="environment"></param>
            public void AssignSuffix(Queue<Token> tokens, IParserEnvironment environment)
                => BuildArray(tokens, ref suffix);

            /// <summary>
            ///     clear the tokenizer
            /// </summary>
            public void Clear() {
                prefix = default;
                suffix = default;
                Value = default;
            }

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            /// <param name="tokenKind"></param>
            /// <returns></returns>
            public bool MatchesKind(int tokenKind)
                => Value.Kind == tokenKind;

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            public bool MatchesKind(int tokenKind1, int tokenKind2)
                => Value.Kind == tokenKind1 || Value.Kind == tokenKind2;

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            public bool MatchesKind(int tokenKind1, int tokenKind2, int tokenKind3)
                => Value.Kind == tokenKind1 || Value.Kind == tokenKind2 || Value.Kind == tokenKind3;

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            public bool MatchesKind(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4)
                => Value.Kind == tokenKind1 || Value.Kind == tokenKind2 || Value.Kind == tokenKind3 ||
                    Value.Kind == tokenKind4;

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            public bool MatchesKind(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5)
                => Value.Kind == tokenKind1 || Value.Kind == tokenKind2 || Value.Kind == tokenKind3 ||
                    Value.Kind == tokenKind4 || Value.Kind == tokenKind5;

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            public bool MatchesKind(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6)
                => Value.Kind == tokenKind1 || Value.Kind == tokenKind2 || Value.Kind == tokenKind3 ||
                    Value.Kind == tokenKind4 || Value.Kind == tokenKind5 || Value.Kind == tokenKind6;

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            public bool MatchesKind(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7)
                => Value.Kind == tokenKind1 || Value.Kind == tokenKind2 || Value.Kind == tokenKind3 ||
                    Value.Kind == tokenKind4 || Value.Kind == tokenKind5 || Value.Kind == tokenKind6 ||
                    Value.Kind == tokenKind7;

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            public bool MatchesKind(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8, int tokenKind9, int tokenKind10)
                => Value.Kind == tokenKind1 || Value.Kind == tokenKind2 || Value.Kind == tokenKind3 ||
                    Value.Kind == tokenKind4 || Value.Kind == tokenKind5 || Value.Kind == tokenKind6 ||
                    Value.Kind == tokenKind7 || Value.Kind == tokenKind8 || Value.Kind == tokenKind9 || Value.Kind == tokenKind10;

            /// <summary>
            ///     test if one of the selected tokens matches
            /// </summary>
            public bool MatchesKind(int tokenKind1, int tokenKind2, int tokenKind3, int tokenKind4, int tokenKind5, int tokenKind6, int tokenKind7, int tokenKind8, int tokenKind9)
                => Value.Kind == tokenKind1 || Value.Kind == tokenKind2 || Value.Kind == tokenKind3 ||
                    Value.Kind == tokenKind4 || Value.Kind == tokenKind5 || Value.Kind == tokenKind6 ||
                    Value.Kind == tokenKind7 || Value.Kind == tokenKind8 || Value.Kind == tokenKind9;

        }

        private OptionSet options;
        private readonly TokenizerMode mode = TokenizerMode.Undefined;
        private readonly IParserEnvironment environment;
        private readonly IRuntimeValueFactory constValues;

        /// <summary>
        ///     create a new tokenizer with lookahead
        /// </summary>
        public TokenizerWithLookahead(IParserEnvironment env, OptionSet optionsSet, ITokenizer baseTokenizer, TokenizerMode tokenizerMode) {
            mode = tokenizerMode;
            BaseTokenizer = baseTokenizer;
            options = optionsSet;
            environment = env;
            constValues = env.Runtime;
        }

        /// <summary>
        ///     base tokenizer
        /// </summary>
        public ITokenizer BaseTokenizer { get; private set; }

        /// <summary>
        ///     list of tokens
        /// </summary>
        private IndexedQueue<PoolItem<TokenSequence>> tokenList
            = new IndexedQueue<PoolItem<TokenSequence>>();

        /// <summary>
        ///     list of invalid tokens (e.g. whitespace)
        /// </summary>
        private Queue<Token> invalidTokens
            = new Queue<Token>();

        /// <summary>
        ///     check if a next token exists
        /// </summary>
        public bool HasNextToken
            => (tokenList.Count > 0) || !BaseTokenizer.AtEof;

        /// <summary>
        ///     fetch a next token / fill token list
        /// </summary>
        private void InternalFetchNextToken() {
            var currentTokenCount = tokenList.Count;

            while (tokenList.Count == currentTokenCount || tokenList.Count < 2) {

                if (BaseTokenizer.AtEof) {
                    if (tokenList.Count > 0) {
                        var item = tokenList.Last.Item;
                        item.AssignSuffix(invalidTokens, environment);
                    }
                    return;
                }

                var currentInput = BaseTokenizer.Input.CurrentFile;
                BaseTokenizer.FetchNextToken();
                var nextToken = BaseTokenizer.CurrentToken;

                if (IsValidToken(ref nextToken)) {
                    var entry = environment.TokenSequencePool.Borrow();
                    entry.Item.Value = nextToken;
                    entry.Item.AssignPrefix(invalidTokens, environment);
                    tokenList.Enqueue(entry);
                    Histograms.Value("TokenPrefixLength", entry.Item.Prefix.Length);
                }
                else {
                    if (IsMacroToken(ref nextToken))
                        ProcessMacroToken(currentInput, ref nextToken);
                    invalidTokens.Enqueue(nextToken);
                }
            }
        }

        /// <summary>
        ///     check if a given token is a valid token
        /// </summary>
        /// <param name="nextToken"></param>
        /// <returns></returns>
        private bool IsValidToken(ref Token nextToken) {

            switch (mode) {

                case TokenizerMode.Standard:
                    return (nextToken.Kind != TokenKind.WhiteSpace) &&
                        nextToken.Kind != TokenKind.ControlChar &&
                        nextToken.Kind != TokenKind.Comment &&
                        nextToken.Kind != TokenKind.Preprocessor &&
                        (!options.ConditionalCompilation.Skip);

                case TokenizerMode.CompilerDirective:
                    return nextToken.Kind != TokenKind.WhiteSpace &&
                        nextToken.Kind != TokenKind.ControlChar;

                default:
                    return true;
            }
        }

        /// <summary>
        ///     test if a token is a macro token
        /// </summary>
        /// <param name="nextToken">token to test</param>
        /// <returns><c>true</c> if the token is a macro token</returns>
        private bool IsMacroToken(ref Token nextToken) {
            if (mode == TokenizerMode.Standard)
                return nextToken.Kind == TokenKind.Preprocessor;
            return false;
        }

        /// <summary>
        ///     do nothing
        /// </summary>
        /// <param name="nextToken"></param>
        /// <param name="path">current path</param>
        private void ProcessMacroToken(FileReference path, ref Token nextToken) {
            var patterns = environment.Patterns.CompilerDirectivePatterns;
            using (var reader = new StackedFileReader()) {
                var macroValue = nextToken.ParsedValue as IStringValue;
                reader.AddStringToRead(path, macroValue.AsUnicodeString);

                using (var parser = new CompilerDirectiveParser(environment, options, reader)) {
                    var result = parser.Parse();
                    var visitor = new CompilerDirectiveVisitor(options, path, Log);
                    result.Accept(visitor.AsVisitor());
                }
            }
        }

        /// <summary>
        ///     gets the current token
        /// </summary>
        public Token CurrentToken
            => LookAhead(0).Value;

        /// <summary>
        ///     gets the current token
        /// </summary>
        public TokenSequence CurrentTokenSequence
            => LookAhead(0);

        /// <summary>
        ///     check if <c>eof</c> is reached
        /// </summary>
        public bool AtEof
            => tokenList.Count < 1 && (BaseTokenizer == null || BaseTokenizer.AtEof);

        /// <summary>
        ///     registered keywords
        /// </summary>
        public IDictionary<string, int> Keywords =>
            BaseTokenizer.Keywords;

        /// <summary>
        ///     file input
        /// </summary>
        public StackedFileReader Input
            => BaseTokenizer.Input;

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log
            => BaseTokenizer.Log;

        /// <summary>
        ///     get tokens and look ahead
        /// </summary>
        /// <param name="number">number of tokens to look ahead</param>
        /// <returns>token</returns>
        public TokenSequence LookAhead(int number) {
            checked {
                while (!BaseTokenizer.AtEof && (tokenList.Count < Math.Max(2, 1 + number))) {
                    InternalFetchNextToken();
                }
            }

            if (tokenList.Count <= number) {
                return new TokenSequence() {
                    Value = Token.Empty
                };
            }
            else {
                return tokenList[number].Item;
            }
        }

        /// <summary>
        ///     fetches the next token
        /// </summary>
        /// <returns></returns>
        public void FetchNextToken() {
            var result = LookAhead(0);
            if (tokenList.Count > 0) {
                var data = tokenList.Dequeue();
                data.Dispose();
                data = null;
            }
        }

        /// <summary>
        ///     dispose the basic tokenizer
        /// </summary>
        public void Dispose() {
            while (tokenList != null && tokenList.Count > 0) {
                var data = tokenList.Dequeue();
                data.Dispose();
                data = null;
            }

            if (BaseTokenizer != null) {
                BaseTokenizer.Dispose();
                BaseTokenizer = null;
            }
        }
    }
}
