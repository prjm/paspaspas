using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly attribute
    /// </summary>
    public class AssemblyAttributeDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///     attribute definition
        /// </summary>
        public UserAttributeDefinition Attribute { get; set; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; set; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; set; }

        /// <summary>
        ///     assembly symbol
        /// </summary>
        public Terminal AssemblySymbol { get; set; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; set; }

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
        public int Length
            => OpenBraces.Length + AssemblySymbol.Length + ColonSymbol.Length + Attribute.Length + CloseBraces.Length;

    }
}
