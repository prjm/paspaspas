using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable sized syntax part node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class VariableLengthSyntaxTreeBase<T> : StandardSyntaxTreeBase where T : ISyntaxPart {

        private IList<T> items;

        /// <summary>
        ///     add an item
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(T item) {
            if (items == null)
                items = new List<T>();
            items.Add(item);
        }

        /// <summary>
        ///     count the length of the items
        /// </summary>
        public int Length {
            get {
                var result = 0;
                for (var i = 0; items != null && i < items.Count; i++)
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
            for (var i = 0; items != null && i < items.Count; i++) {
                var child = items[i];
                childVisitor?.StartVisitChild<TListType>(item, child);
                child.Accept(visitor);
                childVisitor?.EndVisitChild<TListType>(item, child);
            }
        }

    }
}
