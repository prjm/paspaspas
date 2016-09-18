namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface part of a unit
    /// </summary>
    public class UnitInterface : SyntaxPartBase {

        /// <summary>
        ///     interface declaration
        /// </summary>
        public InterfaceDeclaration InterfaceDeclaration { get; set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesClause UsesClause { get; set; }

    }
}