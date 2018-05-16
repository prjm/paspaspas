namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     symbol reference
    /// </summary>
    public enum ReferenceKind {

        /// <summary>
        ///     unknown reference
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     reference to a field
        /// </summary>
        RefToField = 1,

        /// <summary>
        ///     reference to methods
        /// </summary>
        RefToMethod = 2,

        /// <summary>
        ///     reference to type
        /// </summary>
        RefToType = 3,

        /// <summary>
        ///     reference to constant
        /// </summary>
        RefToConstant = 4,

        /// <summary>
        ///     reference to variable
        /// </summary>
        RefToVariable = 5,

        /// <summary>
        ///     reference to unit
        /// </summary>
        RefToUnit = 6,

        /// <summary>
        ///     reference to enumeration member
        /// </summary>
        RefToEnumMember = 7,

        /// <summary>
        ///     reference to class variable
        /// </summary>
        RefToClassField = 8,

        /// <summary>
        ///     reference to global routine
        /// </summary>
        RefToGlobalRoutine = 9,
    }
}
