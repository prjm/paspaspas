using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     object declaration
    /// </summary>
    public class ObjectDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new object declaration symbol
        /// </summary>
        /// <param name="objectSymbol"></param>
        /// <param name="classParent"></param>
        /// <param name="items"></param>
        /// <param name="endSymbol"></param>
        public ObjectDeclarationSymbol(Terminal objectSymbol, ParentClassSymbol classParent, ObjectItems items, Terminal endSymbol) {
            ObjectSymbol = objectSymbol;
            ClassParent = classParent;
            Items = items;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClassSymbol ClassParent { get; }

        /// <summary>
        ///     object items
        /// </summary>
        public ObjectItems Items { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     object symbol
        /// </summary>
        public Terminal ObjectSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ObjectSymbol, visitor);
            AcceptPart(this, ClassParent, visitor);
            AcceptPart(this, Items, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ObjectSymbol.GetSymbolLength() +
                ClassParent.GetSymbolLength() +
                Items.GetSymbolLength() +
                EndSymbol.GetSymbolLength();
    }
}