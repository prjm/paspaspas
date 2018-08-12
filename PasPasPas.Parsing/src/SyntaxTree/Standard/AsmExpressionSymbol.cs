using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     assembly expression term
    /// </summary>
    public class AsmExpressionSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new expression symbol
        /// </summary>
        /// <param name="offsetSymbol"></param>
        /// <param name="offset"></param>
        public AsmExpressionSymbol(Terminal offsetSymbol, AsmOperandSymbol offset, bool typeSymbol) {
            if (typeSymbol) {
                TypeSymbol = offsetSymbol;
                TypeExpression = offset;
            }
            else {
                OffsetSymbol = offsetSymbol;
                Offset = offset;
            }
        }

        /// <summary>
        ///     create a new expression symbol
        /// </summary>
        /// <param name="bytePtrKind"></param>
        /// <param name="bytePtr"></param>
        public AsmExpressionSymbol(IdentifierSymbol bytePtrKind, AsmOperandSymbol bytePtr) {
            BytePtrKind = bytePtrKind;
            BytePtr = bytePtr;
        }

        /// <summary>
        ///     create a new assembly expression
        /// </summary>
        /// <param name="leftOperand"></param>
        /// <param name="rightOperand"></param>
        /// <param name="asmOp"></param>
        /// <param name="kind"></param>
        public AsmExpressionSymbol(AsmTermSymbol leftOperand, Terminal asmOp, AsmOperandSymbol rightOperand, int kind) {
            LeftOperand = leftOperand;
            Operator = asmOp;
            RightOperand = rightOperand;
            BinaryOperatorKind = kind;
        }

        /// <summary>
        ///     byte pointer
        /// </summary>
        public SyntaxPartBase BytePtr { get; }

        /// <summary>
        ///     left operand
        /// </summary>
        public SyntaxPartBase LeftOperand { get; }

        /// <summary>
        ///     operator
        /// </summary>
        public Terminal Operator { get; }

        /// <summary>
        ///     offset expression
        /// </summary>
        public AsmOperandSymbol Offset { get; }

        /// <summary>
        ///     right operand
        /// </summary>
        public SyntaxPartBase RightOperand { get; }

        /// <summary>
        ///     type expression
        /// </summary>
        public AsmOperandSymbol TypeExpression { get; }

        /// <summary>
        ///     byte pointer kind
        /// </summary>
        public SyntaxPartBase BytePtrKind { get; }

        /// <summary>
        ///     token kind
        /// </summary>
        public int BinaryOperatorKind { get; }

        /// <summary>
        ///     offset symbol
        /// </summary>
        public Terminal OffsetSymbol { get; }

        /// <summary>
        ///     type symbol
        /// </summary>
        public Terminal TypeSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OffsetSymbol, visitor);
            AcceptPart(this, Offset, visitor);
            AcceptPart(this, BytePtrKind, visitor);
            AcceptPart(this, BytePtr, visitor);
            AcceptPart(this, TypeSymbol, visitor);
            AcceptPart(this, TypeExpression, visitor);
            AcceptPart(this, LeftOperand, visitor);
            AcceptPart(this, Operator, visitor);
            AcceptPart(this, RightOperand, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Offset.GetSymbolLength() +
                OffsetSymbol.GetSymbolLength() +
                BytePtr.GetSymbolLength() +
                BytePtrKind.GetSymbolLength() +
                TypeSymbol.GetSymbolLength() +
                TypeExpression.GetSymbolLength() +
                LeftOperand.GetSymbolLength() +
                Operator.GetSymbolLength() +
                RightOperand.GetSymbolLength();


    }
}

