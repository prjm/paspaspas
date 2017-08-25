namespace PasPasPas.Parsing.Tokenizer
{

    /// <summary>
    ///     matches old control characters
    /// </summary>
    public class ControlCharacterClass : CharacterClass
    {

        /// <summary>
        ///     test if the char is control characters
        /// </summary>
        /// <param name="input">input char</param>
        /// <returns><c>true</c> if the character is a control character</returns>
        public override bool Matches(char input)
            => (!char.IsWhiteSpace(input)) && (char.IsControl(input));

    }

}
