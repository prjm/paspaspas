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
        int BaseClassId { get; set; }

        /// <summary>
        ///     structured type kind
        /// </summary>
        StructuredTypeKind StructTypeKind { get; }

        /// <summary>
        ///     methods
        /// </summary>
        List<IRoutine> Methods { get; }

        /// <summary>
        ///     find a method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classItem"></param>
        /// <returns></returns>
        IRoutine FindMethod(string name, bool classItem);
    }
}
