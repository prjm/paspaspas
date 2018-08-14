using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     label declaration
    /// </summary>
    public class LabelSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new label
        /// </summary>
        /// <param name="name"></param>
        /// <param name="comma"></param>
        public LabelSymbol(SyntaxPartBase name, Terminal comma) {
            LabelName = name;
            Comma = comma;
        }

        /// <summary>
        ///     label name
        /// </summary>
        public SyntaxPartBase LabelName { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LabelName, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => LabelName.GetSymbolLength() + Comma.GetSymbolLength();

    }
}