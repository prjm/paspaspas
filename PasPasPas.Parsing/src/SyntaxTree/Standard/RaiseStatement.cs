using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     raise statemenmt
    /// </summary>
    public class RaiseStatement : StandardSyntaxTreeBase {

        /// <summary>
        ///     at part
        /// </summary>
        public Expression At { get; set; }

        /// <summary>
        ///     raise part
        /// </summary>
        public Expression Raise { get; set; }

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