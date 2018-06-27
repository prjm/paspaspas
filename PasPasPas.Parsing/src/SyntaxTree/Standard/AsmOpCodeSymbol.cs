using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly op code
    /// </summary>
    public class AsmOpCodeSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     generate a new opcode symbol
        /// </summary>
        /// <param name="opCode"></param>
        public AsmOpCodeSymbol(Identifier opCode)
            => OpCode = opCode;

        /// <summary>
        ///     op code
        /// </summary>
        public Identifier OpCode { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpCode, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpCode.GetSymbolLength();

    }
}
