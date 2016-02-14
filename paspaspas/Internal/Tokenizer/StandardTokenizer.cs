using PasPasPas.Api;
using System;
using System.Collections.Generic;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     Standard tokenizer
    /// </summary>
    public class StandardTokenizer : MessageGenerator, IPascalTokenizer {

        private bool hasEofMessage = false;

        /// <summary>
        ///     punctuators
        /// </summary>
        private Dictionary<char, PunctuatorGroup> punctuators =
            new Dictionary<char, PunctuatorGroup>();

        /// <summary>
        ///     keywords
        /// </summary>
        private static Dictionary<string, int> keywords =
            new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                ["program"] = PascalToken.Program,
                ["uses"] = PascalToken.Uses,
                ["in"] = PascalToken.In,
                ["const"] = PascalToken.Const,
                ["pointer"] = PascalToken.Pointer,
                ["array"] = PascalToken.Array,
                ["of"] = PascalToken.Of,
                ["type"] = PascalToken.TypeKeyword,
                ["packed"] = PascalToken.Packed,
                ["experimental"] = PascalToken.Experimental,
                ["deprecated"] = PascalToken.Deprecated,
                ["platform"] = PascalToken.Platform,
                ["library"] = PascalToken.Library,
                ["set"] = PascalToken.Set,
                ["file"] = PascalToken.File,
                ["class"] = PascalToken.Class,
                ["begin"] = PascalToken.Begin,
                ["end"] = PascalToken.End,
                ["sealed"] = PascalToken.Sealed,
                ["abstract"] = PascalToken.Abstract,
                ["strict"] = PascalToken.Strict,
                ["private"] = PascalToken.Private,
                ["published"] = PascalToken.Published,
                ["protected"] = PascalToken.Protected,
                ["automated"] = PascalToken.Automated,
                ["public"] = PascalToken.Public,
                ["constructor"] = PascalToken.Constructor,
                ["destructor"] = PascalToken.Destructor,
                ["function"] = PascalToken.Function,
                ["procedure"] = PascalToken.Procedure,
                ["record"] = PascalToken.Record,
                ["var"] = PascalToken.Var,
                ["out"] = PascalToken.Out,
                ["resourcestring"] = PascalToken.Resourcestring,
                ["reintroduce"] = PascalToken.Reintroduce,
                ["overload"] = PascalToken.Overload,
                ["message"] = PascalToken.Message,
                ["static"] = PascalToken.Static,
                ["dynamic"] = PascalToken.Dynamic,
                ["override"] = PascalToken.Override,
                ["virtual"] = PascalToken.Virtual,
                ["final"] = PascalToken.Final,
                ["inline"] = PascalToken.Inline,
                ["assembler"] = PascalToken.Assembler,
                ["cdecl"] = PascalToken.Cdecl,
                ["pascal"] = PascalToken.Pascal,
                ["register"] = PascalToken.Register,
                ["safecall"] = PascalToken.Safecall,
                ["stdcall"] = PascalToken.Stdcall,
                ["export"] = PascalToken.Export,
                ["far"] = PascalToken.Far,
                ["local"] = PascalToken.Local,
                ["near"] = PascalToken.Near,
                ["property"] = PascalToken.Property,
                ["index"] = PascalToken.Index,
                ["read"] = PascalToken.Read,
                ["write"] = PascalToken.Write,
                ["add"] = PascalToken.Add,
                ["remove"] = PascalToken.Remove,
                ["readonly"] = PascalToken.ReadOnly,
                ["writeonly"] = PascalToken.WriteOnly,
                ["dispid"] = PascalToken.DispId,
                ["default"] = PascalToken.Default,
                ["nodefault"] = PascalToken.NoDefault,
                ["stored"] = PascalToken.Stored,
                ["implements"] = PascalToken.Implements,
                ["contains"] = PascalToken.Contains,
                ["requires"] = PascalToken.Requires,
                ["package"] = PascalToken.Package,
                ["unit"] = PascalToken.Unit,
                ["interface"] = PascalToken.Interface,
                ["implementation"] = PascalToken.Implementation,
                ["initialization"] = PascalToken.Initialization,
                ["finalization"] = PascalToken.Finalization,
                ["asm"] = PascalToken.Asm,
                ["label"] = PascalToken.Label,
                ["exports"] = PascalToken.Exports,
                ["assembly"] = PascalToken.Assembly,
                ["operator"] = PascalToken.Operator,
                ["absolute"] = PascalToken.Absolute,
                ["name"] = PascalToken.Name,
                ["resident"] = PascalToken.Resident,
                ["shortstring"] = PascalToken.ShortString,
                ["ansistring"] = PascalToken.AnsiString,
                ["string"] = PascalToken.String,
                ["widestring"] = PascalToken.WideString,
                ["unicodestring"] = PascalToken.UnicodeString,
                ["object"] = PascalToken.Object,
                ["to"] = PascalToken.To,
                ["reference"] = PascalToken.Reference,
                ["helper"] = PascalToken.Helper,
                ["dispinterface"] = PascalToken.DispInterface,
                ["for"] = PascalToken.For,
                ["and"] = PascalToken.And,
                ["array"] = PascalToken.Array,
                ["as"] = PascalToken.As,
                ["asm"] = PascalToken.Asm,
                ["begin"] = PascalToken.Begin,
                ["case"] = PascalToken.Case,
                ["class"] = PascalToken.Class,
                ["const"] = PascalToken.Const,
                ["constructor"] = PascalToken.Constructor,
                ["destructor"] = PascalToken.Destructor,
                ["dispinterface"] = PascalToken.DispInterface,
                ["div"] = PascalToken.Div,
                ["do"] = PascalToken.Do,
                ["downto"] = PascalToken.DownTo,
                ["else"] = PascalToken.Else,
                ["end"] = PascalToken.End,
                ["except"] = PascalToken.Except,
                ["exports"] = PascalToken.Exports,
                ["file"] = PascalToken.File,
                ["finalization"] = PascalToken.Finalization,
                ["finally"] = PascalToken.Finally,
                ["for"] = PascalToken.For,
                ["function"] = PascalToken.Function,
                ["goto"] = PascalToken.GoToKeyword,
                ["if"] = PascalToken.If,
                ["implementation"] = PascalToken.Implementation,
                ["in"] = PascalToken.In,
                ["inherited"] = PascalToken.Inherited,
                ["initialization"] = PascalToken.Initialization,
                ["inline"] = PascalToken.Inline,
                ["interface"] = PascalToken.Interface,
                ["is"] = PascalToken.Is,
                ["label"] = PascalToken.Label,
                ["library"] = PascalToken.Library,
                ["mod"] = PascalToken.Mod,
                ["nil"] = PascalToken.Nil,
                ["not"] = PascalToken.Not,
                ["of"] = PascalToken.Of,
                ["or"] = PascalToken.Or,
                ["packed"] = PascalToken.Packed,
                ["procedure"] = PascalToken.Procedure,
                ["program"] = PascalToken.Program,
                ["property"] = PascalToken.Property,
                ["raise"] = PascalToken.Raise,
                ["record"] = PascalToken.Record,
                ["repeat"] = PascalToken.Repeat,
                ["resourcestring"] = PascalToken.Resourcestring,
                ["set"] = PascalToken.Set,
                ["shl"] = PascalToken.Shl,
                ["shr"] = PascalToken.Shr,
                ["string"] = PascalToken.String,
                ["then"] = PascalToken.Then,
                ["threadvar"] = PascalToken.ThreadVar,
                ["to"] = PascalToken.To,
                ["try"] = PascalToken.Try,
                ["unit"] = PascalToken.Unit,
                ["until"] = PascalToken.Until,
                ["uses"] = PascalToken.Uses,
                ["var"] = PascalToken.Var,
                ["while"] = PascalToken.While,
                ["with"] = PascalToken.With,
                ["xor"] = PascalToken.Xor,
                ["varargs"] = PascalToken.VarArgs,
                ["external"] = PascalToken.External,
                ["forward"] = PascalToken.Forward,
                ["true"] = PascalToken.True,
                ["false"] = PascalToken.False,
                ["exit"] = PascalToken.Exit,
                ["break"] = PascalToken.Break,
                ["continue"] = PascalToken.Continue,
                ["at"] = PascalToken.At,
                ["on"] = PascalToken.On
            };

        internal static bool IsKeyword(string value)
            => keywords.ContainsKey(value);

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        public StandardTokenizer() {
            RegisterPuncutators();
        }

        private void RegisterPuncutators() {
            AddPunctuator('.', PascalToken.Dot).Add('.', PascalToken.DotDot);
            AddPunctuator(',', PascalToken.Comma);
            AddPunctuator('(', PascalToken.OpenParen);
            AddPunctuator(')', PascalToken.CloseParen);
            AddPunctuator(';', PascalToken.Semicolon);
            AddPunctuator('=', PascalToken.EqualsSign);
            AddPunctuator('[', PascalToken.OpenBraces);
            AddPunctuator(']', PascalToken.CloseBraces);
            AddPunctuator(':', PascalToken.Colon).Add('=', PascalToken.Assignment);
            AddPunctuator('^', PascalToken.Circumflex);
            AddPunctuator('+', PascalToken.Plus);
            AddPunctuator('-', PascalToken.Minus);
            AddPunctuator('*', PascalToken.Times);
            AddPunctuator('/', PascalToken.Slash);
            AddPunctuator('@', PascalToken.At);
            AddPunctuator('>', PascalToken.GreaterThen).Add('=', PascalToken.GreaterThenEquals);
            var lt = AddPunctuator('<', PascalToken.LessThen);
            lt.Add('=', PascalToken.LessThenEquals);
            lt.Add('>', PascalToken.NotEquals);
        }

        private PunctuatorGroup AddPunctuator(char prefix, int tokenValue) {
            var result = new PunctuatorGroup(prefix, tokenValue);
            punctuators.Add(prefix, result);
            return result;
        }

        /// <summary>
        ///     parser parser input
        /// </summary>
        public IParserInput Input { get; set; }

        /// <summary>
        ///     fetch the next token
        /// </summary>
        /// <returns>next token</returns>
        public PascalToken FetchNextToken() {
            if (Input.AtEof()) {
                return GenerateEofToken();
            }

            char c = Input.NextChar();
            PunctuatorGroup tokenGroup;

            if (char.IsWhiteSpace(c)) {
                return ParseWhitespace(c);
            }

            if (punctuators.TryGetValue(c, out tokenGroup)) {
                return FetchTokenByGroup(tokenGroup);
            }

            if (IsDigit(c)) {
                return ParseNumber(c);
            }

            if (char.IsLetter(c) || c == '_' || c == '&') {
                return ParseIdentifier(c);
            }

            if (c == '$') {
                return ParseHexNumber(c);
            }

            if (c == '\'') {
                return ParseQuotedString(c);
            }
            return GenerateUndefinedToken(c);
        }

        private PascalToken FetchTokenByGroup(PunctuatorGroup tokenGroup) {
            string input = new string(tokenGroup.Prefix, 1);
            while (input.Length < tokenGroup.Length && (!Input.AtEof())) {
                input = input + Input.NextChar();
            }

            string tokenValue;
            var tokenKind = tokenGroup.Match(input, out tokenValue);

            for (int inputIndex = input.Length - 1; inputIndex >= tokenValue.Length; inputIndex--) {
                Input.Putback(input[inputIndex]); ;
            }

            return new PascalToken() { Kind = tokenKind, Value = tokenValue };
        }

        private PascalToken GenerateUndefinedToken(char c) {
            var value = new string(c, 1);
            LogError(MessageData.UndefinedInputToken, value);
            return new PascalToken() {
                Value = value,
                Kind = PascalToken.Undefined
            };
        }

        private PascalToken GenerateEofToken() {
            if (!hasEofMessage) {
                LogError(MessageData.UnexpectedEndOfFile);
                hasEofMessage = true;
            }
            return new PascalToken() { Kind = PascalToken.Eof, Value = string.Empty };
        }

        private PascalToken ParseQuotedString(char currentChar) {
            string value = string.Empty;
            currentChar = Input.NextChar();
            while (currentChar != '\'' && (!Input.AtEof())) {
                value = value + currentChar;
                currentChar = Input.NextChar();
            }

            return new PascalToken() { Value = value, Kind = PascalToken.QuotedString };
        }

        private PascalToken ParseWhitespace(char currentChar) {
            string value = new string(currentChar, 1);
            while (char.IsWhiteSpace(currentChar) && (!Input.AtEof())) {
                currentChar = Input.NextChar();
                if (char.IsWhiteSpace(currentChar))
                    value = value + currentChar;
                else
                    Input.Putback(currentChar);
            }

            return new PascalToken() { Value = value, Kind = PascalToken.WhiteSpace };
        }

        private PascalToken ParseHexNumber(char currentChar) {
            string value = new string(currentChar, 1);
            bool stop = false;

            if (Input.AtEof()) {
                LogError(MessageData.IncompleteHexNumber);
                return new PascalToken() { Kind = PascalToken.Eof, Value = string.Empty };
            }

            do {
                currentChar = Input.NextChar();
                if (IsHexDigit(currentChar)) {
                    value = value + currentChar;
                }
                else {
                    Input.Putback(currentChar);
                    stop = true;
                }
            } while (IsHexDigit(currentChar) && (!Input.AtEof()) && (!stop));

            return new PascalToken() { Value = value, Kind = PascalToken.HexNumber };
        }

        private PascalToken ParseNumber(char currentChar) {
            bool hasDot = false;
            bool hasE = false;
            bool stop = false;
            string value = new string(currentChar, 1);

            while ((char.IsDigit(currentChar) || currentChar == 'E' || currentChar == 'e' || currentChar == '.' || currentChar == '+' || currentChar == '-') && (!Input.AtEof()) && (!stop)) {
                currentChar = Input.NextChar();
                if (char.IsDigit(currentChar)) {
                    value = value + currentChar;
                }
                else if ((currentChar == '.') && (!hasDot)) {
                    if (Input.AtEof()) {
                        stop = true;
                        Input.Putback('.');
                    }
                    else {
                        var nextChar = Input.NextChar();
                        if (!IsDigit(nextChar)) {
                            stop = true;
                            Input.Putback(nextChar);
                            Input.Putback('.');
                        }
                        else {
                            Input.Putback(nextChar);
                            hasDot = true;
                            value = value + currentChar;
                        }
                    }
                }
                else if (((currentChar == 'E') || (currentChar == 'e')) && (!hasE)) {
                    if (Input.AtEof()) {
                        Input.Putback(currentChar);
                        stop = true;
                    }
                    else {
                        hasE = true;
                        value = value + currentChar;
                        currentChar = Input.NextChar();
                        if (currentChar == '+' || currentChar == '-')
                            value = value + currentChar;
                        else {
                            Input.Putback(currentChar);
                            stop = true;
                        }

                    }
                }
                else {
                    Input.Putback(currentChar);
                    stop = true;
                }
            }

            if (hasDot || hasE)
                return new PascalToken() { Value = value, Kind = PascalToken.Real };
            else
                return new PascalToken() { Value = value, Kind = PascalToken.Integer };
        }

        private PascalToken ParseIdentifier(char currentChar) {
            string value;
            int tokenKind;
            bool ignoreKeywords;

            if (currentChar == '&') {
                ignoreKeywords = true;
                if (Input.AtEof()) {
                    LogError(MessageData.IncompleteIdentifier);
                    return new PascalToken() { Value = string.Empty, Kind = PascalToken.Eof };
                }
                else {
                    currentChar = Input.NextChar();
                }
            }
            else {
                ignoreKeywords = false;
            }

            value = new string(currentChar, 1);

            while ((char.IsLetter(currentChar) || char.IsDigit(currentChar) || currentChar == '_') && (!Input.AtEof())) {
                currentChar = Input.NextChar();
                if (char.IsLetter(currentChar) || char.IsDigit(currentChar) || currentChar == '_')
                    value = value + currentChar;
                else
                    Input.Putback(currentChar);
            }

            if ((!ignoreKeywords) && (keywords.TryGetValue(value, out tokenKind)))
                return new PascalToken() { Value = value, Kind = tokenKind };
            else
                return new PascalToken() { Value = value, Kind = PascalToken.Identifier };
        }

        /// <summary>
        ///     check if tokens are availiable
        /// </summary>
        /// <returns><c>true</c> if tokens are avaliable</returns>
        public bool HasNextToken()
            => !Input.AtEof();


        private static bool IsDigit(char c)
            => ('0' <= c) && (c <= '9');

        private static bool IsHexDigit(char c)
            => ('0' <= c) && (c <= '9') ||
               ('a' <= c) && (c <= 'f') ||
               ('A' <= c) && (c <= 'F');
    }
}
