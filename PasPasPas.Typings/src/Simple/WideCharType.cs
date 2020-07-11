using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     wide char type
    /// </summary>
    internal class WideCharType : CompilerDefinedType, ICharType {

        /// <summary>
        ///     wide char type
        /// </summary>
        /// <param name="definingUnit"></param>
        public WideCharType(IUnitType definingUnit) : base(definingUnit) { }

        /// <summary>
        ///     wide char type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Char;

        /// <summary>
        ///     highest element: <c>0xff</c>
        /// </summary>
        public IValue HighestElement
            => TypeRegistry.Runtime.Chars.ToWideCharValue(this, '\uffff');

        /// <summary>
        ///     lowest element: <c>0</c>
        /// </summary>
        public IValue LowestElement
            => TypeRegistry.Runtime.Chars.ToWideCharValue(this, '\u0000');

        /// <summary>
        ///     unsigned data type
        /// </summary>
        public bool IsSigned
            => false;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => 2;

        /// <summary>
        ///     char kind
        /// </summary>
        public CharTypeKind Kind
            => CharTypeKind.WideChar;

        /*
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
        */

        /// <summary>
        ///     long type name
        /// </summary>
        public override string Name
            => KnownNames.WideChar;

        /// <summary>
        ///     short type name
        /// </summary>
        public override string MangledName
            => KnownNames.B;

        public override bool Equals(ITypeDefinition? other)
            => other is ICharType c && c.Kind == Kind &&
                KnownNames.SameIdentifier(Name, c.Name);
    }
}
