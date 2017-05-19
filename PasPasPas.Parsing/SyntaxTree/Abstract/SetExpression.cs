using System.Collections.Generic;
using System.Linq;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     set expression
    /// </summary>
    public class ArrayExpression : AbstractSyntaxPartBase, IExpression, IExpressionTarget {

        /// <summary>
        ///     subexpressions
        /// </summary>
        public ISyntaxPartList<IExpression> Expressions { get; }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value {
            get => Expressions.LastOrDefault();
            set => Expressions.Add(value);
        }

        /// <summary>
        ///     creates a new set expression
        /// </summary>
        public ArrayExpression()
            => Expressions = new SyntaxPartCollection<IExpression>(this);

        /// <summary>
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (IExpression part in Expressions)
                    yield return part;

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
