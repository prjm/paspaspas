#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     resource reference
    /// </summary>
    public class Resource : CompilerDirectiveBase {

        /// <summary>
        ///     create a new resource directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="fileName1"></param>
        /// <param name="rcFile"></param>
        /// <param name="fileName2"></param>
        /// <param name="rcFileName"></param>
        public Resource(Terminal symbol, FilenameSymbol fileName1, Terminal rcFile, string fileName2, string rcFileName) {
            Symbol = symbol;
            FileNameSymbol = fileName1;
            RcFileSymbol = rcFile;
            FileName = fileName2;
            RcFile = rcFileName;
        }

        /// <summary>
        ///     file name
        /// </summary>
        public string FileName { get; }

        /// <summary>
        ///     resource file
        /// </summary>
        public string RcFile { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     file name symbol
        /// </summary>
        public FilenameSymbol FileNameSymbol { get; }

        /// <summary>
        ///     rc file symbol
        /// </summary>
        public Terminal RcFileSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, FileNameSymbol, visitor);
            AcceptPart(this, RcFileSymbol, visitor);
            visitor.EndVisit(this);
        }
    }
}