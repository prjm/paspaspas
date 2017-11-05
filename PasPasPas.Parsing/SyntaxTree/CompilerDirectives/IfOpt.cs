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
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
