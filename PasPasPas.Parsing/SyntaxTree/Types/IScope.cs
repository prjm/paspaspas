namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     scope
    /// </summary>
    public interface IScope {

        /// <summary>
        ///     open another scope
        /// </summary>
        /// <param name="completeName"></param>
        /// <param name="scope"></param>
        void Open(string completeName, IScope scope);
    }
}