﻿namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for visitors
    /// </summary>
    /// <typeparam name="TParameterType">parameter type</typeparam>
    public class SyntaxPartVisitorBase<TParameterType> : ISyntaxPartVisitor<TParameterType> {

        /// <summary>
        ///     start visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        public virtual bool BeginVisit(ISyntaxPart syntaxPart, TParameterType parameter)
            => true;

        /// <summary>
        ///     begin visting a child
        /// </summary>
        /// <typeparam name="TVisitorType"></typeparam>
        /// <param name="parent"></param>
        /// <param name="visitorParameter"></param>
        /// <param name="child"></param>
        public void BeginVisitChild<TVisitorType>(ISyntaxPart parent, TVisitorType visitorParameter, ISyntaxPart child) {
        }

        /// <summary>
        ///     stop visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        public virtual bool EndVisit(ISyntaxPart syntaxPart, TParameterType parameter)
            => true;

        /// <summary>
        ///     end visiting a child
        /// </summary>
        /// <typeparam name="TVisitorType"></typeparam>
        /// <param name="parent"></param>
        /// <param name="visitorParameter"></param>
        /// <param name="child"></param>
        public virtual void EndVisitChild<TVisitorType>(ISyntaxPart parent, TVisitorType visitorParameter, ISyntaxPart child) {
        }
    }
}
