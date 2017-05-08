using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     object item
    /// </summary>
    public class ObjectItem : StandardSyntaxTreeBase, IStructuredTypeMember {
        public ObjectItem(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     field declaration
        /// </summary>
        public ClassField FieldDeclaration { get; set; }

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethod MethodDeclaration { get; set; }

        /// <summary>
        ///     strict modifier
        /// </summary>
        public bool Strict { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public int Visibility { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     class item
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     properties
        /// </summary>
        public ClassProperty Property { get; set; }

        /// <summary>
        ///     type section
        /// </summary>
        public TypeSection TypeSection { get; set; }

        /// <summary>
        ///     const section
        /// </summary>
        public ConstSection ConstSection { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}
