using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     meta class type
    /// </summary>
    internal class MetaClassType : TypeDefinitionBase, IMetaType {

        /// <summary>
        ///     create a new meta class type
        /// </summary>
        public MetaClassType(IUnitType definingUnit, string name, ITypeDefinition baseTypeDefinition) : base(definingUnit) {
            Name = name;
            BaseTypeDefinition = baseTypeDefinition;
        }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     base type definition
        /// </summary>
        public ITypeDefinition BaseTypeDefinition { get; }

        /// <summary>
        ///     base type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.MetaClass;

        /// <summary>
        ///     mangled type name
        /// </summary>
        public override string MangledName
            => string.Concat(DefiningUnit.Name, KnownNames.AtSymbol, Name);

        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
               other is IMetaType m && m.BaseTypeDefinition.Equals(BaseTypeDefinition);
    }
}
