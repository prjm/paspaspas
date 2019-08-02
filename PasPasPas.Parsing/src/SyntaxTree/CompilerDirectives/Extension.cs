using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     extension directive
    /// </summary>
    public class Extension : CompilerDirectiveBase {

        /// <summary>
        ///     create a new extension directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="extension"></param>
        /// <param name="parsedExtension"></param>
        public Extension(Terminal symbol, Terminal extension, string parsedExtension) {
            Symbol = symbol;
            ExtensionSymbol = extension;
            ExecutableExtension = parsedExtension;
        }

        /// <summary>
        ///     file extension
        /// </summary>
        public string ExecutableExtension { get; }

        /// <summary>
        ///     extension
        /// </summary>
        public Terminal ExtensionSymbol { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, ExtensionSymbol, visitor);
            visitor.EndVisit(this);
        }
    }
}
