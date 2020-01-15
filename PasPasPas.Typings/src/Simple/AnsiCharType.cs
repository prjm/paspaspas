using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     ANSI char type (1 byte)
    /// </summary>
    public class AnsiCharType : OrdinalTypeBase, ICharType {

        private readonly object lockObject = new object();
        private IOldTypeReference highestElement;
        private IOldTypeReference lowestElement;

        /// <summary>
        ///     create a new char type
        /// </summary>
        /// <param name="withId">type id</param>
        public AnsiCharType(int withId) : base(withId) { }

        /// <summary>
        ///     type kind: ANSI char type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.AnsiCharType;

        /// <summary>
        ///     highest element: <c>0xff</c>
        /// </summary>
        public IOldTypeReference HighestElement {
            get {
                lock (lockObject) {
                    if (highestElement == default)
                        highestElement = TypeRegistry.Runtime.Chars.ToAnsiCharValue(TypeId, 0xff);
                    return highestElement;
                }
            }
        }

        /// <summary>
        ///     lowest element: <c>0</c>
        /// </summary>
        public IOldTypeReference LowestElement {
            get {
                lock (lockObject) {
                    if (lowestElement == default)
                        lowestElement = TypeRegistry.Runtime.Chars.ToAnsiCharValue(TypeId, 0x00);
                    return lowestElement;
                }
            }
        }

        /// <summary>
        ///     bit size
        /// </summary>
        public uint BitSize
            => 8;

        /// <summary>
        ///     unsigned data type
        /// </summary>
        public bool IsSigned
            => false;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => 1;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeKind == CommonTypeKind.AnsiCharType;
            }

            return base.CanBeAssignedFrom(otherType);
        }

        /// <summary>
        ///     long type name
        /// </summary>
        public override string LongName
            => KnownNames.AnsiChar;

        /// <summary>
        ///     short type name
        /// </summary>
        public override string ShortName
            => KnownNames.C;

    }
}
