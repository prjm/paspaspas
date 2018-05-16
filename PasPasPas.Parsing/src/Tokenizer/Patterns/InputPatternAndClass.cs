using System;
using PasPasPas.Parsing.Tokenizer.CharClass;

namespace PasPasPas.Parsing.Tokenizer.Patterns {

    /// <summary>
    ///     manually group token group values and their character classes
    /// </summary>
    public class InputPatternAndClass {

        /// <summary>
        ///     combination of input pattern and character class
        /// </summary>
        /// <param name="chrClass">character class</param>
        /// <param name="value">group value (tokenizer)</param>
        public InputPatternAndClass(CharacterClass chrClass, InputPattern value) {
            CharClass
                = chrClass ?? throw new ArgumentNullException(nameof(chrClass));

            GroupValue
                = value ?? throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        ///     character class
        /// </summary>
        public CharacterClass CharClass { get; }

        /// <summary>
        ///     tokenizer group        
        /// </summary>
        public InputPattern GroupValue { get; }
    }

}
