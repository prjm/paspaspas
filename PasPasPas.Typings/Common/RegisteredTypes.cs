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

        private readonly IDictionary<ScopedName, ITypeDefinition> typesByName
            = new Dictionary<ScopedName, ITypeDefinition>();

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

            var name = type.TypeName;
            if (name != null)
                typesByName.Add(name, type);
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

            RegisterType(new RealType(TypeIds.Extended, CreateSystemScopeName(pool, "Extended")));

            RegisterType(new TypeAlias(TypeIds.CharType, TypeIds.WideCharType, CreateSystemScopeName(pool, "Char")));
            RegisterType(new TypeAlias(TypeIds.Ucs2CharType, TypeIds.WideCharType, CreateSystemScopeName(pool, "UCS2Char")));
            RegisterType(new TypeAlias(TypeIds.Ucs4CharType, TypeIds.CardinalType, CreateSystemScopeName(pool, "UCS4Char")));
            RegisterType(new TypeAlias(TypeIds.StringType, TypeIds.UnicodeStringType, CreateSystemScopeName(pool, "String")));

            RegisterNativeIntTypes(pool, intSize);
        }

        private void RegisterNativeIntTypes(StringPool pool, NativeIntSize intSize) {
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
        ///     get a type by name
        /// </summary>
        /// <param name="typeName">type name</param>
        /// <returns></returns>
        public ITypeDefinition GetTypeByNameOrUndefinedType(ScopedName typeName) {
            if (!typesByName.TryGetValue(typeName, out var result))
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
