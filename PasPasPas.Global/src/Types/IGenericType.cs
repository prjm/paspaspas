using System.Collections.Immutable;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for generic types
    /// </summary>
    public interface IGenericType {


        /// <summary>
        ///     number of type parameters
        /// </summary>
        int NumberOfTypeParameters { get; }

        /// <summary>
        ///     bind this type to a generic type and resolve a new type
        /// </summary>
        /// <param name="typeIds"></param>
        /// <param name="typeCreator">type creator</param>
        /// <returns></returns>
        ITypeDefinition Bind(ImmutableArray<ITypeDefinition> typeIds, ITypeCreator typeCreator);

    }
}
