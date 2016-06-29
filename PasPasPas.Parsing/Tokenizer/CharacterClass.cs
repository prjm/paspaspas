namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for a character class
    /// </summary>
    public abstract class CharacterClass {

        /// <summary>
        ///     test if the character class matches
        /// </summary>
        /// <param name="input">character to match</param>
        /// <returns></returns>
        public abstract bool Matches(char input);

    }

    /// <summary>
    ///     character class with prefix
    /// </summary>
    public abstract class PrefixedCharacterClass : CharacterClass {

        /// <summary>
        ///     prefix
        /// </summary>
        public abstract char Prefix { get; }

    }

    /// <summary>
    ///     character class for a single char
    /// </summary>
    public class SingleCharClass : PrefixedCharacterClass {

        private readonly char match;

        /// <summary>
        ///     char to match
        /// </summary>
        /// <param name="forValue">char to match</param>
        public SingleCharClass(char forValue) {
            match = forValue;
        }

        /// <summary>
        ///     getsthe single character
        /// </summary>
        public override char Prefix
            => match;

        /// <summary>
        ///     test if the char matches
        /// </summary>
        /// <param name="input">char to test</param>
        /// <returns></returns>
        public override bool Matches(char input) =>
            input == match;
    }

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

    /// <summary>
    ///     matches whitespace
    /// </summary>
    public class WhitspaceCharacterClass : CharacterClass {

        /// <summary>
        ///     test if the char is whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => char.IsWhiteSpace(input);
    }

    /// <summary>
    ///     character class for numbers
    /// </summary>
    public class NumberCharacterClass : CharacterClass {

        /// <summary>
        ///     test if the char is a number
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => char.IsNumber(input);

    }

    /// <summary>
    ///     character class for exponents
    /// </summary>
    public class ExponentCharacterClass : CharacterClass {

        /// <summary>
        ///     test if a charactrer class matches
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input) =>
            input == 'E' ||
            input == 'e';
    }

    /// <summary>
    ///     character class to match +/
    /// </summary>
    public class PlusMinusCharacterClass : CharacterClass {

        /// <summary>
        ///     test if a charactrer class matches
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input) =>
            input == '+' ||
            input == '-';
    }

    /// <summary>
    ///     character class to match identifiers
    /// </summary>
    public class IdentifierCharacterClass : CharacterClass {

        /// <summary>
        ///     allow &amp;   
        /// </summary>
        public bool AllowAmpersand { get; internal set; }
            = true;

        /// <summary>
        ///     allow digits
        /// </summary>
        public bool AllowDigits { get; internal set; }
            = false;

        /// <summary>
        ///     allow dots
        /// </summary>
        public bool AllowDots { get; internal set; }
            = false;

        /// <summary>
        ///     test if a char is accecpable for an identifier
        /// </summary>
        /// <param name="input">input</param>
        /// <returns><c>true</c> if the char can be part of an identifier</returns>
        public override bool Matches(char input) =>
            char.IsLetter(input) ||
            input == '_' ||
            (AllowAmpersand && input == '&') ||
            (AllowDigits && char.IsDigit(input)) ||
            (AllowDots && input == '.');
    }

}
