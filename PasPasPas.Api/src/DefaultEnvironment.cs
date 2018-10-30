using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;
using PasPasPas.Runtime.Values;
using PasPasPas.Typings.Common;

namespace PasPasPas.Api {

    /// <summary>
    ///     default environment: contains all registries and
    ///     factories needed
    /// </summary>
    public class DefaultEnvironment : ITypedEnvironment {

        /// <summary>
        ///     runtime values: constants and type references
        /// </summary>
        public IRuntimeValueFactory Runtime { get; }

        /// <summary>
        ///     integer parser
        /// </summary>
        public IIntegerLiteralParser IntegerParser { get; }

        /// <summary>
        ///     hex number parser
        /// </summary>
        public IIntegerLiteralParser HexNumberParser { get; }

        /// <summary>
        ///     char literal converter
        /// </summary>
        public ICharLiteralConverter CharLiteralConverter { get; }
            = new CharLiteralConverter();

        /// <summary>
        ///     real literal converter
        /// </summary>
        public IRealConverter RealLiteralConverter { get; }

        /// <summary>
        ///     token sequence pool
        /// </summary>
        public TokenSequences TokenSequencePool { get; }
            = new TokenSequences();

        /// <summary>
        ///     tokenizer patterns
        /// </summary>
        public PatternFactory Patterns { get; }

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log { get; }
            = new LogManager();

        /// <summary>
        ///     string builder pool
        /// </summary>
        public StringBuilderPool StringBuilderPool { get; }
            = new StringBuilderPool();

        /// <summary>
        ///     string pool
        /// </summary>
        public StringPool StringPool { get; }
            = new StringPool();

        /// <summary>
        ///     list pools
        /// </summary>
        public ListPools ListPools { get; }
            = new ListPools();

        /// <summary>
        ///     default type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     create a new default environment
        /// </summary>
        /// <param name="intSize">integer size</param>
        public DefaultEnvironment(NativeIntSize intSize = NativeIntSize.Undefined) {
            Patterns = new PatternFactory(StringPool);
            TypeRegistry = new RegisteredTypes(StringPool, intSize);
            Runtime = new RuntimeValueFactory(TypeRegistry);
            IntegerParser = new IntegerParser(Runtime, false);
            HexNumberParser = new IntegerParser(Runtime, true);
            RealLiteralConverter = new RealLiteralConverter(Runtime);
            TypeRegistry.Runtime = Runtime;
        }

        /// <summary>
        ///     all entries
        /// </summary>
        public IEnumerable<object> Entries {
            get {
                var data = new object[] {
                    IntegerParser,
                    HexNumberParser,
                    CharLiteralConverter,
                    RealLiteralConverter,
                    StringBuilderPool,
                    StringPool,
                    TokenSequencePool,
                    Patterns,
                    Log,
                    ListPools,
                    TypeRegistry,
                    TerminalPool,
                    IdentifierPool,
                    Histograms.Instance,
                };
                return data;
            }
        }

        /// <summary>
        ///     terminal pools
        /// </summary>
        public Terminals TerminalPool { get; }
            = new Terminals();

        /// <summary>
        ///     identifiers
        /// </summary>
        public Identifiers IdentifierPool { get; }
            = new Identifiers();

    }
}
