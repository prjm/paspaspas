﻿#nullable disable
using PasPasPas.Globals.Files;

namespace PasPasPas.Globals.Options.DataTypes {

    /// <summary>
    ///     reference to a windows resource file
    /// </summary>
    public class ResourceReference {

        /// <summary>
        ///     original filename
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        ///     associated rc file
        /// </summary>
        public string RcFile { get; set; }

        /// <summary>
        ///     target path
        /// </summary>
        public IFileReference TargetPath { get; set; }


        /// <summary>
        ///     <c>true</c> if the reference is resolved
        /// </summary>
        public bool IsResolved { get; set; }
    }
}