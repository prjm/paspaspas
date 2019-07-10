using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method specifiers
    /// </summary>
    public class MethodDirectiveSpecifier : AbstractSyntaxPartBase, IExpressionTarget {

        /// <summary>
        ///     directive kind
        /// </summary>
        public MethodDirectiveSpecifierKind Kind { get; set; }

        /// <summary>
        ///     specified constraints
        /// </summary>
        public ISyntaxPartCollection<IExpression> Constraints { get; }

        /// <summary>
        ///     create a new method directive specifier
        /// </summary>
        public MethodDirectiveSpecifier()
            => Constraints = new SyntaxPartCollection<IExpression>();

        /// <summary>
        ///     values
        /// </summary>
        public IExpression Value {
            get => Constraints.LastOrDefault();
            set => Constraints.Add(value);
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Constraints, visitor);
            visitor.EndVisit(this);
        }
    }
}