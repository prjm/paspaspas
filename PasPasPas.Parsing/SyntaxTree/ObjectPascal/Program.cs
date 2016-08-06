using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     syntax tree for a program
    /// </summary>
    public class Program : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public Program(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     main block
        /// </summary>
        public Block MainBlock { get; internal set; }

        /// <summary>
        ///     program header
        /// </summary>
        public ProgramHead ProgramHead { get; internal set; }

        /// <summary>
        ///     uses list
        /// </summary>
        public UsesFileClause Uses { get; internal set; }

        /// <summary>
        ///     print the program
        /// </summary>        
        /// <param name="result">output builder</param>
        public override void ToFormatter(PascalFormatter result) {
            if (ProgramHead != null)
                ProgramHead.ToFormatter(result);

            if (Uses != null && Uses.Files != null && Uses.Files.Count > 0) {
                result.NewLine();
                Uses.ToFormatter(result);
                result.NewLine();
            }

            result.NewLine();
            MainBlock.ToFormatter(result);
            result.NewLine();
            result.Punct(".");
        }

    }
}
