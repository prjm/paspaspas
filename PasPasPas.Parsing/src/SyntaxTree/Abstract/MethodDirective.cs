using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method directive
    /// </summary>
    public class MethodDirective : AbstractSyntaxPartBase, IExpressionTarget {

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
        public ISyntaxPartCollection<MethodDirectiveSpecifier> Specifiers { get; }

        /// <summary>
        ///     create a new method directive
        /// </summary>
        public MethodDirective()
            => Specifiers = new SyntaxPartCollection<MethodDirectiveSpecifier>();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            if (Value != null)
                AcceptPart(this, Value, visitor);
            foreach (var specifier in Specifiers)
                AcceptPart(this, specifier, visitor);
            visitor.EndVisit(this);
        }

    }
}
