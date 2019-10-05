using System;
using System.Collections.Generic;
using PasPasPas.Globals.Parsing;

namespace PasPasPasTests.Parser {

    /// <summary>
    ///     abstract syntax tree visitor
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AstVisitor<T> : IStartEndVisitor {

        public AstVisitor() { }
        public AstVisitor(Func<object, T> sf)
            => SearchFunction = sf;

        public T Result { get; internal set; }
        public Func<object, T> SearchFunction { get; set; }

        public IStartEndVisitor AsVisitor()
            => this;

        public void EndVisit<TNodeType>(TNodeType element) { }

        public void StartVisit<TNodeType>(TNodeType element) {
            var data = SearchFunction(element);
            if (EqualityComparer<T>.Default.Equals(default, Result))
                Result = data;
        }

    }
}
