using System;
using System.IO;
using PasPasPas.Globals.Log;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     validate structure elements
    /// </summary>
    public class StructureValidator : IStartVisitor<CompilationUnit> {

        /// <summary>
        ///     visit syntax part structure
        /// </summary>
        public StructureValidator()
            => visitor = new Visitor(this);

        /// <summary>
        ///     visit a compilation unit
        /// </summary>
        /// <param name="element">unit to visit</param>
        public void StartVisit(CompilationUnit element) {

            if (!string.Equals(Path.GetFileNameWithoutExtension(element.FilePath?.Path), element.SymbolName, StringComparison.OrdinalIgnoreCase)) {
                Log.LogError(StructuralErrors.UnitNameDoesNotMatchFileName, element.FilePath?.Path, element.SymbolName);
            }

        }

        private readonly IStartEndVisitor visitor;

        /// <summary>
        ///     common visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Manager { get; set; }

        /// <summary>
        ///     log source
        /// </summary>
        private LogSource logSource
            = null;

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource Log {
            get {
                if (logSource != null)
                    return logSource;

                if (Manager != null) {
                    logSource = new LogSource(Manager, MessageGroups.StructureValidation);
                    return logSource;
                }

                return null;
            }
        }

    }
}
