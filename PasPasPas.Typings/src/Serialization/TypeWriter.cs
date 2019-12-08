using System;
using System.IO;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     write types
    /// </summary>
    internal partial class TypeWriter : TypeIoBase, ITypeWriter {

        /// <summary>
        ///     create a new type write
        /// </summary>
        /// <param name="writableStream"></param>
        /// <param name="registeredTypes"></param>
        /// <param name="stringPool"></param>
        public TypeWriter(Stream writableStream, ITypeRegistry registeredTypes, IStringPool stringPool) {
            WritableStream = writableStream;
            RegisteredTypes = registeredTypes;
            StringPool = stringPool;
        }

        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        ///     stream
        /// </summary>
        public Stream WritableStream { get; }
        public ITypeRegistry RegisteredTypes { get; }
        public IStringPool StringPool { get; }


        /// <summary>
        ///     dispose this writer
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    WritableStream.Dispose();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        ///     dispose this reader
        /// </summary>
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
