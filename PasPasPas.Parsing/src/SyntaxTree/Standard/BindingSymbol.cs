using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     binding directive
    /// </summary>
    public class BindingSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     directive kind
        /// </summary>
        public int Kind
            => Directive.Kind;

        /// <summary>
        ///     message expression
        /// </summary>
        public SyntaxPartBase MessageExpression { get; set; }

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
            => Directive.Length + MessageExpression.Length + Semicolon.Length;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Directive, visitor);
            AcceptPart(this, MessageExpression, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

    }
}
