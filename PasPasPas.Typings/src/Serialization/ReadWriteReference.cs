using System.Collections.Generic;

namespace PasPasPas.Typings.Serialization {
    internal partial class TypeReader {

        /// <summary>
        ///     read an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void ReadReference(Reference value) {
            value.HasAddress = true;
            value.Address = ReadLong();
        }
    }

    internal partial class TypeWriter {

        internal class OpenReference {
            public OpenReference(long position, Reference value) {
                Position = position;
                Value = value;
            }

            public long Position { get; }
            public Reference Value { get; }
        }

        private Queue<OpenReference> openReferences
            = new Queue<OpenReference>();

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public bool WriteReferenceAddress(Reference value) {
            var result = value.HasAddress;
            if (!result)
                openReferences.Enqueue(new OpenReference(WritableStream.Position, value));

            var adr = value.Address;
            WriteLong(ref adr);
            return result;
        }

        /// <summary>
        ///     write an unsigned integer
        /// </summary>
        /// <param name="value"></param>
        public void WriteReferenceValue(Reference value) {
            if (value.HasAddress)
                throw new TypeReaderWriteException();
            value.Address = WritableStream.Position;
        }

        public void WriteOpenReferences() {
            while (openReferences.Count > 0) {
                var value = openReferences.Dequeue();
                if (!value.Value.HasAddress)
                    throw new TypeReaderWriteException();
                WritableStream.Position = value.Position;
                WriteReferenceAddress(value.Value);
            }
        }

    }
}
