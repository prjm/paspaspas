using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {
    internal partial class TypeReader {

        public void ReadStrings(IStringRegistry strings)
            => ReadTag((StringRegistry)strings);

    }

    internal partial class TypeWriter {

        public void WriteStrings(IStringRegistry strings)
           => WriteTag((StringRegistry)strings);

    }
}
