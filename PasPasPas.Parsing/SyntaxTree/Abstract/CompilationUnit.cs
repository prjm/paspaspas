using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     basic representation of a compilation unit
    /// </summary>
    public class CompilationUnit : SymbolTableEntryBase, IStatementTarget {

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

        /// <summary>
        ///     symbols in the interface part of the unit
        /// </summary>
        public DeclaredSymbols InterfaceSymbols { get; }

        /// <summary>
        ///     symbols in the implementation part of the unit
        /// </summary>
        public DeclaredSymbols ImplementationSymbols { get; }

        /// <summary>
        ///     create a new compilation unit
        /// </summary>
        public CompilationUnit() {
            RequiredUnits = new RequiredUnitNameList();
            InterfaceSymbols = new DeclaredSymbols();
            ImplementationSymbols = new DeclaredSymbols();

            RequiredUnits.Parent = this;
            InterfaceSymbols.Parent = this;
            ImplementationSymbols.Parent = this;
        }

        /// <summary>
        ///     get all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (var unit in RequiredUnits)
                    yield return unit;
                yield return InterfaceSymbols;
                yield return ImplementationSymbols;
                if (InitializationBlock != null)
                    yield return InitializationBlock;
                if (FinalizationBlock != null)
                    yield return FinalizationBlock;
            }
        }

        /// <summary>
        ///     assembly attributes
        /// </summary>
        public IEnumerable<SymbolAttribute> AssemblyAttributes
            => assemblyAttributes;

        /// <summary>
        ///     initialization
        /// </summary>
        public BlockOfStatements InitializationBlock { get; set; }

        /// <summary>
        ///     initialization
        /// </summary>
        public BlockOfStatements FinalizationBlock { get; set; }

        /// <summary>
        ///     statements
        /// </summary>
        public BlockOfStatements Statements
            => InitializationBlock;

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