using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     property accessor kind
    /// </summary>
    public class StructurePropertyAccessor : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     accessor kind
        /// </summary>
        public StructurePropertyAccessorKind Kind { get; set; }

        /// <summary>
        ///     accessor member name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     expression for disp ids
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     map accessor lind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns>mapped kind</returns>
        public static StructurePropertyAccessorKind MapKind(int kind) {

            switch (kind) {

                case TokenKind.Read:
                    return StructurePropertyAccessorKind.Read;

                case TokenKind.Write:
                    return StructurePropertyAccessorKind.Write;

                case TokenKind.Add:
                    return StructurePropertyAccessorKind.Add;

                case TokenKind.Remove:
                    return StructurePropertyAccessorKind.Remove;

                default:
                    return StructurePropertyAccessorKind.Remove;


            }
        }

        /// <summary>
        ///     enumerate party
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
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
