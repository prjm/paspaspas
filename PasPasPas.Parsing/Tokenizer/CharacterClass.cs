namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for a character class used by a tokenizer
    /// </summary>
    public abstract class CharacterClass {

        /// <summary>
        ///     test if the character class matches
        /// </summary>
        /// <param name="input">character to match</param>
        /// <returns><c>true</c> if the character matches the input</returns>
        public abstract bool Matches(char input);

    }

}
