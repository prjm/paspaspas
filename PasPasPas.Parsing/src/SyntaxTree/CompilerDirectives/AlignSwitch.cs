using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     syntax tree element to change alignment
    /// </summary>
    public class AlignSwitch : CompilerDirectiveBase {

        /// <summary>
        ///     create a new align switch
        /// </summary>
        /// <param name="alignSymbol"></param>
        /// <param name="alignSwitch"></param>
        /// <param name="alignValue"></param>
        public AlignSwitch(Terminal alignSymbol, Terminal alignSwitch, Alignment alignValue) {
            AlignSymbol = alignSymbol;
            AlignSwitchSymbol = alignSwitch;
            AlignValue = alignValue;
        }

        /// <summary>
        ///     new align setting
        /// </summary>
        public Alignment AlignValue { get; }

        /// <summary>
        ///     align value
        /// </summary>
        public Terminal AlignSymbol { get; }

        /// <summary>
        ///     align switch symbol
        /// </summary>
        public Terminal AlignSwitchSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, AlignSymbol, visitor);
            AcceptPart(this, AlignSwitchSymbol, visitor);
            visitor.EndVisit(this);
        }

    }
}
