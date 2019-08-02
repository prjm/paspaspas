using PasPasPas.Globals.Parsing;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     stack entry
    /// </summary>
    public class WorkingStackEntry {

        /// <summary>
        ///     create a new entry
        /// </summary>
        /// <param name="definingNode"></param>
        /// <param name="createdNote"></param>
        public WorkingStackEntry(object definingNode, ISyntaxPart createdNote) {
            DefiningNode = definingNode;
            Data = createdNote;
        }

        /// <summary>
        ///     create a new entry
        /// </summary>
        /// <param name="definingNode"></param>
        /// <param name="createdNote"></param>
        /// <param name="childNode">child node</param>
        public WorkingStackEntry(object definingNode, ISyntaxPart createdNote, ISyntaxPart childNode) {
            DefiningNode = definingNode;
            Data = createdNote;
            ChildNode = childNode;
        }

        /// <summary>
        ///     defining node
        /// </summary>
        public object DefiningNode { get; }

        /// <summary>
        ///     defining child node
        /// </summary>
        public ISyntaxPart Data { get; }

        /// <summary>
        ///     child node
        /// </summary>
        public ISyntaxPart ChildNode { get; }

        /// <summary>
        ///     format node as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => StringUtils.Invariant($"{DefiningNode} / {Data} / {ChildNode}");
    }



}
