using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Typings.Common {


    /// <summary>
    ///     common type registry
    /// </summary>
    public class RegisteredTypes : IEnvironmentItem {

        private readonly IDictionary<int, ITypeDefinition> types
            = new Dictionary<int, ITypeDefinition>();

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

        private void RegisterType(ITypeDefinition type) {
            types.Add(type.TypeId, type);
            var name = type.TypeName;
            if (name != null)
                typesByName.Add(name, type);
        }

        /// <summary>
        ///     create a new type registry
        /// </summary>
        public RegisteredTypes(StringPool pool)
            => RegisterCommonTypes(pool);

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
            RegisterType(new IntegralType(TypeIds.Uint64Type, CreateSystemScopeName(pool, "UInt64")));
            RegisterType(new BooleanType(TypeIds.BooleanType, CreateSystemScopeName(pool, "Boolean")));
        }

        /// <summary>
        ///     get a type definition or the error fallback
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeDefinition GetTypeOrUndef(int typeId) {
            if (!types.TryGetValue(typeId, out var result))
                result = types[TypeIds.ErrorType];

            return result;
        }

    }
}
