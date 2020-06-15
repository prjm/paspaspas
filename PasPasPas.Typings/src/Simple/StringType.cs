using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     base class for string types
    /// </summary>
    internal abstract class StringTypeBase : TypeDefinitionBase, IStringType {

        /// <summary>
        ///     create a new string type declaration
        /// </summary>
        /// <param name="definingUnit"></param>
        protected StringTypeBase(IUnitType definingUnit) : base(definingUnit) {
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
               other is StringTypeBase s && s.Kind == Kind;

        /// <summary>
        ///     string type kind
        /// </summary>
        public abstract StringTypeKind Kind { get; }

        /*
        /// <summary>
        ///     check if this type can be assigned from another type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind.IsString()) {
                return true;
            }

            if (otherType.TypeKind.IsChar()) {
                return true;
            }

            return base.CanBeAssignedFrom(otherType);
        }
*/
    }


}
