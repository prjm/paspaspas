using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record helper item
    /// </summary>
    public class RecordHelperItem : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     constant declaration
        /// </summary>
        public ConstSection ConstDeclaration { get; set; }

        /// <summary>
        ///     class flag
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     method
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     property
        /// </summary>
        public ClassProperty PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict visibility
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        ///     visibility definition
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     report helper types
        /// </summary>
        public TypeSection TypeSection { get; set; }

        /// <summary>
        ///     field
        /// </summary>
        public ClassField FieldDeclaration { get; internal set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}