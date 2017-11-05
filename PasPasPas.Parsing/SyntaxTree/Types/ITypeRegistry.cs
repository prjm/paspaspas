using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     type registry
    /// </summary>
    public interface ITypeRegistry {

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

    }
}
