namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     charactre class to match whitespace
    /// </summary>
    public class WhiteSpaceCharacterClass : CharacterClass {

        /// <summary>
        ///     test if the char is whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => // 
                input == ' ' ||
                input == '\t' ||
                char.IsWhiteSpace(input);
    }

}
