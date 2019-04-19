using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     file types
    /// </summary>
    public class FileType : StructuredTypeBase, IFileType {

        /// <summary>
        ///     create a new file type
        /// </summary>
        /// <param name="newTypeId"></param>
        /// <param name="baseTypeId"></param>
        public FileType(int newTypeId, int baseTypeId) : base(newTypeId) {
            BaseTypeId = baseTypeId;
        }

        /// <summary>
        ///     get
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.FileType;

        /// <summary>
        ///     get byte size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

        /// <summary>
        ///     base type id
        /// </summary>
        public int BaseTypeId { get; }
    }
}
