using System;
using System.IO;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     read types
    /// </summary>
    internal partial class TypeReader : ITypeReader {

        /// <summary>
        ///     create a new type reader
        /// </summary>
        /// <param name="readableStream"></param>
        /// <param name="log">log</param>
        public TypeReader(Stream readableStream, ILogManager log) {
            ReadableStream = readableStream;
            Log = new LogSource(log, MessageGroups.TypeSerialization);
        }

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        ///     input
        /// </summary>
        public Stream ReadableStream { get; }

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource Log { get; }

        /// <summary>
        ///     dispose this reader
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    ReadableStream.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose this write
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }



    }
}
