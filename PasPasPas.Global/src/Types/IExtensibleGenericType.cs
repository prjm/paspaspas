using System.Collections.Generic;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     extensible generic type
    /// </summary>
    public interface IExtensibleGenericType : IGenericType, ITypeDefinition {

        /// <summary>
        ///     add a generic type parameter
        /// </summary>
        /// <param name="typeId"></param>
        void AddGenericParameter(int typeId);

        /// <summary>
        ///     generic parameters
        /// </summary>
        List<int> GenericParameters { get; }
    }
}
