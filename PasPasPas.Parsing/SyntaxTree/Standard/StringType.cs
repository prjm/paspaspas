using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     string type
    /// </summary>
    public class StringType : StandardSyntaxTreeBase {

        /// <summary>
        ///     code page
        /// </summary>
        public ConstantExpression CodePage { get; set; }

        /// <summary>
        ///     kind of the string
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     string length
        /// </summary>
        public Expression StringLength { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}