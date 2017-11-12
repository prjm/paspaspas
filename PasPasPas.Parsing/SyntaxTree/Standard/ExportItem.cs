using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     exported item
    /// </summary>
    public class ExportItem : StandardSyntaxTreeBase {

        /// <summary>
        ///     index parameter
        /// </summary>
        public Expression IndexParameter { get; set; }

        /// <summary>
        ///     name parameter
        /// </summary>
        public Expression NameParameter { get; set; }

        /// <summary>
        ///     parameter list
        /// </summary>
        public FormalParameters Parameters { get; set; }

        /// <summary>
        ///     resident flag
        /// </summary>
        public bool Resident { get; set; }

        /// <summary>
        ///     export name
        /// </summary>
        public Identifier ExportName { get; set; }

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