using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method
    /// </summary>
    public class StructureMethod : MethodDeclaration, IParameterTarget, ITypeTarget {

        /// <summary>
        ///     generic method parameter
        /// </summary>
        public GenericTypeCollection Generics { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var part in base.Parts)
                    yield return part;
                if (Generics != null)
                    foreach (var genericType in Generics)
                        yield return genericType;
                foreach (var directive in Directives)
                    yield return directive;
            }
        }

        /// <summary>
        ///     <c>true</c> if class method
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     add an overloaded methods
        /// </summary>
        /// <param name="entry"></param>
        public void AddOverload(MethodDeclaration entry)
            => Overloads.Add(entry);

        /// <summary>
        ///     visibility mode
        /// </summary>
        public MemberVisibility Visibility { get; set; }

        /// <summary>
        ///     implementation
        /// </summary>
        public MethodImplementation Implementation { get; set; }

        /// <summary>
        ///     defining type
        /// </summary>
        public StructuredType DefiningType { get; internal set; }

    }
}
