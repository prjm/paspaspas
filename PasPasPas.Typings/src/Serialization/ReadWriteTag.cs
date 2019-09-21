﻿using System.IO;

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
        /// <param name="reference"></param>
        public void WriteTag(Tag tag, Reference reference = default) {
            var v = tag.Kind;

            if (reference != default)
                WriteReferenceValue(reference);

            WriteUint(ref v);
            tag.WriteData(this);
        }

    }

}
