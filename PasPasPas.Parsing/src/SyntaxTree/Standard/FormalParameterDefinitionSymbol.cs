using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formal definition parameter
    /// </summary>
    public class FormalParameterDefinitionSymbol : VariableLengthSyntaxTreeBase<FormalParameterSymbol> {

        /// <summary>
        ///     create a new formal parameter definition
        /// </summary>
        /// <param name="items"></param>
        /// <param name="colon"></param>
        /// <param name="typeDef"></param>
        /// <param name="equals"></param>
        /// <param name="defaultValue"></param>
        /// <param name="semicolon"></param>
        public FormalParameterDefinitionSymbol(ImmutableArray<FormalParameterSymbol> items, Terminal colon, TypeSpecificationSymbol typeDef, Terminal equals, ExpressionSymbol defaultValue, Terminal semicolon) : base(items) {
            ColonSymbol = colon;
            TypeDeclaration = typeDef;
            EqualsSign = equals;
            DefaultValue = defaultValue;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     default value
        /// </summary>
        public ExpressionSymbol DefaultValue { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecificationSymbol TypeDeclaration { get; }

        /// <summary>
        ///     equals sign
        /// </summary>
        public Terminal EqualsSign { get; }

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
            AcceptPart(this, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, TypeDeclaration, visitor);
            AcceptPart(this, EqualsSign, visitor);
            AcceptPart(this, DefaultValue, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength +
                ColonSymbol.GetSymbolLength() +
                TypeDeclaration.GetSymbolLength() +
                EqualsSign.GetSymbolLength() +
                DefaultValue.GetSymbolLength() +
                Semicolon.GetSymbolLength();

    }
}
