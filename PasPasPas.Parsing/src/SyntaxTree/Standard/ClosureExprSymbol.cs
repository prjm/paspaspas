using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     closure expression
    /// </summary>
    public class ClosureExpressionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new closure expression
        /// </summary>
        /// <param name="procSymbol"></param>
        /// <param name="parameters"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="returnType"></param>
        /// <param name="block"></param>
        public ClosureExpressionSymbol(Terminal procSymbol, FormalParameterSection parameters, Terminal colonSymbol, TypeSpecification returnType, BlockSymbol block) {
            ProcSymbol = procSymbol;
            Parameters = parameters;
            ColonSymbol = colonSymbol;
            ReturnType = returnType;
            Block = block;
        }

        /// <summary>
        /// block
        /// </summary>
        public BlockSymbol Block { get; }

        /// <summary>
        ///     closure kind
        /// </summary>
        public int Kind
            => ProcSymbol.GetSymbolKind();

        /// <summary>
        ///     closure parameters
        /// </summary>
        public FormalParameterSection Parameters { get; }

        /// <summary>
        ///     closure return type
        /// </summary>
        public TypeSpecification ReturnType { get; }

        /// <summary>
        ///     <c>function</c> or <c>procedure</c>
        /// </summary>
        public Terminal ProcSymbol { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

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
        public override int Length
            => ProcSymbol.GetSymbolLength() +
                Parameters.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                ReturnType.GetSymbolLength() +
                Block.GetSymbolLength();

    }
}