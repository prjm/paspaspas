using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     ifopt directive
    /// </summary>
    public class IfOpt : CompilerDirectiveBase {

        /// <summary>
        ///     required kind
        /// </summary>
        public string SwitchKind { get; set; }

        /// <summary>
        ///     required info
        /// </summary>
        public SwitchInfo SwitchInfo { get; set; }

        /// <summary>
        ///     required state
        /// </summary>
        public SwitchInfo SwitchState { get; set; }

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
