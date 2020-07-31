using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     ANSI char type (1 byte)
    /// </summary>
    internal class AnsiCharType : CompilerDefinedType, ICharType {

        /// <summary>
        ///     create a new char type
        /// </summary>
        /// <param name="definingUnit">defining unit</param>
        internal AnsiCharType(IUnitType definingUnit) : base(definingUnit) { }

        /// <summary>
        ///     highest element: <c>0xff</c>
        /// </summary>
        public IValue HighestElement
            => TypeRegistry.Runtime.Chars.ToAnsiCharValue(this, 0xff);

        /// <summary>
        ///     lowest element: <c>0</c>
        /// </summary>
        public IValue LowestElement
            => TypeRegistry.Runtime.Chars.ToAnsiCharValue(this, 0);

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
        ///     base type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Char;

        /// <summary>
        ///     char type kind
        /// </summary>
        public CharTypeKind Kind
            => CharTypeKind.AnsiChar;


        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFromType(ITypeDefinition otherType) {

            if (otherType.BaseType == BaseType.Subrange && otherType is ISubrangeType subrange) {
                var subrangeBase = subrange.SubrangeOfType;
                return subrangeBase.BaseType == BaseType.Char;
            }

            return base.CanBeAssignedFromType(otherType);
        }

        /// <summary>
        ///     long type name
        /// </summary>
        public override string Name
            => KnownNames.AnsiChar;

        /// <summary>
        ///     short type name
        /// </summary>
        public override string MangledName
            => KnownNames.C;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => other is ICharType c && c.Kind == CharTypeKind.AnsiChar;

        /// <summary>
        ///     hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => 0;

    }
}
