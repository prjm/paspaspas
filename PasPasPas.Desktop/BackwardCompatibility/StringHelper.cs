#nullable disable
using System;

namespace PasPasPas.Desktop.BackwardCompatibility {

    public static class StringHelper {

        public static StringComparer Comparer(this StringComparison comparison) {
            switch (comparison) {
                case StringComparison.CurrentCulture:
                    return StringComparer.CurrentCulture;
                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase;
                case StringComparison.InvariantCulture:
                    return StringComparer.InvariantCulture;
                case StringComparison.InvariantCultureIgnoreCase:
                    return StringComparer.InvariantCultureIgnoreCase;
                case StringComparison.Ordinal:
                    return StringComparer.Ordinal;
                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase;
            }

            throw new ArgumentException($"Invalid comparison mode: ${comparison}", nameof(comparison));
        }

        /// <summary>
        ///     compute a hash code using a specific comparer
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparison"></param>
        /// <returns></returns>
        public static int GetHashCode(this string value, StringComparison comparison)
            => comparison.Comparer().GetHashCode(value);

    }
}