namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     character class for exponents
    /// </summary>
    public class ExponentCharacterClass : CharacterClass {

        /// <summary>
        ///     test if a character class matches
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input) =>
            input == 'E' ||
            input == 'e';
    }

}
