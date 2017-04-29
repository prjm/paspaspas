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
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}