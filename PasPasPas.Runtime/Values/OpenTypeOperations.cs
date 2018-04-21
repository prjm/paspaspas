using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     open type operations
    /// </summary>
    public class OpenTypeOperations : ITypeOperations {

        private readonly LookupTable<int, IValue> values;

        /// <summary>
        ///     create new open type operations
        /// </summary>
        public OpenTypeOperations()
            => values = new LookupTable<int, IValue>(MakeIndeterminedValue);


        private IValue MakeIndeterminedValue(int typeId)
            => new IndeterminedValue(typeId);

        /// <summary>
        ///     create a new undetermined value
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeReference MakeReference(int typeId)
            => values.GetValue(typeId);
    }
}
