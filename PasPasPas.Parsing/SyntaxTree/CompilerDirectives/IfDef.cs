using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     conditial compilation directive (ifdef)
    /// </summary>
    public class IfDef : CompilerDirectiveBase {

        /// <summary>
        ///     inverts the the check for the symbol ("ifndef")
        /// </summary>
        public bool Negate { get; set; }

        /// <summary>
        ///     symbol to check
        /// </summary>
        public string SymbolName { get; set; }

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
