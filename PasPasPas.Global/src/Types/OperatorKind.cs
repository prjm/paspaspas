#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     operator kind
    /// </summary>
    public enum OperatorKind {

        /// <summary>
        ///     undefined operator
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     <c>@</c> operator
        /// </summary>
        AtOperator = 1,

        /// <summary>
        ///     array concatenation operator
        /// </summary>
        ConcatArrayOperator = 2,

        /// <summary>
        ///     set addition operator
        /// </summary>
        SetAddOperator = 3,

        /// <summary>
        ///     set difference operator
        /// </summary>
        SetDifferenceOperator = 4,

        /// <summary>
        ///     set intersection operator
        /// </summary>
        SetIntersectOperator = 5,

        /// <summary>
        ///     element-in-set operator
        /// </summary>
        InSetOperator = 6,

        /// <summary>
        ///     unary minus
        /// </summary>
        UnaryMinus = 7,

        /// <summary>
        ///     unary plus
        /// </summary>
        UnaryPlus = 8,

        /// <summary>
        ///     plus operator
        /// </summary>
        PlusOperator = 9,

        /// <summary>
        ///     minus operator
        /// </summary>
        MinusOperator = 10,

        /// <summary>
        ///     times operator
        /// </summary>
        TimesOperator = 11,

        /// <summary>
        ///     div operator
        /// </summary>
        DivOperator = 12,

        /// <summary>
        ///     mode operator
        /// </summary>
        ModOperator = 13,

        /// <summary>
        ///     slash operator
        /// </summary>
        SlashOperator = 14,

        /// <summary>
        ///     as operator
        /// </summary>
        AsOperator = 15,

        /// <summary>
        ///     is operator
        /// </summary>
        IsOperator = 16,

        /// <summary>
        ///     not operator
        /// </summary>
        NotOperator = 17,

        /// <summary>
        ///     and operator
        /// </summary>
        AndOperator = 18,

        /// <summary>
        ///     <c>xor</c> operator
        /// </summary>
        XorOperator = 19,

        /// <summary>
        ///     or operator
        /// </summary>
        OrOperator = 20,

        /// <summary>
        ///     <c>shl</c> operator
        /// </summary>
        ShlOperator = 21,

        /// <summary>
        ///     <c>shr</c> operator
        /// </summary>
        ShrOperator = 22,

        /// <summary>
        ///     <c>=</c> operator
        /// </summary>
        EqualsOperator = 23,

        /// <summary>
        ///     <c>!=</c> operator
        /// </summary>
        NotEqualsOperator = 24,

        /// <summary>
        ///     <c>&lt;</c> operator
        /// </summary>
        LessThan = 25,

        /// <summary>
        ///     <c>&gt;</c> operator
        /// </summary>
        GreaterThan = 26,

        /// <summary>
        ///     <c>&lt;=</c> operator
        /// </summary>
        LessThanOrEqual = 27,

        /// <summary>
        ///     <c>&gt;=</c> operator
        /// </summary>
        GreaterThanOrEqual = 28,

        /// <summary>
        ///     string concatenation operator
        /// </summary>
        ConcatOperator = 29,
    }
}