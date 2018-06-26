using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable sized syntax part node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class VariableLengthSyntaxTreeBase<T> : StandardSyntaxTreeBase where T : ISyntaxPart {

        private readonly ImmutableArray<T> items;

        /// <summary>
        ///     items
        /// </summary>
        public ImmutableArray<T> Items
            => items;

        /// <summary>
        ///     initialize a this items
        /// </summary>
        /// <param name="items"></param>
        protected VariableLengthSyntaxTreeBase(ImmutableArray<T> items)
            => this.items = items;

        /// <summary>
        ///     count the length of the items
        /// </summary>
        protected int ItemLength {
            get {
                var result = 0;
                for (var i = 0; items != null && i < items.Length; i++)
                    result += items[i].Length;

                return result;
            }
        }

        /// <summary>
        ///     accept all parts
        /// </summary>
        /// <typeparam name="TListType"></typeparam>
        /// <param name="item"></param>
        /// <param name="visitor"></param>
        protected void AcceptPart<TListType>(TListType item, IStartEndVisitor visitor) {
            var childVisitor = visitor as IChildVisitor;
            for (var i = 0; items != null && i < items.Length; i++) {
                var child = items[i];
                childVisitor?.StartVisitChild<TListType>(item, child);
                child.Accept(visitor);
                childVisitor?.EndVisitChild<TListType>(item, child);
            }
        }

    }
}
