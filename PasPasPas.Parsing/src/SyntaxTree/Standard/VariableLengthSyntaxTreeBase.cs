﻿#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable sized syntax part node
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class VariableLengthSyntaxTreeBase<T> : StandardSyntaxTreeBase where T : ISyntaxPart {

        /// <summary>
        ///     items
        /// </summary>
        public ImmutableArray<T> Items { get; }

        /// <summary>
        ///     initialize a this syntax part
        /// </summary>
        /// <param name="items"></param>
        protected VariableLengthSyntaxTreeBase(ImmutableArray<T> items)
            => Items = items;

        /// <summary>
        ///     count the length of the items
        /// </summary>
        protected int ItemLength {
            get {
                var result = 0;
                for (var i = 0; Items != null && i < Items.Length; i++)
                    result += Items[i].Length;

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
            for (var i = 0; Items != null && i < Items.Length; i++) {
                var child = Items[i];
                childVisitor?.StartVisitChild<TListType>(item, child);
                child.Accept(visitor);
                childVisitor?.EndVisitChild<TListType>(item, child);
            }
        }

    }
}
