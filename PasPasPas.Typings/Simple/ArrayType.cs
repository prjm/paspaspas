using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayType : StructuredTypeBase {

        /// <summary>
        ///     create a new array type
        /// </summary>
        /// <param name="withId"></param>
        public ArrayType(int withId) : base(withId) {
        }

        /// <summary>
        ///     arary index types
        /// </summary>
        public IList<ITypeDefinition> IndexTypes { get; }
            = new List<ITypeDefinition>();

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ArrayType;

        /// <summary>
        ///     base type id
        /// </summary>
        public ITypeDefinition BaseType { get; set; }

    }
}
