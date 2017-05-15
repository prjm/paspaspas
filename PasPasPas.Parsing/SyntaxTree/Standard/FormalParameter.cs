using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formal parameter
    /// </summary>
    public class FormalParameter : StandardSyntaxTreeBase {

        /// <summary>
        ///     parameter attributes
        /// </summary>
        public UserAttributes Attributes { get; set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public Identifier ParameterName { get; set; }

        /// <summary>
        ///     parameter type (var, const, out)
        /// </summary>
        public int ParameterType { get; set; }
            = TokenKind.Undefined;

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