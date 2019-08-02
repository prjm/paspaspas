using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     user defined attribute (rtti)
    /// </summary>
    public class UserAttributeDefinitionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new user attribute definition
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="colon"></param>
        /// <param name="name"></param>
        /// <param name="openParen"></param>
        /// <param name="expressions"></param>
        /// <param name="closeParen"></param>
        /// <param name="comma"></param>
        public UserAttributeDefinitionSymbol(IdentifierSymbol prefix, Terminal colon, NamespaceNameSymbol name, Terminal openParen, ExpressionList expressions, Terminal closeParen, Terminal comma) {
            Prefix = prefix;
            Colon = colon;
            Name = name;
            OpenParen = openParen;
            Expressions = expressions;
            CloseParen = closeParen;
            Comma = comma;
        }

        /// <summary>
        ///     parameter expressions
        /// </summary>
        public ExpressionList Expressions { get; }

        /// <summary>
        ///     name of the attribute
        /// </summary>
        public NamespaceNameSymbol Name { get; }

        /// <summary>
        ///     attribute prefix
        /// </summary>
        public IdentifierSymbol Prefix { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal Colon { get; }

        /// <summary>
        ///     open paren
        /// </summary>
        public Terminal OpenParen { get; }

        /// <summary>
        ///     close paren
        /// </summary>
        public Terminal CloseParen { get; }

        /// <summary>
        ///     comma symbol
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Prefix, visitor);
            AcceptPart(this, Colon, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, Expressions, visitor);
            AcceptPart(this, CloseParen, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Prefix.GetSymbolLength() +
               Colon.GetSymbolLength() +
               Name.GetSymbolLength() +
               OpenParen.GetSymbolLength() +
               Expressions.GetSymbolLength() +
               CloseParen.GetSymbolLength() +
               Comma.GetSymbolLength();
    }
}