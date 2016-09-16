﻿namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     Quoted string
    /// </summary>
    public class QuotedString : SyntaxPartBase {

        /// <summary>
        ///     quoted value
        /// </summary>
        public string QuotedValue
            => UnquotedValue;

        /// <summary>
        ///     unquoted value
        /// </summary>
        public string UnquotedValue { get; set; }
            = string.Empty;

    }
}