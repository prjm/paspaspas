using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayTypeDeclaration : StructuredTypeBase, IExpressionTarget, ITypeTarget {

        /// <summary>
        ///     index items
        /// </summary>
        public ISyntaxPartList<IExpression> IndexItems { get; }

        /// <summary>
        ///     create a new array type declaration
        /// </summary>
        public ArrayTypeDeclaration()
            => IndexItems = new SyntaxPartCollection<IExpression>(this);

        /// <summary>
        ///     constant array items
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ExpressionBase item in IndexItems)
                    yield return item;
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }


        /// <summary>
        ///     array indexes
        /// </summary>
        public IExpression Value {
            get => IndexItems.LastOrDefault();
            set => IndexItems.Add(value);
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
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
