using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessor for disp interfaces
    /// </summary>
    public class ClassPropertyDispInterface : StandardSyntaxTreeBase {

        /// <summary>
        ///     Disp id directive
        /// </summary>
        public DispIdDirective DispId { get; set; }

        /// <summary>
        ///     readonly
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        ///    write only
        /// </summary>
        public bool WriteOnly { get; set; }

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