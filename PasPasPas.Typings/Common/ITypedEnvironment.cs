using PasPasPas.Parsing;
using PasPasPas.Parsing.SyntaxTree.Types;

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
