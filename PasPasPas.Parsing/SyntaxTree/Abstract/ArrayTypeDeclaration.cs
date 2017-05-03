using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayTypeDeclaration : StructuredTypeBase, IExpressionTarget, ITypeTarget {

        /// <summary>
        ///     index items
        /// </summary>
        public IList<IExpression> IndexItems
            => indexItems;

        private List<IExpression> indexItems
            = new List<IExpression>();

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ExpressionBase item in indexItems)
                    yield return item;
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }


        /// <summary>
        ///     array indexes
        /// </summary>
        public IExpression Value {
            get {
                if (indexItems.Count > 0)
                    return indexItems[indexItems.Count - 1];
                else
                    return null;
            }

            set {
                if (value != null)
                    indexItems.Add(value);
            }
        }

        /// <summary>
        ///     array type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

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
