#nullable disable
using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Text;
using PasPasPas.Globals.Runtime;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Runtime.Values.StringValues {

    internal class FormatInfo {
        internal IValue Value { get; set; }
        internal int Width { get; set; }
        internal int Precision { get; set; }
    }

    /// <summary>
    ///     simple formatter
    /// </summary>
    public class SimpleFormatter {

        /// <summary>
        ///     create a new simple formatter
        /// </summary>
        /// <param name="runtime"></param>
        public SimpleFormatter(IRuntimeValueFactory runtime)
            => Runtime = runtime;

        /// <summary>
        ///     used runtime
        /// </summary>
        public IRuntimeValueFactory Runtime { get; }

        private static FormatInfo GetFormat(ImmutableArray<IValue> values) {
            if (values.Length < 1 || values.Length > 3)
                return default;

            var result = new FormatInfo {
                Value = values[0],
                Width = -1,
                Precision = -1
            };

            if (values.Length >= 2) {
                var width = values[1] as IIntegerValue;
                if (width == default) return default;
                result.Width =
                    width.SignedValue > int.MaxValue ? int.MaxValue :
                    width.SignedValue < int.MinValue ? int.MinValue : (int)width.SignedValue;
            }

            if (values.Length >= 3) {
                var precision = values[2] as IIntegerValue;
                if (precision == default) return default;
                result.Precision =
                    precision.SignedValue > int.MaxValue ? int.MaxValue :
                    precision.SignedValue < int.MinValue ? int.MinValue : (int)precision.SignedValue;
            }

            return result;
        }

        /// <summary>
        ///     format a given value
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public IValue Format(ImmutableArray<IValue> values) {
            var format = GetFormat(values);

            if (format == default)
                return Runtime.Strings.Invalid;

            if (format.Width == 0)
                return Runtime.Strings.EmptyString;

            switch (format.Value) {
                case ICharValue c:
                    return FormatChar(c, format.Width);

                case IBooleanValue b:
                    return FormatBoolean(b, format.Width);

                case IStringValue s:
                    return FormatString(s, format.Width);

                case IIntegerValue i:
                    return FormatInteger(i, format.Width);

                case IRealNumberValue r:
                    return FormatReal(r, format.Width, format.Precision);
            }

            return Runtime.Strings.Invalid;
        }

        private IValue FormatReal(IRealNumberValue r, int width, int precision) {
            var sb = new StringBuilder();

            if (precision < 0) {
                var adjustedWidth = Math.Max(8, Math.Min(17, width));
                ExtF80.PrintFloat80(sb, r.AsExtended, PrintFloatFormat.ScientificFormat, adjustedWidth);
            }
            else {
                var adjustedPrecision = Math.Max(0, Math.Min(216, precision));
                ExtF80.PrintFloat80(sb, r.AsExtended, PrintFloatFormat.PositionalFormat, adjustedPrecision);
            }

            var v = sb.ToString();

            if (width <= v.Length)
                return Runtime.Strings.ToUnicodeString(v);

            var d = new string(' ', width - v.Length);
            return Runtime.Strings.ToUnicodeString(d + v);
        }

        private IValue FormatInteger(IIntegerValue i, int width) {
            var v = i.AsBigInteger.ToString(CultureInfo.InvariantCulture);

            if (width <= v.Length)
                return Runtime.Strings.ToUnicodeString(v);

            var d = new string(' ', width - v.Length);
            return Runtime.Strings.ToUnicodeString(d + v);
        }

        private IValue FormatString(IStringValue s, int width) {
            var v = s.AsUnicodeString;

            if (width <= v.Length)
                return Runtime.Strings.ToUnicodeString(v);

            var d = new string(' ', width - v.Length);
            return Runtime.Strings.ToUnicodeString(d + v);
        }

        private IValue FormatBoolean(IBooleanValue b, int width) {
            var v = b.AsBoolean ? "TRUE" : "FALSE";

            if (width <= v.Length)
                return Runtime.Strings.ToUnicodeString(v);

            var s = new string(' ', width - v.Length);
            return Runtime.Strings.ToUnicodeString(s + v);
        }

        private IValue FormatChar(ICharValue c, int width) {
            if (width < 0)
                return Runtime.Strings.ToUnicodeString(c.AsUnicodeString);

            var s = new string(' ', width - 1);
            return Runtime.Strings.ToUnicodeString(s + c.AsUnicodeString);
        }
    }
}
