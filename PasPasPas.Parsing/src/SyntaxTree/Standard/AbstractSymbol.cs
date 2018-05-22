using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {


    /// <summary>
    ///     abstract directive
    /// </summary>
    public class AbstractSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     directive
        /// </summary>
        public Terminal Directive { get; set; }

        /// <summary>
        ///     final or abstract
        /// </summary>
        public int Kind
            => Directive.Kind;

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
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
