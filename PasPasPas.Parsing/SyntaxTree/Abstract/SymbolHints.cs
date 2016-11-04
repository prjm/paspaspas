﻿namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     symbol hints
    /// </summary>
    public class SymbolHints {

        /// <summary>
        ///     deprecated information
        /// </summary>
        public string DeprecatedInformation { get; set; }
            = string.Empty;

        /// <summary>
        ///     symbol in library
        /// </summary>
        public bool SymbolInLibrary { get; set; }
            = false;

        /// <summary>
        ///     <c>true</c> if deprecated
        /// </summary>
        public bool SymbolIsDeprecated { get; set; }
            = false;

        /// <summary>
        ///     experimental symbol
        /// </summary>
        public bool SymbolIsExperimental { get; set; }

        /// <summary>
        ///     platform specific symbol
        /// </summary>
        public bool SymbolIsPlatformSpecific { get; set; }
    }
}