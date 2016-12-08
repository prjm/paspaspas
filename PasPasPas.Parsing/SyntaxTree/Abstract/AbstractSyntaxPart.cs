using PasPasPas.Parsing.Parser;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for parts of the abstract syntax tree
    /// </summary>
    public abstract class AbstractSyntaxPart : ISyntaxPart {

        /// <summary>
        ///     parent node
        /// </summary>
        public ISyntaxPart Parent { get; set; }

        /// <summary>
        ///     child parts
        /// </summary>
        public virtual IEnumerable<ISyntaxPart> Parts
            => EmptyCollection<ISyntaxPart>.ReadOnlyInstance;


    }
}