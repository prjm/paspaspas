using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     enumeration value
    /// </summary>
    public class EnumValueSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new enum value
        /// </summary>
        /// <param name="enumName"></param>
        /// <param name="equals"></param>
        /// <param name="value"></param>
        public EnumValueSymbol(Identifier enumName, Terminal equals, Expression value, Terminal comma) {
            EnumName = enumName;
            EqualsSymbol = equals;
            Value = value;
            Comma = comma;
        }

        /// <summary>
        ///     equals
        /// </summary>
        public Terminal EqualsSymbol { get; }

        /// <summary>
        ///     name
        /// </summary>
        public Identifier EnumName { get; }

        /// <summary>
        ///     value
        /// </summary>
        public Expression Value { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, EnumName, visitor);
            AcceptPart(this, EqualsSymbol, visitor);
            AcceptPart(this, Value, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => EnumName.GetSymbolLength() +
                EqualsSymbol.GetSymbolLength() +
                Value.GetSymbolLength() +
                Comma.GetSymbolLength();

    }
}