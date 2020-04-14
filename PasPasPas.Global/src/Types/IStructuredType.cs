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
        ITypeDefinition BaseClass { get; set; }

        /// <summary>
        ///     structured type kind
        /// </summary>
        StructuredTypeKind StructTypeKind { get; }

        /// <summary>
        ///     methods
        /// </summary>
        List<IRoutineGroup> Methods { get; }

        /// <summary>
        ///     find a method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classItem"></param>
        /// <returns></returns>
        IRoutineGroup FindMethod(string name, bool classItem);
        bool InheritsFrom(ITypeDefinition typeId);

        /// <summary>
        ///     make a constant value for this type
        /// </summary>
        /// <returns></returns>
        IValue MakeConstant();
        void ResolveCall(string symbolName, IList<IRoutineResult> callables, ISignature signature);
    }
}
