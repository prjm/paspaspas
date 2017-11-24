using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using Entry = System.ValueTuple<object, object, bool, object>;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {


    /// <summary>
    ///     helper class to convert real literals
    /// </summary>
    public class RealLiteralConverter : IEnvironmentItem, IRealConverter, ILookupFunction<Entry, object> {

        /// <summary>
        ///     invalid real literal
        /// </summary>
        public readonly object InvalidRealLiteral
            = new object();

        private LookupTable<Entry, object> data;

        /// <summary>
        ///     table entries
        /// </summary>
        public LookupTable<Entry, object> Table
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
        public RealLiteralConverter()
            => data = new LookupTable<Entry, object>(ConvertLiterals);

        /// <summary>
        ///     convert parsd literas to a real kliteral
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public object ConvertLiterals((object digits, object decimals, bool minus, object exponent) data) {
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

                return value;
            }
            else
                return InvalidRealLiteral;
        }

        /// <summary>
        ///     convert a parsed real number to a real literal
        /// </summary>
        /// <param name="digits"></param>
        /// <param name="decimals"></param>
        /// <param name="minus"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public object Convert(object digits, object decimals, bool minus, object exponent)
            => data.GetValue((digits, decimals, minus, exponent));
    }
};