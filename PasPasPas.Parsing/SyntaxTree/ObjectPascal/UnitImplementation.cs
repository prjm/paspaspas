namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     unit implementation part
    /// </summary>
    public class UnitImplementation : SyntaxPartBase {

        /// <summary>
        ///     declaration section
        /// </summary>
        public Declarations DeclarationSections { get; set; }

        /// <summary>
        ///     uses clause
        /// </summary>
        public UsesClause UsesClause { get; set; }

    }
}