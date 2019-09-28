using System.Collections.Generic;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     create a meta structured type
    /// </summary>
    public interface IMetaStructuredType : ITypeDefinition, IExtensibleGenericType {

        /// <summary>
        ///     base type id
        /// </summary>
        int BaseType { get; }

        /// <summary>
        ///     methods
        /// </summary>
        List<IRoutine> Methods { get; }

    }
}
