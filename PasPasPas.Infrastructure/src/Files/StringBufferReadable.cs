using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     simple readable string
    /// </summary>
    public class StringBufferReadable : IBufferReadable {

        private readonly string value;

        /// <summary>
        ///     create a new string buffer readable
        /// </summary>
        /// <param name="content">string content</param>
        public StringBufferReadable(string content)
            => value = content;

        /// <summary>
        ///     set buffer item content as string
        /// </summary>
        /// <param name="item"></param>
        public void ToBufferItem(FileBufferItem item) {
            item.Data.Clear();
            item.Data.Capacity = value.Length;
            item.Data.Append(value);
        }

    }
}
