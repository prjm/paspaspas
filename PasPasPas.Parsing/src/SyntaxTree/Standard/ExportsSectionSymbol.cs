using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     exports section
    /// </summary>
    public class ExportsSectionSymbol : VariableLengthSyntaxTreeBase<ExportItemSymbol> {

        /// <summary>
        ///     create a new export section
        /// </summary>
        /// <param name="exports"></param>
        /// <param name="items"></param>
        /// <param name="semicolon"></param>
        public ExportsSectionSymbol(Terminal exports, ImmutableArray<ExportItemSymbol> items, Terminal semicolon) : base(items) {
            Exports = exports;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     exports section
        /// </summary>
        public Terminal Exports { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Exports, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Exports.GetSymbolLength() +
                ItemLength +
                Semicolon.GetSymbolLength();

    }
}
