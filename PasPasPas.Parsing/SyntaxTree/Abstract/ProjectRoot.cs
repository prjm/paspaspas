using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     project root
    /// </summary>
    public class ProjectRoot : SymbolTableBase<CompilationUnit> {

        /// <summary>
        ///     Source duplicate units
        /// </summary>
        /// <param name="newDuplicate">duplicate</param>
        /// <param name="logSource"></param>
        protected override void LogDuplicateSymbolError(CompilationUnit newDuplicate, LogSource logSource) {
            logSource.Error(StructuralErrors.DuplicateUnitName, newDuplicate);
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}
