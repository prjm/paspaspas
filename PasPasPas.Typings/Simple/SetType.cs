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
    ///     set type declaration
    /// </summary>
    public class SetType : TypeBase {
        private readonly int baseTypeId;

        /// <summary>
        ///     define a new set type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type</param>
        /// <param name="withName"></param>
        public SetType(int withId, int baseType, ScopedName withName = null) : base(withId, withName)
            => baseTypeId = baseType;

        /// <summary>
        ///     set type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.SetType;
    }
}
