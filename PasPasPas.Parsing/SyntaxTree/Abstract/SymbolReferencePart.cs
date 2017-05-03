using System.Linq;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     part of complex symbol references
    /// </summary>
    public class SymbolReferencePart : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     reference kind
        /// </summary>
        public SymbolReferencePartKind Kind { get; set; }

        /// <summary>
        ///     generic part
        /// </summary>
        public GenericTypes GenericType { get; internal set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; internal set; }

        /// <summary>
        ///     expressions
        /// </summary>
        public IList<IExpression> Expressions { get; }
            = new List<IExpression>();

        /// <summary>
        ///     expressiona
        /// </summary>
        public IExpression Value {
            get {
                return Expressions.LastOrDefault();
            }

            set {
                Expressions.Add(value);
            }
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
