using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     boolean type
    /// </summary>
    internal class BooleanType : CompilerDefinedType, IBooleanType {

        /// <summary>
        ///     create a new boolean type
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="kind">boolean kind</param>
        public BooleanType(IUnitType definingUnit, BooleanTypeKind kind) : base(definingUnit)
            => Kind = kind;

        /// <summary>
        ///     base type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Boolean;

        /// <summary>
        ///     unsigned data type
        /// </summary>
        public bool IsSigned
            => false;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => Kind switch
            {
                BooleanTypeKind.Boolean => 1,
                BooleanTypeKind.ByteBool => 1,
                BooleanTypeKind.WordBool => 2,
                BooleanTypeKind.LongBool => 4,
                _ => throw new InvalidOperationException(),
            };

        /// <summary>
        ///     type kind
        /// </summary>
        public BooleanTypeKind Kind { get; }


        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFromType(ITypeDefinition otherType) {

            if (otherType.BaseType == BaseType.Subrange && otherType is ISubrangeType subrange) {
                var subrangeBase = subrange.SubrangeOfType;
                return subrangeBase.BaseType == BaseType.Boolean;
            }

            return base.CanBeAssignedFromType(otherType);
        }

        /// <summary>
        ///     short type name
        /// </summary>
        public override string MangledName
            => Kind switch
            {
                BooleanTypeKind.Boolean => KnownNames.O,
                BooleanTypeKind.ByteBool => KnownNames.UC,
                BooleanTypeKind.WordBool => KnownNames.US,
                BooleanTypeKind.LongBool => KnownNames.I,
                _ => throw new InvalidOperationException(),
            };


        /// <summary>
        ///     long type name
        /// </summary>
        public override string Name
            => Kind switch
            {
                BooleanTypeKind.Boolean => KnownNames.Boolean,
                BooleanTypeKind.ByteBool => KnownNames.ByteBool,
                BooleanTypeKind.WordBool => KnownNames.WordBool,
                BooleanTypeKind.LongBool => KnownNames.LongBool,
                _ => throw new InvalidOperationException(),
            };

        /// <summary>
        ///     highest element
        /// </summary>
        public IValue HighestElement {
            get {
                var values = TypeRegistry.Runtime.Booleans;
                return Kind switch
                {
                    BooleanTypeKind.Boolean => values.Booleans.TrueValue,
                    BooleanTypeKind.ByteBool => values.ToByteBool(0xff, this),
                    BooleanTypeKind.WordBool => values.ToWordBool(0xff_ff, this),
                    BooleanTypeKind.LongBool => values.ToLongBool(0xff_ff_ff_ff, this),
                    _ => throw new InvalidOperationException(),
                };
            }
        }

        /// <summary>
        ///     smallest element
        /// </summary>
        public IValue LowestElement {
            get {
                var values = TypeRegistry.Runtime.Booleans;
                return Kind switch
                {
                    BooleanTypeKind.Boolean => values.Booleans.FalseValue,
                    BooleanTypeKind.ByteBool => values.ToByteBool(0, this),
                    BooleanTypeKind.WordBool => values.ToWordBool(0, this),
                    BooleanTypeKind.LongBool => values.ToLongBool(0, this),
                    _ => throw new InvalidOperationException(),
                };
            }
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => other is IBooleanType b && b.Kind == Kind;

        public override int GetHashCode()
            => HashCode.Combine(Kind);
    }
}
