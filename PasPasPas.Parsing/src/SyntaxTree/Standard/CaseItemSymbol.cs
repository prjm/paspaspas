using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     case item
    /// </summary>
    public class CaseItemSymbol : VariableLengthSyntaxTreeBase<CaseLabelSymbol> {

        /// <summary>
        ///     case item
        /// </summary>
        /// <param name="items"></param>
        /// <param name="colon"></param>
        /// <param name="semicolon"></param>
        /// <param name="statement"></param>
        public CaseItemSymbol(ImmutableArray<CaseLabelSymbol> items, Terminal colon, Statement statement, Terminal semicolon) : base(items) {
            ColonSymbol = colon;
            CaseStatement = statement;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     case statement
        /// </summary>
        public Statement CaseStatement { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, CaseStatement, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength +
               ColonSymbol.GetSymbolLength() +
               CaseStatement.GetSymbolLength() +
               Semicolon.GetSymbolLength();

    }
}