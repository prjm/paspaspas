﻿#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     set expression
    /// </summary>
    public class SetSectionSymbol : VariableLengthSyntaxTreeBase<SetSectionPartSymbol> {

        /// <summary>
        ///     create a new set section
        /// </summary>
        /// <param name="openBraces"></param>
        /// <param name="items"></param>
        /// <param name="closeBraces"></param>
        public SetSectionSymbol(Terminal openBraces, ImmutableArray<SetSectionPartSymbol> items, Terminal closeBraces) : base(items) {
            OpenBraces = openBraces;
            CloseBraces = closeBraces;
        }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal CloseBraces { get; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal OpenBraces { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenBraces, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseBraces, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => OpenBraces.GetSymbolLength() +
                ItemLength +
                CloseBraces.GetSymbolLength();


    }
}