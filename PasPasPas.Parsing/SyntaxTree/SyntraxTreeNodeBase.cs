using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for syntrax tree nodesa
    /// </summary>
    public abstract class SyntraxTreeNodeBase : ISyntaxTreeNode {

        /// <summary>
        ///     parent node, can be <c>null</c>
        /// </summary>
        public ISyntaxTreeNode Parent { get; set; }

        /// <summary>
        ///     syntax tree parts
        /// </summary>
        public abstract IEnumerable<ISyntaxTreeNode> Parts { get; }

        /// <summary>
        ///     accept this visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        /// <param name="param">parameter</param>
        /// <typeparam name="T">parameter type</typeparam>
        public virtual bool Accept<T>(ISyntaxTreeNodeVisitor<T> visitor, T param) {
            if (!visitor.BeginVisit(this, param))
                return false;

            var result = true;

            foreach (var part in Parts)
                result = result && part.Accept(visitor, param);

            if (!visitor.EndVisit(this, param))
                return false;

            return result;
        }
    }
}
