using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     type reference: reference to a type or to a constant value
    /// </summary>
    public interface ITypeReference {

        /// <summary>
        ///     get the well-known type id of this value
        /// </summary>
        /// <see cref="KnownTypeIds"/>
        int TypeId { get; }

        /// <summary>
        ///     get an internal representation of this typed reference
        /// </summary>
        /// <returns></returns>
        string InternalTypeFormat { get; }

        /// <summary>
        ///     reference kind
        /// </summary>
        TypeReferenceKind ReferenceKind { get; }

        /// <summary>
        ///     type kind
        /// </summary>
        CommonTypeKind TypeKind { get; }

    }


    /// <summary>
    ///     helper class for type references
    /// </summary>
    public static class TypeReferenceHelper {


        /// <summary>
        ///     test if this type reference is a constant value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsConstant(this ITypeReference typeReference)
            => typeReference.ReferenceKind == TypeReferenceKind.ConstantValue;


        /// <summary>
        ///     test if this type reference denotes a type name
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsType(this ITypeReference typeReference)
            => typeReference.ReferenceKind == TypeReferenceKind.TypeName;

    }

}