namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     block body
    /// </summary>
    public class BlockBody : SyntaxPartBase {

        /// <summary>
        ///    assembler block
        /// </summary>
        public Terminal AssemblerBlock { get; set; }

        /// <summary>
        ///     block bode
        /// </summary>
        public CompoundStatement Body { get; set; }

    }
}