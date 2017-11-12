using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     struct type
    /// </summary>
    public class StructType : StandardSyntaxTreeBase {

        /// <summary>
        ///     Packed struct type
        /// </summary>
        public bool Packed { get; set; }

        /// <summary>
        ///     part
        /// </summary>
        public StructTypePart Part { get; set; }

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