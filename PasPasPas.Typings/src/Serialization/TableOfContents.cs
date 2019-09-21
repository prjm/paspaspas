namespace PasPasPas.Typings.Serialization {

    internal class TableOfContents : Tag {

        public override uint Kind
            => Constants.TocTag;

        internal StringRegistry Strings { get; }

        internal Reference StringValues { get; }

        internal Reference Metadata { get; }

        internal Metadata Meta { get; }

        public TableOfContents() {
            StringValues = new Reference();
            Metadata = new Reference();
            Strings = new StringRegistry(StringValues);
            Meta = new Metadata(Metadata);
        }


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
