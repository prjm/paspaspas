using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     project root
    /// </summary>
    public class ProjectRoot : SymbolTableBase<CompilationUnit> {

        /// <summary>
        ///     log duplicate units
        /// </summary>
        /// <param name="newDuplicate">duplicate</param>
        /// <param name="log"></param>
        protected override void LogDuplicateSymbolError(CompilationUnit newDuplicate, LogSource log) {
            log.Error(StructuralErrors.DuplicateUnitName, newDuplicate);
        }

    }
}
