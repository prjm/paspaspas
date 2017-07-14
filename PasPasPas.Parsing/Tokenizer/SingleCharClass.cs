namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     character class for a single char
    /// </summary>
    public class SingleCharClass : PrefixedCharacterClass {

        private readonly char match;

        /// <summary>
        ///     char to match
        /// </summary>
        /// <param name="forValue">char to match</param>
        public SingleCharClass(char forValue)
            => match = forValue;

        /// <summary>
        ///     getsthe single character
        /// </summary>
        public override char Prefix
            => match;

        /// <summary>
        ///     test if the char matches
        /// </summary>
        /// <param name="input">char to test</param>
        /// <returns></returns>
        public override bool Matches(char input) =>
            input == match;
    }

}
