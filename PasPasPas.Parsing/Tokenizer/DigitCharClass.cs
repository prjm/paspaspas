namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     character class for numbers
    /// </summary>
    public class DigitCharClass : CharacterClass {

        private bool allowHexChars;

        /// <summary>
        ///     create a new hex character class
        /// </summary>
        /// <param name="allowsHex"></param>
        public DigitCharClass(bool allowsHex)
            => this.allowHexChars = allowsHex;

        /// <summary>
        ///     test if the character class matches 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => (('0' <= input) && (input <= '9')) ||
                (allowHexChars && (('a' <= input) && (input <= 'f') ||
                                   ('A' <= input) && (input <= 'F')));
    }
}