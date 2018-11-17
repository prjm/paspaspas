using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.BooleanValues;
using PasPasPas.Runtime.Values.CharValues;
using PasPasPas.Runtime.Values.FloatValues;
using PasPasPas.Runtime.Values.IntValues;
using PasPasPas.Runtime.Values.StringValues;
using PasPasPas.Runtime.Values.Structured;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     runtime values: value creation and operations
    /// </summary>
    public class RuntimeValueFactory : IRuntimeValueFactory {

        /// <summary>
        ///     create a new runtime value factory
        /// </summary>
        /// <param name="typeKindResolver">type kind resolver</param>
        public RuntimeValueFactory(ITypeRegistry typeKindResolver) {
            Types = new TypeOperations(typeKindResolver);
            Booleans = new BooleanOperations();
            Integers = new IntegerOperations(Booleans, Types);
            RealNumbers = new RealNumberOperations(Booleans);
            Strings = new StringOperations(Booleans);
        }

        /// <summary>
        ///     integer operations: value factory and arithmetics
        /// </summary>
        public IIntegerOperations Integers { get; }

        /// <summary>
        ///     real number operations: value factory and arithmetics
        /// </summary>
        public IRealNumberOperations RealNumbers { get; }

        /// <summary>
        ///     boolean operations: constants, value factory and arithmetics
        /// </summary>
        public IBooleanOperations Booleans { get; }
            = new BooleanOperations();

        /// <summary>
        ///     string operations: value factory, concatenation
        /// </summary>
        public IStringOperations Strings { get; }

        /// <summary>
        ///     operations on characters
        /// </summary>
        public ICharOperations Chars { get; }
            = new CharOperations();

        /// <summary>
        ///     open type operations
        /// </summary>
        public ITypeOperations Types { get; }

        /// <summary>
        ///     structured type operations
        /// </summary>
        public IStructuredTypeOperations Structured { get; }
            = new StructuredTypeOperations();

        /// <summary>
        ///     cast values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeReference Cast(ITypeReference value, int typeId) {
            var typeKind = value.TypeKind;

            if (typeKind.IsIntegral())
                return CastInteger(value, typeId);

            if (typeKind.IsChar())
                return CastChar(value, typeId);

            return Types.MakeReference(KnownTypeIds.ErrorType);
        }

        private ITypeReference CastInteger(ITypeReference value, int typeId) {

            var typeDef = Types.TypeRegistry.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (!(value is IIntegerValue integer))
                return Types.MakeReference(KnownTypeIds.ErrorType);

            if (typeDef is EnumeratedType enumType)
                return new EnumeratedValue(enumType.TypeId, CastInteger(value, enumType.CommonTypeId));

            if (typeDef is SubrangeType subrangeType)
                return CastInteger(value, subrangeType.BaseType.TypeId);

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

            return Types.MakeReference(KnownTypeIds.ErrorType);
        }

        private ITypeReference CastChar(ITypeReference value, int typeId) {

            var typeDef = Types.TypeRegistry.GetTypeByIdOrUndefinedType(typeId);
            typeDef = TypeBase.ResolveAlias(typeDef);

            if (!(value is ICharValue charValue))
                return Types.MakeReference(KnownTypeIds.ErrorType);

            if (typeDef is EnumeratedType enumType)
                return new EnumeratedValue(enumType.TypeId, CastChar(value, enumType.CommonTypeId));

            if (typeDef is SubrangeType subrangeType)
                return CastChar(value, subrangeType.BaseType.TypeId);

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

            return Types.MakeReference(KnownTypeIds.ErrorType);
        }

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference MakeEnumValue(int typeId, ITypeReference value)
            => new EnumeratedValue(typeId, value);
    }
}