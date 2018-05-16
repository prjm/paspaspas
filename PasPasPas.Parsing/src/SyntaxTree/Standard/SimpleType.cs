using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple type definiion
    /// </summary>
    public class SimpleType : StandardSyntaxTreeBase {

        /// <summary>
        ///     enumeration
        /// </summary>
        public EnumTypeDefinition EnumType { get; set; }

        /// <summary>
        ///     <c>true</c> for a new type definition
        /// </summary>
        public bool NewType { get; internal set; }

        /// <summary>
        ///     subrange start
        /// </summary>
        public ConstantExpression SubrangeEnd { get; set; }

        /// <summary>
        ///     subrange end
        /// </summary>
        public ConstantExpression SubrangeStart { get; set; }

        /// <summary>
        ///     <c>type of</c> declaration
        /// </summary>
        public bool TypeOf { get; set; }

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