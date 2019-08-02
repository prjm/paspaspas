using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     region directive
    /// </summary>
    public class Region : CompilerDirectiveBase {

        private readonly Terminal symbol;
        private readonly Terminal regionName;

        /// <summary>
        ///     create a new region directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="regionName"></param>
        /// <param name="region"></param>
        public Region(Terminal symbol, Terminal regionName, string region) {
            this.symbol = symbol;
            this.regionName = regionName;
            RegionName = region;
        }

        /// <summary>
        ///     region name
        /// </summary>
        public string RegionName { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, regionName, visitor);
            visitor.EndVisit(this);
        }




    }
}
