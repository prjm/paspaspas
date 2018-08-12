using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure declaration heading
    /// </summary>
    public class ProcedureDeclarationHeading : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new procedure declaration heading
        /// </summary>
        /// <param name="kindSymbol"></param>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="resultTypeAttributes"></param>
        /// <param name="resultType"></param>
        public ProcedureDeclarationHeading(Terminal kindSymbol, IdentifierSymbol name, FormalParameterSection parameters, Terminal colonSymbol, UserAttributes resultTypeAttributes, TypeSpecification resultType) {
            KindSymbol = kindSymbol;
            Name = name;
            Parameters = parameters;
            ColonSymbol = colonSymbol;
            ResultTypeAttributes = resultTypeAttributes;
            ResultType = resultType;
        }

        /// <summary>
        ///     procedure kind
        /// </summary>
        public int Kind
            => KindSymbol.GetSymbolKind();

        /// <summary>
        ///     procedure name
        /// </summary>
        public IdentifierSymbol Name { get; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; }

        /// <summary>
        ///     result attributes
        /// </summary>
        public SyntaxPartBase ResultTypeAttributes { get; }

        /// <summary>
        ///     kind symbol
        /// </summary>
        public Terminal KindSymbol { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, KindSymbol, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, ResultTypeAttributes, visitor);
            AcceptPart(this, ResultType, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => KindSymbol.GetSymbolLength() +
                Name.GetSymbolLength() +
                Parameters.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                ResultTypeAttributes.GetSymbolLength() +
                ResultType.GetSymbolLength();
    }
}