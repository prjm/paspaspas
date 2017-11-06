using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessor for disp interfaces
    /// </summary>
    public class ClassPropertyDispInterface : StandardSyntaxTreeBase {

        /// <summary>
        ///     Disp id directive
        /// </summary>
        public DispIdDirective DispId { get; set; }

        /// <summary>
        ///     readonly
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        ///    write only
        /// </summary>
        public bool WriteOnly { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}