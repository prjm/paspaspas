using System.Collections.Generic;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     structured type definition
    /// </summary>
    public interface IStructuredType : ITypeDefinition, IExtensibleGenericType {

        /// <summary>
        ///     base class
        /// </summary>
        ITypeReference BaseClass { get; set; }

        /// <summary>
        ///     meta class
        /// </summary>
        ITypeReference MetaType { get; set; }

        /// <summary>
        ///     structured type kind
        /// </summary>
        StructuredTypeKind StructTypeKind { get; }

        /// <summary>
        ///     methods
        /// </summary>
        List<IRoutine> Methods { get; }

    }
}
