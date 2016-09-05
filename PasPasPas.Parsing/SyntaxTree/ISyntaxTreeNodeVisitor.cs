namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     visitor for syntax part elements
    /// </summary>
    /// <typeparam name="TParam">parameter</typeparam>
    public interface ISyntaxTreeNodeVisitor<TParam> {

        /// <summary>
        ///     visit a syntax part (before recursive descent)
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        /// <returns>bool if visiting should be continued</returns>
        bool BeginVisit(ISyntaxTreeNode syntaxPart, TParam parameter);

        /// <summary>
        ///     visit a syntax part (after recursive descent)
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        /// <returns>bool if visiting should be continued</returns>
        bool EndVisit(ISyntaxTreeNode syntaxPart, TParam parameter);

    }


}
