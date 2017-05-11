using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method
    /// </summary>
    public class StructureMethod : MethodDeclaration, IParameterTarget, ITypeTarget {

        /// <summary>
        ///     generic method parameter
        /// </summary>
        public GenericTypes Generics { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ISyntaxPart part in base.Parts)
                    yield return part;
                if (Generics != null)
                    foreach (ISyntaxPart genericType in Generics)
                        yield return genericType;
                foreach (MethodDirective directive in Directives)
                    yield return directive;
                if (Implementation != null)
                    yield return Implementation;
            }
        }

        /// <summary>
        ///     <c>true</c> if class method
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     visibility mode
        /// </summary>
        public MemberVisibility Visibility { get; set; }

        /// <summary>
        ///     implementation
        /// </summary>
        public MethodImplementation Implementation { get; set; }
    }
}
