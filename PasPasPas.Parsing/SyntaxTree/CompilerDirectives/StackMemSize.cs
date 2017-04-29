using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     stack mem size directive
    /// </summary>
    public class StackMemorySize : CompilerDirectiveBase {

        /// <summary>
        ///     maximum stack size
        /// </summary>
        public int? MaxStackSize { get; set; }

        /// <summary>
        ///     minimum stack size
        /// </summary>
        public int? MinStackSize { get; set; }

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
