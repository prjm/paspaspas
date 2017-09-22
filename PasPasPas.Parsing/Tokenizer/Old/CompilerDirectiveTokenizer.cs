using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     helper for compiler directives
    /// </summary>
    public static class CompilerDirectiveTokenizer {

        /// <summary>
        ///     unwrap a preprocessor command
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Unwrap1(string value) {
            var startOffset = 0;
            var endOffset = 0;

            if (value.StartsWith("{", StringComparison.Ordinal))
                startOffset = 2;
            else if (value.StartsWith("(*", StringComparison.Ordinal))
                startOffset = 3;

            if (value.EndsWith("}", StringComparison.Ordinal))
                endOffset = 1;
            else if (value.EndsWith("*)", StringComparison.Ordinal))
                endOffset = 2;

            return value.Substring(startOffset, value.Length - startOffset - endOffset);
        }

    }
}