using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessors
    /// </summary>
    public class ClassPropertyReadWriteSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     kind
        /// </summary>
        public int Kind
            => Modifier.Kind;

        /// <summary>
        ///     member name
        /// </summary>
        public NamespaceName Member { get; set; }

        /// <summary>
        ///     property modifier
        /// </summary>
        public Terminal Modifier { get; set; }

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
        public int Length
            => Modifier.Length + Member.Length;


    }
}