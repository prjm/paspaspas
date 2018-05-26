using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class helper item
    /// </summary>
    public class ClassHelperItem : StandardSyntaxTreeBase, IStructuredTypeMember {

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes1 { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public SyntaxPartBase Attributes2 { get; set; }

        /// <summary>
        ///     marker for class properties
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassProperty PropertyDeclaration { get; set; }

        /// <summary>
        ///     strict
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        ///     variable section
        /// </summary>
        public VarSection VarSection { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     constants
        /// </summary>
        public ConstSection ConstDeclaration { get; internal set; }

        /// <summary>
        ///     types
        /// </summary>
        public TypeSection TypeSection { get; internal set; }

        /// <summary>
        ///     fields
        /// </summary>
        public ClassField FieldDeclaration { get; internal set; }


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