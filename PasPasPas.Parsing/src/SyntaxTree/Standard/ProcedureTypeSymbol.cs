using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedural type
    /// </summary>
    public class ProcedureTypeSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new procedural type def
        /// </summary>
        /// <param name="procedureReferenceSymbol"></param>
        public ProcedureTypeSymbol(ProcedureReferenceSymbol procedureReferenceSymbol)
            => ProcedureReference = procedureReferenceSymbol;

        /// <summary>
        ///     create a new procedure reference symbol
        /// </summary>
        /// <param name="refType"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="objectSymbol"></param>
        public ProcedureTypeSymbol(ProcedureTypeDefinitionSymbol refType, Terminal ofSymbol, Terminal objectSymbol) {
            ProcedureRefType = refType;
            OfSymbol = ofSymbol;
            ObjectSymbol = objectSymbol;
        }

        /// <summary>
        ///     procedure reference
        /// </summary>
        public ProcedureReferenceSymbol ProcedureReference { get; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeDefinitionSymbol ProcedureRefType { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ProcedureReference, visitor);
            AcceptPart(this, ProcedureRefType, visitor);
            AcceptPart(this, OfSymbol, visitor);
            AcceptPart(this, ObjectSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ProcedureRefType.GetSymbolLength() +
                ProcedureReference.GetSymbolLength() +
                OfSymbol.GetSymbolLength() +
                ObjectSymbol.GetSymbolLength();

        /// <summary>
        ///     of symbol
        /// </summary>
        public Terminal OfSymbol { get; }

        /// <summary>
        ///     object symbol
        /// </summary>
        public Terminal ObjectSymbol { get; }

    }
}