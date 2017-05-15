using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     library head
    /// </summary>
    public class LibraryHead : StandardSyntaxTreeBase {

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints { get; set; }

        /// <summary>
        ///     library name
        /// </summary>
        public NamespaceName LibraryName { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}