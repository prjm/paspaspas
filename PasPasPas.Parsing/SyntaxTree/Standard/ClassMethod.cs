using System;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class method definition
    /// </summary>
    public class ClassMethod : StandardSyntaxTreeBase {

        /// <summary>
        ///     directviea
        /// </summary>
        public MethodDirectives Directives { get; set; }

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinition GenericDefinition { get; set; }

        /// <summary>
        ///     method identifier
        /// </summary>
        public Identifier Identifier { get; set; }

        /// <summary>
        ///     method kind
        /// </summary>
        public int MethodKind { get; set; }

        /// <summary>
        ///     formal parameters
        /// </summary>
        public FormalParameters Parameters { get; set; }

        /// <summary>
        ///     Result type attributes
        /// </summary>
        public UserAttributes ResultAttributes { get; set; }

        /// <summary>
        ///     parse a type specification
        /// </summary>
        public TypeSpecification ResultType { get; set; }

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