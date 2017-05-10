using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     enumeration value
    /// </summary>
    public class EnumValue : StandardSyntaxTreeBase {

        /// <summary>
        ///     name
        /// </summary>
        public Identifier EnumName { get; set; }

        /// <summary>
        ///     value
        /// </summary>
        public Expression Value { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}