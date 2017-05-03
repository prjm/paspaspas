using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure type definition
    /// </summary>
    public class ProcedureTypeDefinition : StandardSyntaxTreeBase {

        /// <summary>
        ///     kind (function or procedure)
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     <c>true</c> if this is a method declaration
        /// </summary>
        public bool MethodDeclaration { get; set; }
            = false;

        /// <summary>
        ///     function / procedure parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     <c>true</c> for reference types
        /// </summary>
        public bool AllowAnonymousMethods { get; internal set; }

        /// <summary>
        ///     return types
        /// </summary>
        public TypeSpecification ReturnType { get; set; }

        /// <summary>
        ///     attributes of return types
        /// </summary>
        public UserAttributes ReturnTypeAttributes { get; set; }

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