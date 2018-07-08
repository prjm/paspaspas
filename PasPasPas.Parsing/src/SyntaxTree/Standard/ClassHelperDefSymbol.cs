using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class helper declaration
    /// </summary>
    public class ClassHelperDefSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new class helper def symbol
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="helperSymbol"></param>
        /// <param name="classParent"></param>
        /// <param name="forSymbol"></param>
        /// <param name="helperName"></param>
        /// <param name="helperItems"></param>
        /// <param name="endSymbol"></param>
        public ClassHelperDefSymbol(Terminal classSymbol, Terminal helperSymbol, ParentClass classParent, Terminal forSymbol, TypeName helperName, ClassHelperItemsSymbol helperItems, Terminal endSymbol) {
            HelperItems = helperItems;
            ClassParent = classParent;
            HelperName = helperName;
            ClassSymbol = classSymbol;
            HelperSymbol = helperSymbol;
            ForSymbol = forSymbol;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     items
        /// </summary>
        public ClassHelperItemsSymbol HelperItems { get; }

        /// <summary>
        ///     class parent
        /// </summary>
        public ParentClass ClassParent { get; }

        /// <summary>
        ///     class helper name
        /// </summary>
        public TypeName HelperName { get; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; }

        /// <summary>
        ///     helper symbol
        /// </summary>
        public Terminal HelperSymbol { get; }

        /// <summary>
        ///     for symbol
        /// </summary>
        public Terminal ForSymbol { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ClassSymbol, visitor);
            AcceptPart(this, HelperSymbol, visitor);
            AcceptPart(this, ClassParent, visitor);
            AcceptPart(this, ForSymbol, visitor);
            AcceptPart(this, HelperName, visitor);
            AcceptPart(this, HelperItems, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ClassSymbol.GetSymbolLength() +
               HelperSymbol.GetSymbolLength() +
               ClassParent.GetSymbolLength() +
               ForSymbol.GetSymbolLength() +
               HelperName.GetSymbolLength() +
               HelperItems.GetSymbolLength() +
               EndSymbol.GetSymbolLength();

    }
}