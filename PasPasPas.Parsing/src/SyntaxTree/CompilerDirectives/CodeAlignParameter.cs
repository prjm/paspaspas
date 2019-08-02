using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     code align parameter
    /// </summary>
    public class CodeAlignParameter : CompilerDirectiveBase {
        private readonly Terminal directive;
        private readonly Terminal alignValue;

        /// <summary>
        ///     create  new code align directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="value"></param>
        /// <param name="codeAlign"></param>
        public CodeAlignParameter(Terminal symbol, Terminal value, CodeAlignment codeAlign) {
            directive = symbol;
            alignValue = value;
            CodeAlign = codeAlign;
        }

        /// <summary>
        ///     code align mode
        /// </summary>
        public CodeAlignment CodeAlign { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, directive, visitor);
            AcceptPart(this, alignValue, visitor);
            visitor.EndVisit(this);
        }
    }
}