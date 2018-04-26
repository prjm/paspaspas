using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     errornous type
    /// </summary>
    public class ErrorType : TypeBase {

        /// <summary>
        ///     creates a new errornous type
        /// </summary>
        /// <param name="withId"></param>
        public ErrorType(int withId) : base(withId) { }

        /// <summary>
        ///     unknown type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.UnknownType;

    }
}
