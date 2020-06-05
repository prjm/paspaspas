#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace name with generic suffix
    /// </summary>
    public class GenericNamespaceNameSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new generic namespace name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="genericPart"></param>
        /// <param name="dot"></param>
        public GenericNamespaceNameSymbol(NamespaceNameSymbol name, GenericSuffixSymbol genericPart, Terminal dot) {
            Name = name;
            GenericPart = genericPart;
            Dot = dot;
        }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal Dot { get; }

        /// <summary>
        ///     generic part
        /// </summary>
        public GenericSuffixSymbol GenericPart { get; }

        /// <summary>
        ///     namespace name
        /// </summary>
        public NamespaceNameSymbol Name { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, GenericPart, visitor);
            AcceptPart(this, Dot, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Name.GetSymbolLength() +
               GenericPart.GetSymbolLength() +
               Dot.GetSymbolLength();

    }
}
