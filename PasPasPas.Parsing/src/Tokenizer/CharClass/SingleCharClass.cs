#nullable disable
namespace PasPasPas.Parsing.Tokenizer.CharClass {

    /// <summary>
    ///     character class for a single char
    /// </summary>
    public sealed class SingleCharClass : CharacterClass {

        /// <summary>
        ///     char to match
        /// </summary>
        /// <param name="forValue">char to match</param>
        public SingleCharClass(char forValue)
            => Match = forValue;

        /// <summary>
        ///     test if the char matches
        /// </summary>
        /// <param name="input">char to test</param>
        /// <returns></returns>
        public override bool Matches(char input) =>
            input == Match;

        /// <summary>
        ///     char to match
        /// </summary>
        public char Match { get; }
    }

}
