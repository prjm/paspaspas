namespace PasPasPas.Parsing.Tokenizer.CharClass {

    /// <summary>
    ///     character class to match identifiers
    /// </summary>
    public sealed class IdentifierCharacterClass : CharacterClass {

        /// <summary>
        ///     allow &amp;   
        /// </summary>
        private readonly bool allowAmpersand;

        /// <summary>
        ///     allow digits
        /// </summary>
        private readonly bool allowDigits;

        /// <summary>
        ///     allow dots
        /// </summary>
        private readonly bool allowDot;

        /// <summary>
        ///     create a new identifer character class
        /// </summary>
        /// <param name="ampersands">allow ampersands</param>
        /// <param name="digits">allow digits</param>
        /// <param name="dots">allow dots</param>
        public IdentifierCharacterClass(bool ampersands = true, bool digits = false, bool dots = false) {
            allowAmpersand = ampersands;
            allowDigits = digits;
            allowDot = dots;
        }

        /// <summary>
        ///     test if a char is acceptable for an identifier
        /// </summary>
        /// <param name="input">input</param>
        /// <returns><c>true</c> if the char can be part of an identifier</returns>
        public override bool Matches(char input) =>
            (input >= 'A' && input <= 'Z') ||
            (input >= 'a' && input <= 'z') ||
            input == '_' ||
            (allowAmpersand && input == '&') ||
            (allowDigits && (input >= '0' && input <= '9')) ||
            (allowDot && input == '.') ||
            (input > 127 && ((allowDigits && char.IsLetterOrDigit(input)) || char.IsLetter(input)));
    }

}
