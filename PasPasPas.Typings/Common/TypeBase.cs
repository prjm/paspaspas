using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     basic type definition
    /// </summary>
    public class TypeBase : ITypeDefinition {

        private readonly int typeId;
        private readonly ScopedName typeName;
        private IDictionary<int, IOperation>
            operations = new Dictionary<int, IOperation>();

        /// <summary>
        ///     create a new type definiton
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withName">optional type name</param>
        public TypeBase(int withId, ScopedName withName = null) {
            typeId = withId;
            typeName = withName;
        }

        /// <summary>
        ///     get the type id
        /// </summary>
        public int TypeId => typeId;

        /// <summary>
        ///     type name (can be empty)
        /// </summary>
        public ScopedName TypeName
            => typeName;

        /// <summary>
        ///     gets a registered operation
        /// </summary>
        /// <param name="operationKind"></param>
        /// <returns></returns>
        public IOperation GetOperation(int operationKind) {
            if (!operations.TryGetValue(operationKind, out var result))
                return null;
            return result;
        }

        /// <summary>
        ///     register a operation
        /// </summary>
        /// <param name="operation">operation to register</param>
        protected void RegisterOperation(IOperation operation)
            => operations.Add(operation.Kind, operation);
    }
}
