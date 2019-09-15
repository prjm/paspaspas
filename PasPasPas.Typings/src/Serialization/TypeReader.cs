using System;
using System.IO;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     read types
    /// </summary>
    internal partial class TypeReader : TypeIoBase, ITypeReader {

        /// <summary>
        ///     create a new type reader
        /// </summary>
        /// <param name="readableStream"></param>
        /// <param name="log">log</param>
        /// <param name="pool"></param>
        public TypeReader(Stream readableStream, ILogManager log, IStringPool pool) {
            ReadableStream = readableStream;
            Log = new LogSource(log, MessageGroups.TypeSerialization);
            StringPool = pool;
        }

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        ///     input
        /// </summary>
        public Stream ReadableStream { get; }

        /// <summary>
        ///     string pool
        /// </summary>
        public IStringPool StringPool { get; }

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
