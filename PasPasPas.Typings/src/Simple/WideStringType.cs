#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     wide string type definition
    /// </summary>
    public class WideStringType : StringTypeBase {

        /// <summary>
        ///     create a new wide string type
        /// </summary>
        /// <param name="definingType">type id</param>
        public WideStringType(IUnitType definingType) : base(definingType) {
        }

        /// <summary>
        ///     string type kind
        /// </summary>
        public override StringTypeKind Kind
            => StringTypeKind.WideStringType;

        /// <summary>
        ///     string type
        /// </summary>
        public override BaseType BaseType
            => BaseType.String;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.SystemUnit.NativeIntType.TypeSizeInBytes;

        /// <summary>
        ///     long string name
        /// </summary>
        public override string Name
            => KnownNames.WideString;

        /// <summary>
        ///     short string name
        /// </summary>
        public override string MangledName
            => KnownNames.SystemAtWideString;
    }




}
