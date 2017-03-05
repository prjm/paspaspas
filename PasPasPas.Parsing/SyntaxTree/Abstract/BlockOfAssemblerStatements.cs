using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     block of assembler statements
    /// </summary>
    public class BlockOfAssemblerStatements : StatementBase {

        /// <summary>
        ///     assembler statements
        /// </summary>
        public IList<AssemblerStatement> Statements { get; set; }
            = new List<AssemblerStatement>();

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (AssemblerStatement part in Parts)
                    yield return part;
            }
        }
    }
}
