using System.Collections.Generic;
using System.Globalization;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     basic representation of a compilation unit
    /// </summary>
    public class CompilationUnit : SymbolTableEntryBase, IBlockTarget, IDeclaredSymbolTarget {

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
        public DeclaredSymbols InterfaceSymbols { get; set; }

        /// <summary>
        ///     symbols in the implementation part of the unit
        /// </summary>
        public DeclaredSymbols ImplementationSymbols { get; set; }

        /// <summary>
        ///     create a new compilation unit
        /// </summary>
        public CompilationUnit() =>
            RequiredUnits = new RequiredUnitNameList() {
                ParentItem = this
            };

        /// <summary>
        ///     get all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var unit in RequiredUnits)
                    yield return unit;
                if (InterfaceSymbols != null)
                    yield return InterfaceSymbols;
                if (ImplementationSymbols != null)
                    yield return ImplementationSymbols;
                if (Symbols != null)
                    yield return Symbols;
                if (InitializationBlock != null)
                    yield return InitializationBlock;
                if (FinalizationBlock != null)
                    yield return FinalizationBlock;
            }
        }

        /// <summary>
        ///     initialization
        /// </summary>
        public StatementBase InitializationBlock { get; set; }

        /// <summary>
        ///     initialization
        /// </summary>
        public StatementBase FinalizationBlock { get; set; }

        /// <summary>
        ///     statements
        /// </summary>
        public StatementBase Block {
            get => InitializationBlock;
            set => InitializationBlock = value;
        }

        /// <summary>
        ///     declared symbols (wraps to intf. symbols and impl. symbols for units)
        /// </summary>
        public DeclaredSymbols Symbols { get; set; }

        /// <summary>
        ///     list of assembly attributes
        /// </summary>
        private IList<SymbolAttribute> AssemblyAttributes { get; }
            = new List<SymbolAttribute>();

        /// <summary>
        ///     add an assembly attribute
        /// </summary>
        /// <param name="assemblyAttribute"></param>
        public void AddAssemblyAttribute(SymbolAttribute assemblyAttribute)
            => AssemblyAttributes.Add(assemblyAttribute);

        private int symbolNames = 0;

        /// <summary>
        ///     generate a new symbol name
        /// </summary>
        /// <returns></returns>
        public string GenerateSymbolName() {
            symbolNames++;
            return string.Format(CultureInfo.InvariantCulture, "${0:d}", symbolNames);
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}