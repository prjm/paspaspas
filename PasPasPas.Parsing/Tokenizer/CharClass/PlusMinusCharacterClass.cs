namespace PasPasPas.Parsing.Tokenizer.CharClass {

    /// <summary>
    ///     character class to match <c>+</c> or <c>-</c>
    /// </summary>
    public sealed class PlusMinusCharacterClass : CharacterClass {

        /// <summary>
        ///     test if a character class matches
        /// </summary>
        /// <param name="input">char to test</param>
        /// <returns><c>true</c> if the char is <c>+</c> or <c>-</c></returns>
        public override bool Matches(char input)
            => input == '+' || input == '-';

    }
}
