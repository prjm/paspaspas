#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///    method declaration names
    /// </summary>
    public class MethodDeclarationNameSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new method declaration name
        /// </summary>
        /// <param name="namePart"></param>
        /// <param name="genericPart"></param>
        /// <param name="dot"></param>
        public MethodDeclarationNameSymbol(NamespaceNameSymbol namePart, GenericDefinitionSymbol genericPart, Terminal dot) {
            Name = namePart;
            GenericDefinition = genericPart;
            Dot = dot;
        }

        /// <summary>
        ///     namespace name
        /// </summary>
        public NamespaceNameSymbol Name { get; }

        /// <summary>
        ///     generic parameters
        /// </summary>
        public GenericDefinitionSymbol GenericDefinition { get; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal Dot { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, GenericDefinition, visitor);
            AcceptPart(this, Dot, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Name.GetSymbolLength() +
                GenericDefinition.GetSymbolLength() +
                Dot.GetSymbolLength();

    }
}
