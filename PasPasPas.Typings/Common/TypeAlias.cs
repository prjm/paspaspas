using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     type alias
    /// </summary>
    public class TypeAlias : TypeBase {

        /// <summary>
        ///     basic type id
        /// </summary>
        private readonly int baseId;

        /// <summary>
        ///     if <c>true</c> this is treated as a new type
        /// </summary>
        private readonly bool isNewType;

        /// <summary>
        ///     create a new type alias
        /// </summary>
        /// <param name="withId">own type id</param>
        /// <param name="withBaseId">base id</param>
        /// <param name="newType">if <c>true</c>, this alias is treated as new, distinct type</param>
        public TypeAlias(int withId, int withBaseId, bool newType = false) : base(withId) {
            baseId = withBaseId;
            isNewType = newType;
        }

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => TypeRegistry.GetTypeByIdOrUndefinedType(baseId).TypeKind;

        /// <summary>
        ///     base type / alias type
        /// </summary>
        public ITypeDefinition BaseType
            => TypeRegistry.GetTypeByIdOrUndefinedType(baseId);

        /// <summary>
        ///     <c>true</c> if this is new, separate type
        /// </summary>
        public bool IsNewType
            => isNewType;
    }
}
