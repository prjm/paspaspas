using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     closure expression
    /// </summary>
    public class ClosureExpressionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        /// block
        /// </summary>
        public BlockSymbol Block { get; set; }

        /// <summary>
        ///     closure kind
        /// </summary>
        public int Kind
            => ProcSymbol.Kind;

        /// <summary>
        ///     closure parameters
        /// </summary>
        public SyntaxPartBase Parameters { get; set; }

        /// <summary>
        ///     closure return type
        /// </summary>
        public SyntaxPartBase ReturnType { get; set; }

        /// <summary>
        ///     <c>function</c> or <c>procedure</c>
        /// </summary>
        public Terminal ProcSymbol { get; set; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ProcSymbol, visitor);
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, ReturnType, visitor);
            AcceptPart(this, Block, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => ProcSymbol.Length + Parameters.Length + ColonSymbol.Length + ReturnType.Length + Block.Length;

    }
}