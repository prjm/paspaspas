#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     parser environment
    /// </summary>
    public interface IParserEnvironment : IEnvironment {

        /// <summary>
        ///     interface for constant operations
        /// </summary>
        IRuntimeValueFactory Runtime { get; }

        /// <summary>
        ///     integer literal parser
        /// </summary>
        IILiteralParser IntegerParser { get; }

        /// <summary>
        ///     hex number literal parser
        /// </summary>
        IILiteralParser HexNumberParser { get; }

        /// <summary>
        ///     real literal converter
        /// </summary>
        IILiteralParser RealLiteralConverter { get; }

        /// <summary>
        ///     object pool for token sequences
        /// </summary>
        object TokenSequencePool { get; }

        /// <summary>
        ///     patters
        /// </summary>
        object Patterns { get; }

        /// <summary>
        ///     Terminal pool
        /// </summary>
        object TerminalPool { get; }

        /// <summary>
        ///     identifier pool
        /// </summary>
        object IdentifierPool { get; }

        /// <summary>
        ///     token array pool
        /// </summary>
        object TokenArrays { get; }

    }
}
