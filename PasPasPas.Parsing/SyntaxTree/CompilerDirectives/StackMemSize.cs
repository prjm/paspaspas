using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     stack mem size directive
    /// </summary>
    public class StackMemorySize : CompilerDirectiveBase {

        /// <summary>
        ///     maximum stack size
        /// </summary>
        public ulong? MaxStackSize { get; set; }

        /// <summary>
        ///     minimum stack size
        /// </summary>
        public ulong? MinStackSize { get; set; }

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
