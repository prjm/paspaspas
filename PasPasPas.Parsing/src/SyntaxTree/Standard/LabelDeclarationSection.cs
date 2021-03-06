﻿#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     label section
    /// </summary>
    public class LabelDeclarationSection : VariableLengthSyntaxTreeBase<LabelSymbol> {

        /// <summary>
        ///     create a new label declaration section
        /// </summary>
        /// <param name="labelSymbol"></param>
        /// <param name="items"></param>
        /// <param name="semicolon"></param>
        public LabelDeclarationSection(Terminal labelSymbol, ImmutableArray<LabelSymbol> items, Terminal semicolon) : base(items) {
            Label = labelSymbol;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     label symbo
        /// </summary>
        public Terminal Label { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Label, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Label.GetSymbolLength() + ItemLength + Semicolon.GetSymbolLength();
    }
}
