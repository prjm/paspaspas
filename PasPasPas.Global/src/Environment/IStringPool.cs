using System;
using System.Text;

namespace PasPasPas.Globals.Environment {

    /// <summary>
    ///     interface for a string pooling service
    /// </summary>
    public interface IStringPool : IEnvironmentItem {

        /// <summary>
        ///     add a string manually to the string pool
        /// </summary>
        /// <param name="value">string to add</param>
        void AddString(string value);

        /// <summary>
        ///     get a pooled string from a string builder instance
        /// </summary>
        /// <param name="item">string builder instance</param>
        /// <returns>pooled string</returns>
        string PoolString(StringBuilder item);

        /// <summary>
        ///     pool a string from an utf16 byte span
        /// </summary>
        /// <param name="item"></param>
        string PoolString(Span<byte> item);

        /// <summary>
        ///     test if a string is already included in the pool
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool ContainsString(string value);
    }
}