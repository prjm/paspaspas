using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     parameter definitions
    /// </summary>
    public class ParameterDefinitionCollection : CombinedSymbolTableBaseCollection<ParameterTypeDefinition, ParameterDefinition> {

        /// <summary>
        ///     log duplicate parameter
        /// </summary>
        /// <param name="newDuplicate">duplicate parameter</param>
        /// <param name="logSource">log source</param>
        protected override void LogDuplicateSymbolError(ParameterDefinition newDuplicate, ILogSource logSource)
            => logSource.LogError(StructuralErrors.DuplicateParameterName, newDuplicate);


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Items, visitor);
            visitor.EndVisit(this);
        }
    }
}
