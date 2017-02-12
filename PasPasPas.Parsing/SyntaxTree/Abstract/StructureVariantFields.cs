using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     list of variant
    /// </summary>
    public class StructureVariantFields : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     matching expression for record constants
        /// </summary>
        public IList<ExpressionBase> Expressions { get; }
            = new List<ExpressionBase>();

        /// <summary>
        ///     fields
        /// </summary>
        public IList<StructureFields> Fields { get; }
            = new List<StructureFields>();

        /// <summary>
        ///     expression values
        /// </summary>
        public ExpressionBase Value {
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
    }
}