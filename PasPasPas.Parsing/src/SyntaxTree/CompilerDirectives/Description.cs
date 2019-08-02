using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     description directive
    /// </summary>
    public class Description : CompilerDirectiveBase {

        /// <summary>
        ///     create a new description directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="value"></param>
        /// <param name="description"></param>
        public Description(Terminal symbol, Terminal value, string description) {
            Symbol = symbol;
            Value = value;
            DescriptionValue = description;
        }

        /// <summary>
        ///     description value
        /// </summary>
        public string DescriptionValue { get; }

        /// <summary>
        ///     description value
        /// </summary>
        public Terminal Value { get; }

        /// <summary>
        ///     description symbol
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
