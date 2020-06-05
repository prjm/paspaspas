#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     compound statement
    /// </summary>
    public class CompoundStatementSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new compound statement
        /// </summary>
        /// <param name="asmBlockSymbol"></param>
        public CompoundStatementSymbol(AsmBlockSymbol asmBlockSymbol)
            => AssemblerBlock = asmBlockSymbol;

        /// <summary>
        ///     create a new compound statement
        /// </summary>
        /// <param name="beginSymbol"></param>
        /// <param name="statements"></param>
        /// <param name="endSymbol"></param>
        public CompoundStatementSymbol(Terminal beginSymbol, StatementList statements, Terminal endSymbol) {
            BeginSymbol = beginSymbol;
            Statements = statements;
            EndSymbol = endSymbol;
        }

        /// <summary>
        ///     assembler block
        /// </summary>
        public AsmBlockSymbol AssemblerBlock { get; }

        /// <summary>
        ///     statement list
        /// </summary>
        public StatementList Statements { get; }

        /// <summary>
        ///     begin symbol
        /// </summary>
        public Terminal BeginSymbol { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, AssemblerBlock, visitor);
            AcceptPart(this, BeginSymbol, visitor);
            AcceptPart(this, Statements, visitor);
            AcceptPart(this, EndSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => AssemblerBlock.GetSymbolLength() +
                BeginSymbol.GetSymbolLength() +
                Statements.GetSymbolLength() +
                EndSymbol.GetSymbolLength();
    }
}