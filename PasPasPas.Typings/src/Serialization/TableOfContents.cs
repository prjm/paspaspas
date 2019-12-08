namespace PasPasPas.Typings.Serialization {

    internal class TableOfContents : Tag {

        public override uint Kind
            => Constants.TocTag;

        internal StringRegistry Strings { get; }
        internal Metadata Meta { get; }
        internal CodeBlock Routines { get; }
        internal DataBlock Data { get; }

        internal Reference ReferenceToStrings { get; }
            = new Reference();

        internal Reference ReferenceToMetadata { get; }
            = new Reference();

        internal Reference ReferenceToRoutines { get; }
            = new Reference();

        internal Reference ReferenceToData { get; }
            = new Reference();

        public TableOfContents() {
            Strings = new StringRegistry(ReferenceToStrings);
            Meta = new Metadata(ReferenceToMetadata);
            Routines = new CodeBlock(ReferenceToRoutines, Strings);
            Data = new DataBlock(ReferenceToData, Strings);
        }

        internal override void ReadData(uint kind, TypeReader typeReader) {
            typeReader.ReadReference(ReferenceToStrings);
            typeReader.ReadReference(ReferenceToMetadata);
            typeReader.ReadReference(ReferenceToData);
            typeReader.ReadReference(ReferenceToRoutines);
        }

        internal override void WriteData(TypeWriter typeWriter) {
            typeWriter.WriteReferenceAddress(ReferenceToStrings);
            typeWriter.WriteReferenceAddress(ReferenceToMetadata);
            typeWriter.WriteReferenceAddress(ReferenceToData);
            typeWriter.WriteReferenceAddress(ReferenceToRoutines);
        }
    }
}
