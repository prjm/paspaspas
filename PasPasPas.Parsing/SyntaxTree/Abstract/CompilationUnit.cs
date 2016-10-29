using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     
    /// </summary>
    public class CompilationUnit : SymbolTableEntryBase {

        /// <summary>
        ///     unit file type
        /// </summary>
        public CompilationUnitType FileType { get; set; }

        /// <summary>
        ///     Name of the unit
        /// </summary>
        public SymbolName UnitName { get; set; }

        /// <summary>
        ///     name of the unit
        /// </summary>
        public override string SymbolName
        {
            get
            {
                if (UnitName != null)
                    return UnitName.CompleteName;
                else
                    return "Unit_" + GetHashCode();
            }
        }

        /// <summary>
        ///     file reference of the unit
        /// </summary>
        public IFileReference FilePath { get; set; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     required units
        /// </summary>
        public RequiredUnitNameList RequiredUnits { get; }
            = new RequiredUnitNameList();

        /// <summary>
        ///     symbols in the interface part of the unit
        /// </summary>
        public DeclaredSymbols InterfaceSymbols { get; }
            = new DeclaredSymbols();

        /// <summary>
        ///     symbols in the implementation part of the unit
        /// </summary>
        public DeclaredSymbols ImplementationSymbols { get; }
            = new DeclaredSymbols();

        /// <summary>
        ///     get all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (var unit in RequiredUnits)
                    yield return unit;
                foreach (var unit in InterfaceSymbols)
                    yield return unit;
                foreach (var unit in ImplementationSymbols)
                    yield return unit;
            }
        }
    }
}