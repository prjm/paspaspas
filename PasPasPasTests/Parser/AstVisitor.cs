using PasPasPas.Parsing.SyntaxTree.Visitors;
using System;
using System.Collections.Generic;

namespace PasPasPasTests.Parser {

    public class AstVisitor<T> : IStartEndVisitor {

        public T Result { get; internal set; }
        public Func<object, T> SearchFunction { get; set; }

        public IStartEndVisitor AsVisitor()
            => this;

        public void EndVisit<TNodeType>(TNodeType node) { }

        public void StartVisit<TNodeType>(TNodeType node) {
            var data = SearchFunction(node);
            if (EqualityComparer<T>.Default.Equals(default, Result))
                Result = data;
        }

    }
}
