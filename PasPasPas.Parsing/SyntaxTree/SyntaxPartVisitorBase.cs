namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     base class for visitors
    /// </summary>
    public class SyntaxPartVisitorBase : ISyntaxPartVisitor {


        /// <summary>
        ///     start visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        public virtual void BeginVisit(ISyntaxPart syntaxPart) { }

        /// <summary>
        ///     stop visiting a syntax part
        /// </summary>
        /// <param name="syntaxPart">part to visit</param>
        public virtual void EndVisit(ISyntaxPart syntaxPart) { }

    }
}
