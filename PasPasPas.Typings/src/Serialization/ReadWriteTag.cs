using System.IO;

namespace PasPasPas.Typings.Serialization {

    internal partial class TypeReader {

        public T ReadTag<T>(T tag) where T : Tag {
            var kind = ReadUint();
            tag.ReadData(kind, this);

            if (tag.Kind != kind)
                throw new InvalidDataException();

            return tag;
        }

    }

    internal partial class TypeWriter {

        /// <summary>
        ///     write a tag
        /// </summary>
        /// <param name="tag"></param>
        public void WriteTag(Tag tag) {
            var v = tag.Kind;
            WriteUint(ref v);
            tag.WriteData(this);
        }

    }

}
