using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Runtime.Values.Structured;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Runtime.Values {

    public partial class RuntimeValueFactory {

        /// <summary>
        ///     cast values
        /// </summary>
        /// <param name="types"></param>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeReference Cast(ITypeRegistry types, ITypeReference value, int typeId) {
            var typeKind = CommonTypeKind.UnknownType;

            if (value.TypeId == typeId)
                return value;

            if (value.IsType())
                typeKind = types.GetTypeByIdOrUndefinedType(value.TypeId).TypeKind;
            else
                typeKind = value.TypeKind;

            if (typeKind.IsIntegral())
                return CastInteger(types, value, typeId);

            if (typeKind.IsChar())
                return CastChar(types, value, typeId);

            if (typeKind.IsArray())
                return CastArray(types, value, typeId);

            if (typeKind == CommonTypeKind.EnumerationType)
                return CastEnum(types, value, typeId);

            if (typeKind.IsSubrange() && types.GetTypeByIdOrUndefinedType(typeId) is ISubrangeType subrangeType)
                return Cast(types, value, subrangeType.BaseTypeId);

            if (typeKind == CommonTypeKind.BooleanType)
                return CastBoolean(types, value, typeId);

            return Types.MakeErrorTypeReference();
        }

        private ITypeReference CastEnum(ITypeRegistry types, ITypeReference value, int typeId) {
            var typeDef = types.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (typeDef is SubrangeType subrangeType) {
                var castedValue = Cast(types, value, subrangeType.BaseType.TypeId);
                return MakeSubrangeValue(typeDef.TypeId, castedValue);
            }

            return Types.MakeErrorTypeReference();
        }

        private ITypeReference CastInteger(ITypeRegistry types, ITypeReference value, int typeId) {

            var typeDef = types.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (!(value is IIntegerValue integer))
                return Types.MakeErrorTypeReference();

            if (typeDef is EnumeratedType enumType)
                return new EnumeratedValue(enumType.TypeId, CastInteger(types, value, enumType.CommonTypeId));

            if (typeDef is SubrangeType subrangeType) {
                var castedValue = CastInteger(types, value, subrangeType.BaseType.TypeId);
                return MakeSubrangeValue(typeDef.TypeId, castedValue);
            }

            switch (typeDef.TypeId) {
                case KnownTypeIds.ShortInt:
                    return Integers.ToIntegerValue((sbyte)integer.SignedValue);
                case KnownTypeIds.ByteType:
                    return Integers.ToIntegerValue((byte)integer.UnsignedValue);
                case KnownTypeIds.SmallInt:
                    return Integers.ToIntegerValue((short)integer.SignedValue);
                case KnownTypeIds.WordType:
                    return Integers.ToIntegerValue((ushort)integer.UnsignedValue);
                case KnownTypeIds.IntegerType:
                    return Integers.ToIntegerValue((int)integer.SignedValue);
                case KnownTypeIds.CardinalType:
                    return Integers.ToIntegerValue((uint)integer.UnsignedValue);
                case KnownTypeIds.Int64Type:
                    return Integers.ToIntegerValue(integer.SignedValue);
                case KnownTypeIds.Uint64Type:
                    return Integers.ToIntegerValue(integer.UnsignedValue);
                case KnownTypeIds.WideCharType:
                    return Chars.ToWideCharValue((char)integer.UnsignedValue);
                case KnownTypeIds.AnsiCharType:
                    return Chars.ToAnsiCharValue((byte)integer.UnsignedValue);
                case KnownTypeIds.BooleanType:
                    return Booleans.ToBoolean(integer.UnsignedValue != 0);
                case KnownTypeIds.ByteBoolType:
                    return Booleans.ToByteBool((byte)integer.UnsignedValue);
                case KnownTypeIds.WordBoolType:
                    return Booleans.ToWordBool((ushort)integer.UnsignedValue);
            }

            return Types.MakeErrorTypeReference();
        }

        private ITypeReference CastChar(ITypeRegistry types, ITypeReference value, int typeId) {

            var typeDef = types.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (!value.IsConstant()) {
                // TODO: add some type checking here
                if (value.IsType())
                    return types.MakeTypeReference(typeId);
                return types.MakeReference(typeId);
            }

            if (!(value is ICharValue charValue))
                return Types.MakeErrorTypeReference();

            if (typeDef is EnumeratedType enumType)
                return new EnumeratedValue(enumType.TypeId, CastChar(types, value, enumType.CommonTypeId));

            if (typeDef is SubrangeType subrangeType) {
                var castedValue = CastChar(types, value, subrangeType.BaseType.TypeId);
                return MakeSubrangeValue(typeDef.TypeId, castedValue);
            }

            switch (typeDef.TypeId) {
                case KnownTypeIds.ShortInt:
                    return Integers.ToIntegerValue((sbyte)charValue.AsWideChar);
                case KnownTypeIds.ByteType:
                    return Integers.ToIntegerValue((byte)charValue.AsWideChar);
                case KnownTypeIds.SmallInt:
                    return Integers.ToIntegerValue((short)charValue.AsWideChar);
                case KnownTypeIds.WordType:
                    return Integers.ToIntegerValue(charValue.AsWideChar);
                case KnownTypeIds.IntegerType:
                    return Integers.ToIntegerValue((int)charValue.AsWideChar);
                case KnownTypeIds.CardinalType:
                    return Integers.ToIntegerValue((uint)charValue.AsWideChar);
                case KnownTypeIds.Int64Type:
                    return Integers.ToIntegerValue((long)charValue.AsWideChar);
                case KnownTypeIds.Uint64Type:
                    return Integers.ToIntegerValue((ulong)charValue.AsWideChar);
                case KnownTypeIds.WideCharType:
                    return Chars.ToWideCharValue(charValue.AsWideChar);
                case KnownTypeIds.AnsiCharType:
                    return Chars.ToAnsiCharValue((byte)charValue.AsWideChar);
                case KnownTypeIds.BooleanType:
                    return Booleans.ToBoolean(charValue.AsWideChar != 0);
                case KnownTypeIds.ByteBoolType:
                    return Booleans.ToByteBool((byte)charValue.AsWideChar);
                case KnownTypeIds.WordBoolType:
                    return Booleans.ToWordBool(charValue.AsWideChar);

            }

            return Types.MakeErrorTypeReference();
        }

        /// <summary>
        ///     cast a boolean value
        /// </summary>
        /// <param name="types"></param>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        private ITypeReference CastBoolean(ITypeRegistry types, ITypeReference value, int typeId) {
            var type = TypeBase.ResolveAlias(types.GetTypeByIdOrUndefinedType(typeId));

            if (type is ISubrangeType subrangeType) {
                var castedValue = CastBoolean(types, value, subrangeType.BaseType.TypeId);
                return MakeSubrangeValue(type.TypeId, castedValue);
            }

            if (value.IsBooleanValue(out var boolValue) && type is IBooleanType booleanType) {
                switch (booleanType.BitSize) {
                    case 1:
                        return boolValue.AsBoolean ? Booleans.TrueValue : Booleans.FalseValue;
                    case 8:
                        return Booleans.ToByteBool((byte)boolValue.AsUint);
                    case 16:
                        return Booleans.ToWordBool((ushort)boolValue.AsUint);
                    case 32:
                        return Booleans.ToLongBool(boolValue.AsUint);
                }
            }

            return Types.MakeErrorTypeReference();
        }

        /// <summary>
        ///     cast an array type
        /// </summary>
        /// <param name="types"></param>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        private ITypeReference CastArray(ITypeRegistry types, ITypeReference value, int typeId) {
            var type = TypeBase.ResolveAlias(types.GetTypeByIdOrUndefinedType(typeId));

            if (value.IsConstant() && type is ArrayType arrayType) {
                var array = value as IArrayValue;
                var newBaseType = Cast(types, types.MakeTypeReference(array.BaseType), arrayType.BaseTypeId);

                if (newBaseType.TypeId == KnownTypeIds.ErrorType)
                    return Types.MakeErrorTypeReference();

                using (var values = ListPools.GetList<ITypeReference>()) {

                    foreach (var itemValue in array.Values) {
                        values.Item.Add(Cast(types, itemValue, array.BaseType));
                    }

                    var castedValue = new ArrayValue(arrayType.TypeId, arrayType.BaseTypeId, ListPools.GetFixedArray(values));
                    return castedValue;
                }
            }

            return Types.MakeErrorTypeReference();
        }
    }
}
