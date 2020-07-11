using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     type alias
    /// </summary>
    internal class TypeAlias : TypeDefinitionBase, IAliasedType {

        /// <summary>
        ///     create a new type alias
        /// </summary>
        /// <param name="baseType">own type id</param>
        /// <param name="aliasName">alias name</param>
        /// <param name="definingUnit">defining unit</param>
        /// <param name="newType">if <c>true</c>, this alias is treated as new, distinct type</param>
        internal TypeAlias(IUnitType definingUnit, ITypeDefinition baseType, string aliasName, bool newType) : base(definingUnit) {
            BaseTypeDefinition = baseType;
            IsNewType = newType;
            AliasName = aliasName;
        }

        /// <summary>
        ///     base type (aliased type)
        /// </summary>
        public ITypeDefinition BaseTypeDefinition { get; }

        /// <summary>
        ///     <c>true</c> if this is new, separate type
        /// </summary>
        public bool IsNewType { get; }

        /// <summary>
        ///     alias name
        /// </summary>
        public string AliasName { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => BaseTypeDefinition.TypeSizeInBytes;

        /// <summary>
        ///     alias name
        /// </summary>
        public string Name
            => AliasName;

        /// <summary>
        ///     base type: <c>type alias</c>
        /// </summary>
        public override BaseType BaseType
            => BaseType.TypeAlias;

        public SymbolTypeKind SymbolKind
            => SymbolTypeKind.TypeDefinition;

        public string MangledName
            => (BaseTypeDefinition as IMangledNameTypeSymbol)?.MangledName ?? string.Concat(DefiningUnit.Name, "@", Name);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => other is IAliasedType a &&
                a.BaseTypeDefinition.Equals(BaseTypeDefinition);
    }
}
