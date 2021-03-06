﻿#nullable disable
namespace PasPasPas.Parsing.Tokenizer.CharClass {

    /// <summary>
    ///     character class to match whitespace chars
    /// </summary>
    public sealed class WhiteSpaceCharacterClass : CharacterClass {

        /// <summary>
        ///     test if a given character is an ASCII whitespace
        /// </summary>
        /// <param name="input">input char</param>
        /// <returns><c>true</c> if the passed character is 9, 10, 11, 12, or 13</returns>
        public static bool IsAsciiWhitespace(char input)
            => input >= 9 && input <= 13;

        /// <summary>
        ///     test if the char is whitespace
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public override bool Matches(char input)
            => IsAsciiWhitespace(input) || char.IsWhiteSpace(input);

    }
}
