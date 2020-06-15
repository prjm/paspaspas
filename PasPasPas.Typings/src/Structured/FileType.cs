using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     file types
    /// </summary>
    internal class FileType : StructuredTypeBase, IFileType {

        /// <summary>
        ///     create a new file type
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="name"></param>
        /// <param name="baseTypeDef"></param>
        public FileType(IUnitType definingUnit, string name, ITypeDefinition baseTypeDef) : base(definingUnit) {
            Name = name;
            BaseTypeDefinition = baseTypeDef;
        }

        /// <summary>
        ///     get
        /// </summary>
        public override BaseType BaseType
            => BaseType.File;

        /// <summary>
        ///     get byte size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     mangled name
        /// </summary>
        public override string MangledName
            => string.Concat(DefiningUnit.Name, KnownNames.AtSymbol, Name);

        /// <summary>
        ///     base type id
        /// </summary>
        public ITypeDefinition BaseTypeDefinition { get; }

        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
               other is IFileType f && f.BaseTypeDefinition.Equals(BaseTypeDefinition);
    }
}
