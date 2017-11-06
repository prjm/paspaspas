using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     warn switch
    /// </summary>
    public class WarnSwitch : CompilerDirectiveBase {

        /// <summary>
        ///     warning mode
        /// </summary>
        public WarningMode Mode { get; set; }

        /// <summary>
        ///     warning type
        /// </summary>
        public string WarningType { get; set; }

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
