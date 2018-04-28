﻿using System.Collections.Generic;
using System.Text;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;
using PasPasPas.Runtime.Values;
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
        ///     constant values
        /// </summary>
        public IRuntimeValueFactory ConstantValues { get; }

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
        ///     default type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; }

        /// <summary>
        ///     create a new default environment
        /// </summary>
        /// <param name="intSize">integer size</param>
        public DefaultEnvironment(NativeIntSize intSize = NativeIntSize.Undefined) {
            TypeRegistry = new RegisteredTypes(StringPool, intSize);
            ConstantValues = new RuntimeValueFactory(TypeRegistry);
            IntegerParser = new IntegerParser(ConstantValues, false);
            HexNumberParser = new IntegerParser(ConstantValues, true);
            RealLiteralConverter = new RealLiteralConverter(ConstantValues);
            TypeRegistry.Runtime = ConstantValues;
        }

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
                    TypeRegistry
                };
                return data;
            }
        }


    }
}
