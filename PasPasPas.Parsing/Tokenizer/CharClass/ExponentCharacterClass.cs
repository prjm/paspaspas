namespace PasPasPas.Parsing.Tokenizer.CharClass {

    /// <summary>
    ///     character class to match <c>e</c> or <c>E</c> for exponents
    /// </summary>
    public sealed class ExponentCharacterClass : CharacterClass {

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
