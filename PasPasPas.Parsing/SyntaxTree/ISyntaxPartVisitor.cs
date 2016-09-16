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

    }


}
