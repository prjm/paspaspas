﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Parsing {

    /// <summary>
    ///     parser environment
    /// </summary>
    public interface IParserEnvironment : IBasicEnvironment {

        /// <summary>
        ///     interface for constant operations
        /// </summary>
        IRuntimeValueFactory Runtime { get; }

        /// <summary>
        ///     integer literal parser
        /// </summary>
        IIntegerLiteralParser IntegerParser { get; }

        /// <summary>
        ///     hex number literal parser
        /// </summary>
        IIntegerLiteralParser HexNumberParser { get; }

        /// <summary>
        ///     char literal converter
        /// </summary>
        ICharLiteralConverter CharLiteralConverter { get; }

        /// <summary>
        ///     real literal converter
        /// </summary>
        IRealConverter RealLiteralConverter { get; }

        /// <summary>
        ///     object pool for token sequences
        /// </summary>
        TokenSequences TokenSequencePool { get; }

        /// <summary>
        ///     patters
        /// </summary>
        PatternFactory Patterns { get; }

    }
}