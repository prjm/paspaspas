namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     visitor for syntax part elements
    /// </summary>
    /// <typeparam name="TParameterType">parameter</typeparam>
    public interface ISyntaxPartVisitor<TParameterType> {

        /// <summary>
        ///     visit a syntax part (before recursive descent)
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        /// <returns>bool if visiting should be continued</returns>
        bool BeginVisit(ISyntaxPart syntaxPart, TParameterType parameter);

        /// <summary>
        ///     visit a syntax part (after recursive descent)
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        /// <returns>bool if visiting should be continued</returns>
        bool EndVisit(ISyntaxPart syntaxPart, TParameterType parameter);

        /// <summary>
        ///     begin visiting a syntax cild
        /// </summary>
        /// <typeparam name="TVisitorType"></typeparam>
        /// <param name="parent"></param>
        /// <param name="visitorParameter"></param>
        /// <param name="child"></param>
        void BeginVisitChild<TVisitorType>(ISyntaxPart parent, TVisitorType visitorParameter, ISyntaxPart child);

        /// <summary>
        ///     end visiting a syntax chid
        /// </summary>
        /// <typeparam name="TVisitorType"></typeparam>
        /// <param name="parent"></param>
        /// <param name="visitorParameter"></param>
        /// <param name="child"></param>
        void EndVisitChild<TVisitorType>(ISyntaxPart parent, TVisitorType visitorParameter, ISyntaxPart child);
    }


}
