using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     exports section
    /// </summary>
    public class ExportsSection : VariableLengthSyntaxTreeBase<ExportItem> {

        /// <summary>
        ///     create a new export section
        /// </summary>
        /// <param name="exports"></param>
        /// <param name="immutableArray"></param>
        /// <param name="semicolon"></param>
        public ExportsSection(Terminal exports, ImmutableArray<ExportItem> immutableArray, Terminal semicolon) : base(immutableArray) {
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
