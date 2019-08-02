using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     switch to deny units in packages
    /// </summary>
    public class ParseDenyPackageUnit : CompilerDirectiveBase {

        /// <summary>
        ///     parse a deny package unit directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="denyUnit"></param>
        public ParseDenyPackageUnit(Terminal symbol, Terminal mode, DenyUnitInPackage denyUnit) {
            Symbol = symbol;
            Mode = mode;
            DenyUnit = denyUnit;
        }

        /// <summary>
        ///     switch value
        /// </summary>
        public DenyUnitInPackage DenyUnit { get; }

        /// <summary>
        ///     mode
        /// </summary>
        public Terminal Mode { get; }

        /// <summary>
        ///     symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, Mode, visitor);
            visitor.EndVisit(this);
        }


    }
}
