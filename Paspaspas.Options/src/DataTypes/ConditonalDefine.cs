namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     Conditional symbol
    /// </summary>
    public class ConditionalSymbol {

        /// <summary>
        ///     name of the conditional symbol
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        ///     flag, if <c>true </c>the symbol is active
        /// </summary>
        public bool IsActive { get; set; }
            = true;

        /// <summary>
        ///     flag, if <c>true</c> the symbol is only valid for the current unit
        /// </summary>
        public bool IsLocal { get; set; }
        = false;

    }
}
