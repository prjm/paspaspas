using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     real type definition
    /// </summary>
    public class RealType : TypeBase {
        readonly bool isCurrency;

        /// <summary>
        ///     real type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withBitSize"></param>
        /// <param name="isComp"></param>
        /// <param name="isCurrency"></param>
        public RealType(int withId, uint withBitSize, bool isComp = false, bool isCurrency = false) : base(withId) {
            BitSize = withBitSize;
            IsComp = isComp;
            this.isCurrency = isCurrency;
        }

        /// <summary>
        ///     common type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.RealType;

        /// <summary>
        ///     bitsize
        /// </summary>
        public uint BitSize { get; }

        /// <summary>
        ///     <c>true</c> if this is the comp data type
        /// </summary>
        public bool IsComp { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => (BitSize - 1) / 8u + 1u;

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

        /// <summary>
        ///     long type name
        /// </summary>
        public override string LongName {
            get {
                switch (BitSize) {
                    case 32:
                        return KnownNames.Single;
                    case 48:
                        return KnownNames.Real48;
                    case 64:
                        if (IsComp)
                            return KnownNames.Comp;
                        if (isCurrency)
                            return KnownNames.Currency;
                        return KnownNames.Double;
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     short name for types
        /// </summary>
        public override string ShortName {
            get {
                switch (BitSize) {
                    case 32:
                        return KnownNames.F;
                    case 48:
                        return KnownNames.SReal48;
                    case 64:
                        if (IsComp)
                            return KnownNames.SystemAtComp;
                        if (isCurrency)
                            return KnownNames.SystemAtCurrency;
                        return KnownNames.D;
                }
                throw new InvalidOperationException();
            }
        }
    }
}
