using PasPasPas.Globals.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     field definitions
    /// </summary>
    public class StructureFieldDefinitionCollection : CombinedSymbolTableBaseCollection<StructureFields, StructureField> {

        /// <summary>
        ///     log duplicate field
        /// </summary>
        /// <param name="newDuplicate">duplicate parameter</param>
        /// <param name="logSource">log source</param>
        protected override void LogDuplicateSymbolError(StructureField newDuplicate, ILogSource logSource)
            => logSource.LogError(StructuralErrors.DuplicateFieldName, newDuplicate);

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
