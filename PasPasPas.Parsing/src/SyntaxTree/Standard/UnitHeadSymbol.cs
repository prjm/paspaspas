using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     unit head
    /// </summary>
    public class UnitHeadSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     hinting directives
        /// </summary>
        public ISyntaxPart Hint { get; set; }

        /// <summary>
        ///     unit name
        /// </summary>
        public NamespaceName UnitName { get; set; }

        /// <summary>
        ///     unit symbol
        /// </summary>
        public Terminal Unit { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     symbol length
        /// </summary>
        public int Length
            => Unit.Length + UnitName.Length + Hint.Length + Semicolon.Length;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Unit, visitor);
            AcceptPart(this, UnitName, visitor);
            AcceptPart(this, Hint, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }


    }
}