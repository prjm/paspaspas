using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {
    /// <summary>
    ///     token group for identifiers
    /// </summary>
    public class IdentifierTokenGroupValue : PatternContinuation {

        private IdentifierCharacterClass identifierCharClass
            = new IdentifierCharacterClass() { AllowAmpersand = false, AllowDigits = true };

        /// <summary>
        ///     if <c>true</c> asm blocks are parsed as a token
        /// </summary>
        public bool ParseAsm { get; set; }
            = false;

        /// <summary>
        ///     allow dots in identifiers
        /// </summary>
        public bool AllowDots {
            get { return identifierCharClass.AllowDots; }
            set { identifierCharClass.AllowDots = value; }
        }

        /// <summary>
        ///     allow digits in identifiers
        /// </summary>
        public bool AllowDigits {
            get { return identifierCharClass.AllowDigits; }
            set { identifierCharClass.AllowDigits = value; }
        }

        /// <summary>
        ///     allow ampersand in identifiers
        /// </summary>
        public bool AllowAmpersand {
            get { return identifierCharClass.AllowAmpersand; }
            set { identifierCharClass.AllowAmpersand = value; }
        }

        private readonly IDictionary<string, int> knownKeywords;

        /// <summary>
        ///     create a new token group for ids and keywords
        /// </summary>
        /// <param name="keywords"></param>
        public IdentifierTokenGroupValue(IDictionary<string, int> keywords) {

            if (keywords == null)
                throw new ArgumentNullException(nameof(keywords));

            knownKeywords = keywords;
        }

        /// <summary>
        ///     parse the complete token
        /// </summary>
        /// <param name="state">current tokenizer state</param>
        public override Token Tokenize(ITokenizerState state) {
            var hasAmpersand = state.GetBufferCharAt(0) == '&';
            var ignoreKeywords = AllowAmpersand && hasAmpersand;

            if (ignoreKeywords)
                state.NextChar(false);
            else if (hasAmpersand)
                return new Token(TokenKind.Undefined, state);

            while (!state.AtEof) {
                var currentChar = state.CurrentCharacter;
                if (!identifierCharClass.Matches(currentChar))
                    break;
                state.Append(currentChar);
                state.NextChar(false);
            }

            int tokenKind;

            if (hasAmpersand && state.Length < 2)
                state.Error(StandardTokenizer.IncompleteIdentifier);

            var value = state.GetBufferContent();

            if ((!ignoreKeywords) && (knownKeywords.TryGetValue(value, out tokenKind)))
                return new Token(tokenKind, state);
            else
                return new Token(TokenKind.Identifier, state);

        }


    }

}
