using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using PasPasPas.Globals.Parsing;
using PasPasPasTests.Common;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     abstract syntax tree visitor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AstVisitor<T> : IStartEndVisitor {

        /// <summary>
        ///     create a new visitor
        /// </summary>
        /// <param name="tester"></param>
        public AstVisitor(TestFunction<T> tester)
            => SearchFunction = tester;

        /// <summary>
        ///     result of searching the abstract syntax tree
        /// </summary>
        [MaybeNull]
        public T Result { get; internal set; }

        /// <summary>
        ///     search function
        /// </summary>
        public TestFunction<T> SearchFunction { get; set; }

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
