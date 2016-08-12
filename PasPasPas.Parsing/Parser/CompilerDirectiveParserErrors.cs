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

        /// <summary>
        ///     invalid assertion type
        /// </summary>
        public static readonly Guid InvalidAssertDirective
            = new Guid("{7680773E-1D99-48BF-8513-F3B6802046B0}");


        /// <summary>
        ///     invalid boolean evaluation switch
        /// </summary>
        public static readonly Guid InvalidBoolEvalDirective
            = new Guid("{4F5AA792-A1F2-4093-9228-E4FE94A6C8E6}");

        /// <summary>
        ///     invalid code align parameter value
        /// </summary>
        public static readonly Guid InvalidCodeAlignDirective
            = new Guid("{6D9AF0DC-F85E-4B8A-94F5-A07A349D33B0}");

        /// <summary>
        ///     invalid debug info directive
        /// </summary>
        public static readonly Guid InvalidDebugInfoDirective
            = new Guid("{F726A55C-7D19-4E45-9554-49AE2AC17E3C}");

        /// <summary>
        ///     invalid define
        /// </summary>
        public static readonly Guid InvalidDefineDirective
            = new Guid("{4A7F2635-8320-4C57-BF1E-E4344619AD92}");
    }
}
