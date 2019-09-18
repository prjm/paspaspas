namespace PasPasPas.Typings.Serialization {

    internal class TableOfContents : Tag {

        public override uint Kind
            => Constants.TocTag;

        public Metadata Meta { get; set; }
        public StringRegistry Strings { get; set; }

        internal Reference StringValues { get; }
            = new Reference();

        internal Reference Metadata { get; }
            = new Reference();

        internal override void ReadData(uint kind, TypeReader typeReader) {
            typeReader.ReadReference(StringValues);
            typeReader.ReadReference(Metadata);
        }

        internal override void WriteData(TypeWriter typeWriter) {
            typeWriter.WriteReferenceAddress(StringValues);
            typeWriter.WriteReferenceAddress(Metadata);
        }
    }
}
