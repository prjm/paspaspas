using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     basic type definition
    /// </summary>
    public abstract class TypeBase : ITypeDefinition {

        private readonly int typeId;

        /// <summary>
        ///     create a new type definiton
        /// </summary>
        /// <param name="withId">type id</param>
        public TypeBase(int withId)
            => typeId = withId;

        /// <summary>
        ///     get the type id
        /// </summary>
        public int TypeId => typeId;

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
