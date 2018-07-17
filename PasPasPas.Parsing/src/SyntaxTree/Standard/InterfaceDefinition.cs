using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface definition
    /// </summary>
    public class InterfaceDefinition : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new interface definition
        /// </summary>
        /// <param name="interfaceSymbol"></param>
        /// <param name="parentInterface"></param>
        /// <param name="guidSymbol"></param>
        /// <param name="items"></param>
        /// <param name="end"></param>
        public InterfaceDefinition(Terminal interfaceSymbol, ParentClass parentInterface, InterfaceGuid guidSymbol, InterfaceItems items, Terminal end) {
            InterfaceSymbol = interfaceSymbol;
            ParentInterface = parentInterface;
            GuidSymbol = guidSymbol;
            Items = items;
            EndSymbol = end;
        }

        /// <summary>
        ///     <c>true</c> if dispinterface
        /// </summary>
        public bool DisplayInterface
            => InterfaceSymbol.GetSymbolLength() == TokenKind.DispInterface;

        /// <summary>
        ///     guid declaration
        /// </summary>
        public InterfaceGuid GuidSymbol { get; }

        /// <summary>
        ///     interface items
        /// </summary>
        public InterfaceItems Items { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     parent interface
        /// </summary>
        public Terminal InterfaceSymbol { get; }

        /// <summary>
        ///     parent interface
        /// </summary>
        public ParentClass ParentInterface { get; }

        /// <summary>
        ///     <c>true</c> for forward declarations
        /// </summary>
        public bool ForwardDeclaration
            => EndSymbol == default && (Items == default || Items.Items == default || Items.Items.Length < 1);

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, InterfaceSymbol, visitor);
            AcceptPart(this, GuidSymbol, visitor);
            AcceptPart(this, Items, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => InterfaceSymbol.GetSymbolLength() +
                GuidSymbol.GetSymbolLength() +
                Items.GetSymbolLength() +
                EndSymbol.GetSymbolLength();

    }
}