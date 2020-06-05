#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     link directive
    /// </summary>
    public class Link : CompilerDirectiveBase {

        /// <summary>
        ///     create a new link symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="fileName"></param>
        /// <param name="name">name</param>
        public Link(Terminal symbol, FilenameSymbol fileName, string name) {
            Symbol = symbol;
            FileNameSymbol = fileName;
            FileName = name;
        }

        /// <summary>
        ///     linked file
        /// </summary>
        public FilenameSymbol FileNameSymbol { get; }

        /// <summary>
        ///     file name
        /// </summary>
        public string FileName { get; }

        /// <summary>
        ///     symbol
        /// </summary>
        public Terminal Symbol { get; }

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
