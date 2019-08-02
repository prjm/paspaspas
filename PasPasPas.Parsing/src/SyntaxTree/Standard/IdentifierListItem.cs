using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     identifier list item
    /// </summary>
    public class IdentifierListItem : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new list item
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="identifier"></param>
        /// <param name="comma"></param>
        public IdentifierListItem(UserAttributesSymbol attributes, IdentifierSymbol identifier, Terminal comma) {
            Attributes = attributes;
            Identifier = identifier;
            Comma = comma;
        }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     identifier
        /// </summary>
        public IdentifierSymbol Identifier { get; }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributesSymbol Attributes { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes, visitor);
            AcceptPart(this, Identifier, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Attributes.GetSymbolLength() +
               Identifier.GetSymbolLength() +
               Comma.GetSymbolLength();

    }
}