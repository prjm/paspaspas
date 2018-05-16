using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     Quoted string
    /// </summary>
    public class QuotedString : StandardSyntaxTreeBase {

        /// <summary>
        ///     unquoted value
        /// </summary>
        public object UnquotedValue { get; set; }
            = null;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}