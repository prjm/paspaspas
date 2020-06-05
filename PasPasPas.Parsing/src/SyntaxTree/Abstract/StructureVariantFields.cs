#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     list of variant
    /// </summary>
    public class StructureVariantFields : AbstractSyntaxPartBase, IExpressionTarget {

        /// <summary>
        ///     matching expression for record constants
        /// </summary>
        public ISyntaxPartCollection<IExpression> Expressions { get; }

        /// <summary>
        ///     fields
        /// </summary>
        public ISyntaxPartCollection<StructureFields> Fields { get; }

        /// <summary>
        ///     expression values
        /// </summary>
        public IExpression Value {
            get => Expressions.LastOrDefault();
            set => Expressions.Add(value);
        }

        /// <summary>
        ///     create a new set of structure variant fields
        /// </summary>
        public StructureVariantFields() {
            Expressions = new SyntaxPartCollection<IExpression>();
            Fields = new SyntaxPartCollection<StructureFields>();
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            foreach (ExpressionBase expression in Expressions)
                AcceptPart(this, expression, visitor);
            foreach (var field in Fields)
                AcceptPart(this, field, visitor);
            visitor.EndVisit(this);
        }
    }
}