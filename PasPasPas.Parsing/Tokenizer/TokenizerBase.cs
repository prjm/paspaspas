using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using System;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     helper methods for tokenizers
    /// </summary>
    public static class TokenizerHelper {

        /// <summary>
        ///     create a pseudo-token for the current input file
        /// </summary>
        /// <param name="tokenKind">token kind</param>
        /// <param name="tokenizer">tokenizer</param>
        /// <returns>pseudotoken (empty)</returns>
        public static Token CreatePseudoToken(this ITokenizer tokenizer, int tokenKind) {
            var result = new Token() {
                FilePath = null,
                Kind = tokenKind,
                Value = string.Empty
            };
            return result;
        }
    }

    /// <summary>
    ///     base class for tokenizers
    /// </summary>
    public abstract class TokenizerBase : ITokenizer {

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
        ///     message: unexptected end of token
        /// </summary>
        /// <remarks>
        ///     data: expected-token-end sequence
        /// </remarks>
        public static readonly Guid UnexpectedEndOfToken
            = new Guid("{9FADE757-DA74-4DE8-8346-EBCC04E173DD}");

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        protected TokenizerBase(ParserServices environment, StackedFileReader input) {

            if (environment == null)
                throw new ArgumentNullException(nameof(environment));

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            Environment = environment;
            Input = input;
            LogSource = new LogSource(environment.Log, TokenizerLogMessage);
            Lines = new LineCounters();
        }

        /// <summary>
        ///     check if tokens are availiable
        /// </summary>
        /// <returns><c>true</c> if tokens are avaliable</returns>
        public bool HasNextToken() {
            return Input.CurrentFile != null;
        }

        /// <summary>
        ///     generates an undefined token
        /// </summary>
        /// <param name="currentChar"></param>
        /// <param name="file">file</param>
        /// <returns></returns>
        protected Token GenerateUndefinedToken(char currentChar, IFileReference file) {
            var value = new string(currentChar, 1);
            LogSource.Error(UnexpectedCharacter, value);
            return null; // new PascalToken(PascalToken.Undefined, value, file);
        }

        /// <summary>
        ///     used char classes
        /// </summary>
        protected abstract InputPatterns CharacterClasses { get; }

        /// <summary>
        ///     fetch the next token
        /// </summary>
        /// <returns>next token</returns>
        public virtual Token FetchNextToken() {
            Token result;

            result = CharacterClasses.FetchNextToken(Input, LogSource);

            Lines.ProcessToken(result);
            return result;
        }

        /// <summary>
        ///     environment
        /// </summary>
        public ParserServices Environment { get; }

        /// <summary>
        ///     log source
        /// </summary>
        public LogSource LogSource { get; }

        /// <summary>
        ///     line counters
        /// </summary>
        public LineCounters Lines { get; }

        /// <summary>
        ///     file input
        /// </summary>
        public StackedFileReader Input { get; private set; }
    }
}