using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     stack mem size directive
    /// </summary>
    public class StackMemorySize : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal size1;
        private readonly Terminal comma;
        private readonly Terminal size2;

        public StackMemorySize(Terminal symbol, Terminal size1, Terminal comma, Terminal size2, ulong minStackSize, ulong maxStackSize) {
            this.symbol = symbol;
            this.size1 = size1;
            this.comma = comma;
            this.size2 = size2;
            MinStackSize = minStackSize;
            MaxStackSize = maxStackSize;
        }

        /// <summary>
        ///     maximum stack size
        /// </summary>
        public ulong? MaxStackSize { get; }

        /// <summary>
        ///     minimum stack size
        /// </summary>
        public ulong? MinStackSize { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, size1, visitor);
            AcceptPart(this, comma, visitor);
            AcceptPart(this, size2, visitor);
            visitor.EndVisit(this);
        }


    }
}
