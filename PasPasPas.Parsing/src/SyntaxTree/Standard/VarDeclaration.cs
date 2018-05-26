using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable declaration
    /// </summary>
    public class VarDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes { get; set; }

        /// <summary>
        ///     hints
        /// </summary>
        public ISyntaxPart Hints { get; set; }

        /// <summary>
        ///     var names
        /// </summary>
        public IdentifierList Identifiers { get; set; }

        /// <summary>
        ///     var types
        /// </summary>
        public TypeSpecification TypeDeclaration { get; set; }

        /// <summary>
        ///     var values
        /// </summary>
        public VarValueSpecification ValueSpecification { get; set; }


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