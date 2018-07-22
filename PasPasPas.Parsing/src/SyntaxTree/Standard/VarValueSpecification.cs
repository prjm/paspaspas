using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     variable value
    /// </summary>
    public class VarValueSpecification : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new variable value specification
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="expression"></param>
        public VarValueSpecification(Terminal symbol, ConstantExpressionSymbol expression) {
            ValueSymbol = symbol;
            Value = expression;
        }

        /// <summary>
        ///     value symbol
        /// </summary>
        public Terminal ValueSymbol { get; }

        /// <summary>
        ///     variable value
        /// </summary>
        public ConstantExpressionSymbol Value { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ValueSymbol, visitor);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ValueSymbol.GetSymbolLength() + Value.GetSymbolLength();

    }
}