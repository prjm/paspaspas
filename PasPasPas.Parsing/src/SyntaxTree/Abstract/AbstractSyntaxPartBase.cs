using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Infrastructure.Utils;
using System;

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
        public virtual IEnumerable<ISyntaxPart> Parts
            => Array.Empty<ISyntaxPart>();

        /// <summary>
        ///     accept visitors
        /// </summary>
        public abstract void Accept(IStartEndVisitor visitor);

        /// <summary>
        ///     accept parts
        /// </summary>
        /// <param name="element">element to visit</param>
        /// <param name="visitor">visitor to use</param>
        protected void AcceptParts<T>(T element, IStartEndVisitor visitor) {
            var childVisitor = visitor as IChildVisitor;
            foreach (var part in Parts) {
                childVisitor?.StartVisitChild<T>(element, part);
                part.Accept(visitor);
                childVisitor?.EndVisitChild<T>(element, part);
            }
        }

    }
}