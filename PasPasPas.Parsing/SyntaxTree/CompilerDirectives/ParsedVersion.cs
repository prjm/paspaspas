using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     pe version directive
    /// </summary>
    public class ParsedVersion : CompilerDirectiveBase {

        /// <summary>
        ///     version kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     major version
        /// </summary>
        public int MajorVersion { get; set; }

        /// <summary>
        ///     minor version
        /// </summary>
        public int MinorVersion { get; set; }

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
