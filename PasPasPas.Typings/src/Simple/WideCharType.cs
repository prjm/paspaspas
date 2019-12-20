using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     wide char type
    /// </summary>
    public class WideCharType : OrdinalTypeBase, ICharType {

        private readonly object lockObject = new object();
        private ITypeReference highestElement;
        private ITypeReference lowestElement;

        /// <summary>
        ///     wide char type
        /// </summary>
        /// <param name="withId"></param>
        public WideCharType(int withId) : base(withId) {
        }

        /// <summary>
        ///     wide char type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.WideCharType;

        /// <summary>
        ///     highest element: <c>0xffff</c>
        /// </summary>
        public ITypeReference HighestElement {
            get {
                lock (lockObject) {
                    if (highestElement == default)
                        highestElement = TypeRegistry.Runtime.Chars.ToWideCharValue(TypeId, char.MaxValue);
                    return highestElement;
                }
            }
        }

        /// <summary>
        ///     lowest element: <c>0</c>
        /// </summary>
        public ITypeReference LowestElement {
            get {
                lock (lockObject) {
                    if (lowestElement == default)
                        lowestElement = TypeRegistry.Runtime.Chars.ToWideCharValue(TypeId, char.MinValue);
                    return lowestElement;
                }
            }
        }

        /// <summary>
        ///     bit size
        /// </summary>
        public uint BitSize
            => 16;

        /// <summary>
        ///     unsigned data types
        /// </summary>
        public bool IsSigned
            => false;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => 2;

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeKind == CommonTypeKind.WideCharType;
            }

            return base.CanBeAssignedFrom(otherType);
        }

        /// <summary>
        ///     long type name
        /// </summary>
        public override string LongName
            => KnownNames.WideChar;

        /// <summary>
        ///     short type name
        /// </summary>
        public override string ShortName
            => KnownNames.B;

    }
}
