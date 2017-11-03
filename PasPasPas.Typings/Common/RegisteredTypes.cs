using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
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

        private void RegisterType(TypeBase type) {
            types.Add(type.TypeId, type);
            type.TypeRegistry = this;
            var name = type.TypeName;
            if (name != null)
                typesByName.Add(name, type);
        }

        /// <summary>
        ///     create a new type registry
        /// </summary>
        public RegisteredTypes(StringPool pool) {
            RegisterCommonTypes(pool);
            RegisterCommonOperators();
        }

        private void RegisterCommonOperators() {
            RegisterOperator(new LogicalOperators(DefinedOperators.NotOperation));
            RegisterOperator(new LogicalOperators(DefinedOperators.AndOperation));
            RegisterOperator(new LogicalOperators(DefinedOperators.XorOperation));
            RegisterOperator(new LogicalOperators(DefinedOperators.OrOperation));
        }

        private void RegisterOperator(IOperator newOperator) {
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
        private void RegisterCommonTypes(StringPool pool) {
            RegisterType(new ErrorType(TypeIds.ErrorType));
            RegisterType(new IntegralType(TypeIds.ByteType, CreateSystemScopeName(pool, "Byte")));
            RegisterType(new IntegralType(TypeIds.WordType, CreateSystemScopeName(pool, "Word")));
            RegisterType(new IntegralType(TypeIds.CardinalType, CreateSystemScopeName(pool, "Cardinal")));
            RegisterType(new Integral64BitType(TypeIds.Uint64Type, CreateSystemScopeName(pool, "UInt64")));
            RegisterType(new IntegralType(TypeIds.IntegerType, CreateSystemScopeName(pool, "Integer")));
            RegisterType(new BooleanType(TypeIds.BooleanType, CreateSystemScopeName(pool, "Boolean")));
            RegisterType(new AnsiCharType(TypeIds.AnsiCharType, CreateSystemScopeName(pool, "AnsiChar")));
            RegisterType(new UnicodeStringType(TypeIds.UnicodeStringType, CreateSystemScopeName(pool, "UnicodeString")));
            RegisterType(new UnicodeStringType(TypeIds.Extended, CreateSystemScopeName(pool, "Extended")));

            RegisterType(new TypeAlias(TypeIds.StringType, TypeIds.UnicodeStringType, CreateSystemScopeName(pool, "String")));
            RegisterType(new TypeAlias(TypeIds.CharType, TypeIds.AnsiCharType, CreateSystemScopeName(pool, "Char")));
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

    }
}
