namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     block body
    /// </summary>
    public class BlockBody : SyntaxPartBase {

        /// <summary>
        ///    assembler block
        /// </summary>
        public Token AssemblerBlock { get; set; }

        /// <summary>
        ///     block bode
        /// </summary>
        public CompoundStatement Body { get; set; }

    }
}