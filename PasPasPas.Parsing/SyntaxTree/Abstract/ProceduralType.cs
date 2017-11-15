using PasPasPas.Parsing.SyntaxTree.Types;
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
        ///     type information
        /// </summary>
        public ITypeDefinition TypeInfo { get; set; }
    }
}
