using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     if-opt directive
    /// </summary>
    public class IfOpt : CompilerDirectiveBase {

        /// <summary>
        ///     switch kind
        /// </summary>
        /// <param name="ifOpt"></param>
        /// <param name="switchKind"></param>
        /// <param name="mode"></param>
        /// <param name="info"></param>
        public IfOpt(Terminal ifOpt, Terminal switchKind, Terminal mode, SwitchInfo info) {
            IfOptSymbol = ifOpt;
            SwitchKindSymbol = switchKind;
            Mode = mode;
            SwitchInfo = info;
        }

        /// <summary>
        ///     required kind
        /// </summary>
        public string SwitchKind
            => SwitchKindSymbol?.Value;

        /// <summary>
        ///     required state
        /// </summary>
        public SwitchInfo SwitchState
            => CompilerDirectiveParser.GetSwitchInfo(Mode.GetSymbolKind());

        /// <summary>
        ///     if-opt keyword
        /// </summary>
        public Terminal IfOptSymbol { get; }

        /// <summary>
        ///     switch kind symbol
        /// </summary>
        public Terminal SwitchKindSymbol { get; }
        /// <summary>
        ///     switch mode
        /// </summary>
        public Terminal Mode { get; }

        /// <summary>
        ///     switch info
        /// </summary>
        public SwitchInfo SwitchInfo { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, IfOptSymbol, visitor);
            AcceptPart(this, SwitchKindSymbol, visitor);
            AcceptPart(this, Mode, visitor);
            visitor.EndVisit(this);
        }
    }
}
