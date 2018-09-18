using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Options.DataTypes {

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
        public FileReference TargetPath { get; set; }
    }
}