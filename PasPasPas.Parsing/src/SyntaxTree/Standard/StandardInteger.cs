using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant integer
    /// </summary>
    public class StandardInteger : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new integer value
        /// </summary>
        /// <param name="value"></param>
        public StandardInteger(Terminal value)
            => Value = value;

        /// <summary>
        ///     integer value
        /// </summary>
        public Terminal Value { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Value.GetSymbolLength();

    }
}
