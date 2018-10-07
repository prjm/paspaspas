using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unsafe directive
    /// </summary>
    public class UnsafeDirectiveSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new unsafe directive
        /// </summary>
        /// <param name="directive"></param>
        /// <param name="semicolon"></param>
        public UnsafeDirectiveSymbol(Terminal directive, Terminal semicolon) {
            Directive = directive;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Directive.GetSymbolLength() + Semicolon.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor to use</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Directive, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }



    }
}
