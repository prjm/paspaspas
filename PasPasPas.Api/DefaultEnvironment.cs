using System.Collections.Generic;
using System.Linq;
using System.Text;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;
using PasPasPas.Typings.Common;

namespace PasPasPas.Api {

    /// <summary>
    ///     default environment
    /// </summary>
    public class DefaultEnvironment : ITypedEnvironment {

        /// <summary>
        ///     default file access
        /// </summary>
        private StandardFileAccess files
            = new StandardFileAccess();

        /// <summary>
        ///     integer parser
        /// </summary>
        public ILiteralParser IntegerParser { get; }
            = new IntegerParser(false);

        /// <summary>
        ///     hex number parser
        /// </summary>
        public ILiteralParser HexNumberParser { get; }
            = new IntegerParser(true);

        /// <summary>
        ///     char literal converter
        /// </summary>
        public ICharLiteralConverter CharLiteralConverter { get; }
            = new CharLiteralConverter();

        /// <summary>
        ///     real literal converter
        /// </summary>
        public IRealConverter RealLiteralConverter { get; }
            = new RealLiteralConverter();

        /// <summary>
        ///     token sequence pool
        /// </summary>
        public ObjectPool<TokenizerWithLookahead.TokenSequence> TokenSequencePool { get; }
            = new ObjectPool<TokenizerWithLookahead.TokenSequence>() { PoolName = "TokenSequencePool" };

        /// <summary>
        ///     tokenizer patterbs
        /// </summary>
        public PatternFactory Patterns { get; }
            = new PatternFactory();

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log { get; }
            = new LogManager();

        /// <summary>
        ///     file access
        /// </summary>
        public IFileAccess Files
            => files;

        /// <summary>
        ///     string builder pool
        /// </summary>
        public ObjectPool<StringBuilder> StringBuilderPool { get; }
            = new ObjectPool<StringBuilder>() { PoolName = "StringBuilderPool" };

        /// <summary>
        ///     char pool
        /// </summary>
        public CharsAsString CharStringPool { get; }
            = new CharsAsString();

        /// <summary>
        ///     string pool
        /// </summary>
        public StringPool StringPool { get; }
            = new StringPool();

        /// <summary>
        ///     standard literal unwrapper
        /// </summary>
        public ILiteralUnwrapper LiteralUnwrapper { get; }
            = new LiteralUnwrapper();

        /// <summary>
        ///     default type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     boolean literal provider
        /// </summary>
        public IBooleanLiteralProvider BooleanLiterals { get; }
            = new BooleanLiteralProvider();

        /// <summary>
        ///     create a new default environment
        /// </summary>
        public DefaultEnvironment()
            => TypeRegistry = new RegisteredTypes(StringPool);

        /// <summary>
        ///     all entries
        /// </summary>
        public IEnumerable<object> Entries {
            get {
                var data = new object[] {
                    files,
                    IntegerParser,
                    HexNumberParser,
                    CharLiteralConverter,
                    RealLiteralConverter,
                    StringBuilderPool,
                    CharStringPool,
                    TokenSequencePool,
                    Patterns,
                    Log,
                    Files,
                    LiteralUnwrapper,
                    BooleanLiterals,
                    TypeRegistry
                };
                return data;
            }
        }

    }
}
