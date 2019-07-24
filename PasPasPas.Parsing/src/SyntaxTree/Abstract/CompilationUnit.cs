using System.Collections.Generic;
using System.Globalization;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     basic representation of a compilation unit
    /// </summary>
    public class CompilationUnit : SymbolTableEntryBase, IBlockTarget, IDeclaredSymbolTarget, ITypedSyntaxNode {

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
        public FileReference FilePath { get; set; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     required units
        /// </summary>
        public RequiredUnitNameListCollection RequiredUnits { get; }

        /// <summary>
        ///     symbols in the interface part of the unit
        /// </summary>
        public DeclaredSymbolCollection InterfaceSymbols { get; set; }

        /// <summary>
        ///     symbols in the implementation part of the unit
        /// </summary>
        public DeclaredSymbolCollection ImplementationSymbols { get; set; }

        /// <summary>
        ///     create a new compilation unit
        /// </summary>
        public CompilationUnit() =>
            RequiredUnits = new RequiredUnitNameListCollection();

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
        ///     declared symbols (wraps to interface symbols and implementation symbols for units)
        /// </summary>
        public DeclaredSymbolCollection Symbols { get; set; }

        /// <summary>
        ///     list of assembly attributes
        /// </summary>
        private IList<SymbolAttributeItem> AssemblyAttributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     unit type
        /// </summary>
        public ITypeReference TypeInfo { get; set; }

        /// <summary>
        ///     add an assembly attribute
        /// </summary>
        /// <param name="assemblyAttribute"></param>
        public void AddAssemblyAttribute(SymbolAttributeItem assemblyAttribute)
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
            AcceptPart(this, RequiredUnits, visitor);
            AcceptPart(this, InterfaceSymbols, visitor);
            AcceptPart(this, ImplementationSymbols, visitor);
            AcceptPart(this, Symbols, visitor);
            AcceptPart(this, InitializationBlock, visitor);
            AcceptPart(this, FinalizationBlock, visitor);
            visitor.EndVisit(this);
        }
    }
}