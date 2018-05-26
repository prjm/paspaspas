using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record item
    /// </summary>
    public class RecordItem : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     class item
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     const section
        /// </summary>
        public ConstSection ConstSection { get; set; }

        /// <summary>
        ///     record fields
        /// </summary>
        public RecordFieldList Fields { get; set; }

        /// <summary>
        ///     method
        /// </summary>
        public ClassMethodSymbol MethodDeclaration { get; set; }

        /// <summary>
        ///     property
        /// </summary>
        public ClassProperty PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict visibility
        /// </summary>
        public bool Strict { get; internal set; }

        /// <summary>
        ///     type
        /// </summary>
        public TypeSection TypeSection { get; set; }

        /// <summary>
        ///     record variant section
        /// </summary>
        public RecordVariantSection VariantSection { get; set; }

        /// <summary>
        ///     visibility declaration
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes1 { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes2 { get; set; }

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