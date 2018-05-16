using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper
    /// </summary>
    public class RecordHelperDefinition : StandardSyntaxTreeBase {

        /// <summary>
        ///     record helper items
        /// </summary>
        public RecordHelperItems Items { get; set; }

        /// <summary>
        ///     record helper name
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