using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     procedural type specification
    /// </summary>
    public class ProceduralType : MethodDeclaration, ITypeSpecification, IParameterTarget, ITypeTarget {

        /// <summary>
        ///     true if this is a method declaration (... of object)
        /// </summary>
        public bool MethodDeclaration { get; set; }

        /// <summary>
        ///     <true></true> if anonymous methods can be assigned (ref to function / proc)
        /// </summary>
        public bool AllowAnonymousMethods { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }
    }
}
