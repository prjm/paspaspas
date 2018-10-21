using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     enum size directive
    /// </summary>
    public class MinEnumSize : CompilerDirectiveBase {

        /// <summary>
        ///     minimum enum size
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="mode"></param>
        /// <param name="size"></param>
        public MinEnumSize(Terminal symbol, Terminal mode, EnumSize size) {
            Symbol = symbol;
            Mode = mode;
            Size = size;
        }

        /// <summary>
        ///     enum size
        /// </summary>
        public EnumSize Size { get; }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     mode
        /// </summary>
        public Terminal Mode { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, Mode, visitor);
            visitor.EndVisit(this);
        }


    }
}
