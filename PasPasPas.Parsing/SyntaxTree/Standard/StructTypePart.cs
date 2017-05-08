using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     struct type part
    /// </summary>
    public class StructTypePart : StandardSyntaxTreeBase {
        public StructTypePart(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     array type
        /// </summary>
        public ArrayType ArrayType { get; set; }

        /// <summary>
        ///     class type declaration
        /// </summary>
        public ClassTypeDeclaration ClassDeclaration { get; set; }

        /// <summary>
        ///     file type declaration
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        ///     set type declaration
        /// </summary>
        public SetDefinition SetType { get; set; }

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