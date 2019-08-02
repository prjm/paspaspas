using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formal parameter section
    /// </summary>
    public class FormalParameterSection : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new formal parameter section
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="parameters"></param>
        /// <param name="closeParen"></param>
        public FormalParameterSection(Terminal openParen, FormalParametersSymbol parameters, Terminal closeParen) {
            OpenParen = openParen;
            ParameterList = parameters;
            CloseParen = closeParen;
        }

        /// <summary>
        ///     parameter list
        /// </summary>
        public FormalParametersSymbol ParameterList { get; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, ParameterList, visitor);
            AcceptPart(this, CloseParen, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpenParen.GetSymbolLength() +
                ParameterList.GetSymbolLength() +
                CloseParen.GetSymbolLength();

    }
}