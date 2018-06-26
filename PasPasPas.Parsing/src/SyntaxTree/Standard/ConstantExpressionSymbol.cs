using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     constant expression
    /// </summary>
    public class ConstantExpressionSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new constant expression symbol
        /// </summary>
        /// <param name="items"></param>
        public ConstantExpressionSymbol(ImmutableArray<SyntaxPartBase> items) : base(items) {
        }

        /// <summary>
        ///     <c>true</c> if this is an array constant
        /// </summary>
        public bool IsArrayConstant { get; set; }

        /// <summary>
        ///     <c>true</c> if this in an record constant
        /// </summary>
        public bool IsRecordConstant { get; set; }

        /// <summary>
        ///     value of the expression
        /// </summary>
        public Expression Value { get; set; }

        /// <summary>
        ///     open parenthesis
        /// </summary>
        public Terminal OpenParen { get; set; }

        /// <summary>
        ///     close parenthesis
        /// </summary>
        public Terminal CloseParen { get; set; }

        /// <summary>
        ///     separator
        /// </summary>
        public Terminal Separator { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, OpenParen, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, CloseParen, visitor);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     token length
        /// </summary>
        public override int Length
            => OpenParen.GetSymbolLength() +
               ItemLength +
               CloseParen.GetSymbolLength() +
               Value.GetSymbolLength();


    }
}