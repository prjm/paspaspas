using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace name with generic suffix
    /// </summary>
    public class GenericNamespaceName : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new generic namespace name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="genericPart"></param>
        public GenericNamespaceName(NamespaceName name, GenericSuffix genericPart, Terminal dot) {
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
        public GenericSuffix GenericPart { get; }

        /// <summary>
        ///     namespace name
        /// </summary>
        public NamespaceName Name { get; }

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
            => Name.GetSymbolLength() + GenericPart.GetSymbolLength() + Dot.GetSymbolLength();

    }
}
