using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     character based type
    /// </summary>
    public class AnsiCharType : TypeBase {

        /// <summary>cccy
        ///     create a new char type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withName">type name</param>
        public AnsiCharType(int withId, ScopedName withName = null) : base(withId, withName) {
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.AnsiCharType;
    }
}
