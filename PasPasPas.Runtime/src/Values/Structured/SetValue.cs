using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.Structured {

    /// <summary>
    ///     set values
    /// </summary>
    public class SetValue : ITypeReference {

        /// <summary>
        ///     create a new set value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        public SetValue(int typeId, ImmutableArray<ITypeReference> values) {
            TypeId = typeId;
            Values = values;
        }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId { get; }

        /// <summary>
        ///     set values
        /// </summary>
        public ImmutableArray<ITypeReference> Values { get; }

        /// <summary>
        ///     internal type format
        /// </summary>
        public string InternalTypeFormat
            => $"set [({string.Join(", ", Values)})]";

        /// <summary>
        ///     internal type format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///     type reference kind
        /// </summary>
        public TypeReferenceKind ReferenceKind
            => TypeReferenceKind.ConstantValue;

        /// <summary>
        ///     type kind
        /// </summary>
        public CommonTypeKind TypeKind
            => CommonTypeKind.SetType;
    }
}