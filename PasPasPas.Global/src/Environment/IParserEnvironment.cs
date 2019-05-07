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
        IEnvironmentItem TokenSequencePool { get; }

        /// <summary>
        ///     patters
        /// </summary>
        IEnvironmentItem Patterns { get; }

        /// <summary>
        ///     Terminal pool
        /// </summary>
        IEnvironmentItem TerminalPool { get; }

        /// <summary>
        ///     identifier pool
        /// </summary>
        IEnvironmentItem IdentifierPool { get; }

        /// <summary>
        ///     token array pool
        /// </summary>
        IEnvironmentItem TokenArrays { get; }

    }
}
