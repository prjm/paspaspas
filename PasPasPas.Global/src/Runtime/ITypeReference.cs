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
        ///     test if this is a in integral value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsBoolean(this ITypeReference typeReference)
            => typeReference.TypeKind == CommonTypeKind.BooleanType;


        /// <summary>
        ///     test if this is a in real value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsReal(this ITypeReference typeReference)
            => typeReference.TypeKind == CommonTypeKind.RealType;


        /// <summary>
        ///     test if this is a in ordinal value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsOrdinal(this ITypeReference typeReference)
            => typeReference.TypeKind.IsOrdinal();

        /// <summary>
        ///     test if this is an array value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsArray(this ITypeReference typeReference)
            => typeReference.TypeKind.IsArray();

        /// <summary>
        ///     test if this is a set value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsSet(this ITypeReference typeReference)
            => typeReference.TypeKind.IsSet();


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
        ///     test if this is a string value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsString(this ITypeReference typeReference)
            => typeReference.TypeKind.IsString();

        /// <summary>
        ///     test if this is a text value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsTextual(this ITypeReference typeReference)
            => typeReference.TypeKind.IsTextual();

        /// <summary>
        ///     test if this is a unicode text value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsUnicodeText(this ITypeReference typeReference)
            => typeReference.TypeKind.IsUnicodeText();

        /// <summary>
        ///     test if this is a in char value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <returns></returns>
        public static bool IsChar(this ITypeReference typeReference)
            => typeReference.TypeKind.IsChar();


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

        /// <summary>
        ///     test if this value is a string value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsStringValue(this ITypeReference typeReference, out IStringValue value) {
            value = typeReference as IStringValue;

            if (typeReference.TypeKind.IsString() && value != default)
                return true;

            value = default;
            return false;
        }

        /// <summary>
        ///     test if this value is a string value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsOrdinalValue(this ITypeReference typeReference, out IOrdinalValue value) {
            value = typeReference as IOrdinalValue;

            if (typeReference.TypeKind.IsOrdinal() && value != default)
                return true;

            value = default;
            return false;
        }


        /// <summary>
        ///     test if this value is an array value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsArrayValue(this ITypeReference typeReference, out IArrayValue value) {
            value = typeReference as IArrayValue;

            if (typeReference.TypeKind.IsArray() && value != default)
                return true;

            value = default;
            return false;
        }

        /// <summary>
        ///     boolean value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBooleanValue(this ITypeReference typeReference, out IBooleanValue value) {
            value = typeReference as IBooleanValue;

            if (typeReference.TypeKind == CommonTypeKind.BooleanType && value != default)
                return true;

            value = default;
            return false;
        }

        /// <summary>
        ///     boolean value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsEnumValue(this ITypeReference typeReference, out IEnumeratedValue value) {
            value = typeReference as IEnumeratedValue;

            if (typeReference.TypeKind == CommonTypeKind.EnumerationType && value != default)
                return true;

            value = default;
            return false;
        }

        /// <summary>
        ///     test if this value is a char value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCharValue(this ITypeReference typeReference, out ICharValue value) {
            value = typeReference as ICharValue;

            if (typeReference.TypeKind.IsChar() && value != default)
                return true;

            value = default;
            return false;
        }

        /// <summary>
        ///     test if this value is a char value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsAnsiCharValue(this ITypeReference typeReference, out ICharValue value) {
            value = typeReference as ICharValue;

            if (typeReference.TypeKind == CommonTypeKind.AnsiCharType && value != default)
                return true;

            value = default;
            return false;
        }

        /// <summary>
        ///     test if this value is a char value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsWideCharValue(this ITypeReference typeReference, out ICharValue value) {
            value = typeReference as ICharValue;

            if (typeReference.TypeKind == CommonTypeKind.WideCharType && value != default)
                return true;

            value = default;
            return false;
        }

        /// <summary>
        ///     test if this value is an integral value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIntegralValue(this ITypeReference typeReference, out IIntegerValue value) {
            value = typeReference as IIntegerValue;

            if (typeReference.TypeKind.IsIntegral() && value != default)
                return true;

            value = default;
            return false;
        }

        /// <summary>
        ///     test if this value is an integral value
        /// </summary>
        /// <param name="typeReference"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsRealValue(this ITypeReference typeReference, out IRealNumberValue value) {
            value = typeReference as IRealNumberValue;

            if (typeReference.TypeKind == CommonTypeKind.RealType && value != default)
                return true;

            value = default;
            return false;
        }


    }

}