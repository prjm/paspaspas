using System;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     real type definition
    /// </summary>
    public class RealType : TypeDefinitionBase, IRealType {

        /// <summary>
        ///     real type definition
        /// </summary>
        public RealType(IUnitType definingType, RealTypeKind kind) : base(definingType)
            => Kind = kind;

        /// <summary>
        ///     common type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.Real;

        /*

        /// <summary>
        ///     test for assignment type compatibility
        /// </summary>
        /// <param name="otherType">other type to check</param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.RealType)
                return true;

            if (otherType.TypeKind == CommonTypeKind.Int64Type)
                return true;

            if (otherType.TypeKind == CommonTypeKind.IntegerType)
                return true;

            return base.CanBeAssignedFrom(otherType);
        }

        */

        /// <summary>
        ///     long type name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case RealTypeKind.Single:
                        return KnownNames.Single;

                    case RealTypeKind.Double:
                        return KnownNames.Double;

                    case RealTypeKind.Extended:
                        return KnownNames.Extended;

                    case RealTypeKind.Real48:
                        return KnownNames.Real48;

                    case RealTypeKind.Comp:
                        return KnownNames.Comp;

                    case RealTypeKind.Currency:
                        return KnownNames.Currency;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        ///     short name for types
        /// </summary>
        public override string MangledName {
            get {
                switch (Kind) {
                    case RealTypeKind.Single:
                        return KnownNames.F;

                    case RealTypeKind.Double:
                        return KnownNames.D;

                    case RealTypeKind.Extended:
                        return KnownNames.G;

                    case RealTypeKind.Real48:
                        return KnownNames.SReal48;

                    case RealTypeKind.Comp:
                        return KnownNames.SystemAtComp;

                    case RealTypeKind.Currency:
                        return KnownNames.SystemAtCurrency;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                switch (Kind) {
                    case RealTypeKind.Single:
                        return 4;

                    case RealTypeKind.Comp:
                    case RealTypeKind.Currency:
                    case RealTypeKind.Double:
                        return 8;

                    case RealTypeKind.Extended:
                        return 10;

                    case RealTypeKind.Real48:
                        return 6;

                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        /// <summary>
        ///     real type kind
        /// </summary>
        public RealTypeKind Kind { get; }
    }
}
