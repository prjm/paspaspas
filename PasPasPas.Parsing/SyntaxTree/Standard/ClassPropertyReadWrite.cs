using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessor
    /// </summary>
    public class ClassPropertyReadWrite : StandardSyntaxTreeBase {

        /// <summary>
        ///     accessor kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     member name
        /// </summary>
        public NamespaceName Member { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}