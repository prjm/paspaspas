﻿using System.Collections.Generic;
using System.Text;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
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
    public class DefaultEnvironment : IAssemblyBuilderEnvironment {

        /// <summary>
        ///     runtime values: constants and type references
        /// </summary>
        public IRuntimeValueFactory Runtime { get; }

        /// <summary>
        ///     integer parser
        /// </summary>
        public IILiteralParser IntegerParser { get; }

        /// <summary>
        ///     hex number parser
        /// </summary>
        public IILiteralParser HexNumberParser { get; }

        /// <summary>
        ///     real literal converter
        /// </summary>
        public IILiteralParser RealLiteralConverter { get; }

        /// <summary>
        ///     token sequence pool
        /// </summary>
        public IEnvironmentItem TokenSequencePool { get; }
            = new TokenSequences();

        /// <summary>
        ///     tokenizer patterns
        /// </summary>
        public IEnvironmentItem Patterns { get; }

        /// <summary>
        ///     log manager
        /// </summary>
        public ILogManager Log { get; }
            = new LogManager();

        /// <summary>
        ///     string builder pool
        /// </summary>
        public IObjectPool<StringBuilder> StringBuilderPool { get; }
            = new StringBuilderPool();

        /// <summary>
        ///     string pool
        /// </summary>
        public IStringPool StringPool { get; }
            = new StringPool();

        /// <summary>
        ///     list pools
        /// </summary>
        public IListPools ListPools { get; }
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
            Runtime = new RuntimeValueFactory(ListPools);
            TypeRegistry = new RegisteredTypes(Runtime, ListPools, intSize);
            IntegerParser = new IntegerParser(Runtime, false);
            HexNumberParser = new IntegerParser(Runtime, true);
            RealLiteralConverter = new RealLiteralConverter(Runtime);
        }

        /// <summary>
        ///     all entries
        /// </summary>
        public IEnumerable<IEnvironmentItem> Entries {
            get {
                var data = new IEnvironmentItem[] {
                    IntegerParser,
                    HexNumberParser,
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
                    TokenArrays,
                    Histograms.Instance,
                };
                return data;
            }
        }

        /// <summary>
        ///     terminal pools
        /// </summary>
        public IEnvironmentItem TerminalPool { get; }
            = new Terminals();

        /// <summary>
        ///     identifiers
        /// </summary>
        public IEnvironmentItem IdentifierPool { get; }
            = new Identifiers();

        /// <summary>
        ///     token array pool
        /// </summary>
        public IEnvironmentItem TokenArrays { get; }
            = new TokenArrays();

    }
}
