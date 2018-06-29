using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly attribute
    /// </summary>
    public class AssemblyAttributeDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///
        /// </summary>
        /// <param name="terminal1"></param>
        /// <param name="terminal2"></param>
        /// <param name="terminal3"></param>
        /// <param name="userAttributeDefinition"></param>
        /// <param name="terminal4"></param>
        public AssemblyAttributeDeclaration(Terminal terminal1, Terminal terminal2, Terminal terminal3, UserAttributeDefinition userAttributeDefinition, Terminal terminal4) {
            OpenBraces = terminal1;
            AssemblySymbol = terminal2;
            ColonSymbol = terminal3;
            Attribute = userAttributeDefinition;
            CloseBraces = terminal4;
        }

        /// <summary>
        ///     attribute definition
        /// </summary>
        public UserAttributeDefinition Attribute { get; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     assembly symbol
        /// </summary>
        public Terminal AssemblySymbol { get; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenBraces, visitor);
            AcceptPart(this, AssemblySymbol, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, Attribute, visitor);
            AcceptPart(this, CloseBraces, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpenBraces.GetSymbolLength() +
                AssemblySymbol.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                Attribute.GetSymbolLength() +
                CloseBraces.GetSymbolLength();

    }
}
