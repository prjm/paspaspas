namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for a character class
    /// </summary>
    public abstract class CharacterClass {

        /// <summary>
        ///     test if the character class matches
        /// </summary>
        /// <param name="input">character to match</param>
        /// <returns></returns>
        public abstract bool Matches(char input);

    }

}
