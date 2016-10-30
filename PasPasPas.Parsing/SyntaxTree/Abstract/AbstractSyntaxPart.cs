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


        /// <summary>
        ///     accept syntax tree visitors
        /// </summary>
        /// <typeparam name="TVisitorType"></typeparam>
        /// <param name="visitor"></param>
        /// <param name="visitorParameter"></param>
        /// <returns></returns>
        public bool Accept<TVisitorType>(ISyntaxPartVisitor<TVisitorType> visitor, TVisitorType visitorParameter) {
            if (!visitor.BeginVisit(this, visitorParameter))
                return false;

            var result = true;

            foreach (var part in Parts) {
                visitor.BeginVisitChild(this, visitorParameter, part);
                result = result && part.Accept(visitor, visitorParameter);
                visitor.EndVisitChild(this, visitorParameter, part);
            }

            if (!visitor.EndVisit(this, visitorParameter))
                return false;

            return result;
        }

    }
}