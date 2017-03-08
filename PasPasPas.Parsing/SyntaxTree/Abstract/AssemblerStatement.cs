namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     assembler statement
    /// </summary>
    public class AssemblerStatement : AbstractSyntaxPart, ILabelTarget {

        /// <summary>
        ///     kind of the assembler statement
        /// </summary>
        public AssemblerStatementKind Kind { get; set; }

        /// <summary>
        ///     first operand
        /// </summary>
        public IExpression FirstOperand { get; set; }

        /// <summary>
        ///     operand code
        /// </summary>
        public SymbolName OpCode { get; internal set; }

        /// <summary>
        ///     statement label
        /// </summary>
        public SymbolName LabelName { get; set; }
    }
}
