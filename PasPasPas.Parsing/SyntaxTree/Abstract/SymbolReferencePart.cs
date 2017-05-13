using System.Linq;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     part of complex symbol references
    /// </summary>
    public class SymbolReferencePart : AbstractSyntaxPartBase, IExpressionTarget {

        /// <summary>
        ///     reference kind
        /// </summary>
        public SymbolReferencePartKind Kind { get; set; }

        /// <summary>
        ///     generic part
        /// </summary>
        public GenericTypes GenericType { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     expressions
        /// </summary>
        public ISyntaxPartList<IExpression> Expressions { get; }

        /// <summary>
        ///     create a new symbol reference part
        /// </summary>
        public SymbolReferencePart()
            => Expressions = new SyntaxPartCollection<IExpression>(this);

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value {
            get => Expressions.LastOrDefault();
            set => Expressions.Add(value);
        }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (GenericType != null)
                    yield return GenericType;
                foreach (IExpression expression in Expressions)
                    yield return expression;
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
