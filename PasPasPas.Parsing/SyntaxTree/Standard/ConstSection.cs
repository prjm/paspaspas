using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     section for constants
    /// </summary>
    public class ConstSection : StandardSyntaxTreeBase {

        /// <summary>
        ///     kind of constant
        /// </summary>
        public int Kind { get; set; }
                = TokenKind.Undefined;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}
