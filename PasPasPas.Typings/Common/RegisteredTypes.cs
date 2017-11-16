using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Typings.Common {


    /// <summary>
    ///     common type registry
    /// </summary>
    public class RegisteredTypes : ITypeRegistry, IEnvironmentItem {

        private readonly IDictionary<int, ITypeDefinition> types
            = new Dictionary<int, ITypeDefinition>();

        private readonly IDictionary<int, IOperator> operators
            = new Dictionary<int, IOperator>();

        private readonly object idLock = new object();
        private int userTypeIds = 1000;

        /// <summary>
        ///     number of types
        /// </summary>
        public int Count
            => types.Count;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => "TypeRegistry";

        /// <summary>
        ///     registered types
        /// </summary>
        IEnumerable<ITypeDefinition> ITypeRegistry.RegisteredTypes
            => types.Values;

        /// <summary>
        ///     register a new type
        /// </summary>
        /// <param name="type">type to register</param>
        public void RegisterType(ITypeDefinition type) {
            types.Add(type.TypeId, type);

            if (type is TypeBase baseType)
                baseType.TypeRegistry = this;
        }

        /// <summary>
        ///     create a new type registry
        /// </summary>
        /// <param name="intSize">integer size</param>
        /// <param name="pool">string pool</param>
        public RegisteredTypes(StringPool pool, NativeIntSize intSize) {
            RegisterCommonTypes(pool, intSize);
            RegisterCommonOperators();
        }

        private void RegisterCommonOperators() {
            LogicalOperators.RegisterOperators(this);
            ArithmeticOperators.RegisterOperators(this);
            RelationalOperators.RegisterOperators(this);
            StringOperators.RegisterOperators(this);
        }

        /// <summary>
        ///     register an operator
        /// </summary>
        /// <param name="newOperator">operator to register</param>
        public void RegisterOperator(IOperator newOperator) {
            operators.Add(newOperator.Kind, newOperator);
            newOperator.TypeRegistry = this;
        }

        private ScopedName CreateSystemScopeName(StringPool pool, string typeName) {
            var system = pool.PoolString("System");
            typeName = pool.PoolString(typeName);
            return new ScopedName(system, typeName);
        }

        /// <summary>
        ///     register built-in types
        /// </summary>
        private void RegisterCommonTypes(StringPool pool, NativeIntSize intSize) {
            RegisterType(new ErrorType(TypeIds.ErrorType));

            RegisterIntTypes(pool);
            RegisterBoolTypes(pool);
            RegisterStringTypes(pool);
            RegisterRealTypes(pool);
            RegisterPointerTypes(pool);

            RegisterType(new TypeAlias(TypeIds.CharType, TypeIds.WideCharType, CreateSystemScopeName(pool, "Char")));
            RegisterType(new TypeAlias(TypeIds.Ucs2CharType, TypeIds.WideCharType, CreateSystemScopeName(pool, "UCS2Char")));
            RegisterType(new TypeAlias(TypeIds.Ucs4CharType, TypeIds.CardinalType, CreateSystemScopeName(pool, "UCS4Char")));
            RegisterType(new TypeAlias(TypeIds.StringType, TypeIds.UnicodeStringType, CreateSystemScopeName(pool, "String")));
            RegisterType(new TypeAlias(TypeIds.Real, TypeIds.Double, CreateSystemScopeName(pool, "Real")));
            RegisterType(new TypeAlias(TypeIds.PChar, TypeIds.PAnsiChar, CreateSystemScopeName(pool, "PChar")));
            RegisterType(new TypeAlias(TypeIds.PString, TypeIds.PUnicodeString, CreateSystemScopeName(pool, "PString")));

            RegisterNativeIntTypes(pool, intSize);
        }

        private void RegisterPointerTypes(StringPool pool) {
            RegisterType(new PointerType(TypeIds.GenericPointer, TypeIds.UnspecifiedType, CreateSystemScopeName(pool, "Pointer")));
            RegisterType(new PointerType(TypeIds.PByte, TypeIds.ByteType, CreateSystemScopeName(pool, "PByte")));
            RegisterType(new PointerType(TypeIds.PShortInt, TypeIds.ShortInt, CreateSystemScopeName(pool, "PShortInt")));
            RegisterType(new PointerType(TypeIds.PWord, TypeIds.WordType, CreateSystemScopeName(pool, "PWord")));
            RegisterType(new PointerType(TypeIds.PSmallInt, TypeIds.SmallInt, CreateSystemScopeName(pool, "PSmallInt")));
            RegisterType(new PointerType(TypeIds.PCardinal, TypeIds.CardinalType, CreateSystemScopeName(pool, "PCardinal")));
            RegisterType(new PointerType(TypeIds.PLongword, TypeIds.LongWord, CreateSystemScopeName(pool, "PLongword")));
            RegisterType(new PointerType(TypeIds.PFixedUint, TypeIds.FixedUInt, CreateSystemScopeName(pool, "PFixedUint")));
            RegisterType(new PointerType(TypeIds.PInteger, TypeIds.IntegerType, CreateSystemScopeName(pool, "PInteger")));
            RegisterType(new PointerType(TypeIds.PLongInt, TypeIds.LongInt, CreateSystemScopeName(pool, "PLongInt")));
            RegisterType(new PointerType(TypeIds.PFixedInt, TypeIds.FixedInt, CreateSystemScopeName(pool, "PFixedInt")));
            RegisterType(new PointerType(TypeIds.PUInt64, TypeIds.Uint64Type, CreateSystemScopeName(pool, "PUInt64")));
            RegisterType(new PointerType(TypeIds.PInt64, TypeIds.CardinalType, CreateSystemScopeName(pool, "PInt64")));
            RegisterType(new PointerType(TypeIds.PNativeUInt, TypeIds.NativeUInt, CreateSystemScopeName(pool, "PNativeUInt")));
            RegisterType(new PointerType(TypeIds.PNativeInt, TypeIds.NativeInt, CreateSystemScopeName(pool, "PNativeInt")));
            RegisterType(new PointerType(TypeIds.PSingle, TypeIds.SingleType, CreateSystemScopeName(pool, "PSingle")));
            RegisterType(new PointerType(TypeIds.PDouble, TypeIds.Double, CreateSystemScopeName(pool, "PDouble")));
            RegisterType(new PointerType(TypeIds.PExtended, TypeIds.Extended, CreateSystemScopeName(pool, "PExtended")));
            RegisterType(new PointerType(TypeIds.PAnsiChar, TypeIds.AnsiCharType, CreateSystemScopeName(pool, "PAnsiChar")));
            RegisterType(new PointerType(TypeIds.PWideChar, TypeIds.WideCharType, CreateSystemScopeName(pool, "PWideChar")));
            RegisterType(new PointerType(TypeIds.PAnsiString, TypeIds.AnsiStringType, CreateSystemScopeName(pool, "PAnsiString")));
            RegisterType(new PointerType(TypeIds.PRawByteString, TypeIds.RawByteString, CreateSystemScopeName(pool, "PRawByteString")));
            RegisterType(new PointerType(TypeIds.PUnicodeString, TypeIds.UnicodeStringType, CreateSystemScopeName(pool, "PUnicodeString")));
            RegisterType(new PointerType(TypeIds.PShortString, TypeIds.ShortStringType, CreateSystemScopeName(pool, "PShortString")));
            RegisterType(new PointerType(TypeIds.PWideString, TypeIds.WideStringType, CreateSystemScopeName(pool, "PWideString")));
            RegisterType(new PointerType(TypeIds.PBoolean, TypeIds.BooleanType, CreateSystemScopeName(pool, "PBoolean")));
            RegisterType(new PointerType(TypeIds.PLongBool, TypeIds.LongBoolType, CreateSystemScopeName(pool, "PLongBool")));
            RegisterType(new PointerType(TypeIds.PWordBool, TypeIds.WordBoolType, CreateSystemScopeName(pool, "PWordBool")));
            RegisterType(new PointerType(TypeIds.PPointer, TypeIds.GenericPointer, CreateSystemScopeName(pool, "PPointer")));
            RegisterType(new PointerType(TypeIds.PCurrency, TypeIds.Currency, CreateSystemScopeName(pool, "PCurrency")));
        }

        private void RegisterRealTypes(StringPool pool) {
            RegisterType(new RealType(TypeIds.Real48Type, CreateSystemScopeName(pool, "Real48")));
            RegisterType(new RealType(TypeIds.SingleType, CreateSystemScopeName(pool, "Single")));
            RegisterType(new RealType(TypeIds.Double, CreateSystemScopeName(pool, "Double")));
            RegisterType(new RealType(TypeIds.Extended, CreateSystemScopeName(pool, "Extended")));
            RegisterType(new RealType(TypeIds.Comp, CreateSystemScopeName(pool, "Comp")));
            RegisterType(new RealType(TypeIds.Currency, CreateSystemScopeName(pool, "Currency")));
        }

        private void RegisterNativeIntTypes(StringPool pool, NativeIntSize intSize) {

            RegisterType(new TypeAlias(TypeIds.FixedInt, TypeIds.IntegerType, CreateSystemScopeName(pool, "FixedInt")));
            RegisterType(new TypeAlias(TypeIds.FixedUInt, TypeIds.CardinalType, CreateSystemScopeName(pool, "FixedInt")));


            if (intSize == NativeIntSize.Windows64bit) {
                RegisterType(new TypeAlias(TypeIds.NativeInt, TypeIds.Int64Type, CreateSystemScopeName(pool, "NativeInt")));
                RegisterType(new TypeAlias(TypeIds.NativeUInt, TypeIds.Uint64Type, CreateSystemScopeName(pool, "NativeUInt")));
                RegisterType(new TypeAlias(TypeIds.LongInt, TypeIds.IntegerType, CreateSystemScopeName(pool, "LongInt")));
                RegisterType(new TypeAlias(TypeIds.LongWord, TypeIds.CardinalType, CreateSystemScopeName(pool, "LongWord")));
            }
            else if (intSize == NativeIntSize.All64bit) {
                RegisterType(new TypeAlias(TypeIds.NativeInt, TypeIds.Int64Type, CreateSystemScopeName(pool, "NativeInt")));
                RegisterType(new TypeAlias(TypeIds.NativeUInt, TypeIds.Uint64Type, CreateSystemScopeName(pool, "NativeUInt")));
                RegisterType(new TypeAlias(TypeIds.LongInt, TypeIds.Int64Type, CreateSystemScopeName(pool, "LongInt")));
                RegisterType(new TypeAlias(TypeIds.LongWord, TypeIds.Uint64Type, CreateSystemScopeName(pool, "LongWord")));
            }
            else {
                RegisterType(new TypeAlias(TypeIds.NativeInt, TypeIds.IntegerType, CreateSystemScopeName(pool, "NativeInt")));
                RegisterType(new TypeAlias(TypeIds.NativeUInt, TypeIds.CardinalType, CreateSystemScopeName(pool, "NativeUInt")));
                RegisterType(new TypeAlias(TypeIds.LongInt, TypeIds.IntegerType, CreateSystemScopeName(pool, "LongInt")));
                RegisterType(new TypeAlias(TypeIds.LongWord, TypeIds.CardinalType, CreateSystemScopeName(pool, "LongWord")));
            }
        }

        private void RegisterStringTypes(StringPool pool) {
            RegisterType(new AnsiCharType(TypeIds.AnsiCharType, CreateSystemScopeName(pool, "AnsiChar")));
            RegisterType(new WideCharType(TypeIds.WideCharType, CreateSystemScopeName(pool, "WideChar")));

            RegisterType(new AnsiStringType(TypeIds.AnsiStringType, CreateSystemScopeName(pool, "AnsiString")));
            RegisterType(new AnsiStringType(TypeIds.RawByteString, CreateSystemScopeName(pool, "RawByteString")));
            RegisterType(new ShortStringType(TypeIds.ShortStringType, CreateSystemScopeName(pool, "ShortString")));
            RegisterType(new UnicodeStringType(TypeIds.UnicodeStringType, CreateSystemScopeName(pool, "UnicodeString")));
            RegisterType(new WideStringType(TypeIds.WideStringType, CreateSystemScopeName(pool, "WideString")));
        }

        private void RegisterBoolTypes(StringPool pool) {
            RegisterType(new BooleanType(TypeIds.BooleanType, 1, CreateSystemScopeName(pool, "Boolean")));
            RegisterType(new BooleanType(TypeIds.ByteBoolType, 8, CreateSystemScopeName(pool, "ByteBool")));
            RegisterType(new BooleanType(TypeIds.WordBoolType, 16, CreateSystemScopeName(pool, "WordBool")));
            RegisterType(new BooleanType(TypeIds.LongBoolType, 32, CreateSystemScopeName(pool, "LongBool")));
        }

        private void RegisterIntTypes(StringPool pool) {
            RegisterType(new IntegralType(TypeIds.ByteType, false, 8, CreateSystemScopeName(pool, "Byte")));
            RegisterType(new IntegralType(TypeIds.ShortInt, true, 8, CreateSystemScopeName(pool, "ShortInt")));
            RegisterType(new IntegralType(TypeIds.WordType, false, 16, CreateSystemScopeName(pool, "Word")));
            RegisterType(new IntegralType(TypeIds.SmallInt, true, 16, CreateSystemScopeName(pool, "SmallInt")));
            RegisterType(new IntegralType(TypeIds.CardinalType, false, 32, CreateSystemScopeName(pool, "Cardinal")));
            RegisterType(new IntegralType(TypeIds.IntegerType, true, 32, CreateSystemScopeName(pool, "Integer")));
            RegisterType(new Integral64BitType(TypeIds.Uint64Type, false, CreateSystemScopeName(pool, "UInt64")));
            RegisterType(new Integral64BitType(TypeIds.Int64Type, true, CreateSystemScopeName(pool, "Int64")));
        }

        /// <summary>
        ///     gets an registered operator
        /// </summary>
        /// <param name="operatorKind">operator kind</param>
        /// <returns></returns>
        public IOperator GetOperator(int operatorKind) {
            operators.TryGetValue(operatorKind, out var result);
            return result;
        }

        /// <summary>
        ///     get a type definition or the error fallback
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeDefinition GetTypeByIdOrUndefinedType(int typeId) {
            if (!types.TryGetValue(typeId, out var result))
                result = types[TypeIds.ErrorType];

            return result;
        }

        /// <summary>
        ///     generate a new user type ids
        /// </summary>
        /// <returns></returns>
        public int RequireUserTypeId() {
            lock (idLock)
                return userTypeIds++;
        }
    }
}
