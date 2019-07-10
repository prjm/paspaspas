using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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

        /// <summary>
        ///     accept a visitor
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IStartEndVisitor visitor) {
            AcceptBaseParts(visitor);
            AcceptPart(this, Generics, visitor);
            AcceptPart(this, Directives, visitor);
        }
    }
}
