using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessors
    /// </summary>
    public class ClassPropertyReadWriteSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new class property read write symbol
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="namespaceName"></param>
        public ClassPropertyReadWriteSymbol(Terminal terminal, NamespaceNameSymbol namespaceName) {
            Modifier = terminal;
            Member = namespaceName;
        }

        /// <summary>
        ///     kind
        /// </summary>
        public int Kind
            => Modifier.GetSymbolKind();

        /// <summary>
        ///     member name
        /// </summary>
        public NamespaceNameSymbol Member { get; }

        /// <summary>
        ///     property modifier
        /// </summary>
        public Terminal Modifier { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Modifier, visitor);
            AcceptPart(this, Member, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Modifier.GetSymbolLength() + Member.GetSymbolLength();


    }
}