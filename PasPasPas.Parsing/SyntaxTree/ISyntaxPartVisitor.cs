namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     visitor for syntax part elements
    /// </summary>
    /// <typeparam name="TParam">parameter</typeparam>
    public interface ISyntaxPartVisitor<TParam> {

        /// <summary>
        ///     visit a syntax part (before recursive descent)
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        /// <returns>bool if visiting should be continued</returns>
        void BeginVisit(ISyntaxPart syntaxPart, TParam parameter);

        /// <summary>
        ///     visit a syntax part (after recursive descent)
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        /// <returns>bool if visiting should be continued</returns>
        void EndVisit(ISyntaxPart syntaxPart, TParam parameter);

    }


}
