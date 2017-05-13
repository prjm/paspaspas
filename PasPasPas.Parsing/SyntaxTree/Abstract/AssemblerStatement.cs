using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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
        public ISyntaxPartList<IExpression> Operands { get; }

        /// <summary>
        ///     create a new assembler statement
        /// </summary>
        public AssemblerStatement()
            => Operands = new SyntaxPartCollection<IExpression>(this);

        /// <summary>
        ///     operands
        /// </summary>
        public IExpression Value {
            get => Operands.LastOrDefault();
            set => Operands.Add(value);
        }

        /// <summary>
        ///     get all operands
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (IExpression operand in Operands)
                    yield return operand;
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
