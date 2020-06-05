#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     pe version directive
    /// </summary>
    public class ParsedVersion : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal number;

        /// <summary>
        ///     parsed pe version
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="number"></param>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        public ParsedVersion(Terminal symbol, Terminal number, int majorVersion, int minorVersion) {
            this.symbol = symbol;
            this.number = number;
            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
        }

        /// <summary>
        ///     version kind
        /// </summary>
        public int Kind
            => symbol.GetSymbolKind();

        /// <summary>
        ///     major version
        /// </summary>
        public int MajorVersion { get; }

        /// <summary>
        ///     minor version
        /// </summary>
        public int MinorVersion { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, number, visitor);
            visitor.EndVisit(this);
        }


    }
}
