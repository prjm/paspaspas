using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     subrange type
    /// </summary>
    public class SubrangeType : TypeBase {

        private readonly int baseTypeId;

        /// <summary>
        ///     create a new subrange type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type</param>
        public SubrangeType(int withId, int baseType) : base(withId)
            => baseTypeId = baseType;

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeId).TypeKind;

    }
}
