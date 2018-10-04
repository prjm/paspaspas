using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure type definition
    /// </summary>
    public class ProcedureTypeDefinitionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new procedure type definition
        /// </summary>
        /// <param name="kindSymbol"></param>
        /// <param name="parameters"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="attributes"></param>
        /// <param name="returnType"></param>
        /// <param name="allowAnonymousMethods"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="objectSymbol"></param>
        public ProcedureTypeDefinitionSymbol(Terminal kindSymbol, FormalParameterSection parameters, Terminal colonSymbol, UserAttributesSymbol attributes, TypeSpecificationSymbol returnType, bool allowAnonymousMethods, Terminal ofSymbol, Terminal objectSymbol) {
            KindSymbol = kindSymbol;
            Parameters = parameters;
            ColonSymbol = colonSymbol;
            ReturnTypeAttributes = attributes;
            ReturnType = returnType;
            AllowAnonymousMethods = allowAnonymousMethods;
            OfSymbol = ofSymbol;
            ObjectSymbol = objectSymbol;
        }

        /// <summary>
        ///     kind (function or procedure)
        /// </summary>
        public int Kind
            => KindSymbol.GetSymbolKind();

        /// <summary>
        ///     function / procedure parameters
        /// </summary>
        public FormalParameterSection Parameters { get; }

        /// <summary>
        ///     return types
        /// </summary>
        public TypeSpecificationSymbol ReturnType { get; }
        public bool AllowAnonymousMethods { get; }
        public Terminal OfSymbol { get; }
        public Terminal ObjectSymbol { get; }

        /// <summary>
        ///     attributes of return types
        /// </summary>
        public SyntaxPartBase ReturnTypeAttributes { get; }

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
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, ReturnTypeAttributes, visitor);
            AcceptPart(this, ReturnType, visitor);
            AcceptPart(this, OfSymbol, visitor);
            AcceptPart(this, ObjectSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => KindSymbol.GetSymbolLength() +
                Parameters.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                ReturnTypeAttributes.GetSymbolLength() +
                OfSymbol.GetSymbolLength() +
                ObjectSymbol.GetSymbolLength() +
                ReturnType.GetSymbolLength();

    }
}