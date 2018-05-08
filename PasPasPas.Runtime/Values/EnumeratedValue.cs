using PasPasPas.Global.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     constant enumerated value
    /// </summary>
    public class EnumeratedValue : IEnumeratedValue {

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="enumTypeId">base type id</param>
        /// <param name="value">constant value</param>
        public EnumeratedValue(int enumTypeId, ITypeReference value) {
            TypeId = enumTypeId;
            Value = value;
        }

        /// <summary>
        ///     enumerated type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     <c>true</c> for this enumerated value
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.EnumerationType;

        /// <summary>
        ///     enumerated value
        /// </summary>
        public ITypeReference Value { get; }

        /// <summary>
        ///     convert this type to a short string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "Enum$" + Value.ToString();

    }
}