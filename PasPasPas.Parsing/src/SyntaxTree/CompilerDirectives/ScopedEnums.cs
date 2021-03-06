﻿#nullable disable
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     scoped enum flags
    /// </summary>
    public class ScopedEnums : CompilerDirectiveBase {

        /// <summary>
        ///     create a new scoped enum directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        public ScopedEnums(Terminal symbol, Terminal mode, RequireScopedEnumMode parsedMode) {
            Symbol = symbol;
            ModeSymbol = mode;
            Mode = parsedMode;
        }

        /// <summary>
        ///     switch mode
        /// </summary>
        public RequireScopedEnumMode Mode { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     mode symbol
        /// </summary>
        public Terminal ModeSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, ModeSymbol, visitor);
            visitor.EndVisit(this);
        }


    }
}
