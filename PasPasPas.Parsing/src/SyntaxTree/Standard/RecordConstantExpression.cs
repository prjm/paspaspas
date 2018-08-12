using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class for a record constant expression
    /// </summary>
    public class RecordConstantExpression : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new record constant expression
        /// </summary>
        /// <param name="name"></param>
        /// <param name="colon"></param>
        /// <param name="value"></param>
        /// <param name="semicolon"></param>
        public RecordConstantExpression(IdentifierSymbol name, Terminal colon, ConstantExpressionSymbol value, Terminal semicolon) {
            Name = name;
            ColonSymbol = colon;
            Value = value;
            Separator = semicolon;
        }

        /// <summary>
        ///     field name
        /// </summary>
        public IdentifierSymbol Name { get; }

        /// <summary>
        ///     field value
        /// </summary>
        public ConstantExpressionSymbol Value { get; }

        /// <summary>
        ///     separator
        /// </summary>
        public Terminal Separator { get; }

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
            AcceptPart(this, Name, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, Value, visitor);
            AcceptPart(this, Separator, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Name.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                Value.GetSymbolLength() +
                Separator.GetSymbolLength();


    }
}