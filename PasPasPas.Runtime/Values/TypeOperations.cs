using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     open type operations
    /// </summary>
    public class TypeOperations : ITypeOperations {

        private readonly LookupTable<int, IValue> values;

        /// <summary>
        ///     nil pointer
        /// </summary>
        public IValue Nil { get; }
            = new SpecialValue(SpecialConstantKind.Nil, KnownTypeIds.GenericPointer);

        /// <summary>
        ///     create new open type operations
        /// </summary>
        public TypeOperations()
            => values = new LookupTable<int, IValue>(MakeIndeterminedValue);

        private IValue MakeIndeterminedValue(int typeId)
            => new IndeterminedValue(typeId);

        /// <summary>
        ///     create a new type reference value
        /// </summary>
        /// <param name="typeId">registered type id</param>
        /// <returns>type reference</returns>
        public ITypeReference MakeReference(int typeId)
            => values.GetValue(typeId);
    }
}
