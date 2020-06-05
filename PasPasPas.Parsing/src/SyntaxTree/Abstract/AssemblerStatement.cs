#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     assembler statement
    /// </summary>
    public class AssemblerStatement : AbstractSyntaxPartBase, ILabelTarget, IExpressionTarget {

        /// <summary>
        ///     kind of the assembler statement
        /// </summary>
        public AssemblerStatementKind Kind { get; set; }

        /// <summary>
        ///     operand code
        /// </summary>
        public SymbolName OpCode { get; set; }

        /// <summary>
        ///     statement label
        /// </summary>
        public SymbolName LabelName { get; set; }

        /// <summary>
        ///     lock prefix
        /// </summary>
        public SymbolName LockPrefix { get; set; }

        /// <summary>
        ///     segment prefix
        /// </summary>
        public SymbolName SegmentPrefix { get; set; }

        /// <summary>
        ///     operands
        /// </summary>
        public ISyntaxPartCollection<IExpression> Operands { get; }

        /// <summary>
        ///     create a new assembler statement
        /// </summary>
        public AssemblerStatement()
            => Operands = new SyntaxPartCollection<IExpression>();

        /// <summary>
        ///     operands
        /// </summary>
        public IExpression Value {
            get => Operands.LastOrDefault();
            set => Operands.Add(value);
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Operands, visitor);
            visitor.EndVisit(this);
        }
    }
}
