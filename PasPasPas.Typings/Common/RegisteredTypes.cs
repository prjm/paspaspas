using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Typings.Common {


    /// <summary>
    ///     common type registry
    /// </summary>
    public class RegisteredTypes {

        private readonly IDictionary<int, ITypeDefinition> types
            = new Dictionary<int, ITypeDefinition>();

        private readonly IDictionary<ScopedName, ITypeDefinition> typesByName
            = new Dictionary<ScopedName, ITypeDefinition>();

        private void RegisterType(ITypeDefinition type) {
            types.Add(type.TypeId, type);
            var name = type.TypeName;
            if (name != null)
                typesByName.Add(name, type);
        }

        /// <summary>
        ///     create a new type registry
        /// </summary>
        public RegisteredTypes()
            => RegisterCommonTypes();

        /// <summary>
        ///     register built-in types
        /// </summary>
        private void RegisterCommonTypes() {
            RegisterType(new ErrorType(TypeIds.ErrorType));
            RegisterType(new IntegralType(TypeIds.ByteType));
            RegisterType(new IntegralType(TypeIds.WordType));
            RegisterType(new IntegralType(TypeIds.CardinalType));
            RegisterType(new IntegralType(TypeIds.Uint64Type));
            RegisterType(new BooleanType(TypeIds.BooleanType));
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
