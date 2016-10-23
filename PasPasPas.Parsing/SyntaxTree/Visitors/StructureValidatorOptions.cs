using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using System;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     options for validating
    /// </summary>
    public class StructureValidatorOptions {

        /// <summary>
        ///     log manager
        /// </summary>
        public LogManager Manager { get; set; }

        /// <summary>
        ///     log source
        /// </summary>
        private LogSource logSource
            = null;

        /// <summary>
        ///     log source group id
        /// </summary>
        public readonly static Guid StructureValidatorGroupId
             = new Guid(new byte[] { 0xcf, 0x1b, 0x1a, 0xa4, 0x41, 0x79, 0xd7, 0x47, 0xb4, 0xc0, 0x53, 0xed, 0xa7, 0xb8, 0x39, 0x37 });
        /* {a41a1bcf-7941-47d7-b4c0-53eda7b83937} */


        /// <summary>
        ///     file access
        /// </summary>
        public IFileAccess FileAccess { get; set; }

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource Log
        {
            get
            {
                if (logSource != null)
                    return logSource;

                if (Manager != null) {
                    logSource = new LogSource(Manager, StructureValidatorGroupId);
                    return logSource;
                }

                return null;
            }
        }

    }
}