using System;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     compiler directive parser error
    /// </summary>
    public static class CompilerDirectiveParserErrors {

        /// <summary>
        ///     invalid align directive
        /// </summary>
        public static readonly Guid InvalidAlignDirective
            = new Guid("{A664F5C8-1F90-4D8D-96E4-A5100A11B4DB}");

        /// <summary>
        ///     invalid application type
        /// </summary>
        public static readonly Guid InvalidApplicationType
            = new Guid("{D3121EB7-29C9-4D4A-AE35-4F8D519E381B}");

    }
}
