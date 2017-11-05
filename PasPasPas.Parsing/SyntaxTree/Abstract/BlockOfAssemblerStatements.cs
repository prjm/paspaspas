using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     block of assembler statements
    /// </summary>
    public class BlockOfAssemblerStatements : StatementBase {

        /// <summary>
        ///     assembler statements
        /// </summary>
        public ISyntaxPartList<AssemblerStatement> Statements { get; }

        /// <summary>
        ///     create a new block of assembler statements
        /// </summary>
        public BlockOfAssemblerStatements()
            => Statements = new SyntaxPartCollection<AssemblerStatement>(this);

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var part in Statements)
                    yield return part;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
