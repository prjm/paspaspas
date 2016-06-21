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
            LogSource = new LogSource(environment.Log, TokenizerLogMessage, Messages.ResourceManager);
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
        /// <param name="file">file</param>
        /// <returns></returns>
        protected PascalToken GenerateUndefinedToken(char currentChar, IFileReference file) {
            var value = new string(currentChar, 1);
            LogSource.Error(UnexpectedCharacter, value);
            return new PascalToken(PascalToken.Undefined, value, file);
        }

        /// <summary>
        ///     generates an eof token
        /// </summary>
        /// <param name="file">file reference</param>
        /// <returns></returns>
        protected static PascalToken GenerateEofToken(IFileReference file)
            => new PascalToken(PascalToken.Eof, string.Empty, file);


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
                return GenerateEofToken(new FileReference(string.Empty));
            }

            var file = Input.CurrentInputFile.FilePath;
            char c = Input.FetchChar();
            PunctuatorGroup tokenGroup;

            if (CharacterClasses.Match(c, out tokenGroup)) {
                return Punctuators.FetchTokenByGroup(Input, c, tokenGroup);
            }

            return GenerateUndefinedToken(c, file);
        }

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