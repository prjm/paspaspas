#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     enumerated types
    /// </summary>
    public interface IEnumeratedType : IOrdinalType {

        /// <summary>
        ///     enumerated type definition
        /// </summary>
        ITypeDefinition CommonTypeId { get; }

        /// <summary>
        ///     values
        /// </summary>
        IList<IEnumeratedValue> Values { get; }

        /// <summary>
        ///     define an enumerated value
        /// </summary>
        /// <param name="runtimeValues"></param>
        /// <param name="symbolName"></param>
        /// <param name="withValue"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        IValue DefineEnumValue(IRuntimeValueFactory runtimeValues, string symbolName, bool withValue, IValue enumValue);
    }
}
