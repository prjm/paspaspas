using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     base class for parts of the abstract syntax tree
    /// </summary>
    public abstract class AbstractSyntaxPartBase : ISyntaxPart {

        /// <summary>
        ///     parent node
        /// </summary>
        public ISyntaxPart ParentItem { get; set; }

        /// <summary>
        ///     child parts
        /// </summary>
        public virtual IEnumerable<ISyntaxPart> Parts { get; }
            = new EmptyEnumerable<ISyntaxPart>();

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
        protected void AcceptParts<T>(T element, IStartVisitor startVisitor, IEndVisitor endVisitor) where T : class
            => SyntaxPartBase.AcceptParts<T>(element, Parts, startVisitor, endVisitor);

    }
}