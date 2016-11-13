using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayTypeDeclaration : StructuredType, IExpressionTarget, ITypeTarget {

        /// <summary>
        ///     index items
        /// </summary>
        public IList<ExpressionBase> IndexItems
            => indexItems;

        private List<ExpressionBase> indexItems
            = new List<ExpressionBase>();

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (var item in indexItems)
                    yield return item;
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }


        /// <summary>
        ///     array indexes
        /// </summary>
        public ExpressionBase Value
        {
            get
            {
                if (indexItems.Count > 0)
                    return indexItems[indexItems.Count - 1];
                else
                    return null;
            }

            set
            {
                if (value != null)
                    indexItems.Add(value);
            }
        }

        /// <summary>
        ///     array type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

    }
}
