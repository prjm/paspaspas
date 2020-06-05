#nullable disable
using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     boolean type
    /// </summary>
    public class BooleanType : TypeDefinitionBase, IBooleanType {

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
        public override uint TypeSizeInBytes {
            get {
                switch (Kind) {
                    case BooleanTypeKind.Boolean:
                        return 1;

                    case BooleanTypeKind.ByteBool:
                        return 1;

                    case BooleanTypeKind.WordBool:
                        return 2;

                    case BooleanTypeKind.LongBool:
                        return 4;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        ///     type kind
        /// </summary>
        public BooleanTypeKind Kind { get; }

        /*
        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.SubrangeType && otherType is SubrangeType subrange) {
                var subrangeBase = subrange.BaseType;
                return subrangeBase.TypeKind == CommonTypeKind.BooleanType;
            }

            return base.CanBeAssignedFrom(otherType);
        }
        */
        /// <summary>
        ///     short type name
        /// </summary>
        public override string MangledName {
            get {
                switch (Kind) {
                    case BooleanTypeKind.Boolean:
                        return KnownNames.O;

                    case BooleanTypeKind.ByteBool:
                        return KnownNames.UC;

                    case BooleanTypeKind.WordBool:
                        return KnownNames.US;

                    case BooleanTypeKind.LongBool:
                        return KnownNames.I;
                }

                throw new InvalidOperationException();
            }
        }


        /// <summary>
        ///     long type name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case BooleanTypeKind.Boolean:
                        return KnownNames.Boolean;

                    case BooleanTypeKind.ByteBool:
                        return KnownNames.ByteBool;

                    case BooleanTypeKind.WordBool:
                        return KnownNames.WordBool;

                    case BooleanTypeKind.LongBool:
                        return KnownNames.LongBool;
                }

                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     highest element
        /// </summary>
        public IValue HighestElement {
            get {
                var values = TypeRegistry.Runtime.Booleans;
                switch (Kind) {
                    case BooleanTypeKind.Boolean:
                        return values.Booleans.TrueValue;

                    case BooleanTypeKind.ByteBool:
                        return values.ToByteBool(0xff, this);

                    case BooleanTypeKind.WordBool:
                        return values.ToWordBool(0xff_ff, this);

                    case BooleanTypeKind.LongBool:
                        return values.ToLongBool(0xff_ff_ff_ff, this);

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        ///     smallest element
        /// </summary>
        public IValue LowestElement {
            get {
                var values = TypeRegistry.Runtime.Booleans;
                switch (Kind) {
                    case BooleanTypeKind.Boolean:
                        return values.Booleans.FalseValue;

                    case BooleanTypeKind.ByteBool:
                        return values.ToByteBool(0, this);

                    case BooleanTypeKind.WordBool:
                        return values.ToWordBool(0, this);

                    case BooleanTypeKind.LongBool:
                        return values.ToLongBool(0, this);

                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
