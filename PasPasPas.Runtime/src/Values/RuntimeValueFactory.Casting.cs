using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
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
        public IOldTypeReference Cast(ITypeRegistry types, IOldTypeReference value, int typeId) {
            var typeKind = CommonTypeKind.UnknownType;

            if (value.TypeId == typeId)
                return value;

            if (value.IsType())
                typeKind = types.GetTypeByIdOrUndefinedType(value.TypeId).TypeKind;
            else
                typeKind = value.TypeKind;

            if (!value.IsConstant())
                return Types.MakeTypeInstanceReference(types.Cast(value.TypeId, typeId), types.GetTypeKindOf(typeId));

            if (typeKind.IsIntegral())
                return CastInteger(types, value, typeId);

            if (typeKind.IsChar())
                return CastChar(types, value, typeId);

            if (typeKind.IsString())
                return CastString(types, value, typeId);

            if (typeKind.IsArray())
                return CastArray(types, value, typeId);

            if (typeKind.IsEnum())
                return CastEnum(types, value, typeId);

            if (typeKind.IsSubrange() && types.GetTypeByIdOrUndefinedType(typeId) is ISubrangeType subrangeType)
                return Cast(types, value, subrangeType.BaseTypeId);

            if (typeKind == CommonTypeKind.BooleanType)
                return CastBoolean(types, value, typeId);

            if (typeKind == CommonTypeKind.SetType)
                return CastSet(types, value, typeId);

            if (typeKind == CommonTypeKind.RecordType)
                return CastRecord(types, value, typeId);

            return Types.MakeErrorTypeReference();
        }

        private IOldTypeReference CastSet(ITypeRegistry types, IOldTypeReference value, int typeId) {
            var typeDef = types.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (typeDef is ISetType setType && value is SetValue setValue) {

                using (var list = ListPools.GetList<IOldTypeReference>()) {
                    foreach (var sourceValue in setValue.Values) {
                        var targetValue = Cast(types, sourceValue, setType.BaseTypeId);

                        if (targetValue.TypeId != setType.BaseTypeId)
                            return Types.MakeErrorTypeReference();

                        list.Add(targetValue);
                    }

                    return new SetValue(setType.TypeId, ListPools.GetFixedArray(list));
                }
            }

            return Types.MakeErrorTypeReference();
        }

        private IOldTypeReference CastEnum(ITypeRegistry types, IOldTypeReference value, int typeId) {
            var typeDef = types.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (typeDef is SubrangeType subrangeType) {
                var castedValue = Cast(types, value, subrangeType.BaseType.TypeId);
                return MakeSubrangeValue(typeDef.TypeId, castedValue);
            }

            return Types.MakeErrorTypeReference();
        }

        private IOldTypeReference CastInteger(ITypeRegistry types, IOldTypeReference value, int typeId) {

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

            if (typeDef is IIntegralType integralType) {
                switch (integralType.BitSize) {
                    case 8:
                        return integralType.IsSigned ?
                            Integers.ToIntegerValue((sbyte)integer.SignedValue) :
                            Integers.ToIntegerValue((byte)integer.UnsignedValue);


                    case 16:
                        return integralType.IsSigned ?
                            Integers.ToIntegerValue((short)integer.SignedValue) :
                            Integers.ToIntegerValue((ushort)integer.UnsignedValue);

                    case 32:
                        return integralType.IsSigned ?
                            Integers.ToIntegerValue((int)integer.SignedValue) :
                            Integers.ToIntegerValue((uint)integer.UnsignedValue);


                }
            }

            switch (typeDef.TypeId) {
                case KnownTypeIds.Int64Type:
                    return Integers.ToIntegerValue(integer.SignedValue);
                case KnownTypeIds.UInt64Type:
                    return Integers.ToIntegerValue(integer.UnsignedValue);
                case KnownTypeIds.WideCharType:
                    return Chars.ToWideCharValue(typeId, (char)integer.UnsignedValue);
                case KnownTypeIds.AnsiCharType:
                    return Chars.ToAnsiCharValue(typeId, (byte)integer.UnsignedValue);
                case KnownTypeIds.BooleanType:
                    return Booleans.ToBoolean(integer.UnsignedValue != 0, KnownTypeIds.BooleanType);
                case KnownTypeIds.ByteBoolType:
                    return Booleans.ToByteBool((byte)integer.UnsignedValue, KnownTypeIds.ByteBoolType);
                case KnownTypeIds.WordBoolType:
                    return Booleans.ToWordBool((ushort)integer.UnsignedValue, KnownTypeIds.WordBoolType);
            }

            return Types.MakeErrorTypeReference();
        }

        private IOldTypeReference CastString(ITypeRegistry types, IOldTypeReference value, int typeId) {
            var typeDef = types.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (!value.IsConstant()) {
                // TODO: add some type checking here
                if (value.IsType())
                    return types.MakeTypeReference(typeId);
                return types.MakeTypeInstanceReference(typeId);
            }

            if (value is IStringValue stringValue) {

                if (typeDef is IStringType stringType) {

                    if (typeDef.TypeKind == CommonTypeKind.ShortStringType)
                        return Strings.ToShortString(stringValue.AsUnicodeString);

                    if (typeDef.TypeKind == CommonTypeKind.LongStringType)
                        return Strings.ToShortString(stringValue.AsUnicodeString);

                    if (typeDef.TypeKind == CommonTypeKind.UnicodeStringType)
                        return Strings.ToUnicodeString(stringValue.AsUnicodeString);

                }

                if (typeDef is IArrayType arrayType && types.GetTypeByIdOrUndefinedType(arrayType.BaseTypeId).TypeKind.IsChar()) {

                    using (var values = ListPools.GetList<IOldTypeReference>()) {

                        for (var index = 0; index < stringValue.NumberOfCharElements; index++)
                            values.Item.Add(Cast(types, stringValue.CharAt(index), arrayType.BaseTypeId));

                        return Structured.CreateArrayValue(typeDef.TypeId, arrayType.BaseTypeId, ListPools.GetFixedArray(values));

                    }

                }

            }

            if (value is ICharValue charValue) {

            }

            return Types.MakeErrorTypeReference();
        }


        private IOldTypeReference CastChar(ITypeRegistry types, IOldTypeReference value, int typeId) {

            var typeDef = types.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (!value.IsConstant()) {
                // TODO: add some type checking here
                if (value.IsType())
                    return types.MakeTypeReference(typeId);
                return types.MakeTypeInstanceReference(typeId);
            }

            if (!(value is ICharValue charValue))
                return Types.MakeErrorTypeReference();

            if (typeDef is EnumeratedType enumType)
                return new EnumeratedValue(enumType.TypeId, CastChar(types, value, enumType.CommonTypeId));

            if (typeDef is SubrangeType subrangeType) {
                var castedValue = CastChar(types, value, subrangeType.BaseType.TypeId);
                return MakeSubrangeValue(typeDef.TypeId, castedValue);
            }

            if (typeDef is IStringType stringType) {


                if (typeDef.TypeKind == CommonTypeKind.ShortStringType)
                    return Strings.ToShortString(charValue.AsUnicodeString);

                if (typeDef.TypeKind == CommonTypeKind.LongStringType)
                    return Strings.ToAnsiString(charValue.AsUnicodeString);

                if (typeDef.TypeKind == CommonTypeKind.UnicodeStringType)
                    return Strings.ToUnicodeString(charValue.AsUnicodeString);

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
                case KnownTypeIds.UInt64Type:
                    return Integers.ToIntegerValue((ulong)charValue.AsWideChar);
                case KnownTypeIds.WideCharType:
                    return Chars.ToWideCharValue(typeId, charValue.AsWideChar);
                case KnownTypeIds.AnsiCharType:
                    return Chars.ToAnsiCharValue(typeId, (byte)charValue.AsWideChar);
                case KnownTypeIds.BooleanType:
                    return Booleans.ToBoolean(charValue.AsWideChar != 0, KnownTypeIds.BooleanType);
                case KnownTypeIds.ByteBoolType:
                    return Booleans.ToByteBool((byte)charValue.AsWideChar, KnownTypeIds.ByteBoolType);
                case KnownTypeIds.WordBoolType:
                    return Booleans.ToWordBool(charValue.AsWideChar, KnownTypeIds.WordBoolType);

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
        private IOldTypeReference CastBoolean(ITypeRegistry types, IOldTypeReference value, int typeId) {
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
                        return Booleans.ToByteBool((byte)boolValue.AsUint, KnownTypeIds.ByteBoolType);
                    case 16:
                        return Booleans.ToWordBool((ushort)boolValue.AsUint, KnownTypeIds.WordBoolType);
                    case 32:
                        return Booleans.ToLongBool(boolValue.AsUint, KnownTypeIds.LongBoolType);
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
        private IOldTypeReference CastArray(ITypeRegistry types, IOldTypeReference value, int typeId) {
            var type = TypeBase.ResolveAlias(types.GetTypeByIdOrUndefinedType(typeId));

            if (!value.IsConstant())
                return Types.MakeErrorTypeReference();

            if (type is ArrayType arrayType) {

                if (value is IArrayValue array) {
                    var newBaseType = Cast(types, types.MakeTypeReference(array.BaseType), arrayType.BaseTypeId);

                    if (newBaseType.TypeId == KnownTypeIds.Unused)
                        return Types.MakeErrorTypeReference();

                    using (var values = ListPools.GetList<IOldTypeReference>()) {

                        foreach (var itemValue in array.Values) {
                            values.Item.Add(Cast(types, itemValue, newBaseType.TypeId));
                        }

                        var castedValue = new ArrayValue(typeId, newBaseType.TypeId, ListPools.GetFixedArray(values));
                        return castedValue;
                    }
                }

            }

            return Types.MakeErrorTypeReference();
        }

        private IOldTypeReference CastRecord(ITypeRegistry types, IOldTypeReference value, int typeId) {
            var typeDef = types.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (!(typeDef is StructuredTypeDeclaration structType) || !types.AreRecordTypesCompatible(value.TypeId, typeId) || !(value is RecordValue record))
                return types.Runtime.Types.MakeErrorTypeReference();

            using (var list = ListPools.GetList<IOldTypeReference>()) {

                for (var i = 0; i < record.Values.Length; i++)
                    list.Add(Cast(types, record.Values[i], structType.Fields[i].TypeId));

                return types.Runtime.Structured.CreateRecordValue(typeId, ListPools.GetFixedArray(list));
            }
        }

    }
}
