using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     closure expression
    /// </summary>
    public class ClosureExpression : StandardSyntaxTreeBase {

        /// <summary>
        /// block
        /// </summary>
        public Block Block { get; set; }

        /// <summary>
        ///     closue kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     closure parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     closure return type
        /// </summary>
        public TypeSpecification ReturnType { get; set; }

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