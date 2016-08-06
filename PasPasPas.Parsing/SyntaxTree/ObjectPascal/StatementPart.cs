using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     statement part
    /// </summary>
    public class StatementPart : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="parser"></param>
        public StatementPart(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     assembler block
        /// </summary>
        public AsmStatement Asm { get; internal set; }

        /// <summary>
        ///     assignment
        /// </summary>
        public Expression Assignment { get; internal set; }

        /// <summary>
        ///     case statement
        /// </summary>
        public CaseStatement Case { get; internal set; }

        /// <summary>
        ///     compunt statement
        /// </summary>
        public CompoundStatement CompundStatement { get; internal set; }

        /// <summary>
        ///     deisgnator part
        /// </summary>
        public DesignatorStatement DesignatorPart { get; internal set; }

        /// <summary>
        ///     for statement
        /// </summary>
        public ForStatement For { get; internal set; }

        /// <summary>
        ///     goto statement
        /// </summary>
        public GoToStatement GoTo { get; internal set; }

        /// <summary>
        ///     if statement
        /// </summary>
        public IfStatement If { get; internal set; }

        /// <summary>
        ///     raise statement
        /// </summary>
        public RaiseStatement Raise { get; internal set; }

        /// <summary>
        ///     repeat statement
        /// </summary>
        public RepeatStatement Reapeat { get; internal set; }

        /// <summary>
        ///     try statement
        /// </summary>
        public TryStatement Try { get; internal set; }

        /// <summary>
        ///     while statement
        /// </summary>
        public WhileStatement While { get; internal set; }

        /// <summary>
        ///     with statement
        /// </summary>
        public WithStatement With { get; internal set; }

        /// <summary>
        ///     format statement part
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(If);
            result.Part(Case);
            result.Part(Reapeat);
            result.Part(While);
            result.Part(For);
            result.Part(With);
            result.Part(Try);
            result.Part(Raise);
            result.Part(Asm);
            result.Part(CompundStatement);
            result.Part(GoTo);
            result.Part(DesignatorPart);
            if (Assignment != null) {
                result.Space();
                result.Operator(":=");
                result.Space().Part(Assignment);
            }
        }
    }
}