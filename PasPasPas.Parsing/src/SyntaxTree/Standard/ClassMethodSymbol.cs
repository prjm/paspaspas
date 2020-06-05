#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class method definition
    /// </summary>
    public class ClassMethodSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new class method symbol
        /// </summary>
        /// <param name="methodSymbol"></param>
        /// <param name="identifier"></param>
        /// <param name="genericDefinition"></param>
        /// <param name="openParen"></param>
        /// <param name="parameters"></param>
        /// <param name="closeParen"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="resultAttributes"></param>
        /// <param name="resultType"></param>
        /// <param name="semicolon"></param>
        /// <param name="directives"></param>
        public ClassMethodSymbol(Terminal methodSymbol, IdentifierSymbol identifier, GenericDefinitionSymbol genericDefinition, Terminal openParen, FormalParametersSymbol parameters, Terminal closeParen, Terminal colonSymbol, UserAttributesSymbol resultAttributes, TypeSpecificationSymbol resultType, Terminal semicolon, MethodDirectivesSymbol directives) {
            MethodSymbol = methodSymbol;
            Identifier = identifier;
            GenericDefinition = genericDefinition;
            OpenParen = openParen;
            Parameters = parameters;
            CloseParen = closeParen;
            ColonSymbol = colonSymbol;
            ResultAttributes = resultAttributes;
            ResultType = resultType;
            Semicolon = semicolon;
            Directives = directives;
        }

        /// <summary>
        ///     method directives
        /// </summary>
        public MethodDirectivesSymbol Directives { get; }

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinitionSymbol GenericDefinition { get; }

        /// <summary>
        ///     method identifier
        /// </summary>
        public IdentifierSymbol Identifier { get; }

        /// <summary>
        ///     method kind
        /// </summary>
        public int MethodKind
            => MethodSymbol.Kind;

        /// <summary>
        ///     formal parameters
        /// </summary>
        public FormalParametersSymbol Parameters { get; }

        /// <summary>
        ///     Result type attributes
        /// </summary>
        public UserAttributesSymbol ResultAttributes { get; }

        /// <summary>
        ///     parse a type specification
        /// </summary>
        public TypeSpecificationSymbol ResultType { get; }

        /// <summary>
        ///     method symbol
        /// </summary>
        public Terminal MethodSymbol { get; }

        /// <summary>
        ///     opening parenthesis
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     closing parenthesis
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, MethodSymbol, visitor);
            AcceptPart(this, Identifier, visitor);
            AcceptPart(this, GenericDefinition, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, CloseParen, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, ResultAttributes, visitor);
            AcceptPart(this, ResultType, visitor);
            AcceptPart(this, Semicolon, visitor);
            AcceptPart(this, Directives, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => MethodSymbol.GetSymbolLength()
            + Identifier.GetSymbolLength()
            + GenericDefinition.GetSymbolLength()
            + OpenParen.GetSymbolLength()
            + Parameters.GetSymbolLength()
            + CloseParen.GetSymbolLength()
            + ColonSymbol.GetSymbolLength()
            + ResultAttributes.GetSymbolLength()
            + ResultType.GetSymbolLength()
            + Semicolon.GetSymbolLength()
            + Directives.GetSymbolLength();

    }
}