using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {
    /// <summary>
    ///     string type definition
    /// </summary>
    internal class ShortStringType : StringTypeBase, IShortStringType {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="definingUnit">type id</param>
        /// <param name="size">string size</param>
        public ShortStringType(IUnitType definingUnit, byte size) : base(definingUnit)
            => Size = size;

        /// <summary>
        ///     short string type
        /// </summary>
        public override BaseType BaseType
            => BaseType.String;

        /// <summary>
        ///     string type
        /// </summary>
        public override StringTypeKind Kind
            => StringTypeKind.ShortString;

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
        public override string Name
            => Size == 0xff ? KnownNames.ShortString : $"string[{Size}]";

        /// <summary>
        ///     short type name
        /// </summary>
        public override string MangledName
            => $"System@%SmallString$uc$i{Size}$%";
    }


}
