using PasPasPas.Parsing.SyntaxTree.Abstract;
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
        public WorkingStackEntry(object definingNode, ISyntaxPart createdNote, ISyntaxPart childNode) {
            node = definingNode;
            data = createdNote;
            child = ChildNode;
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

        public ISyntaxPart ChildNode
            => child;

        public override string ToString()
            => string.Format("{0} / {1} / {2} ", DefiningNode, data, ChildNode);
    }



}
