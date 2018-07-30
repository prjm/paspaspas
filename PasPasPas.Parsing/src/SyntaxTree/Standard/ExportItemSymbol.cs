using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     exported item
    /// </summary>
    public class ExportItemSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new export item
        /// </summary>
        /// <param name="exportName"></param>
        /// <param name="openParen"></param>
        /// <param name="parameters"></param>
        /// <param name="closeParen"></param>
        /// <param name="indexSymbol"></param>
        /// <param name="indexParameter"></param>
        /// <param name="nameSymbol"></param>
        /// <param name="nameParameter"></param>
        /// <param name="resident"></param>
        /// <param name="comma"></param>
        public ExportItemSymbol(Identifier exportName, Terminal openParen, FormalParameters parameters, Terminal closeParen, Terminal indexSymbol, Expression indexParameter, Terminal nameSymbol, Expression nameParameter, Terminal resident, Terminal comma) {
            ExportName = exportName;
            OpenParen = openParen;
            Parameters = parameters;
            CloseParen = closeParen;
            IndexSymbol = indexSymbol;
            IndexParameter = indexParameter;
            NameSymbol = nameSymbol;
            NameParameter = nameParameter;
            Resident = resident;
            Comma = comma;
        }

        /// <summary>
        ///     index parameter
        /// </summary>
        public Expression IndexParameter { get; }

        /// <summary>
        ///     name parameter
        /// </summary>
        public Expression NameParameter { get; }

        /// <summary>
        ///     comma symbol
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     parameter list
        /// </summary>
        public FormalParameters Parameters { get; }

        /// <summary>
        ///     resident flag
        /// </summary>
        public Terminal Resident { get; }

        /// <summary>
        ///     export name
        /// </summary>
        public Identifier ExportName { get; }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     index symbol
        /// </summary>
        public Terminal IndexSymbol { get; }

        /// <summary>
        ///     name symbol
        /// </summary>
        public Terminal NameSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ExportName, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, CloseParen, visitor);
            AcceptPart(this, NameSymbol, visitor);
            AcceptPart(this, NameParameter, visitor);
            AcceptPart(this, IndexSymbol, visitor);
            AcceptPart(this, IndexParameter, visitor);
            AcceptPart(this, Resident, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ExportName.GetSymbolLength() +
                OpenParen.GetSymbolLength() +
                Parameters.GetSymbolLength() +
                CloseParen.GetSymbolLength() +
                NameSymbol.GetSymbolLength() +
                NameParameter.GetSymbolLength() +
                IndexSymbol.GetSymbolLength() +
                IndexParameter.GetSymbolLength() +
                Resident.GetSymbolLength() +
                Comma.GetSymbolLength();

    }
}