using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayTypeDeclaration : StructuredTypeBase, IExpressionTarget, ITypeTarget {

        /// <summary>
        ///     index items
        /// </summary>
        public ISyntaxPartCollection<IExpression> IndexItems { get; }

        /// <summary>
        ///     create a new array type declaration
        /// </summary>
        public ArrayTypeDeclaration()
            => IndexItems = new SyntaxPartCollection<IExpression>();

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
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, IndexItems, visitor);
            AcceptPart(this, TypeValue, visitor);
            visitor.EndVisit(this);
        }

    }
}
