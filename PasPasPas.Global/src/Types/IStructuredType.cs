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
        ///     list of fields
        /// </summary>
        List<IVariable> Fields { get; }

        /// <summary>
        ///     find a method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classItem"></param>
        /// <returns></returns>
        IRoutineGroup FindMethod(string name, bool classItem);

        /// <summary>
        ///     test type inheritance
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        bool InheritsFrom(ITypeDefinition typeId);

        /// <summary>
        ///     make a constant value for this type
        /// </summary>
        /// <returns></returns>
        IValue MakeConstant();

        /// <summary>
        ///     try to resolve a call
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="callables"></param>
        /// <param name="signature"></param>
        void ResolveCall(string symbolName, IList<IRoutineResult> callables, ISignature signature);


        /// <summary>
        ///     add a field definition
        /// </summary>
        /// <param name="fieldDef"></param>
        void AddField(IVariable fieldDef);
    }
}
