#nullable disable
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     switch to toggle assertions
    /// </summary>
    public class AssertSwitch : CompilerDirectiveBase {
        private readonly Terminal assert;
        private readonly Terminal mode;
        private readonly AssertionMode option;

        /// <summary>
        ///     create a new assert switch
        /// </summary>
        /// <param name="assert"></param>
        /// <param name="mode"></param>
        /// <param name="option"></param>
        public AssertSwitch(Terminal assert, Terminal mode, AssertionMode option) {
            this.assert = assert;
            this.mode = mode;
            this.option = option;
        }

        /// <summary>
        ///     assertion mode
        /// </summary>
        public AssertionMode Assertions
            => option;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, assert, visitor);
            AcceptPart(this, mode, visitor);
            visitor.EndVisit(this);
        }
    }
}
