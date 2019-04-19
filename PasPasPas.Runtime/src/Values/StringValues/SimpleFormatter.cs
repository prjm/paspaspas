using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values.StringValues {

    internal class FormatInfo {
        internal ITypeReference Value { get; set; }
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


        private static FormatInfo GetFormat(ImmutableArray<ITypeReference> values) {
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
        public ITypeReference Format(ImmutableArray<ITypeReference> values) {
            var format = GetFormat(values);

            if (format == default)
                return Runtime.Types.MakeErrorTypeReference();

            if (format.Width == 0)
                return Runtime.Strings.EmptyString;

            switch (format.Value) {
                case ICharValue c:
                    return FormatChar(c, format.Width);
            }

            return Runtime.Types.MakeErrorTypeReference();
        }

        /// <summary>
        ///     used runtime
        /// </summary>
        public IRuntimeValueFactory Runtime { get; }


        private ITypeReference FormatChar(ICharValue c, int width) {
            if (width < 0)
                return Runtime.Strings.ToUnicodeString(c.AsUnicodeString);

            var s = new string(' ', width - 1);
            return Runtime.Strings.ToUnicodeString(s + c.AsUnicodeString);
        }
    }
}
