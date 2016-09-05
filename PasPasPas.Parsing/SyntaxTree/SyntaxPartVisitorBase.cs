namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for visitors
    /// </summary>
    /// <typeparam name="TParam">parameter type</typeparam>
    public class SyntaxPartVisitorBase<TParam> : ISyntaxTreeNodeVisitor<TParam> {

        /// <summary>
        ///     start visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        public virtual bool BeginVisit(ISyntaxTreeNode syntaxPart, TParam parameter)
            => true;

        /// <summary>
        ///     stop visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        public virtual bool EndVisit(ISyntaxTreeNode syntaxPart, TParam parameter)
            => true;

    }
}
