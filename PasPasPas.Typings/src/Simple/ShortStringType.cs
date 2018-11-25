using PasPasPas.Globals.Runtime;

namespace PasPasPas.Typings.Simple {
    /// <summary>
    ///     string type definition
    /// </summary>
    public class ShortStringType : StringTypeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        public ShortStringType(int withId, ITypeReference size) : base(withId)
            => Size = size;

        /// <summary>
        ///     unicode string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ShortStringType;

        public ITypeReference Size { get; }

        public override string ToString()
            => $"ShortString[{Size}]";

    }


}
