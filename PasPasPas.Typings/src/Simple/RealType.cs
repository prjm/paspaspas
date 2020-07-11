using System;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     real type definition
    /// </summary>
    internal class RealType : CompilerDefinedType, IRealType {

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
        public override string Name
            => Kind switch
            {
                RealTypeKind.Single => KnownNames.Single,
                RealTypeKind.Double => KnownNames.Double,
                RealTypeKind.Extended => KnownNames.Extended,
                RealTypeKind.Real48 => KnownNames.Real48,
                RealTypeKind.Comp => KnownNames.Comp,
                RealTypeKind.Currency => KnownNames.Currency,
                _ => throw new InvalidOperationException(),
            };

        /// <summary>
        ///     short name for types
        /// </summary>
        public override string MangledName
            => Kind switch
            {
                RealTypeKind.Single => KnownNames.F,
                RealTypeKind.Double => KnownNames.D,
                RealTypeKind.Extended => KnownNames.G,
                RealTypeKind.Real48 => KnownNames.SReal48,
                RealTypeKind.Comp => KnownNames.SystemAtComp,
                RealTypeKind.Currency => KnownNames.SystemAtCurrency,
                _ => throw new InvalidOperationException(),
            };

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

        public override bool Equals(ITypeDefinition? other)
            => other is IRealType r && r.Kind == Kind;
    }
}
