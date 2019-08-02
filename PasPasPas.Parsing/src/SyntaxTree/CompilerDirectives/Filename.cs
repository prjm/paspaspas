using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     file name
    /// </summary>
    public class FilenameSymbol : CompilerDirectiveBase {

        /// <summary>
        ///     create a new filename symbol
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="terminal"></param>
        public FilenameSymbol(Terminal filename, Terminal terminal) {
            Filename = filename;
            Identifier = terminal;
        }

        /// <summary>
        ///     filename
        /// </summary>
        public Terminal Filename { get; }

        /// <summary>
        ///     file identifier
        /// </summary>
        public Terminal Identifier { get; }

        /// <summary>
        ///     accept a visitor
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Filename, visitor);
            AcceptPart(this, Identifier, visitor);
            visitor.EndVisit(this);
        }
    }
}
