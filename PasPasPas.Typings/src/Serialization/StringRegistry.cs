﻿using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    internal class StringRegistry : Tag, IStringRegistry {

        public override uint Kind
            => Constants.StringRegistryTag;

        private readonly Dictionary<string, uint> mapping
            = new Dictionary<string, uint>();

        private readonly Dictionary<uint, string> reverseMapping
            = new Dictionary<uint, string>();

        private uint count = 0;
        private Reference stringValues;

        public StringRegistry(Reference stringValues) => this.stringValues = stringValues;

        internal override void ReadData(uint kind, TypeReader typeReader) {
            var size = typeReader.ReadUint();
            var stringTag = new StringTag();

            for (var i = 0u; i < size; i++) {
                typeReader.ReadTag(stringTag);
                reverseMapping.Add(stringTag.Id, stringTag.Value);
            }
        }

        internal override void WriteData(TypeWriter typeWriter) {
            var size = (uint)mapping.Count;
            var stringTag = new StringTag();

            typeWriter.WriteUint(size);
            foreach (var item in mapping) {
                stringTag.Id = item.Value;
                stringTag.Value = item.Key;
                typeWriter.WriteTag(stringTag);
            }
        }

        public uint this[string v] {
            get {
                if (!mapping.TryGetValue(v, out var result)) {
                    result = count;
                    mapping.Add(v, result);
                    count++;
                }

                return result;
            }
        }

        public string this[uint v]
            => reverseMapping[v];

    }
}