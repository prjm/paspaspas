namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     character class to match <c>+</c> and <c>-</c>
    /// </summary>
    public class PlusMinusCharacterClass : CharacterClass {

        /// <summary>
        ///     test if a character class matches
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input) =>
            input == '+' ||
            input == '-';
    }

}
