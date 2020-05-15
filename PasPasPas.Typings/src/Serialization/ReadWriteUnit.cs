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
            ReadTag(toc.Strings);
            ReadTag(toc.Meta);

            var result = Types.CreateUnitType(toc.Strings[toc.Meta.UnitName]);
            toc.Routines.TargetUnit = result;

            ReadTag(toc.Data);
            ReadTag(toc.Routines);

            return result;
        }

    }

    internal partial class TypeWriter {

        public void WriteUnit(IUnitType unit) {
            var number = Constants.MagicNumber;
            WriteUint(number);

            var toc = new TableOfContents();
            toc.Meta.PrepareStrings(unit, toc.Strings);
            toc.Data.PrepareData(unit);
            toc.Routines.PrepareRoutines(unit);

            WriteTag(toc);

            WriteTag(toc.Strings, toc.ReferenceToStrings);
            WriteTag(toc.Meta, toc.ReferenceToMetadata);
            WriteTag(toc.Data, toc.ReferenceToData);
            WriteTag(toc.Routines, toc.ReferenceToRoutines);

            WriteOpenReferences();
        }

    }

}
