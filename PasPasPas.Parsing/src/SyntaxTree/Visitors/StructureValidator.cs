using System;
using System.IO;
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
        /// <param name="unit">unit to visit</param>
        public void StartVisit(CompilationUnit unit) {

            if (!string.Equals(Path.GetFileNameWithoutExtension(unit.FilePath?.Path), unit.SymbolName, StringComparison.OrdinalIgnoreCase)) {
                Log.Error(StructuralErrors.UnitNameDoesNotMatchFileName, unit.FilePath?.Path, unit.SymbolName);
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
        ///     log source group id
        /// </summary>
        public static readonly Guid StructureValidatorGroupId
             = new Guid(new byte[] { 0xcf, 0x1b, 0x1a, 0xa4, 0x41, 0x79, 0xd7, 0x47, 0xb4, 0xc0, 0x53, 0xed, 0xa7, 0xb8, 0x39, 0x37 });
        /* {a41a1bcf-7941-47d7-b4c0-53eda7b83937} */

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource Log {
            get {
                if (logSource != null)
                    return logSource;

                if (Manager != null) {
                    logSource = new LogSource(Manager, StructureValidatorGroupId);
                    return logSource;
                }

                return null;
            }
        }

    }
}
