#nullable disable
using System.Text;

namespace PasPasPas.Infrastructure.ObjectPooling {

    /// <summary>
    ///     a pool of string builders
    /// </summary>
    public class StringBuilderPool : ObjectPool<StringBuilder> {

        /// <summary>
        ///     clears the string buffer
        /// </summary>
        /// <param name="result"></param>
        protected override void Prepare(StringBuilder result)
            => result.Clear();

    }
}