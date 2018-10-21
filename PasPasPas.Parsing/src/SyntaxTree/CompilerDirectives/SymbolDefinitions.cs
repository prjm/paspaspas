using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     symbol definitions switch
    /// </summary>
    public class SymbolDefinitions : CompilerDirectiveBase {

        /// <summary>
        ///     create a new symbol definitions directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="referencesMode"></param>
        /// <param name="mode"></param>
        public SymbolDefinitions(Terminal symbol, SymbolReferenceInfo referencesMode, SymbolDefinitionInfo mode) {
            Symbol = symbol;
            ReferencesMode = referencesMode;
            Mode = mode;
        }

        /// <summary>
        ///     create a mew symbol definitions directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        public SymbolDefinitions(Terminal symbol, Terminal mode, SymbolReferenceInfo parsedMode) {
            Symbol = symbol;
            ModeSymbol = mode;
            Mode = SymbolDefinitionInfo.Undefined;
            ReferencesMode = parsedMode;
        }

        /// <summary>
        ///     create a new symbols definition mode
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        /// <param name="parsedMode1"></param>
        public SymbolDefinitions(Terminal symbol, Terminal mode, SymbolReferenceInfo parsedMode, SymbolDefinitionInfo parsedMode1) :
            this(symbol, mode, parsedMode) => Mode = parsedMode1;

        /// <summary>
        ///     definition mode
        /// </summary>
        public SymbolDefinitionInfo Mode { get; }

        /// <summary>
        ///     references mode
        /// </summary>
        public SymbolReferenceInfo ReferencesMode { get; }

        /// <summary>
        ///     symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     mode symbol
        /// </summary>
        public Terminal ModeSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, ModeSymbol, visitor);
            visitor.EndVisit(this);
        }


    }
}
