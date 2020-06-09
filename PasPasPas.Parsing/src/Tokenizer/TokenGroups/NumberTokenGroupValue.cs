using System.Collections.Generic;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;

namespace PasPasPas.Parsing.Tokenizer.TokenGroups {

    /// <summary>
    ///     token group for numbers
    /// </summary>
    public sealed class NumberTokenGroupValue : PatternContinuation {

        private readonly CharacterClassTokenGroupValue digitTokenizer
            = new CharacterClassTokenGroupValue(TokenKind.IntegralNumber, new DigitCharClass(false), 0, LiteralParserKind.IntegerNumbers, 0);

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
                var content = state.GetBufferContent();
                var number = state.ParserLiteral(content, LiteralParserKind.IntegerNumbers);
                return new Token(TokenKind.IntegralNumber, content, number);
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
                    decimals = digitTokenizer.Tokenize(state).ParsedValue.ToValueString();
                }
            }

            var nextChar = state.LookAhead();

            if (nextChar == 'E' || nextChar == 'e') {
                state.NextChar(true);
                if (state.AtEof) {
                    state.Error(MessageNumbers.UnexpectedEndOfToken);
                }
                else {
                    nextChar = state.LookAhead();
                    minus = nextChar == '-';
                    if (nextChar == '-' || nextChar == '+')
                        state.NextChar(true);

                    if (!state.AtEof) {
                        if (digitTokenizer.CharClass.Matches(state.LookAhead())) {
                            exp = digitTokenizer.Tokenize(state).ParsedValue.ToValueString();
                        }
                        else {
                            state.Error(MessageNumbers.UnexpectedEndOfToken);
                        }
                    }
                    else {
                        state.Error(MessageNumbers.UnexpectedEndOfToken);
                    }
                }

                withExponent = true;
            }

            if (AllowIdents && allIdents.Matches(state.LookAhead())) {
                identTokenizer.Tokenize(state);
                return new Token(TokenKind.Identifier, state.GetBufferContent());
            }

            if (withDot || withExponent) {
                var literalValue = string.Concat(digits, ".", decimals, "E", minus ? "-" : "+", exp);
                return new Token(TokenKind.RealNumber, state.GetBufferContent(), state.ConvertRealLiteral(literalValue));
            }
            else {
                return new Token(TokenKind.IntegralNumber, state.GetBufferContent(), digitValue);
            }
        }

    }

}