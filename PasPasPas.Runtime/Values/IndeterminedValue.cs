using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     indetermined value
    /// </summary>
    public class IndeterminedRuntimeValue : ITypeReference {

        private readonly int typeId;

        /// <summary>
        ///     create a new indetermined runtime value
        /// </summary>
        /// <param name="typeId"></param>
        public IndeterminedRuntimeValue(int typeId)
            => this.typeId = typeId;

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
        ///     short string for this runtime value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "RuntimeValue";

    }
}