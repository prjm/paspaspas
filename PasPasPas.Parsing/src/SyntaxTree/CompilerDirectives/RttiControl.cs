using System.Collections.Immutable;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     rtti control directive
    /// </summary>
    public class RttiControl : CompilerDirectiveBase {

        /// <summary>
        ///     create a new rtti directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="parsedMode"></param>
        /// <param name="list"></param>
        public RttiControl(Terminal symbol, Terminal mode, RttiGenerationMode parsedMode, ImmutableArray<RttiControlSpecifier> list) {
            Symbol = symbol;
            ModeSymbol = mode;
            Mode = parsedMode;
            Specifiers = list;
        }

        /// <summary>
        ///     fields visibility
        /// </summary>
        public RttiForVisibility Fields { get; }

        /// <summary>
        ///     methods visibility
        /// </summary>
        public RttiForVisibility Methods { get; }

        /// <summary>
        ///     properties visibility
        /// </summary>
        public RttiForVisibility Properties { get; }


        /// <summary>
        ///     selected rtti mode
        /// </summary>
        public RttiGenerationMode Mode { get; }

        /// <summary>
        ///     specifiers
        /// </summary>
        public ImmutableArray<RttiControlSpecifier> Specifiers { get; }

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

            foreach (var item in Specifiers)
                AcceptPart(this, item, visitor);

            visitor.EndVisit(this);
        }


    }
}
