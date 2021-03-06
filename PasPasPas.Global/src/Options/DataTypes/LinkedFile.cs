﻿#nullable disable
using PasPasPas.Globals.Files;

namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     directly linked file
    /// </summary>
    public class LinkedFile {

        /// <summary>
        ///     original file name
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        ///     file to link
        /// </summary>
        public IFileReference TargetPath { get; set; }

        /// <summary>
        ///     <c>true</c> if the reference is resolved
        /// </summary>
        public bool IsResolved { get; set; }
    }
}