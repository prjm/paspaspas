using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     stack entry
    /// </summary>
    public class WorkingStackEntry {
        private ISyntaxPart data;
        private object node;
        private ISyntaxPart child;

        /// <summary>
        ///     create a new entry
        /// </summary>
        /// <param name="definingNode"></param>
        /// <param name="createdNote"></param>
        public WorkingStackEntry(object definingNode, ISyntaxPart createdNote) {
            node = definingNode;
            data = createdNote;
        }

        /// <summary>
        ///     create a new entry
        /// </summary>
        /// <param name="definingNode"></param>
        /// <param name="createdNote"></param>
        /// <param name="childNode">child node</param>
        public WorkingStackEntry(object definingNode, ISyntaxPart createdNote, ISyntaxPart childNode) {
            node = definingNode;
            data = createdNote;
            child = childNode;
        }

        /// <summary>
        ///     defining node
        /// </summary>
        public object DefiningNode
            => node;

        /// <summary>
        ///     defining child node
        /// </summary>
        public ISyntaxPart Data
             => data;

        /// <summary>
        ///     child node
        /// </summary>
        public ISyntaxPart ChildNode
            => child;

        /// <summary>
        ///     format node as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => string.Format("{0} / {1} / {2} ", DefiningNode, data, ChildNode);
    }



}
