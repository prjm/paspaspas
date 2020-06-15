using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     create a new integral type
    /// </summary>
    internal class IntegralType : TypeDefinitionBase, IIntegralType {

        /// <summary>
        ///     create a new integral type
        /// </summary>
        /// <param name="definingUnit">defining unit</param>
        /// <param name="kind">type kind</param>
        public IntegralType(IUnitType definingUnit, IntegralTypeKind kind) : base(definingUnit)
            => Kind = kind;

        /// <summary>
        ///     type kind
        /// </summary>
        public IntegralTypeKind Kind { get; }

        /// <summary>
        ///     integer type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Integer;

        /// <summary>
        ///     check if this type is signed
        /// </summary>
        public bool IsSigned {
            get {
                switch (Kind) {

                    case IntegralTypeKind.Byte:
                    case IntegralTypeKind.Word:
                    case IntegralTypeKind.Cardinal:
                    case IntegralTypeKind.UInt64:
                        return false;

                    case IntegralTypeKind.ShortInt:
                    case IntegralTypeKind.SmallInt:
                    case IntegralTypeKind.Integer:
                    case IntegralTypeKind.Int64:
                        return true;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        ///     lowest element
        /// </summary>
        public IValue LowestElement {
            get {
                var f = TypeRegistry.Runtime.Integers;

                return Kind switch
                {
                    IntegralTypeKind.Byte => f.ToIntegerValue(byte.MinValue),
                    IntegralTypeKind.ShortInt => f.ToIntegerValue(sbyte.MinValue),
                    IntegralTypeKind.Word => f.ToIntegerValue(ushort.MinValue),
                    IntegralTypeKind.SmallInt => f.ToIntegerValue(short.MinValue),
                    IntegralTypeKind.Cardinal => f.ToIntegerValue(uint.MinValue),
                    IntegralTypeKind.Integer => f.ToIntegerValue(int.MinValue),
                    IntegralTypeKind.UInt64 => f.ToIntegerValue(ulong.MinValue),
                    IntegralTypeKind.Int64 => f.ToIntegerValue(long.MinValue),
                    _ => throw new InvalidOperationException(),
                };
            }
        }

        /// <summary>
        ///     highest element
        /// </summary>
        public IValue HighestElement {
            get {
                var f = TypeRegistry.Runtime.Integers;

                return Kind switch
                {
                    IntegralTypeKind.Byte => f.ToIntegerValue(byte.MaxValue),
                    IntegralTypeKind.ShortInt => f.ToIntegerValue(sbyte.MaxValue),
                    IntegralTypeKind.Word => f.ToIntegerValue(ushort.MaxValue),
                    IntegralTypeKind.SmallInt => f.ToIntegerValue(short.MaxValue),
                    IntegralTypeKind.Cardinal => f.ToIntegerValue(uint.MaxValue),
                    IntegralTypeKind.Integer => f.ToIntegerValue(int.MaxValue),
                    IntegralTypeKind.UInt64 => f.ToIntegerValue(ulong.MaxValue),
                    IntegralTypeKind.Int64 => f.ToIntegerValue(long.MaxValue),
                    _ => throw new InvalidOperationException(),
                };
            }
        }

        /// <summary>
        ///     size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                switch (Kind) {
                    case IntegralTypeKind.Byte:
                    case IntegralTypeKind.ShortInt:
                        return 1;

                    case IntegralTypeKind.Word:
                    case IntegralTypeKind.SmallInt:
                        return 2;

                    case IntegralTypeKind.Cardinal:
                    case IntegralTypeKind.Integer:
                        return 4;

                    case IntegralTypeKind.UInt64:
                    case IntegralTypeKind.Int64:
                        return 8;

                    default:
                        throw new InvalidOperationException(); ;
                }
            }
        }

        /*
        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.IntegerType)
                return true;

            if (otherType.TypeKind == CommonTypeKind.Int64Type)
                return true;

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeKind == CommonTypeKind.IntegerType || subrangeBase.TypeKind == CommonTypeKind.Int64Type;
            }

            return base.CanBeAssignedFrom(otherType);
        }
        */

        /// <summary>
        ///     get the long type name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case IntegralTypeKind.Byte:
                        return KnownNames.Byte;

                    case IntegralTypeKind.ShortInt:
                        return KnownNames.ShortInt;

                    case IntegralTypeKind.Word:
                        return KnownNames.Word;

                    case IntegralTypeKind.SmallInt:
                        return KnownNames.SmallInt;

                    case IntegralTypeKind.Cardinal:
                        return KnownNames.Cardinal;

                    case IntegralTypeKind.Integer:
                        return KnownNames.Integer;

                    case IntegralTypeKind.UInt64:
                        return KnownNames.UInt64;

                    case IntegralTypeKind.Int64:
                        return KnownNames.Int64;

                    default:
                        throw new InvalidOperationException(); ;
                }
            }
        }

        /// <summary>
        ///     get the short type name
        /// </summary>
        public override string MangledName
            => Kind switch
            {
                IntegralTypeKind.SmallInt => KnownNames.ZC,
                IntegralTypeKind.Byte => KnownNames.UC,
                IntegralTypeKind.ShortInt => KnownNames.S,
                IntegralTypeKind.Word => KnownNames.US,
                IntegralTypeKind.Cardinal => KnownNames.UI,
                IntegralTypeKind.Integer => KnownNames.I,
                IntegralTypeKind.UInt64 => KnownNames.UJ,
                IntegralTypeKind.Int64 => KnownNames.J,
                _ => throw new InvalidOperationException(),
            };

        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
               other is IIntegralType i && i.Kind == Kind;
    }
}
