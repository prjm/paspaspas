using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.Parser.Standard {

    public partial class StandardParser {


        #region ParseSetSection

        public SetSectionPartSymbol ParseSetSectionPart() {
            var setExpression = ParseExpression();
            var continuation = ContinueWith(TokenKind.Comma, TokenKind.DotDot);
            return new SetSectionPartSymbol(setExpression, continuation);
        }

        /// <summary>
        ///     parse a set section
        /// </summary>
        /// <returns></returns>

        [Rule("SetSection", "'[' [ Expression ] { (',' | '..') Expression } ']'")]
        public SetSectionSymbol ParseSetSection() {
            var openBraces = ContinueWithOrMissing(TokenKind.OpenBraces);
            var items = ImmutableArray<SetSectionPartSymbol>.Empty;

            if (!Match(TokenKind.CloseBraces)) {
                var part = default(SetSectionPartSymbol);
                using (var list = GetList<SetSectionPartSymbol>()) {
                    do {
                        part = AddToList(list, ParseSetSectionPart());
                    } while (part != default && part.Continuation != default);

                    items = GetFixedArray(list);
                }
            }

            var closeBraces = ContinueWithOrMissing(TokenKind.CloseBraces);
            return new SetSectionSymbol(openBraces, items, closeBraces);
        }

        #endregion
        #region ParseSimpleExpression

        /// <summary>
        ///     parse a simple expression
        /// </summary>
        /// <returns></returns>

        [Rule("SimpleExpression", "Term { ('+'|'-'|'or'|'xor') SimpleExpression }")]
        public SimpleExpression ParseSimpleExpression() {
            var leftOperand = ParseTerm();
            var @operator = ContinueWith(TokenKind.Plus, TokenKind.Minus, TokenKind.Or, TokenKind.Xor);
            var rightOperand = default(SimpleExpression);

            if (@operator != default)
                rightOperand = ParseSimpleExpression();

            return new SimpleExpression(leftOperand, @operator, rightOperand);
        }

        #endregion



    }
}
