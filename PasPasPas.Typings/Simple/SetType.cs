using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     set type declaration
    /// </summary>
    public class SetType : StructuredTypeBase {

        private readonly int baseTypeId;

        /// <summary>
        ///     define a new set type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type</param>
        public SetType(int withId, int baseType) : base(withId)
            => baseTypeId = baseType;

        /// <summary>
        ///     set type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.SetType;

    }
}
