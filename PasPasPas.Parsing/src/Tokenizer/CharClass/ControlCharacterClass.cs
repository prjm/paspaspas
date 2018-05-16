namespace PasPasPas.Parsing.Tokenizer.CharClass {

    /// <summary>
    ///     matches old control characters
    /// </summary>
    public sealed class ControlCharacterClass : CharacterClass {

        /// <summary>
        ///     test if the char is control characters
        /// </summary>
        /// <param name="input">input char</param>
        /// <returns><c>true</c> if the character is a control character</returns>
        public override bool Matches(char input)
            => (input != 0x1A) && // soft eof
               (!WhiteSpaceCharacterClass.IsAsciiWhitespace(input)) &&
               (char.IsControl(input));

    }

}
