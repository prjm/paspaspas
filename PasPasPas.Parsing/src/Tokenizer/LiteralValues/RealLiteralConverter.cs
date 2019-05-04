using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Environment;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {


    /// <summary>
    ///     helper class to convert real literals
    /// </summary>
    public class RealLiteralConverter : IEnvironmentItem, IRealConverter, ILookupFunction<string, ITypeReference> {

        private readonly IRuntimeValueFactory constantsValues;
        private readonly LookupTable<string, ITypeReference> data;

        /// <summary>
        ///     table entries
        /// </summary>
        public LookupTable<string, ITypeReference> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        /// <summary>
        ///     number of parsed numbers
        /// </summary>
        public int Count
            => Table.Count;

        /// <summary>
        ///     create a new real literal converter
        /// </summary>
        public RealLiteralConverter(IRuntimeValueFactory constValues) {
            data = new LookupTable<string, ITypeReference>(ConvertLiterals);
            constantsValues = constValues;
        }

        /// <summary>
        ///     convert parsed literals to a real literal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference ConvertLiterals(string value) {
            if (ExtF80.TryParse(value, out var realValue))
                return constantsValues.RealNumbers.ToExtendedValue(KnownTypeIds.Extended, realValue);
            return constantsValues.RealNumbers.Invalid;
        }

        /// <summary>
        ///     convert a parsed real number to a real literal
        /// </summary>
        /// <returns></returns>
        public ITypeReference Convert(string value)
            => data.GetValue(value);
    }
};