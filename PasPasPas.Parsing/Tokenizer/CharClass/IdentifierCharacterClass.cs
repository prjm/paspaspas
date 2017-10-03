namespace PasPasPas.Parsing.Tokenizer.CharClass {

    /// <summary>
    ///     character class to match identifiers
    /// </summary>
    public sealed class IdentifierCharacterClass : CharacterClass {

        /// <summary>
        ///     allow &amp;   
        /// </summary>
        public bool AllowAmpersand { get; set; }
            = true;

        /// <summary>
        ///     allow digits
        /// </summary>
        public bool AllowDigits { get; set; }
            = false;

        /// <summary>
        ///     allow dots
        /// </summary>
        public bool AllowDots { get; set; }
            = false;

        /// <summary>
        ///     test if a char is acceptable for an identifier
        /// </summary>
        /// <param name="input">input</param>
        /// <returns><c>true</c> if the char can be part of an identifier</returns>
        public override bool Matches(char input) =>
            (input >= 'A' && input <= 'Z') ||
            (input >= 'a' && input <= 'z') ||
            input == '_' ||
            (AllowAmpersand && input == '&') ||
            (AllowDigits && (input >= '0' && input <= '9')) ||
            (AllowDots && input == '.') ||
            (input > 127 && ((AllowDigits && char.IsLetterOrDigit(input)) || char.IsLetter(input)));
    }

}
