using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     block body
    /// </summary>
    public class BlockBodySymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new block body
        /// </summary>
        /// <param name="assemblerBlock"></param>
        /// <param name="body"></param>
        public BlockBodySymbol(AsmBlockSymbol assemblerBlock, CompoundStatementSymbol body) {
            AssemblerBlock = assemblerBlock;
            Body = body;
        }

        /// <summary>
        ///    assembler block
        /// </summary>
        public AsmBlockSymbol AssemblerBlock { get; }

        /// <summary>
        ///     block bode
        /// </summary>
        public CompoundStatementSymbol Body { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => AssemblerBlock.GetSymbolLength() + Body.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Body, visitor);
            AcceptPart(this, AssemblerBlock, visitor);
            visitor.EndVisit(this);
        }

    }
}