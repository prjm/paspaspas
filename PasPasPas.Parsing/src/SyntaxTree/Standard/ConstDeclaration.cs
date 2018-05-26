using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant declaration
    /// </summary>
    public class ConstDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///     user defined attributes
        /// </summary>
        public SyntaxPartBase Attributes { get; set; }

        /// <summary>
        ///     additional hint for that constant
        /// </summary>
        public ISyntaxPart Hint { get; set; }

        /// <summary>
        ///     identifier
        /// </summary>
        public Identifier Identifier { get; set; }

        /// <summary>
        ///     type specification
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

        /// <summary>
        ///     expression
        /// </summary>
        public ConstantExpression Value { get; set; }

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
