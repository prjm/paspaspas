using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method resolution
    /// </summary>
    public class MethodResolution : StandardSyntaxTreeBase {

        /// <summary>
        ///     kind (procedure/function)
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     resolve identifier
        /// </summary>
        public IdentifierSymbol ResolveIdentifier { get; set; }

        /// <summary>
        ///     identifier to be resolved
        /// </summary>
        public TypeName Name { get; set; }

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