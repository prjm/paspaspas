﻿using PasPasPas.Globals.Types;

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

        /// <summary>
        ///     test if this is a numerical value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsNumerical(this ITypeReference typeReference)
            => typeReference.TypeKind.IsNumerical();

        /// <summary>
        ///     test if this is a in integral value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsIntegral(this ITypeReference typeReference)
            => typeReference.TypeKind.IsIntegral();

        /// <summary>
        ///     test if this is a in ordinal value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsOrdinal(this ITypeReference typeReference)
            => typeReference.TypeKind.IsOrdinal();

        /// <summary>
        ///     test if this is a in array value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsArray(this ITypeReference typeReference)
            => typeReference.TypeKind.IsArray();

        /// <summary>
        ///     subrange values
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsSubrange(this ITypeReference typeReference)
            => typeReference.TypeKind == CommonTypeKind.SubrangeType;

        /// <summary>
        ///     test if this is a in short string value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsShortString(this ITypeReference typeReference)
            => typeReference.TypeKind == CommonTypeKind.ShortStringType;

        /// <summary>
        ///     test if this value is a subrange value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsSubrangeValue(this ITypeReference typeReference, out ISubrangeValue value) {
            value = typeReference as ISubrangeValue;

            if (typeReference.TypeKind == CommonTypeKind.SubrangeType && value != default)
                return true;

            value = default;
            return false;
        }
    }

}