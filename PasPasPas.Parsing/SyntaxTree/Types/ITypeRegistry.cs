using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     type registry
    /// </summary>
    public interface ITypeRegistry {

        /// <summary>
        ///     all registered types
        /// </summary>
        IEnumerable<ITypeDefinition> RegisteredTypes { get; }

        /// <summary>
        ///     system unit
        /// </summary>
        IRefSymbol SystemUnit { get; }

        /// <summary>
        ///     get a type by type id
        /// </summary>
        /// <param name="typeId">given type id</param>
        /// <returns>type definition or the undefined type if the type id is not found</returns>
        ITypeDefinition GetTypeByIdOrUndefinedType(int typeId);

        /// <summary>
        ///     get an operator by operator id
        /// </summary>
        /// <param name="operatorKind">operator id</param>
        /// <returns>operator definition</returns>
        IOperator GetOperator(int operatorKind);

        /// <summary>
        ///     register an operator
        /// </summary>
        /// <param name="newOperator">operator to register</param>
        void RegisterOperator(IOperator newOperator);

        /// <summary>
        ///     register a new type
        /// </summary>
        /// <param name="typeDef"></param>
        ITypeDefinition RegisterType(ITypeDefinition typeDef);

        /// <summary>
        ///     generate a new user type id
        /// </summary>
        /// <returns></returns>
        int RequireUserTypeId();
    }
}
