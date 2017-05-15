using System.Collections.Generic;
using System.Linq;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     list of variant
    /// </summary>
    public class StructureVariantFields : AbstractSyntaxPartBase, IExpressionTarget {

        /// <summary>
        ///     matching expression for record constants
        /// </summary>
        public ISyntaxPartList<IExpression> Expressions { get; }

        /// <summary>
        ///     fields
        /// </summary>
        public ISyntaxPartList<StructureFields> Fields { get; }

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
            Expressions = new SyntaxPartCollection<IExpression>(this);
            Fields = new SyntaxPartCollection<StructureFields>(this);
        }

        /// <summary>
        ///     expressions
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ExpressionBase expression in Expressions)
                    yield return expression;
                foreach (StructureFields field in Fields)
                    yield return field;
            }
        }

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