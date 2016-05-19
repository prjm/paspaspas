using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Parser;
using System;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     base class for tokenizers
    /// </summary>
    public abstract class TokenizerBase {

        /// <summary>
        ///     message group for tokenizer logs
        /// </summary>
        public static readonly Guid TokenizerLogMessage
            = new Guid("{1E7738B4-6758-4493-B4AC-654353CF7228}");

        /// <summary>
        ///     message: unexpected token
        /// </summary>    
        public static readonly Guid UnexpectedCharacter
            = new Guid("{FA4EBD35-325B-4869-A2CD-B21EE430BAC4}");

        /// <summary>
        ///     dummy constructor
        /// </summary>
        protected TokenizerBase(ParserServices environment) {
            Environment = environment;
            LogSource = new LogSource(environment.Environment.LogManager, TokenizerLogMessage, Messages.ResourceManager);
        }

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
            LogSource.Error(UnexpectedCharacter, value);
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

        /// <summary>
        ///     environment
        /// </summary>
        public ParserServices Environment { get; }

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource LogSource { get; }
    }
}