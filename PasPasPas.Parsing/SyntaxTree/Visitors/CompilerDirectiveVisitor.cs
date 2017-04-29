using System;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     visitor for compiler directives
    /// </summary>
    public class CompilerDirectiveVisitor :
        IStartVisitor<AlignSwitch> {

        private ParserServices services;
        private LogSource logSource;
        private readonly Visitor visitor;

        /// <summary>
        ///     creates a new visitor
        /// </summary>
        public CompilerDirectiveVisitor() {
            visitor = new Visitor(this);
        }

        private static readonly Guid messageSource
            = new Guid(new byte[] { 0xcc, 0x3b, 0xd8, 0xdd, 0xbf, 0x76, 0x5f, 0x40, 0xa2, 0xe8, 0x8a, 0xbd, 0x9f, 0xb6, 0x20, 0xc4 });
        /* {ddd83bcc-76bf-405f-a2e8-8abd9fb620c4} */


        /// <summary>
        ///     file access
        /// </summary>
        public IFileAccess FileAccess
            => Environment.Options.Files;

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
        ///     warnings
        /// </summary>
        public WarningOptions Warnings
            => Environment.Options.Warnings;

        /// <summary>
        ///     parsing environemnt
        /// </summary>
        public ParserServices Environment {
            get {
                return services;
            }
            set {
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

        /// <summary>
        ///     include reader
        /// </summary>
        public StackedFileReader IncludeInput { get; set; }
        /// <summary>
        ///     align switch
        /// </summary>
        /// <param name="alignSwitch">switch to visit</param>
        public void StartVisit(AlignSwitch alignSwitch)
            => CompilerOptions.Align.Value = alignSwitch.AlignValue;

        public IStartVisitor AsVisitor()
            => visitor;

    }
}
