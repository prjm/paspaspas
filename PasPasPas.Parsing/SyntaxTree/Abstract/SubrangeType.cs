using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     subrange type
    /// </summary>
    public class SubrangeType : TypeSpecificationBase, IExpressionTarget {

        /// <summary>
        ///     expression kind
        /// </summary>
        public ExpressionKind Kind { get; set; }

        /// <summary>
        ///     left operand
        /// </summary>
        public IExpression RangeStart { get; set; }

        /// <summary>
        ///     right operand
        /// </summary>
        public IExpression RangeEnd { get; set; }

        /// <summary>
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (RangeStart != null)
                    yield return RangeStart;
                if (RangeEnd != null)
                    yield return RangeEnd;
            }
        }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value {
            get => RangeEnd != null ? RangeEnd : RangeStart;

            set {
                if (RangeStart == null)
                    RangeStart = value;
                else if (RangeEnd == null)
                    RangeEnd = value;
                else
                    ExceptionHelper.InvalidOperation();
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
