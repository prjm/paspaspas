using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     parameter definitions
    /// </summary>
    public class ParameterDefinitions : CombinedSymbolTableBase<ParameterTypeDefinition, ParameterDefinition> {

        /// <summary>
        ///     log duplicate parameter
        /// </summary>
        /// <param name="newDuplicate">duplicate parameter</param>
        /// <param name="logSource">log source</param>
        protected override void LogDuplicateSymbolError(ParameterDefinition newDuplicate, LogSource logSource)
            => logSource.Error(StructuralErrors.DuplicateParameterName, newDuplicate);


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
