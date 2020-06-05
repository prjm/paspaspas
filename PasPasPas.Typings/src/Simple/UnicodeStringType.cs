#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     string type definition
    /// </summary>
    public class UnicodeStringType : StringTypeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="definingUnit">type id</param>
        public UnicodeStringType(IUnitType definingUnit) : base(definingUnit) {
        }

        /// <summary>
        ///     Unicode string type
        /// </summary>
        public override BaseType BaseType
            => BaseType.String;

        /// <summary>
        ///     string type kind
        /// </summary>
        public override StringTypeKind Kind
            => StringTypeKind.UnicodeString;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.SystemUnit.NativeIntType.TypeSizeInBytes;

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name
            => KnownNames.UnicodeString;

        /// <summary>
        ///      short string
        /// </summary>
        public override string MangledName
            => KnownNames.SystemAtUnicodeString;
    }

}
