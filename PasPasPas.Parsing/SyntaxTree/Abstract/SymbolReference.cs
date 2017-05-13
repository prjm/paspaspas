using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     reference to a symbol
    /// </summary>
    public class SymbolReference : AbstractSyntaxPartBase, IExpression, ILabelTarget, ITypeTarget, IExpressionTarget {

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
        public GenericTypes GenericType { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public SyntaxPartCollection<SymbolReferencePart> SymbolParts { get; }

        /// <summary>
        ///     name parameter
        /// </summary>
        public bool NamedParameter { get; set; }

        /// <summary>
        ///     create a a new symbol reference
        /// </summary>
        public SymbolReference()
            => SymbolParts = new SyntaxPartCollection<SymbolReferencePart>(this);

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (TypeValue != null)
                    yield return TypeValue;
                if (Value != null)
                    yield return Value;
                foreach (ISyntaxPart part in SymbolParts)
                    yield return part;
            }
        }

        /// <summary>
        ///     add a part
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(SymbolReferencePart part)
            => SymbolParts.Add(part);

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
