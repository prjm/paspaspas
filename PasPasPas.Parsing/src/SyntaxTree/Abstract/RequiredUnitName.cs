using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a unit name
    /// </summary>
    public class RequiredUnitName : SymbolTableEntryBase {

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     unit mode
        /// </summary>
        public UnitMode Mode { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     file name of to the unit
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            visitor.EndVisit(this);
        }
    }
}