using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     parameter definitions
    /// </summary>
    public class ParameterDefinitions : SymbolTableBase<ParameterDefinition> {

        /// <summary>
        ///     log duplicate parameter
        /// </summary>
        /// <param name="newDuplicate">duplicate parameter</param>
        /// <param name="logSource">log source</param>
        protected override void LogDuplicateSymbolError(ParameterDefinition newDuplicate, LogSource logSource) {
            logSource.Error(StructuralErrors.DuplicateParameterName, newDuplicate);
        }

    }
}
