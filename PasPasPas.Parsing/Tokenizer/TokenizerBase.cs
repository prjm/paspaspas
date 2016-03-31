using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for tokenizers
    /// </summary>
    public abstract class TokenizerBase : MessageGenerator {

        /// <summary>
        ///     dummy constructor
        /// </summary>
        protected TokenizerBase() { }

        /// <summary>
        ///     parser parser input
        /// </summary>
        public StackedFileReader Input { get; set; }

        /// <summary>
        ///     check if tokens are availiable
        /// </summary>
        /// <returns><c>true</c> if tokens are avaliable</returns>
        public bool HasNextToken()
            => !Input.AtEof;

        /// <summary>
        ///     generates an undefined token
        /// </summary>
        /// <param name="currentChar"></param>
        /// <returns></returns>
        protected PascalToken GenerateUndefinedToken(char currentChar) {
            var value = new string(currentChar, 1);
            LogError(MessageData.UndefinedInputToken, value);
            return new PascalToken() {
                Value = value,
                Kind = PascalToken.Undefined
            };
        }

        /// <summary>
        ///     generates an eof token
        /// </summary>
        /// <returns></returns>
        protected static PascalToken GenerateEofToken()
            => new PascalToken() { Kind = PascalToken.Eof, Value = string.Empty };


        /// <summary>
        ///     used char classes
        /// </summary>
        protected abstract Punctuators CharacterClasses { get; }


        /// <summary>
        ///     fetch the next token
        /// </summary>
        /// <returns>next token</returns>
        public PascalToken FetchNextToken() {
            if (Input.AtEof) {
                return GenerateEofToken();
            }

            char c = Input.FetchChar();
            PunctuatorGroup tokenGroup;

            if (CharacterClasses.Match(c, out tokenGroup)) {
                return Punctuators.FetchTokenByGroup(Input, c, tokenGroup);
            }

            return GenerateUndefinedToken(c);
        }

        /// <summary>
        ///     get the currently read file
        /// </summary>
        public IFile CurrentFile
            => Input.CurrentFile;
    }
}