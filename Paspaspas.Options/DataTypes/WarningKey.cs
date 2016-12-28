using System;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     warning key
    /// </summary>
    public class WarningKey {

        /// <summary>
        ///     create a new warning key
        /// </summary>
        /// <param name="number"></param>
        /// <param name="ident"></param>
        public WarningKey(string number, string ident) {
            WarningIdentifier = ident;
            WarningNumber = number;
        }

        /// <summary>
        ///     name of the warning
        /// </summary>
        public string WarningIdentifier { get; }

        /// <summary>
        ///     warning number
        /// </summary>
        public string WarningNumber { get; }

        /// <summary>
        ///     check for equeality
        /// </summary>
        /// <param name="obj">other object</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            var other = obj as WarningKey;
            if (ReferenceEquals(other, null))
                return false;

            return string.Equals(WarningIdentifier, other.WarningIdentifier, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     compute a hash code for this object
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            result = result * 31 + StringComparer.OrdinalIgnoreCase.GetHashCode(WarningIdentifier);
            return result;
        }

    }
}