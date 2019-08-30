using System;
using System.IO;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     write types
    /// </summary>
    internal partial class TypeWriter : ITypeWriter {

        /// <summary>
        ///     create a new type write
        /// </summary>
        /// <param name="writableStream"></param>
        public TypeWriter(Stream writableStream)
            => WritableStream = writableStream;

        /// <summary>
        ///     write a unit
        /// </summary>
        /// <param name="unitType"></param>
        public void WriteUnit(ITypeDefinition unitType)
            => throw new System.NotImplementedException();


        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        ///     stream
        /// </summary>
        public Stream WritableStream { get; }


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
