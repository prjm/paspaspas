﻿namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     statement part
    /// </summary>
    public class StatementPart : SyntaxPartBase {

        /// <summary>
        ///     assembler block
        /// </summary>
        public AsmStatement Asm { get; set; }

        /// <summary>
        ///     assignment
        /// </summary>
        public Expression Assignment { get; set; }

        /// <summary>
        ///     case statement
        /// </summary>
        public CaseStatement Case { get; set; }

        /// <summary>
        ///     compunt statement
        /// </summary>
        public CompoundStatement CompundStatement { get; set; }

        /// <summary>
        ///     deisgnator part
        /// </summary>
        public DesignatorStatement DesignatorPart { get; set; }

        /// <summary>
        ///     for statement
        /// </summary>
        public ForStatement For { get; set; }

        /// <summary>
        ///     goto statement
        /// </summary>
        public GoToStatement GoTo { get; set; }

        /// <summary>
        ///     if statement
        /// </summary>
        public IfStatement If { get; set; }

        /// <summary>
        ///     raise statement
        /// </summary>
        public RaiseStatement Raise { get; set; }

        /// <summary>
        ///     repeat statement
        /// </summary>
        public RepeatStatement Reapeat { get; set; }

        /// <summary>
        ///     try statement
        /// </summary>
        public TryStatement Try { get; set; }

        /// <summary>
        ///     while statement
        /// </summary>
        public WhileStatement While { get; set; }

        /// <summary>
        ///     with statement
        /// </summary>
        public WithStatement With { get; set; }

    }
}