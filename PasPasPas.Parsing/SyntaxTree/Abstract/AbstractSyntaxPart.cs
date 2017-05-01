using PasPasPas.Parsing.Parser;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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

        /// <summary>
        ///     accept visitors
        /// </summary>
        /// <param name="startVisitor"></param>
        /// <param name="endVisitor"></param>
        public abstract void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor);

        /// <summary>
        ///     accept parts
        /// </summary>
        /// <param name="startVisitor"></param>
        /// <param name="endVisitor"></param>
        protected void AcceptParts(IStartVisitor startVisitor, IEndVisitor endVisitor)
            => SyntaxPartBase.AcceptParts(Parts, startVisitor, endVisitor);



    }
}