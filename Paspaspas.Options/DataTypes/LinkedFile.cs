using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Input;

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
        public IFileReference TargetPath { get; set; }
    }
}