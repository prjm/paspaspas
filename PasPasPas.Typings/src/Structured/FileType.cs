using System;
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
        public FileType(IUnitType definingUnit, ITypeDefinition baseTypeDef) : base(definingUnit) {
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
        ///     mangled name
        /// </summary>
        /// <param name="definingUnit">defining unit</param>
        /// <param name="name">file type name</param>
        public static string GetMangledName(IUnitType definingUnit, string name)
            => string.Concat(definingUnit.Name, KnownNames.AtSymbol, name);

        /// <summary>
        ///     base type id
        /// </summary>
        public ITypeDefinition BaseTypeDefinition { get; }

        public override bool Equals(ITypeDefinition? other)
               => other is IFileType f && f.BaseTypeDefinition.Equals(BaseTypeDefinition);

        public override int GetHashCode()
            => HashCode.Combine(BaseTypeDefinition);
    }
}
