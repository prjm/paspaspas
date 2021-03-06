﻿#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple pascal identifier
    /// </summary>
    public class IdentifierSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     identifier symbol
        /// </summary>
        /// <param name="identifierSymbol"></param>
        public IdentifierSymbol(Terminal identifierSymbol)
            => Symbol = identifierSymbol;

        /// <summary>
        /// symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     identifier value
        /// </summary>
        public string Value
            => Symbol.Value;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Symbol.GetSymbolLength();

    }
}
