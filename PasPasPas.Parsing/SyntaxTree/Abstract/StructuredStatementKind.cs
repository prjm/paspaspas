namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     statement kind
    /// </summary>
    public enum StructuredStatementKind {


        /// <summary>
        ///     unknown statement
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     raise statement
        /// </summary>
        Raise = 1,

        /// <summary>
        ///     at statement
        /// </summary>
        RaiseAtOnly = 2,

        /// <summary>
        ///     raise at statement
        /// </summary>
        RaiseAt = 3,

        /// <summary>
        ///     try / finally statement
        /// </summary>
        TryFinally = 4,

        /// <summary>
        ///     try / except statement
        /// </summary>
        TryExcept = 5,

        /// <summary>
        ///     except / else statement
        /// </summary>
        ExceptElse = 6,

        /// <summary>
        ///     except handler
        /// </summary>
        ExceptOn = 7,

        /// <summary>
        ///     with statement
        /// </summary>
        With = 8,

        /// <summary>
        ///     for / to statement
        /// </summary>
        ForTo = 9,

        /// <summary>
        ///     for / downto statement
        /// </summary>
        ForDownTo = 10,

        /// <summary>
        ///     for / in statement
        /// </summary>
        ForIn = 11,

        /// <summary>
        ///     while statement
        /// </summary>
        While = 12,

        /// <summary>
        ///     repeat statement
        /// </summary>
        Repeat = 13,

        /// <summary>
        ///     case statement
        /// </summary>
        Case = 14,

        /// <summary>
        ///     else part of case statement
        /// </summary>
        CaseElse = 15,

        /// <summary>
        ///     case item
        /// </summary>
        CaseItem = 16,

        /// <summary>
        ///     if / then
        /// </summary>
        IfThen = 17,

        /// <summary>
        ///     if / else
        /// </summary>
        IfElse = 18,

        /// <summary>
        ///     break
        /// </summary>
        Break = 19,

        /// <summary>
        ///     continue
        /// </summary>
        Continue = 20,

        /// <summary>
        ///     goto
        /// </summary>
        GoToLabel = 21,

        /// <summary>
        ///     exit label
        /// </summary>
        Exit = 22,

        /// <summary>
        ///     assignment
        /// </summary>
        Assignment = 23,

        /// <summary>
        ///     expression statement
        /// </summary>
        ExpressionStatement = 24,
    }
}
