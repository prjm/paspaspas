using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     type alias
    /// </summary>
    public class TypeAlias : TypeBase, IAliasedType {

        /// <summary>
        ///     create a new type alias
        /// </summary>
        /// <param name="withId">own type id</param>
        /// <param name="withBaseId">base id</param>
        /// <param name="aliasName">alias name</param>
        /// <param name="newType">if <c>true</c>, this alias is treated as new, distinct type</param>
        public TypeAlias(int withId, string aliasName, int withBaseId, bool newType = false) : base(withId) {
            BaseTypeId = withBaseId;
            IsNewType = newType;
            AliasName = aliasName;
        }

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => TypeRegistry.GetTypeByIdOrUndefinedType(BaseTypeId).TypeKind;

        /// <summary>
        ///     base type / alias type
        /// </summary>
        public ITypeDefinition BaseType
            => TypeRegistry.GetTypeByIdOrUndefinedType(BaseTypeId);

        /// <summary>
        ///     <c>true</c> if this is new, separate type
        /// </summary>
        public bool IsNewType { get; }

        /// <summary>
        ///     alias name
        /// </summary>
        public string AliasName { get; }

        /// <summary>
        ///     get the base type id
        /// </summary>
        public int BaseTypeId { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => BaseType.TypeSizeInBytes;

        /// <summary>
        ///     alias name
        /// </summary>
        public override string LongName
            => AliasName;

        /// <summary>
        ///     short name
        /// </summary>
        public override string ShortName
            => BaseType.ShortName;

        /// <summary>
        ///     format this type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "$" + BaseType.ToString();

    }
}
