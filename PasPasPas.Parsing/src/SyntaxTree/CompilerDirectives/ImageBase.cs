using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     image base address directive
    /// </summary>
    public class ImageBase : CompilerDirectiveBase {

        /// <summary>
        ///     create a new image base directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="value"></param>
        /// <param name="baseValue"></param>
        public ImageBase(Terminal symbol, Terminal value, ulong baseValue) {
            Symbol = symbol;
            Value = value;
            BaseValue = baseValue;
        }

        /// <summary>
        ///     image base address
        /// </summary>
        public ulong BaseValue { get; }

        /// <summary>
        ///     value
        /// </summary>
        public Terminal Value { get; }

        /// <summary>
        ///     directive symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }

    }
}
