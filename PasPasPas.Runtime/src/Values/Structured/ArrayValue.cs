using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     constant array value
    /// </summary>
    public class ArrayValue : IArrayValue {

        /// <summary>
        ///     create a new array value
        /// </summary>
        /// <param name="baseTypeId"></param>
        /// <param name="typeId"></param>
        public ArrayValue(int typeId, int baseTypeId, ImmutableArray<ITypeReference> constantValues) {
            TypeId = typeId;
            BaseType = baseTypeId;
            Values = constantValues;
        }

        /// <summary>
        ///     base type
        /// </summary>
        public int BaseType { get; }

        /// <summary>
        ///     constant values
        /// </summary>
        public ImmutableArray<ITypeReference> Values { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.ConstantArrayType;

        /// <summary>
        ///     format this type
        /// </summary>
        public string InternalTypeFormat
            => $"[({string.Join(", ", Values)})]";

        /// <summary>
        ///     format this value as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     reference kind: constant
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;
    }
}
