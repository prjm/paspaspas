using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic program parameter
    /// </summary>
    public class ProgramParameterSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new program parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="comma"></param>
        public ProgramParameterSymbol(IdentifierSymbol id, Terminal comma) {
            ParameterName = id;
            Comma = comma;
        }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public IdentifierSymbol ParameterName { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ParameterName, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ParameterName.GetSymbolLength() + Comma.GetSymbolLength();

    }
}
