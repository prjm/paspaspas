namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     enum vale declaration
    /// </summary>
    public class EnumValue {
        private readonly string symbolName;
        private readonly int enumValue;

        /// <summary>
        ///     create a new enum value
        /// </summary>
        /// <param name="name">symbol name</param>
        /// <param name="value">symbol value</param>
        public EnumValue(string name, int value) {
            symbolName = name;
            enumValue = value;
        }

        /// <summary>
        ///     name of the enum item
        /// </summary>
        public string Name
            => symbolName;

        /// <summary>
        ///     value of the enum item
        /// </summary>
        public int Value
            => enumValue;
    }
}