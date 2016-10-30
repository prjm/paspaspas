using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     basic representation of a compilation unit
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
        protected override string InternalSymbolName
            => UnitName?.CompleteName;

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
                foreach (var attribute in AssemblyAttributes)
                    yield return attribute;
                foreach (var unit in RequiredUnits)
                    yield return unit;
                foreach (var unit in InterfaceSymbols)
                    yield return unit;
                foreach (var unit in ImplementationSymbols)
                    yield return unit;
            }
        }

        /// <summary>
        ///     assembly attributes
        /// </summary>
        public IEnumerable<SymbolAttribute> AssemblyAttributes
            => assemblyAttributes;

        /// <summary>
        ///     list of assembly attributes
        /// </summary>
        private IList<SymbolAttribute> assemblyAttributes
            = new List<SymbolAttribute>();

        /// <summary>
        ///     add an assembly attribute
        /// </summary>
        /// <param name="assemblyAttribute"></param>
        public void AddAssemblyAttribute(SymbolAttribute assemblyAttribute) {
            assemblyAttributes.Add(assemblyAttribute);
        }
    }
}