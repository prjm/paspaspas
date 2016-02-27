using System;
using System.Collections.Generic;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     helper for compiler directives
    /// </summary>
    public class MacroProcessor {

        private PreprocessorPunctuators punctuators
            = new PreprocessorPunctuators();



        public MacroProcessor() {
        }

        private static string Unwrap(string value) {
            int startOffset = 0;
            var endOffset = 0;

            if (value.StartsWith("{$"))
                startOffset = 2;
            else if (value.StartsWith("(*$"))
                startOffset = 3;

            if (value.EndsWith("}"))
                endOffset = 1;
            else if (value.EndsWith("*)"))
                endOffset = 2;

            return value.Substring(startOffset, value.Length - startOffset - endOffset);
        }

        /// <summary>
        ///     process directive
        /// </summary>
        /// <param name="value"></param>
        public void ProcessValue(string value) {
            var directive = Unwrap(value);

        }
    }
}