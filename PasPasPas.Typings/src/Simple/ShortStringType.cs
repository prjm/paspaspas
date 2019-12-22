using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {
    /// <summary>
    ///     string type definition
    /// </summary>
    public class ShortStringType : StringTypeBase, IShortStringType {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="size">string size</param>
        public ShortStringType(int withId, byte size) : base(withId)
            => Size = size;

        /// <summary>
        ///     UNICODE string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ShortStringType;

        /// <summary>
        ///     string size
        /// </summary>
        public byte Size { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => 1u + Size;

        /// <summary>
        ///     long name
        /// </summary>
        public override string LongName
            => Size == 0xff ? KnownNames.ShortString : $"string[{Size}]";

        /// <summary>
        ///     short type name
        /// </summary>
        public override string ShortName
            => $"System@%SmallString$uc$i{Size}$%";
    }


}
