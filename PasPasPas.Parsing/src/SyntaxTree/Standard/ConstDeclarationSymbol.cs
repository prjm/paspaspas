using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant declaration
    /// </summary>
    public class ConstDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new const declaration
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="identifier"></param>
        /// <param name="colon"></param>
        /// <param name="typeSpecification"></param>
        /// <param name="equalsSign"></param>
        /// <param name="value"></param>
        /// <param name="hint"></param>
        /// <param name="semicolon"></param>
        public ConstDeclarationSymbol(UserAttributes attributes, IdentifierSymbol identifier, Terminal colon, TypeSpecificationSymbol typeSpecification, Terminal equalsSign, ConstantExpressionSymbol value, ISyntaxPart hint, Terminal semicolon) {
            Attributes = attributes;
            Identifier = identifier;
            Colon = colon;
            TypeSpecification = typeSpecification;
            EqualsSign = equalsSign;
            Value = value;
            Hint = hint;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     user defined attributes
        /// </summary>
        public SyntaxPartBase Attributes { get; }

        /// <summary>
        ///     additional hint for that constant
        /// </summary>
        public ISyntaxPart Hint { get; }

        /// <summary>
        ///     identifier
        /// </summary>
        public IdentifierSymbol Identifier { get; }

        /// <summary>
        ///     type specification
        /// </summary>
        public TypeSpecificationSymbol TypeSpecification { get; }

        /// <summary>
        ///     expression
        /// </summary>
        public ConstantExpressionSymbol Value { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     equals sign
        /// </summary>
        public Terminal EqualsSign { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal Colon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes, visitor);
            AcceptPart(this, Identifier, visitor);
            AcceptPart(this, Colon, visitor);
            AcceptPart(this, TypeSpecification, visitor);
            AcceptPart(this, EqualsSign, visitor);
            AcceptPart(this, Value, visitor);
            AcceptPart(this, Hint, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Attributes.GetSymbolLength()
             + Identifier.GetSymbolLength()
             + Colon.GetSymbolLength()
             + TypeSpecification.GetSymbolLength()
             + EqualsSign.GetSymbolLength()
             + Value.GetSymbolLength()
             + Hint.GetSymbolLength()
             + Semicolon.GetSymbolLength();
    }
}