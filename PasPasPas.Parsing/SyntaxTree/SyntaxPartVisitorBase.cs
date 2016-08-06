using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for visitors
    /// </summary>
    /// <typeparam name="TParam">parameter type</typeparam>
    public class SyntaxPartVisitorBase<TParam> : ISyntaxPartVisitor<TParam> {

        /// <summary>
        ///     start visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        public virtual void BeginVisit(ISyntaxPart syntaxPart, TParam parameter) {
            dynamic element = syntaxPart;
            BeginVisitElement(element, parameter);
        }

        /// <summary>
        ///     start visiting a syntax part
        /// </summary>
        /// <param name="element">part to visit</param>
        /// <param name="parameter">parameter</param>
        protected virtual void BeginVisitElement(ISyntaxPart element, TParam parameter) { }

        /// <summary>
        ///     stop visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        public virtual void EndVisit(ISyntaxPart syntaxPart, TParam parameter) {
            dynamic element = syntaxPart;
            EndVisitElement(element, parameter);
        }

        /// <summary>
        ///     end visiting a syntax part
        /// </summary>
        /// <param name="element">part to visit</param>
        /// <param name="parameter">parameter</param>
        protected virtual void EndVisitElement(ISyntaxPart element, TParam parameter) { }

    }
}
