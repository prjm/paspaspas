using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     label declaration
    /// </summary>
    public class Label : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new label
        /// </summary>
        /// <param name="name"></param>
        public Label(SyntaxPartBase name)
            => LabelName = name;

        /// <summary>
        ///     label name
        /// </summary>
        public SyntaxPartBase LabelName { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, LabelName, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => LabelName.GetSymbolLength();

    }
}