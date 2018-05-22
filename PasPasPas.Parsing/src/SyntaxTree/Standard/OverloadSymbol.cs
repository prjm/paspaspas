using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     overload directive
    /// </summary>
    public class OverloadSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public object Length
            => Directive.Length + Semicolon.Length;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Directive, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

    }
}
