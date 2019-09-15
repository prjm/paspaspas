namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     tag for strings
    /// </summary>
    internal class StringTag : Tag {

        internal uint Id { get; set; }

        internal string Value { get; set; }

        /// <summary>
        ///     tag kind
        /// </summary>
        public override uint Kind
            => Constants.StringTag;

        internal override void WriteData(TypeWriter typeWriter) {
            var aId = Id;
            typeWriter.WriteUint(ref aId);
            typeWriter.WriteString(Value);
        }

        internal override void ReadData(uint kind, TypeReader typeReader) {
            Id = typeReader.ReadUint();
            Value = typeReader.ReadString();
        }
    }
}
