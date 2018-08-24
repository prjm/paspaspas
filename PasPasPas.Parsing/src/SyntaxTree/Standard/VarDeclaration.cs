using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable declaration
    /// </summary>
    public class VarDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new variable declaration
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="identifiers"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="typeDeclaration"></param>
        /// <param name="value"></param>
        /// <param name="hints"></param>
        /// <param name="semicolon"></param>
        public VarDeclaration(UserAttributes attributes, IdentifierListSymbol identifiers, Terminal colonSymbol, TypeSpecificationSymbol typeDeclaration, VarValueSpecification value, HintingInformationListSymbol hints, Terminal semicolon) {
            Attributes = attributes;
            Identifiers = identifiers;
            ColonSymbol = colonSymbol;
            TypeDeclaration = typeDeclaration;
            Value = value;
            Hints = hints;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes { get; }

        /// <summary>
        ///     hints
        /// </summary>
        public ISyntaxPart Hints { get; }

        /// <summary>
        ///     var names
        /// </summary>
        public IdentifierListSymbol Identifiers { get; }

        /// <summary>
        ///     var types
        /// </summary>
        public TypeSpecificationSymbol TypeDeclaration { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     variable value
        /// </summary>
        public VarValueSpecification Value { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes, visitor);
            AcceptPart(this, Identifiers, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, TypeDeclaration, visitor);
            AcceptPart(this, Value, visitor);
            AcceptPart(this, Hints, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Attributes.GetSymbolLength() +
                Identifiers.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                TypeDeclaration.GetSymbolLength() +
                Value.GetSymbolLength() +
                Hints.GetSymbolLength() +
                Semicolon.GetSymbolLength();

    }
}