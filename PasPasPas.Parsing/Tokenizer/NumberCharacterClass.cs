namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     character class for numbers
    /// </summary>
    public class NumberCharacterClass : CharacterClass {

        /// <summary>
        ///     test if the char is a number
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => ('0' <= input) && (input <= '9');

    }

}
