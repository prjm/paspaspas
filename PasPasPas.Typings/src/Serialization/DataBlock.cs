using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     data block
    /// </summary>
    internal class DataBlock : Tag {

        public DataBlock(Reference referenceToData, StringRegistry strings) {
            ReferenceToData = referenceToData;
            Strings = strings;
        }

        public Reference ReferenceToData { get; }

        public StringRegistry Strings { get; }

        public override uint Kind
            => Constants.DataTag;

        private readonly List<IVariable> vars
            = new List<IVariable>();

        internal override void ReadData(uint kind, TypeReader typeReader) {
            var n = typeReader.ReadUint();
            vars.Capacity = (int)n;

            for (var i = 0; i < n; i++) {
                var nameIndex = typeReader.ReadUint();
                var _ = Strings[nameIndex];
            }
        }

        internal override void WriteData(TypeWriter typeWriter) {
            var n = (uint)vars.Count;
            typeWriter.WriteUint(n);

            for (var i = 0; i < n; i++) {
                var variable = vars[i];
                n = Strings[variable.Name];
                typeWriter.WriteUint(n);
            }
        }

        internal void PrepareData(IUnitType unit) {
            foreach (var symbol in unit.Symbols) {
                var reference = symbol;
                if (reference.SymbolKind == SymbolTypeKind.Variable) {
                    var variable = reference as IVariable;
                    vars.Add(variable);
                    var _ = Strings[variable.Name];
                }
            }
        }
    }
}
