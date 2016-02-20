namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     base class for a character class
    /// </summary>
    public abstract class CharacterClass {

        /// <summary>
        ///     prefix
        /// </summary>
        public abstract char Prefix { get; }

        /// <summary>
        ///     test if the character class matches
        /// </summary>
        /// <param name="input">character to match</param>
        /// <returns></returns>
        public abstract bool Matches(char input);

    }


    /// <summary>
    ///     character class for a single char
    /// </summary>
    public class SingleCharClass : CharacterClass {

        private readonly char match;

        /// <summary>
        ///     char to match
        /// </summary>
        /// <param name="forChar">char to match</param>
        public SingleCharClass(char forChar) {
            match = forChar;
        }

        /// <summary>
        ///     getsthe single character
        /// </summary>
        public override char Prefix
            => match;

        /// <summary>
        ///     test if the char matches
        /// </summary>
        /// <param name="c">char to test</param>
        /// <returns></returns>
        public override bool Matches(char c) =>
            c == match;
    }

    /// <summary>
    ///     matches whitespace
    /// </summary>
    public class WhitspaceCharacterClass : CharacterClass {

        /// <summary>
        ///     undefined prefix
        /// </summary>
        public override char Prefix => '\0';

        /// <summary>
        ///     test if the char is whitespace
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool Matches(char c)
            => char.IsWhiteSpace(c);
    }

    /// <summary>
    ///     character class for numbers
    /// </summary>
    public class NumberCharacterClass : CharacterClass {

        /// <summary>
        ///     undefined prefix
        /// </summary>
        public override char Prefix => '\0';

        /// <summary>
        ///     test if the char is whitespace
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public override bool Matches(char c)
            => ('0' <= c) && (c <= '9');

    }

    public class ExponentCharacterClass : CharacterClass {

        /// <summary>
        ///     undefined prefix
        /// </summary>
        public override char Prefix => '\0';

        /// <summary>
        ///     test if a charactrer class matches
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => input == 'E' || input == 'e';
    }

    /// <summary>
    ///     character class to match +/
    /// </summary>
    public class PlusMinusCharacterClass : CharacterClass {

        /// <summary>
        ///     undefined prefix
        /// </summary>
        public override char Prefix => '\0';

        /// <summary>
        ///     test if a charactrer class matches
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => input == '+' || input == '-';
    }

}
