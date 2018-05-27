using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class method definition
    /// </summary>
    public class ClassMethodSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     method directives
        /// </summary>
        public MethodDirectives Directives { get; set; }

        /// <summary>
        ///     generic definition
        /// </summary>
        public SyntaxPartBase GenericDefinition { get; set; }

        /// <summary>
        ///     method identifier
        /// </summary>
        public Identifier Identifier { get; set; }

        /// <summary>
        ///     method kind
        /// </summary>
        public int MethodKind
            => MethodSymbol.Kind;

        /// <summary>
        ///     formal parameters
        /// </summary>
        public SyntaxPartBase Parameters { get; set; }

        /// <summary>
        ///     Result type attributes
        /// </summary>
        public SyntaxPartBase ResultAttributes { get; set; }

        /// <summary>
        ///     parse a type specification
        /// </summary>
        public SyntaxPartBase ResultType { get; set; }

        /// <summary>
        ///     method symbol
        /// </summary>
        public Terminal MethodSymbol { get; set; }

        /// <summary>
        ///     opening parenthesis
        /// </summary>
        public Terminal OpenParen { get; set; }

        /// <summary>
        ///     closing parenthesis
        /// </summary>
        public Terminal CloseParen { get; set; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

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
        public int Length
            => MethodSymbol.Length
            + Identifier.Length
            + GenericDefinition.Length
            + OpenParen.Length
            + Parameters.Length
            + CloseParen.Length
            + ColonSymbol.Length
            + ResultAttributes.Length
            + ResultType.Length
            + Semicolon.Length
            + Directives.Length;

    }
}