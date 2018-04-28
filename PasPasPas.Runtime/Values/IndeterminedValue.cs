using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     indetermined value
    /// </summary>
    public class IndeterminedRuntimeValue : ITypeReference {

        private readonly int typeId;
        private readonly CommonTypeKind typeKind;

        /// <summary>
        ///     create a new indetermined runtime value
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <param name="typeKind">type kind</param>
        public IndeterminedRuntimeValue(int typeId, CommonTypeKind typeKind) {
            this.typeId = typeId;
            this.typeKind = typeKind;
        }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => typeId;

        /// <summary>
        ///     always <c>false</c> for runtime values
        /// </summary>
        public bool IsConstant
            => false;

        /// <summary>
        ///     fixed type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => typeKind;

        /// <summary>
        ///     short string for this runtime value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "RuntimeValue";

    }
}