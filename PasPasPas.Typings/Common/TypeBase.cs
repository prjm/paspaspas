using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     basic type definition
    /// </summary>
    public abstract class TypeBase : ITypeDefinition {

        private readonly int typeId;
        private readonly ScopedName typeName;

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
        ///     get the type kind
        /// </summary>
        public abstract CommonTypeKind TypeKind { get; }

        /// <summary>
        ///     type registryB
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }
    }
}
