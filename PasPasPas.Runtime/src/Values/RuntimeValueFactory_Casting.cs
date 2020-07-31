using System;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.Structured;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values {

    public partial class RuntimeValueFactory {

        private readonly Lazy<IValue> invalidCast;

        /// <summary>
        ///     invalid cast value
        /// </summary>
        public IValue InvalidCast
            => invalidCast.Value;

        /// <summary>
        ///     cast values
        /// </summary>
        /// <param name="types"></param>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IValue Cast(ITypeRegistry types, IValue value, ITypeDefinition typeId) {

            if (value.TypeDefinition.Equals(typeId))
                return value;

            switch (value.TypeDefinition.BaseType) {

                case BaseType.Subrange:
                    return Cast(types, value, typeId);

                case BaseType.Boolean:
                    return CastBoolean(types, value, typeId);

                case BaseType.Char:
                    return CastChar(types, value, typeId);

                case BaseType.String:
                    return CastString(types, value, typeId);

                case BaseType.Integer:
                    return CastInteger(types, value, typeId);

                case BaseType.Enumeration:
                    return CastEnum(types, value, typeId);

                case BaseType.Array:
                    return CastArray(types, value, typeId);

                case BaseType.Set:
                    return CastSet(types, value, typeId);

                case BaseType.Structured:
                    return CastRecord(types, value, typeId);

                case BaseType.Unkown:
                case BaseType.Hidden:
                case BaseType.Error:
                case BaseType.Real:
                case BaseType.Pointer:
                case BaseType.Unit:
                case BaseType.File:
                case BaseType.MetaClass:
                case BaseType.GenericTypeParameter:
                case BaseType.Routine:
                    return invalidCast.Value;

                default:
                    return invalidCast.Value;
            }
        }

        private IValue CastSet(ITypeRegistry types, IValue value, ITypeDefinition typeDef) {
            typeDef = typeDef.ResolveAlias();

            if (typeDef is ISetType setType && value is SetValue setValue) {
                using var list = ListPools.GetList<IValue>();
                foreach (var sourceValue in setValue.Values) {
                    var targetValue = Cast(types, sourceValue, setType.BaseTypeDefinition);

                    if (!targetValue.TypeDefinition.Equals(setType.BaseTypeDefinition))
                        return invalidCast.Value;

                    list.Add(targetValue);
                }

                return new SetValue(setType, ListPools.GetFixedArray(list));
            }

            return invalidCast.Value;
        }

        private IValue CastEnum(ITypeRegistry types, IValue value, ITypeDefinition typeDef) {
            typeDef = typeDef.ResolveAlias();

            if (typeDef is ISubrangeType subrangeType) {
                var castedValue = Cast(types, value, subrangeType.SubrangeOfType);
                return MakeSubrangeValue(typeDef, castedValue);
            }

            return invalidCast.Value;
        }

        private IValue CastInteger(ITypeRegistry types, IValue value, ITypeDefinition typeDef) {
            typeDef = typeDef.ResolveAlias();

            if (!(value is IIntegerValue integer))
                return invalidCast.Value;

            if (typeDef is IEnumeratedType enumType) {
                var castResult = CastInteger(types, integer, enumType.CommonTypeId);
                if (castResult is IIntegerValue ci)
                    return MakeEnumValue(enumType, ci, string.Empty);
                else
                    return invalidCast.Value;
            }

            if (typeDef is ISubrangeType subrangeType) {
                var castedValue = CastInteger(types, value, subrangeType.SubrangeOfType);
                return MakeSubrangeValue(typeDef, castedValue);
            }

            if (typeDef is IIntegralType integralType) {
                switch (integralType.Kind) {

                    case IntegralTypeKind.ShortInt:
                        return Integers.ToIntegerValue((sbyte)integer.SignedValue);

                    case IntegralTypeKind.Byte:
                        return Integers.ToIntegerValue((byte)integer.UnsignedValue);

                    case IntegralTypeKind.Word:
                        return Integers.ToIntegerValue((short)integer.SignedValue);

                    case IntegralTypeKind.SmallInt:
                        return Integers.ToIntegerValue((ushort)integer.UnsignedValue);

                    case IntegralTypeKind.Cardinal:
                        return Integers.ToIntegerValue((uint)integer.UnsignedValue);

                    case IntegralTypeKind.Integer:
                        return Integers.ToIntegerValue((int)integer.SignedValue);

                    case IntegralTypeKind.Int64:
                        return Integers.ToIntegerValue(integer.SignedValue);

                    case IntegralTypeKind.UInt64:
                        return Integers.ToIntegerValue(integer.UnsignedValue);

                }
            }

            if (typeDef is ICharType charType) {
                switch (charType.Kind) {
                    case CharTypeKind.AnsiChar:
                        return Chars.ToAnsiCharValue(typeDef, (byte)integer.UnsignedValue);
                    case CharTypeKind.WideChar:
                        return Chars.ToWideCharValue(typeDef, (char)integer.UnsignedValue);
                }
            }

            if (typeDef is IBooleanType booleanType) {
                switch (booleanType.Kind) {
                    case BooleanTypeKind.Boolean:
                        return Booleans.ToBoolean(integer.UnsignedValue != 0, typeDef);

                    case BooleanTypeKind.ByteBool:
                        return Booleans.ToByteBool((byte)integer.UnsignedValue, typeDef);

                    case BooleanTypeKind.WordBool:
                        return Booleans.ToWordBool((ushort)integer.UnsignedValue, typeDef);

                    case BooleanTypeKind.LongBool:
                        return Booleans.ToLongBool((uint)integer.UnsignedValue, typeDef);
                }
            }

            return invalidCast.Value;
        }

        private IValue CastString(ITypeRegistry types, IValue value, ITypeDefinition typeDef) {
            typeDef = typeDef.ResolveAlias();

            if (value is IStringValue stringValue) {

                if (typeDef is IStringType stringType) {

                    switch (stringType.Kind) {
                        case StringTypeKind.AnsiString:
                            return Strings.ToAnsiString(stringValue.AsUnicodeString);

                        case StringTypeKind.WideStringType:
                            return Strings.ToWideString(stringValue.AsUnicodeString);

                        case StringTypeKind.UnicodeString:
                            return Strings.ToUnicodeString(stringValue.AsUnicodeString);

                        case StringTypeKind.ShortString:
                            return Strings.ToShortString(stringValue.AsUnicodeString);
                    }
                }

                if (typeDef is IArrayType arrayType && arrayType.BaseTypeDefinition.BaseType == BaseType.Char) {
                    using var values = ListPools.GetList<IValue>();
                    for (var index = 0; index < stringValue.NumberOfCharElements; index++)
                        values.Item.Add(Cast(types, stringValue.CharAt(index), arrayType.BaseTypeDefinition));

                    return Structured.CreateArrayValue(typeDef, arrayType.BaseTypeDefinition, ListPools.GetFixedArray(values));

                }

            }

            return invalidCast.Value;
        }


        private IValue CastChar(ITypeRegistry types, IValue value, ITypeDefinition typeDef) {
            typeDef = typeDef.ResolveAlias();

            if (!(value is ICharValue charValue))
                return invalidCast.Value;

            if (typeDef is IEnumeratedType enumType) {
                var castResult = CastChar(types, value, enumType.CommonTypeId);
                if (castResult is IIntegerValue ci)
                    return MakeEnumValue(enumType, ci, string.Empty);
                else
                    return invalidCast.Value;
            }

            if (typeDef is ISubrangeType subrangeType) {
                var castedValue = CastChar(types, value, subrangeType.SubrangeOfType);
                return MakeSubrangeValue(typeDef, castedValue);
            }

            if (typeDef is IStringType stringType) {

                switch (stringType.Kind) {
                    case StringTypeKind.AnsiString:
                        return Strings.ToAnsiString(charValue.AsUnicodeString);

                    case StringTypeKind.WideStringType:
                        return Strings.ToWideString(charValue.AsUnicodeString);

                    case StringTypeKind.UnicodeString:
                        return Strings.ToUnicodeString(charValue.AsUnicodeString);

                    case StringTypeKind.ShortString:
                        return Strings.ToShortString(charValue.AsUnicodeString);
                }


            }

            if (typeDef is IIntegralType intType) {

                switch (intType.Kind) {

                    case IntegralTypeKind.Byte:
                        return Integers.ToIntegerValue((byte)charValue.AsWideChar);

                    case IntegralTypeKind.ShortInt:
                        return Integers.ToIntegerValue((sbyte)charValue.AsWideChar);

                    case IntegralTypeKind.Word:
                        return Integers.ToIntegerValue(charValue.AsWideChar);

                    case IntegralTypeKind.SmallInt:
                        return Integers.ToIntegerValue((short)charValue.AsWideChar);

                    case IntegralTypeKind.Cardinal:
                        return Integers.ToIntegerValue((uint)charValue.AsWideChar);

                    case IntegralTypeKind.Integer:
                        return Integers.ToIntegerValue((int)charValue.AsWideChar);

                    case IntegralTypeKind.UInt64:
                        return Integers.ToIntegerValue((ulong)charValue.AsWideChar);

                    case IntegralTypeKind.Int64:
                        return Integers.ToIntegerValue((long)charValue.AsWideChar);
                }
            }

            if (typeDef is ICharType charType) {
                switch (charType.Kind) {
                    case CharTypeKind.AnsiChar:
                        return Chars.ToAnsiCharValue(typeDef, (byte)charValue.AsWideChar);
                    case CharTypeKind.WideChar:
                        return Chars.ToWideCharValue(typeDef, charValue.AsWideChar);
                }
            }

            if (typeDef is IBooleanType booleanType) {

                switch (booleanType.Kind) {
                    case BooleanTypeKind.Boolean:
                        return Booleans.ToBoolean(charValue.AsWideChar != 0, typeDef);
                    case BooleanTypeKind.ByteBool:
                        return Booleans.ToByteBool((byte)charValue.AsWideChar, typeDef);
                    case BooleanTypeKind.WordBool:
                        return Booleans.ToWordBool(charValue.AsWideChar, typeDef);
                    case BooleanTypeKind.LongBool:
                        return Booleans.ToLongBool(charValue.AsWideChar, typeDef);
                }

            }

            return invalidCast.Value;
        }

        /// <summary>
        ///     cast a boolean value
        /// </summary>
        /// <param name="types"></param>
        /// <param name="value"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        private IValue CastBoolean(ITypeRegistry types, IValue value, ITypeDefinition typeDef) {
            typeDef = typeDef.ResolveAlias();

            if (typeDef is ISubrangeType subrangeType) {
                var castedValue = CastBoolean(types, value, subrangeType.SubrangeOfType);
                return MakeSubrangeValue(typeDef, castedValue);
            }

            if (value is IBooleanValue b && typeDef is IBooleanType booleanType) {

                switch (booleanType.Kind) {
                    case BooleanTypeKind.Boolean:
                        return Booleans.ToBoolean(b.AsBoolean, typeDef);
                    case BooleanTypeKind.ByteBool:
                        return Booleans.ToByteBool((byte)b.AsUint, typeDef);
                    case BooleanTypeKind.WordBool:
                        return Booleans.ToWordBool((ushort)b.AsUint, typeDef);
                    case BooleanTypeKind.LongBool:
                        return Booleans.ToLongBool(b.AsUint, typeDef);
                }
            }

            return invalidCast.Value;
        }

        /// <summary>
        ///     cast an array type
        /// </summary>
        /// <param name="types"></param>
        /// <param name="value"></param>
        /// <param name="typeDef"></param>
        /// <returns></returns>
        private IValue CastArray(ITypeRegistry types, IValue value, ITypeDefinition typeDef) {
            typeDef = typeDef.ResolveAlias();

            if (typeDef is IArrayType arrayType) {

                if (value is IArrayValue array) {
                    var newBaseType = types.Cast(array.BaseTypeDefinition.Reference, arrayType.BaseTypeDefinition.Reference);

                    if (newBaseType.GetBaseType() == BaseType.Error)
                        return invalidCast.Value;

                    using var values = ListPools.GetList<IValue>();
                    foreach (var itemValue in array.Values) {
                        values.Item.Add(Cast(types, itemValue, newBaseType.TypeDefinition));
                    }

                    var castedValue = new ArrayValue(typeDef, newBaseType.TypeDefinition, ListPools.GetFixedArray(values));
                    return castedValue;
                }

            }

            return invalidCast.Value;
        }

        private IValue CastRecord(ITypeRegistry types, IValue value, ITypeDefinition typeDef) {
            typeDef = typeDef.ResolveAlias();

            if (!(typeDef is IStructuredType structType) || !types.AreRecordTypesCompatible(value.TypeDefinition, typeDef) || !(value is RecordValue record))
                return invalidCast.Value;


            using var list = ListPools.GetList<IValue>();

            for (var i = 0; i < record.Values.Length; i++)
                list.Add(Cast(types, record.Values[i], structType.Fields[i].TypeDefinition));

            return types.Runtime.Structured.CreateRecordValue(typeDef, ListPools.GetFixedArray(list));
        }

    }
}
