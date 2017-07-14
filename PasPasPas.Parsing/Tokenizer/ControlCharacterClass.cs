namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     matches old control characters
    /// </summary>
    public class ControlCharacterClass : CharacterClass {

        /// <summary>
        ///     test if the char is whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => (!char.IsWhiteSpace(input)) && (char.IsControl(input));

    }

}
