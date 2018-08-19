using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure reference
    /// </summary>
    public class ProcedureReferenceSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new procedure reference symbol
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="toSymbol"></param>
        /// <param name="proceduralType"></param>
        public ProcedureReferenceSymbol(Terminal reference, Terminal toSymbol, ProcedureTypeDefinitionSymbol proceduralType) {
            Reference = reference;
            ToSymbol = toSymbol;
            ProcedureType = proceduralType;
        }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeDefinitionSymbol ProcedureType { get; }

        /// <summary>
        ///     to symbol
        /// </summary>
        public Terminal ToSymbol { get; }

        /// <summary>
        ///     reference symbol
        /// </summary>
        public Terminal Reference { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Reference, visitor);
            AcceptPart(this, ToSymbol, visitor);
            AcceptPart(this, ProcedureType, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Reference.GetSymbolLength() +
                ToSymbol.GetSymbolLength() +
                ProcedureType.GetSymbolLength();

    }
}