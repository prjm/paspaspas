using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     struct type part
    /// </summary>
    public class StructTypePart : StandardSyntaxTreeBase {

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
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}