using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Environment {

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
