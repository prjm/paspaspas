#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;

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

        /// <summary>
        ///     defining type
        /// </summary>
        public StructuredType DefiningType { get; internal set; }

        /// <summary>
        ///     accept base parts
        /// </summary>
        /// <param name="visitor"></param>
        protected override void AcceptBaseParts(IStartEndVisitor visitor) {
            base.AcceptBaseParts(visitor);
            AcceptPart(this, Generics, visitor);
            AcceptPart(this, Directives, visitor);
        }

    }
}
