﻿#nullable disable
using PasPasPas.Globals.Parsing;

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
        /// <param name="comma"></param>
        public EnumValueSymbol(IdentifierSymbol enumName, Terminal equals, ExpressionSymbol value, Terminal comma) {
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
        public IdentifierSymbol EnumName { get; }

        /// <summary>
        ///     value
        /// </summary>
        public ExpressionSymbol Value { get; }

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