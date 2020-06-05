#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     noinclude directive
    /// </summary>
    public class NoInclude : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal name;

        /// <summary>
        ///     create a new noinclude directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="name"></param>
        public NoInclude(Terminal symbol, Terminal name) {
            this.symbol = symbol;
            this.name = name;
        }

        /// <summary>
        ///     unit name
        /// </summary>
        public string UnitName
            => name?.Token.Value;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, name, visitor);
            visitor.EndVisit(this);
        }


    }
}
