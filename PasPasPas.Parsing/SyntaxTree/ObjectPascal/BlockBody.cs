namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     block body
    /// </summary>
    public class BlockBody : SyntaxPartBase {

        /// <summary>
        ///    assembler block
        /// </summary>
        public AsmStatement AssemblerBlock { get; set; }

        /// <summary>
        ///     block bode
        /// </summary>
        public CompoundStatement Body { get; set; }

    }
}