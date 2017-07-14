﻿namespace PasPasPas.Parsing.Tokenizer {
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

}
