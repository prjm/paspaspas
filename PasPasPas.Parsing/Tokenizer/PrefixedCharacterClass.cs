namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     character class with prefix
    /// </summary>
    public abstract class PrefixedCharacterClass : CharacterClass {

        /// <summary>
        ///     prefix
        /// </summary>
        public abstract char Prefix { get; }

    }

}
