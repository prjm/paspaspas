using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        ///     create a new type alias
        /// </summary>
        /// <param name="withId">own type id</param>
        /// <param name="withBaseId">base id</param>
        /// <param name="withName">type name</param>
        public TypeAlias(int withId, int withBaseId, ScopedName withName = null) : base(withId, withName)
            => baseId = withBaseId;

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
    }
}
