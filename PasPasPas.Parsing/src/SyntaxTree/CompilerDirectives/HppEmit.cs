using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     hpp emit
    /// </summary>
    public class HppEmit : CompilerDirectiveBase {

        /// <summary>
        ///     value to emit
        /// </summary>
        public string EmitValue { get; set; }

        /// <summary>
        ///     emit mode
        /// </summary>
        public HppEmitMode Mode { get; set; }

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
