using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     method info directive
    /// </summary>
    public class MethodInfo : CompilerDirectiveBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public MethodInfoRtti Mode { get; set; }

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
