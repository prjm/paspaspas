using System;
using PasPasPas.Global.Constants;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     open type
    /// </summary>
    public class TypeOperations : ITypeOperations {

        private readonly LookupTable<int, ITypeReference> values;
        private readonly ITypeKindResolver resolver;

        /// <summary>
        ///     create a new type operations class
        /// </summary>
        /// <param name="typeKindResolver">resolver for type kinds</param>
        public TypeOperations(ITypeKindResolver typeKindResolver) {
            resolver = typeKindResolver;
            values = new LookupTable<int, ITypeReference>(MakeIndeterminedValue);
        }


        /// <summary>
        ///     nil pointer constant
        /// </summary>
        public IValue Nil { get; }
            = new SpecialValue(SpecialConstantKind.Nil, KnownTypeIds.GenericPointer);

        /// <summary>
        ///     type resolver
        /// </summary>
        public ITypeKindResolver Resolver
            => resolver;

        private ITypeReference MakeIndeterminedValue(int typeId)
            => new IndeterminedRuntimeValue(typeId, resolver.GetTypeKindOf(typeId));

        /// <summary>
        ///     create a new type reference value
        /// </summary>
        /// <param name="typeId">registered type id</param>
        /// <returns>type reference</returns>
        public ITypeReference MakeReference(int typeId)
            => values.GetValue(typeId);
    }
}
