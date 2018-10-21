using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     include directive
    /// </summary>
    public class Include : CompilerDirectiveBase {

        /// <summary>
        ///     create a new include directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="fileName"></param>
        /// <param name="name"></param>
        public Include(Terminal symbol, FilenameSymbol fileName, string name) {
            Symbol = symbol;
            FileNameSymbol = fileName;
            FileName = name;
        }

        /// <summary>
        ///     include file name
        /// </summary>
        public string FileName { get; }

        /// <summary>
        ///     file name symbol
        /// </summary>
        public FilenameSymbol FileNameSymbol { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, FileNameSymbol, visitor);
            visitor.EndVisit(this);
        }


    }
}
