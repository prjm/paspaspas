using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using Entry = System.ValueTuple<object, object, bool, object>;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {


    /// <summary>
    ///     helper class to convert real literals
    /// </summary>
    public class RealLiteralConverter : IEnvironmentItem, IRealConverter, ILookupFunction<Entry, IValue> {

        private readonly IRuntimeValues constantsValues;
        private LookupTable<Entry, IValue> data;

        /// <summary>
        ///     table entries
        /// </summary>
        public LookupTable<Entry, IValue> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        /// <summary>
        ///     number of parsed numbers
        /// </summary>
        public int Count
            => Table.Count;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => "RealParser";

        /// <summary>
        ///     create a new real literal converter
        /// </summary>
        public RealLiteralConverter(IRuntimeValues constValues) {
            data = new LookupTable<Entry, IValue>(ConvertLiterals);
            constantsValues = constValues;
        }

        /// <summary>
        ///     convert parsed literals to a real literal
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public IValue ConvertLiterals((object digits, object decimals, bool minus, object exponent) data) {
            return constantsValues.ToExtendedValue(0);
            var digits = data.digits;
            var decimals = data.decimals;
            var minus = data.minus;
            var e = minus ? -1 : 1;
            var exponent = data.exponent;

            if (digits.IsNumber() && (decimals == null || decimals.IsNumber()) && (exponent == null || exponent.IsNumber())) {
                var value = System.Convert.ToDouble(digits);

                if (decimals != null)
                    value += System.Convert.ToDouble(decimals) / Math.Pow(10, (1 + Math.Truncate(Math.Log10(System.Convert.ToDouble(decimals)))));

                if (exponent != null)
                    value = value * Math.Pow(10, e * System.Convert.ToDouble(exponent));

                return constantsValues.ToExtendedValue(value);
            }
            else
                return constantsValues[SpecialConstantKind.InvalidReal];
        }

        /// <summary>
        ///     convert a parsed real number to a real literal
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="decimals"></param>
        /// <param name="minus"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public IValue Convert(object digits, object decimals, bool minus, object exponent)
            => data.GetValue((digits, decimals, minus, exponent));
    }
};