using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     guid declaration
    /// </summary>
    public class InterfaceGuid : StandardSyntaxTreeBase {

        /// <summary>
        ///     guid for this interface
        /// </summary>
        public QuotedString Id { get; set; }

        /// <summary>
        ///     named guid for this interface
        /// </summary>
        public Identifier IdIdentifier { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}