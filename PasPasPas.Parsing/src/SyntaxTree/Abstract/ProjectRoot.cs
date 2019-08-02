using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     project root
    /// </summary>
    public class ProjectItemCollection : SymbolTableBaseCollection<CompilationUnit> {

        /// <summary>
        ///     Source duplicate units
        /// </summary>
        /// <param name="newDuplicate">duplicate</param>
        /// <param name="logSource"></param>
        protected override void LogDuplicateSymbolError(CompilationUnit newDuplicate, ILogSource logSource)
            => logSource.LogError(StructuralErrors.DuplicateUnitName, newDuplicate);

        /// <summary>
        ///     project name
        /// </summary>
        public string ProjectName {
            get {
                var result = string.Empty;

                for (var i = 0; i < Count; i++) {

                    var item = this[i];

                    if (item.FileType == CompilationUnitType.Unit)
                        continue;

                    if (item.FileType == CompilationUnitType.Unknown)
                        continue;

                    if (!string.IsNullOrEmpty(result))
                        return string.Empty;

                    result = item.SymbolName;
                }

                return result;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart<ProjectItemCollection, CompilationUnit>(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
