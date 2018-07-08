using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class declaration
    /// </summary>
    public class ClassDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     class declaration symbol
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="sealedSymbol"></param>
        /// <param name="abstractSymbol"></param>
        /// <param name="classParent"></param>
        /// <param name="classItems"></param>
        /// <param name="forwardDeclaration"></param>
        /// <param name="endSymbol"></param>
        public ClassDeclarationSymbol(Terminal classSymbol, Terminal sealedSymbol, Terminal abstractSymbol, ParentClass classParent, ClassDeclarationItemsSymbol classItems, bool forwardDeclaration, Terminal endSymbol) {
            ClassSymbol = classSymbol;
            SealedSymbol = sealedSymbol;
            AbstractSymbol = abstractSymbol;
            ClassParent = classParent;
            ClassItems = classItems;
            ForwardDeclaration = forwardDeclaration;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     items of a class declaration
        /// </summary>
        public ClassDeclarationItemsSymbol ClassItems { get; }

        /// <summary>
        ///     <c>true</c> if this is a forward declaration
        /// </summary>
        public bool ForwardDeclaration { get; }

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClass ClassParent { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     abstract symbol
        /// </summary>
        public Terminal AbstractSymbol { get; }

        /// <summary>
        ///     sealed symbol
        /// </summary>
        public Terminal SealedSymbol { get; }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ClassSymbol, visitor);
            AcceptPart(this, SealedSymbol, visitor);
            AcceptPart(this, AbstractSymbol, visitor);
            AcceptPart(this, ClassParent, visitor);
            AcceptPart(this, ClassItems, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ClassSymbol.GetSymbolLength() +
               SealedSymbol.GetSymbolLength() +
               AbstractSymbol.GetSymbolLength() +
               ClassParent.GetSymbolLength() +
               ClassItems.GetSymbolLength() +
               EndSymbol.GetSymbolLength();

    }
}