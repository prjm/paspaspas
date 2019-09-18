using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     unit meta data
    /// </summary>
    internal class Metadata : Tag {


        public uint UnitName { get; set; }

        public override uint Kind
            => Constants.UnitMetaDataTag;

        internal override void ReadData(uint kind, TypeReader typeReader)
            => UnitName = typeReader.ReadUint();

        internal override void WriteData(TypeWriter typeWriter) {
            var n = UnitName;
            typeWriter.WriteUint(ref n);
        }

        internal void PrepareStrings(IUnitType unitType, StringRegistry strings)
            => UnitName = strings[unitType.Name];
    }
}