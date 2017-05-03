
using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     unary operator
    /// </summary>
    public class UnaryOperator : ExpressionBase, IExpressionTarget {

        /// <summary>
        ///     operator kind
        /// </summary>
        public ExpressionKind Kind { get; set; }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     operator parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
            }
        }

        /// <summary>
        ///     map asm byte pointer kind
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ExpressionKind MapKind(string value) {

            if (string.IsNullOrWhiteSpace(value))
                return ExpressionKind.Undefined;

            if (value.Equals("byte", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerByte;
            else if (value.Equals("word", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerWord;
            else if (value.Equals("dword", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerDWord;
            else if (value.Equals("qword", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerQWord;
            else if (value.Equals("tbyte", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerTByte;

            return ExpressionKind.Undefined;
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
