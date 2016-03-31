using System;
using PasPasPas.Infrastructure.Input;

namespace PasPasPas.DesktopPlatform {

    /// <summary>
    ///     standard file access
    /// </summary>
    public class StandardFileAccess : FileAccessBase {

        /// <summary>
        ///     service id
        /// </summary>
        public static readonly Guid StandardFileAccessGuid
            = new Guid("F6FF5F92-D63E-4C93-AFB0-4127C4D1015F");

        /// <summary>
        ///     service id
        /// </summary>
        public override Guid ServiceId
            => StandardFileAccessGuid;

        /// <summary>
        ///     service name
        /// </summary>
        public override string ServiceName
            => "StandardFileAccess";

        /// <summary>
        ///     open a textfile for reading
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <returns>opened file</returns>
        protected override IParserInput DoOpenFileForReading(string path)
            => new FileInput(path);
    }
}
