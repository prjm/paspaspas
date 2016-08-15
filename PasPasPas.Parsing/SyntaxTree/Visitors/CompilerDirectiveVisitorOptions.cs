﻿using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using System;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor options
    /// </summary>
    public class CompilerDirectiveVisitorOptions {

        private ParserServices services;
        private LogSource logSource;
        private static readonly Guid messageSource
            = new Guid(new byte[] { 0xcc, 0x3b, 0xd8, 0xdd, 0xbf, 0x76, 0x5f, 0x40, 0xa2, 0xe8, 0x8a, 0xbd, 0x9f, 0xb6, 0x20, 0xc4 });
        /* {ddd83bcc-76bf-405f-a2e8-8abd9fb620c4} */

        /// <summary>
        ///     compile options
        /// </summary>
        public CompileOptions CompilerOptions
            => Environment.Options.CompilerOptions;

        /// <summary>
        ///     conditional compilation options
        /// </summary>
        public ConditionalCompilationOptions ConditionalCompilation
            => Environment.Options.ConditionalCompilation;

        /// <summary>
        ///     parsing environemnt
        /// </summary>
        public ParserServices Environment
        {
            get
            {
                return services;
            }
            set
            {
                logSource = null;
                services = value;

                if (services != null) {
                    logSource = new LogSource(services.Log, messageSource);
                }
            }
        }

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource LogSource
            => logSource;

        /// <summary>
        ///     meta information
        /// </summary>
        public MetaInformation Meta
            => Environment.Options.Meta;
    }
}