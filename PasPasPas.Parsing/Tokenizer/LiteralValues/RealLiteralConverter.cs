using System;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    public class RealLiteralConverter : IRealConverter, ILookupFunction<Tuple<object, object, bool, object>, object> {

        /// <summary>
        ///     invalid real literal
        /// </summary>
        public readonly object InvalidRealLiteral
            = new object();

        private LookupTable<Tuple<object, object, bool, object>, object> data;

        public LookupTable<Tuple<object, object, bool, object>, object> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        public RealLiteralConverter()
            => data = new LookupTable<Tuple<object, object, bool, object>, object>(ConvertLiterals);

        public object ConvertLiterals(Tuple<object, object, bool, object> data) {
            var digits = data.Item1;
            var decimals = data.Item2;
            var minus = data.Item3;
            var e = minus ? -1 : 1;
            var exponent = data.Item4;

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

        public object Convert(object digits, object decimals, bool minus, object exponent)
            => data.GetValue(new Tuple<object, object, bool, object>(digits, decimals, minus, exponent));
    }
};