using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     constant array value
    /// </summary>
    public class ArrayValue : IArrayValue {

        private readonly int typeId;
        private readonly int baseTypeId;

        /// <summary>
        ///     create a new array value
        /// </summary>
        /// <param name="baseTypeId"></param>
        /// <param name="typeId"></param>
        public ArrayValue(int typeId, int baseTypeId) {
            this.typeId = typeId;
            this.baseTypeId = baseTypeId;
        }

        /// <summary>
        ///     base type
        /// </summary>
        public int BaseType
            => baseTypeId;

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => typeId;

        /// <summary>
        ///     returns always <c>true</c>
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.ConstantArrayType;
    }
}
