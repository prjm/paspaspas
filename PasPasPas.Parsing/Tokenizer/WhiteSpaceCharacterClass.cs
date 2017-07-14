namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     matches whitespace
    /// </summary>
    public class WhiteSpaceCharacterClass : CharacterClass {

        /// <summary>
        ///     test if the char is whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => char.IsWhiteSpace(input);
    }

}
