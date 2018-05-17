using PasPasPas.Globals.Types;
using PasPasPas.Parsing;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     environment for type annotated syntax trees
    /// </summary>
    public interface ITypedEnvironment : IParserEnvironment {

        /// <summary>
        ///     type registry: contains all registered types
        /// </summary>
        ITypeRegistry TypeRegistry { get; }

    }
}
