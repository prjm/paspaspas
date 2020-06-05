#nullable disable
using System;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     string helpers
    /// </summary>
    public static class StringUtils {

        /// <summary>
        ///     format a string to invariant culture
        /// </summary>
        /// <param name="formattable"></param>
        /// <returns></returns>
        public static string Invariant(FormattableString formattable) {
            if (formattable == null)
                throw new ArgumentNullException(nameof(formattable));
            return formattable.ToString(System.Globalization.CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     format a string to current culture
        /// </summary>
        /// <param name="formattable"></param>
        /// <returns></returns>
        public static string Current(FormattableString formattable) {
            if (formattable == null) {
                throw new ArgumentNullException(nameof(formattable));
            }
            return formattable.ToString(System.Globalization.CultureInfo.CurrentCulture);
        }
    }
}
