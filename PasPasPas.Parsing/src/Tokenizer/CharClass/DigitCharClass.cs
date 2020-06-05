#nullable disable
namespace PasPasPas.Parsing.Tokenizer.CharClass {

    /// <summary>
    ///     character class for numbers
    /// </summary>
    public sealed class DigitCharClass : CharacterClass {

        private readonly bool allowHexChars;

        /// <summary>
        ///     create a new hex character class
        /// </summary>
        /// <param name="allowsHex"></param>
        public DigitCharClass(bool allowsHex)
            => allowHexChars = allowsHex;

        /// <summary>
        ///     test if the character class matches 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => '0' <= input && input <= '9' ||
                allowHexChars && ('a' <= input && input <= 'f' ||
                                   'A' <= input && input <= 'F');
    }
}