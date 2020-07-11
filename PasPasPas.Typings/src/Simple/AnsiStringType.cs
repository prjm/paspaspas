using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     string type definition
    /// </summary>
    internal class AnsiStringType : StringTypeBase, IAnsiStringType {

        /// <summary>
        ///     default system code page
        /// </summary>
        public const ushort DefaultSystemCodePage = 0;

        /// <summary>
        ///     no code page
        /// </summary>
        public const ushort NoCodePage = 0xffff;

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withCodePage">code page</param>
        /// <param name="definingUnit">defining unit</param>
        public AnsiStringType(IUnitType definingUnit, ushort withCodePage) : base(definingUnit)
            => WithCodePage = withCodePage;

        /// <summary>
        ///     ANSI string
        /// </summary>
        public override StringTypeKind Kind
            => StringTypeKind.AnsiString;

        /// <summary>
        ///      string type
        /// </summary>
        public override BaseType BaseType
            => BaseType.String;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.SystemUnit.NativeIntType.TypeSizeInBytes;

        /// <summary>
        ///     code page
        /// </summary>
        public ushort WithCodePage { get; }

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name
            => WithCodePage == NoCodePage ? KnownNames.RawByteString : KnownNames.AnsiString;

        public override string MangledName
            => GetMangledName(WithCodePage);

        /// <summary>
        ///     short type name
        /// </summary>
        internal static string GetMangledName(ushort codePage)
            => $"%AnsiStringT$us$i{codePage}$%";
    }


}
