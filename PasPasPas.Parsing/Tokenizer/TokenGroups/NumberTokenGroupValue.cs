using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for numbers
    /// </summary>
    public sealed class NumberTokenGroupValue : PatternContinuation {

        private readonly CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.Integer, new DigitCharClass(false), 0, LiteralParserKind.IntegerNumbers, Guid.Empty);

        private readonly IdentifierCharacterClass allIdents
            = new IdentifierCharacterClass(ampersands: true, digits: true, dots: true);

        private readonly IdentifierTokenGroupValue identTokenizer
            = new IdentifierTokenGroupValue(new Dictionary<string, int>());

        /// <summary>
        ///     flag, if <c>true</c> idents are generated if possible
        /// </summary>d
        public bool AllowIdents { get; set; }
            = false;

        /// <summary>
        ///     tokenize a number
        /// </summary>
        /// <param name="state">tokenizer state</param>
        /// <returns>created token</returns>
        public override Token Tokenize(TokenizerState state) {
            var withDot = false;
            var withExponent = false;

            if (state.AtEof) {
                var number = state.ParserLiteral(state.GetBufferContent(), LiteralParserKind.IntegerNumbers);
                return new Token(TokenKind.Integer, state, number);
            }

            state.Clear();
            state.PreviousChar();
            var digitToken = digitTokenizer.Tokenize(state);
            var digits = digitToken.Value;
            var digitValue = digitToken.ParsedValue;
            var decimals = "0";
            var exp = "0";
            var minus = false;

            if (state.LookAhead(1) == '.') {
                if (digitTokenizer.CharClass.Matches(state.LookAhead(2))) {
                    state.NextChar(true);
                    withDot = true;
                    decimals = digitTokenizer.Tokenize(state).ParsedValue.ToString();
                }
            }

            var nextChar = state.LookAhead();

            if (nextChar == 'E' || nextChar == 'e') {
                state.NextChar(true);
                if (state.AtEof) {
                    state.Error(Tokenizer.UnexpectedEndOfToken);
                }
                else {
                    nextChar = state.LookAhead();
                    minus = nextChar == '-';
                    if (nextChar == '-' || nextChar == '+')
                        state.NextChar(true);

                    if (!state.AtEof) {
                        if (digitTokenizer.CharClass.Matches(state.LookAhead())) {
                            exp = digitTokenizer.Tokenize(state).ParsedValue.ToString();
                        }
                        else {
                            state.Error(Tokenizer.UnexpectedEndOfToken);
                        }
                    }
                    else {
                        state.Error(Tokenizer.UnexpectedEndOfToken);
                    }
                }

                withExponent = true;
            }

            if (AllowIdents && allIdents.Matches(state.LookAhead())) {
                identTokenizer.Tokenize(state);
                return new Token(TokenKind.Identifier, state);
            }

            if (withDot || withExponent) {
                var literalValue = string.Concat(digits, ".", decimals, "E", minus ? "-" : "+", exp);
                return new Token(TokenKind.Real, state, state.ConvertRealLiteral(literalValue));
            }
            else {
                return new Token(TokenKind.Integer, state, digitValue);
            }
        }

    }

}