namespace PasPasPas.Parsing.SyntaxTree {

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
        ///     stop visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        /// <param name="parameter">parameter</param>
        public virtual bool EndVisit(ISyntaxPart syntaxPart, TParameterType parameter)
            => true;

    }
}
