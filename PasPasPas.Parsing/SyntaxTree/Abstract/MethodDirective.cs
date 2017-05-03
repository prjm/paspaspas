using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method directive
    /// </summary>
    public class MethodDirective : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     kind
        /// </summary>
        public MethodDirectiveKind Kind { get; set; }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     directive specifiers
        /// </summary>
        public IList<MethodDirectiveSpecifier> Specifiers { get; } =
            new List<MethodDirectiveSpecifier>();

        /// <summary>
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
                foreach (MethodDirectiveSpecifier specifier in Specifiers)
                    yield return specifier;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}
