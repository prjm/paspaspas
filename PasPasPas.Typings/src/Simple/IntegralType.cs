using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     create a new integral type
    /// </summary>
    public class IntegralType : TypeDefinitionBase, IIntegralType {

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

                switch (Kind) {
                    case IntegralTypeKind.Byte:
                        return f.ToIntegerValue(byte.MinValue);

                    case IntegralTypeKind.ShortInt:
                        return f.ToIntegerValue(sbyte.MinValue);

                    case IntegralTypeKind.Word:
                        return f.ToIntegerValue(ushort.MinValue);

                    case IntegralTypeKind.SmallInt:
                        return f.ToIntegerValue(short.MinValue);

                    case IntegralTypeKind.Cardinal:
                        return f.ToIntegerValue(uint.MinValue);

                    case IntegralTypeKind.Integer:
                        return f.ToIntegerValue(int.MinValue);

                    case IntegralTypeKind.UInt64:
                        return f.ToIntegerValue(ulong.MinValue);

                    case IntegralTypeKind.Int64:
                        return f.ToIntegerValue(long.MinValue);

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        ///     highest element
        /// </summary>
        public IValue HighestElement {
            get {
                var f = TypeRegistry.Runtime.Integers;

                switch (Kind) {
                    case IntegralTypeKind.Byte:
                        return f.ToIntegerValue(byte.MaxValue);

                    case IntegralTypeKind.ShortInt:
                        return f.ToIntegerValue(sbyte.MaxValue);

                    case IntegralTypeKind.Word:
                        return f.ToIntegerValue(ushort.MaxValue);

                    case IntegralTypeKind.SmallInt:
                        return f.ToIntegerValue(short.MaxValue);

                    case IntegralTypeKind.Cardinal:
                        return f.ToIntegerValue(uint.MaxValue);

                    case IntegralTypeKind.Integer:
                        return f.ToIntegerValue(int.MaxValue);

                    case IntegralTypeKind.UInt64:
                        return f.ToIntegerValue(ulong.MaxValue);

                    case IntegralTypeKind.Int64:
                        return f.ToIntegerValue(long.MaxValue);

                    default:
                        throw new InvalidOperationException();
                }
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
        public override string MangledName {
            get {

                switch (Kind) {
                    case IntegralTypeKind.SmallInt:
                        return KnownNames.ZC;

                    case IntegralTypeKind.Byte:
                        return KnownNames.UC;

                    case IntegralTypeKind.ShortInt:
                        return KnownNames.S;

                    case IntegralTypeKind.Word:
                        return KnownNames.US;

                    case IntegralTypeKind.Cardinal:
                        return KnownNames.UI;

                    case IntegralTypeKind.Integer:
                        return KnownNames.I;

                    case IntegralTypeKind.UInt64:
                        return KnownNames.UJ;

                    case IntegralTypeKind.Int64:
                        return KnownNames.J;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
