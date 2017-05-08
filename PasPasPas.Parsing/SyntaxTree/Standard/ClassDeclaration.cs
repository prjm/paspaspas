using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class declaration
    /// </summary>
    public class ClassDeclaration : StandardSyntaxTreeBase {
        public ClassDeclaration(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     sealed class
        /// </summary>
        public bool Abstract { get; set; }

        /// <summary>
        ///     items of a class declaration
        /// </summary>
        public ClassDeclarationItems ClassItems { get; set; }

        /// <summary>
        ///     parent class
        /// </summary>
        public ParentClass ClassParent { get; set; }

        /// <summary>
        ///     forward declaration
        /// </summary>
        public bool ForwardDeclaration { get; set; }

        /// <summary>
        ///     abstract class
        /// </summary>
        public bool Sealed { get; set; }


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