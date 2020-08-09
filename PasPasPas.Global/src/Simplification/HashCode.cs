using System;
using System.Collections.Generic;

namespace PasPasPas.Globals.Simplification {

    /// <summary>
    ///     helper function for hash code
    /// </summary>
    public static class HashCodeExtension {

        /// <summary>
        ///     add a range of items to a hash code computation
        /// </summary>
        /// <typeparam name="T">item class</typeparam>
        /// <param name="hash">hash to modify</param>
        /// <param name="values">values to add</param>
        public static void AddRange<T>(this ref HashCode hash, IEnumerable<T> values) {
            foreach (var value in values)
                hash.Add(value);
        }

    }
}
