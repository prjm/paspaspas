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



            return default;
        }
    }

    internal partial class TypeWriter {
    }

}
