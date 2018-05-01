using PasPasPas.Global.Types;
using PasPasPas.Parsing;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     typed environment
    /// </summary>
    public interface ITypedEnvironment : IParserEnvironment {

        /// <summary>
        ///     type registry
        /// </summary>
        ITypeRegistry TypeRegistry { get; }

    }
}
