using System.Collections.Generic;
using System.Linq;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     list of variant
    /// </summary>
    public class StructureVariantFields : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     matching expression for record constants
        /// </summary>
        public IList<IExpression> Expressions { get; }
            = new List<IExpression>();

        /// <summary>
        ///     fields
        /// </summary>
        public IList<StructureFields> Fields { get; }
            = new List<StructureFields>();

        /// <summary>
        ///     expression values
        /// </summary>
        public IExpression Value {
            get { return Expressions.Last(); }
            set { Expressions.Add(value); }
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
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }
    }
}