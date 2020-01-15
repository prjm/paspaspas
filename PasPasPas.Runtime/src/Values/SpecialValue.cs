using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     special constant values
    /// </summary>
    public class SpecialValue : IOldTypeReference {

        private readonly SpecialConstantKind kind;
        private readonly int typeIdentifier;
        private readonly CommonTypeKind typeKind;

        /// <summary>
        ///     create a new special kind
        /// </summary>
        /// <param name="constantKind">constant kind</param>
        /// <param name="typeId">type id</param>
        /// <param name="kind">type kind</param>
        public SpecialValue(SpecialConstantKind constantKind, int typeId = KnownTypeIds.ErrorType, CommonTypeKind kind = CommonTypeKind.UnknownType) {
            this.kind = constantKind;
            typeIdentifier = typeId;
            typeKind = kind;
        }

        /// <summary>
        ///     type identifier
        /// </summary>
        public int TypeId
            => typeIdentifier;

        /// <summary>
        ///     kind of this special value
        /// </summary>
        public SpecialConstantKind Kind
            => kind;

        /// <summary>
        ///     constant value
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;

        /// <summary>
        ///     common type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => typeKind;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is SpecialValue v) {
                return v.kind == kind;
            }

            return false;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => unchecked(17 + 31 * (int)kind);

        /// <summary>
        ///     convert this value to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     convert special value to string
        /// </summary>
        /// <returns></returns>
        public string InternalTypeFormat {
            get {
                switch (kind) {
                    case SpecialConstantKind.IntegerOverflow:
                        return "IO";
                    case SpecialConstantKind.InvalidInteger:
                        return "II";
                    case SpecialConstantKind.InvalidReal:
                        return "IR";
                    case SpecialConstantKind.DivisionByZero:
                        return "DZ";
                    case SpecialConstantKind.InvalidBool:
                        return "IB";
                    case SpecialConstantKind.Nil:
                        return "NIL";
                    default:
                        return StringUtils.Invariant($"{kind}");
                }
            }
        }
    }
}