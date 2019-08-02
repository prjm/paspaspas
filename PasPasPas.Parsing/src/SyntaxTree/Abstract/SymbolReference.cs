using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     reference to a symbol
    /// </summary>
    public class SymbolReference : ExpressionBase, IExpression, ILabelTarget, ITypeTarget, IExpressionTarget {

        /// <summary>
        ///     identifier name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     <c>true</c> if a pointer was used
        /// </summary>
        public bool PointerTo { get; set; }

        /// <summary>
        ///     referencing label
        /// </summary>
        public SymbolName LabelName { get; set; }

        /// <summary>
        ///     reference to inherited symbol
        /// </summary>
        public bool Inherited { get; set; }

        /// <summary>
        ///     type name for designators
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     expression target
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     generic types for symbol reference
        /// </summary>
        public GenericTypeCollection GenericType { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public SyntaxPartCollection<ISyntaxPart> SymbolParts { get; }

        /// <summary>
        ///     name parameter
        /// </summary>
        public bool NamedParameter { get; set; }

        /// <summary>
        ///     create a a new symbol reference
        /// </summary>
        public SymbolReference()
            => SymbolParts = new SyntaxPartCollection<ISyntaxPart>();

        /// <summary>
        ///     add a part
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(ISyntaxPart part)
            => SymbolParts.Add(part);

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, TypeValue, visitor);
            AcceptPart(this, Value, visitor);
            foreach (var part in SymbolParts)
                AcceptPart(this, part, visitor);
            visitor.EndVisit(this);
        }
    }
}
