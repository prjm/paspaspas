using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface item
    /// </summary>
    public class InterfaceItem : StandardSyntaxTreeBase {

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethodSymbol Method { get; set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassPropertySymbol Property { get; set; }

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