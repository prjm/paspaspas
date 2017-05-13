using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Parsing.SyntaxTree.Utils {

    /// <summary>
    ///     generic interface for syntax part lists
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISyntaxPartList<T> :
        ICollection<T>,
        IList<T> where T : class, ISyntaxPart {

        /// <summary>
        ///     get the last item (or default)
        /// </summary>
        /// <returns>last or default item</returns>
        T LastOrDefault();
    }
}
