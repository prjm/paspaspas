using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     list of required units
    /// </summary>
    public class RequiredUnitNameListCollection : SymbolTableBaseCollection<RequiredUnitName> {

        /// <summary>
        ///     log duplicated unit name
        /// </summary>
        /// <param name="newDuplicate"></param>
        /// <param name="logSource"></param>
        protected override void LogDuplicateSymbolError(RequiredUnitName newDuplicate, ILogSource logSource) {
            base.LogDuplicateSymbolError(newDuplicate, logSource);
            logSource.LogError(MessageNumbers.RedeclaredUnitNameInUsesList, newDuplicate);
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            foreach (var unit in this)
                AcceptPart(this, unit, visitor);
            visitor.EndVisit(this);
        }
    }
}