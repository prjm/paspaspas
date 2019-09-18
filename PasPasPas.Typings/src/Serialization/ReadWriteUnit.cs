using PasPasPas.Globals.Log;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {


    internal partial class TypeReader {

        /// <summary>
        ///     read a unit
        /// </summary>
        /// <returns></returns>
        public ITypeDefinition ReadUnit() {
            var number = ReadUint();
            if (number != Constants.MagicNumber) {
                Log.LogError(MessageNumbers.InvalidFileFormat);
                return default;
            }

            var toc = ReadTag(new TableOfContents());
            toc.Strings = ReadTag(new StringRegistry());
            toc.Meta = ReadTag(new Metadata());

            var result = Types.TypeCreator.CreateUnitType(toc.Strings[toc.Meta.UnitName]);

            return result;
        }

    }

    internal partial class TypeWriter {

        public void WriteUnit(IUnitType unit) {
            var number = Constants.MagicNumber;
            WriteUint(ref number);

            var toc = new TableOfContents() {
                Strings = new StringRegistry(),
                Meta = new Metadata()
            };

            toc.Meta.PrepareStrings(unit, toc.Strings);

            WriteTag(toc);
            WriteReferenceValue(toc.StringValues);
            WriteTag(toc.Strings);

            WriteReferenceValue(toc.Metadata);
            WriteTag(toc.Meta);
        }

    }

}
