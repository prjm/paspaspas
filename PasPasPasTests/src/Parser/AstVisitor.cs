using System;
using System.Collections.Generic;
using PasPasPas.Globals.Parsing;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     abstract syntax tree visitor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AstVisitor<T> : IStartEndVisitor {

        /// <summary>
        ///     create a new visitor
        /// </summary>
        public AstVisitor() { }

        /// <summary>
        ///     create a new visitor
        /// </summary>
        /// <param name="sf"></param>
        public AstVisitor(Func<object, T> sf)
            => SearchFunction = sf;

        /// <summary>
        ///     result of searching the abstract syntax tree
        /// </summary>
        public T Result { get; internal set; }

        /// <summary>
        ///     search function
        /// </summary>
        public Func<object, T> SearchFunction { get; set; }

        /// <summary>
        ///     implementation
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => this;

        /// <summary>
        ///     end visiting a node
        /// </summary>
        /// <typeparam name="TNodeType"></typeparam>
        /// <param name="element"></param>
        public void EndVisit<TNodeType>(TNodeType element) { }

        /// <summary>
        ///     start visiting a node
        /// </summary>
        /// <typeparam name="TNodeType"></typeparam>
        /// <param name="element"></param>
        public void StartVisit<TNodeType>(TNodeType element) {
            var data = SearchFunction(element);
            if (EqualityComparer<T>.Default.Equals(default, Result))
                Result = data;
        }

    }
}
