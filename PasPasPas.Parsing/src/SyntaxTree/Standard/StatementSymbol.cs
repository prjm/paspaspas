﻿#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     single statement
    /// </summary>
    public class StatementSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new statement
        /// </summary>
        /// <param name="label"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="part"></param>
        /// <param name="semicolon"></param>
        public StatementSymbol(LabelSymbol label, Terminal colonSymbol, StatementPart part, Terminal semicolon) {
            Label = label;
            ColonSymbol = colonSymbol;
            Part = part;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     label
        /// </summary>
        public LabelSymbol Label { get; }

        /// <summary>
        ///     statement part
        /// </summary>
        public StatementPart Part { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Label, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, Part, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Label.GetSymbolLength() +
               ColonSymbol.GetSymbolLength() +
               Part.GetSymbolLength() +
               Semicolon.GetSymbolLength();

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }
    }
}