namespace PasPasPas.Typings.Serialization {

    internal class TableOfContents : Tag {

        public override uint Kind
            => Constants.TocTag;

        internal StringRegistry Strings { get; }
        internal Metadata Meta { get; }
        internal CodeBlock Routines { get; }

        internal Reference ReferenceToStrings { get; }
        internal Reference ReferenceToMetadata { get; }
        internal Reference ReferenceToRoutines { get; }

        public TableOfContents() {
            ReferenceToStrings = new Reference();
            ReferenceToMetadata = new Reference();
            ReferenceToRoutines = new Reference();

            Strings = new StringRegistry(ReferenceToStrings);
            Meta = new Metadata(ReferenceToMetadata);
            Routines = new CodeBlock(ReferenceToRoutines, Strings);
        }


        internal override void ReadData(uint kind, TypeReader typeReader) {
            typeReader.ReadReference(ReferenceToStrings);
            typeReader.ReadReference(ReferenceToMetadata);
            typeReader.ReadReference(ReferenceToRoutines);
        }

        internal override void WriteData(TypeWriter typeWriter) {
            typeWriter.WriteReferenceAddress(ReferenceToStrings);
            typeWriter.WriteReferenceAddress(ReferenceToMetadata);
            typeWriter.WriteReferenceAddress(ReferenceToRoutines);
        }
    }
}
