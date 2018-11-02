using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.Parser.Standard {

    public partial class StandardParser {


        #region ParseSetSection

        /// <summary>
        ///     parse a set section
        /// </summary>
        /// <returns></returns>
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
        public ISyntaxPart ParseSimpleExpression() {
            var leftOperand = ParseTerm();
            var @operator = ContinueWith(TokenKind.Plus, TokenKind.Minus, TokenKind.Or, TokenKind.Xor);

            if (@operator != default) {
                var rightOperand = ParseSimpleExpression();
                return new SimpleExpression(leftOperand, @operator, rightOperand);
            }

            return leftOperand;
        }

        #endregion

        #region ParseTerm

        /// <summary>
        ///     parse a term
        /// </summary>
        /// <returns></returns>
        [Rule("Term", "Factor [ ('*'|'/'|'div'|'mod'|'and'|'shl'|'shr'|'as') Term ]")]
        public ISyntaxPart ParseTerm() {
            var leftOperand = ParseFactor();
            var @operator = ContinueWith(TokenKind.Times, TokenKind.Slash, TokenKind.Div, TokenKind.Mod, TokenKind.And, TokenKind.Shl, TokenKind.Shr, TokenKind.As);

            if (@operator != default) {
                var rightOperand = ParseTerm();
                return new TermSymbol(leftOperand, @operator, rightOperand);
            }

            return leftOperand;
        }

        #endregion

    }
}
