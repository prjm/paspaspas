using System;
using System.Text;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     simple token group value: no more characters
    /// </summary>
    public class SimpleTokenGroupValue : PatternContinuation {


        /// <summary>
        ///     creates a new simple token without suffix
        /// </summary>
        /// <param name="tokenValue"></param>
        public SimpleTokenGroupValue(int tokenValue) : base()
            => TokenId = tokenValue;

        /// <summary>
        ///     token kind
        /// </summary>
        public int TokenId { get; set; }

        /// <summary>
        ///     create a simple token
        /// </summary>
        public override Token Tokenize(StringBuilder buffer, int position, ITokenizer tokenizer)
           => new Token(TokenId, position, buffer);


    }
}