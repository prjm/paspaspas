using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPasTests.Parser {

    public class AstVisitor<T> : IStartEndVisitor {

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
