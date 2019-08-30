using System.IO;
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

        /// <summary>
        ///     create a type writer
        /// </summary>
        /// <param name="writableStream"></param>
        /// <returns></returns>
        ITypeWriter CreateTypeWriter(Stream writableStream);

        /// <summary>
        ///     create a type reader
        /// </summary>
        /// <param name="readableStream"></param>
        /// <returns></returns>
        ITypeReader CreateTypeReader(Stream readableStream);

    }
}
