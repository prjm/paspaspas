﻿#nullable disable
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     run only directive
    /// </summary>
    public class RunOnly : CompilerDirectiveBase {

        /// <summary>
        ///     create a new run only directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        public RunOnly(Terminal symbol, Terminal mode, RuntimePackageMode parsedMode) {
            Symbol = symbol;
            ModeSymbol = mode;
            Mode = parsedMode;
        }

        /// <summary>
        ///     switch mode
        /// </summary>
        public RuntimePackageMode Mode { get; }

        /// <summary>
        ///     mode symbol
        /// </summary>
        public Terminal ModeSymbol { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

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
