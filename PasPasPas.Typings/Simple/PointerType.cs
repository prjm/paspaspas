using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     pointer type definition
    /// </summary>
    public class PointerType : TypeBase {

        private readonly int baseTypeId;

        /// <summary>
        ///     create a new pointer type definition
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="baseType">base type id</param>
        /// <param name="withName">type name</param>
        public PointerType(int withId, int baseType, ScopedName withName = null) : base(withId, withName)
            => baseTypeId = baseType;

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.PointerType;

        /// <summary>
        ///     provides scope information
        /// </summary>
        /// <param name="completeName"></param>
        /// <param name="scope"></param>
        public override void ProvideScope(string completeName, IScope scope) {

        }

    }
}
