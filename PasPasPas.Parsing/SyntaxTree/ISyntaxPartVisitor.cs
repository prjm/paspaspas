namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     visitor for syntax part elements
    /// </summary>
    public interface ISyntaxPartVisitor {

        /// <summary>
        ///     visit a syntax part (before recursive descent)
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <returns>bool if visiting should be continued</returns>
        void BeginVisit(ISyntaxPart syntaxPart);

        /// <summary>
        ///     visit a syntax part (after recursive descent)
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <returns>bool if visiting should be continued</returns>
        void EndVisit(ISyntaxPart syntaxPart);

    }


}
