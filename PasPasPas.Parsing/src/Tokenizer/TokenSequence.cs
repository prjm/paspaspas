using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    public sealed partial class TokenizerWithLookahead {
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

            /// <summary>
            ///     gets the buffer current prefix of invalid tokens
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="environment"></param>
            public void AssignPrefix(Queue<Token> tokens, IParserEnvironment environment)
                => prefix = environment.TokenArrays.GetTokenArray(tokens);

            /// <summary>
            ///     get the current suffix of invalid tokens
            /// </summary>
            /// <param name="tokens"></param>
            /// <param name="environment"></param>
            public void AssignSuffix(Queue<Token> tokens, IParserEnvironment environment)
                => suffix = environment.TokenArrays.GetTokenArray(tokens);

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
    }
}
