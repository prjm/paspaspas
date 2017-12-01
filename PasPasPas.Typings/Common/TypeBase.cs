using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     basic type definition
    /// </summary>
    public abstract class TypeBase : ITypeDefinition {

        private readonly int typeId;

        /// <summary>
        ///     create a new type definiton
        /// </summary>
        /// <param name="withId">type id</param>
        public TypeBase(int withId)
            => typeId = withId;

        /// <summary>
        ///     get the type id
        /// </summary>
        public int TypeId => typeId;

        /// <summary>
        ///     get the type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     type registryB
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     test if this type can be assigned from another type
        /// </summary>
        /// <param name="otherType">other type</param>
        /// <returns><c>true</c> if the type can be assigned from</returns>
        public virtual bool CanBeAssignedFrom(ITypeDefinition otherType) {
            if (otherType.TypeId.In(TypeIds.ErrorType, TypeIds.NoType, TypeIds.UnspecifiedType))
                return false;

            if (otherType.TypeId == TypeId)
                return true;

            return false;
        }
    }
}
