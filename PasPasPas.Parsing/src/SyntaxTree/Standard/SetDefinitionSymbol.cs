using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     a set definition
    /// </summary>
    public class SetDefinitionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new set definition
        /// </summary>
        /// <param name="setSymbol"></param>
        /// <param name="ofSymbol"></param>
        /// <param name="typeDefinition"></param>
        public SetDefinitionSymbol(Terminal setSymbol, Terminal ofSymbol, TypeSpecificationSymbol typeDefinition) {
            SetSymbol = setSymbol;
            OfSymbol = ofSymbol;
            TypeDefinition = typeDefinition;
        }

        /// <summary>
        ///     inner type reference
        /// </summary>
        public TypeSpecificationSymbol TypeDefinition { get; }

        /// <summary>
        ///     of symbol
        /// </summary>
        public Terminal OfSymbol { get; }

        /// <summary>
        ///     set symbol
        /// </summary>
        public Terminal SetSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, SetSymbol, visitor);
            AcceptPart(this, OfSymbol, visitor);
            AcceptPart(this, TypeDefinition, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => SetSymbol.GetSymbolLength() +
                OfSymbol.GetSymbolLength() +
                TypeDefinition.GetSymbolLength();

    }
}
