using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     Standard tokenizer
    /// </summary>
    public class StandardTokenizer : TokenizerBase, ITokenizer {

        /// <summary>
        ///     create a new tokenizer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="input">input files</param>
        public StandardTokenizer(ParserServices services, StackedFileReader input) : base(services, input) { }

        private StandardPatterns punctuators
            = new StandardPatterns();

        /// <summary>
        ///     message id: incomplete hex number
        /// </summary>
        public static readonly Guid IncompleteHexNumber
            = new Guid(new byte[] { 0xfc, 0x7b, 0x96, 0xb1, 0xaf, 0x9e, 0x7e, 0x4a, 0x88, 0x7c, 0xc9, 0xa9, 0xaa, 0xab, 0xa7, 0xa8 });
        /* {b1967bfc-9eaf-4a7e-887c-c9a9aaaba7a8} */

        /// <summary>
        ///     message id: incomplete identifier
        /// </summary>
        public static readonly Guid IncompleteIdentifier
            = new Guid(new byte[] { 0x68, 0x3f, 0x9c, 0x79, 0x2, 0xe0, 0xf3, 0x42, 0x98, 0x8a, 0xef, 0xa3, 0x59, 0x4f, 0xf9, 0x42 });
        /* {799c3f68-e002-42f3-988a-efa3594ff942} */

        /// <summary>
        ///     get punctuators
        /// </summary>
        protected override InputPatterns CharacterClasses
            => punctuators;

        /// <summary>
        ///     input patterns
        /// </summary>
        public InputPatterns Patterns
            => punctuators;

        /// <summary>
        ///     keywords
        /// </summary>
        public static IDictionary<string, int> Keywords { get; }
            = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                ["program"] = TokenKind.Program,
                ["uses"] = TokenKind.Uses,
                ["in"] = TokenKind.In,
                ["const"] = TokenKind.Const,
                ["pointer"] = TokenKind.Pointer,
                ["array"] = TokenKind.Array,
                ["of"] = TokenKind.Of,
                ["type"] = TokenKind.TypeKeyword,
                ["packed"] = TokenKind.Packed,
                ["experimental"] = TokenKind.Experimental,
                ["deprecated"] = TokenKind.Deprecated,
                ["platform"] = TokenKind.Platform,
                ["library"] = TokenKind.Library,
                ["set"] = TokenKind.Set,
                ["file"] = TokenKind.File,
                ["class"] = TokenKind.Class,
                ["begin"] = TokenKind.Begin,
                ["end"] = TokenKind.End,
                ["sealed"] = TokenKind.Sealed,
                ["abstract"] = TokenKind.Abstract,
                ["strict"] = TokenKind.Strict,
                ["private"] = TokenKind.Private,
                ["published"] = TokenKind.Published,
                ["protected"] = TokenKind.Protected,
                ["automated"] = TokenKind.Automated,
                ["public"] = TokenKind.Public,
                ["constructor"] = TokenKind.Constructor,
                ["destructor"] = TokenKind.Destructor,
                ["function"] = TokenKind.Function,
                ["procedure"] = TokenKind.Procedure,
                ["record"] = TokenKind.Record,
                ["var"] = TokenKind.Var,
                ["out"] = TokenKind.Out,
                ["resourcestring"] = TokenKind.Resourcestring,
                ["reintroduce"] = TokenKind.Reintroduce,
                ["overload"] = TokenKind.Overload,
                ["message"] = TokenKind.Message,
                ["static"] = TokenKind.Static,
                ["dynamic"] = TokenKind.Dynamic,
                ["override"] = TokenKind.Override,
                ["virtual"] = TokenKind.Virtual,
                ["final"] = TokenKind.Final,
                ["inline"] = TokenKind.Inline,
                ["assembler"] = TokenKind.Assembler,
                ["cdecl"] = TokenKind.Cdecl,
                ["pascal"] = TokenKind.Pascal,
                ["register"] = TokenKind.Register,
                ["safecall"] = TokenKind.Safecall,
                ["stdcall"] = TokenKind.Stdcall,
                ["export"] = TokenKind.Export,
                ["far"] = TokenKind.Far,
                ["local"] = TokenKind.Local,
                ["near"] = TokenKind.Near,
                ["property"] = TokenKind.Property,
                ["index"] = TokenKind.Index,
                ["read"] = TokenKind.Read,
                ["write"] = TokenKind.Write,
                ["add"] = TokenKind.Add,
                ["remove"] = TokenKind.Remove,
                ["readonly"] = TokenKind.ReadOnly,
                ["writeonly"] = TokenKind.WriteOnly,
                ["dispid"] = TokenKind.DispId,
                ["default"] = TokenKind.Default,
                ["nodefault"] = TokenKind.NoDefault,
                ["stored"] = TokenKind.Stored,
                ["implements"] = TokenKind.Implements,
                ["contains"] = TokenKind.Contains,
                ["requires"] = TokenKind.Requires,
                ["package"] = TokenKind.Package,
                ["unit"] = TokenKind.Unit,
                ["interface"] = TokenKind.Interface,
                ["implementation"] = TokenKind.Implementation,
                ["initialization"] = TokenKind.Initialization,
                ["finalization"] = TokenKind.Finalization,
                ["asm"] = TokenKind.Asm,
                ["label"] = TokenKind.Label,
                ["exports"] = TokenKind.Exports,
                ["assembly"] = TokenKind.Assembly,
                ["operator"] = TokenKind.Operator,
                ["absolute"] = TokenKind.Absolute,
                ["name"] = TokenKind.Name,
                ["resident"] = TokenKind.Resident,
                ["shortstring"] = TokenKind.ShortString,
                ["ansistring"] = TokenKind.AnsiString,
                ["string"] = TokenKind.String,
                ["widestring"] = TokenKind.WideString,
                ["unicodestring"] = TokenKind.UnicodeString,
                ["object"] = TokenKind.Object,
                ["to"] = TokenKind.To,
                ["reference"] = TokenKind.Reference,
                ["helper"] = TokenKind.Helper,
                ["dispinterface"] = TokenKind.DispInterface,
                ["for"] = TokenKind.For,
                ["and"] = TokenKind.And,
                ["array"] = TokenKind.Array,
                ["as"] = TokenKind.As,
                ["begin"] = TokenKind.Begin,
                ["case"] = TokenKind.Case,
                ["class"] = TokenKind.Class,
                ["const"] = TokenKind.Const,
                ["constructor"] = TokenKind.Constructor,
                ["destructor"] = TokenKind.Destructor,
                ["dispinterface"] = TokenKind.DispInterface,
                ["div"] = TokenKind.Div,
                ["do"] = TokenKind.Do,
                ["downto"] = TokenKind.DownTo,
                ["else"] = TokenKind.Else,
                ["end"] = TokenKind.End,
                ["except"] = TokenKind.Except,
                ["exports"] = TokenKind.Exports,
                ["file"] = TokenKind.File,
                ["finalization"] = TokenKind.Finalization,
                ["finally"] = TokenKind.Finally,
                ["for"] = TokenKind.For,
                ["function"] = TokenKind.Function,
                ["goto"] = TokenKind.GoToKeyword,
                ["if"] = TokenKind.If,
                ["implementation"] = TokenKind.Implementation,
                ["in"] = TokenKind.In,
                ["inherited"] = TokenKind.Inherited,
                ["initialization"] = TokenKind.Initialization,
                ["inline"] = TokenKind.Inline,
                ["interface"] = TokenKind.Interface,
                ["is"] = TokenKind.Is,
                ["label"] = TokenKind.Label,
                ["library"] = TokenKind.Library,
                ["mod"] = TokenKind.Mod,
                ["nil"] = TokenKind.Nil,
                ["not"] = TokenKind.Not,
                ["of"] = TokenKind.Of,
                ["or"] = TokenKind.Or,
                ["packed"] = TokenKind.Packed,
                ["procedure"] = TokenKind.Procedure,
                ["program"] = TokenKind.Program,
                ["property"] = TokenKind.Property,
                ["raise"] = TokenKind.Raise,
                ["record"] = TokenKind.Record,
                ["repeat"] = TokenKind.Repeat,
                ["resourcestring"] = TokenKind.Resourcestring,
                ["set"] = TokenKind.Set,
                ["shl"] = TokenKind.Shl,
                ["shr"] = TokenKind.Shr,
                ["then"] = TokenKind.Then,
                ["threadvar"] = TokenKind.ThreadVar,
                ["to"] = TokenKind.To,
                ["try"] = TokenKind.Try,
                ["unit"] = TokenKind.Unit,
                ["until"] = TokenKind.Until,
                ["uses"] = TokenKind.Uses,
                ["var"] = TokenKind.Var,
                ["while"] = TokenKind.While,
                ["with"] = TokenKind.With,
                ["xor"] = TokenKind.Xor,
                ["varargs"] = TokenKind.VarArgs,
                ["external"] = TokenKind.External,
                ["forward"] = TokenKind.Forward,
                ["true"] = TokenKind.True,
                ["false"] = TokenKind.False,
                ["exit"] = TokenKind.Exit,
                ["break"] = TokenKind.Break,
                ["continue"] = TokenKind.Continue,
                ["at"] = TokenKind.AtWord,
                ["on"] = TokenKind.On,
                ["dependency"] = TokenKind.Dependency,
                ["delayed"] = TokenKind.Delayed,
            };

        /// <summary>
        ///     toggle semicolons as asm connets
        /// </summary>
        public bool AllowAsmComment
        {
            get
            {
                return punctuators.SemicolonOrAsmComment.AllowComment;
            }

            set
            {
                punctuators.SemicolonOrAsmComment.AllowComment = value;
            }
        }

        /// <summary>
        ///     fetch next token
        /// </summary>
        /// <returns></returns>
        public override Token FetchNextToken() {
            Token result = base.FetchNextToken();

            if (result != null && result.Kind == TokenKind.Asm) {
                AllowAsmComment = true;
            }
            else if (AllowAsmComment && (result == null || result.Kind == TokenKind.End)) {
                AllowAsmComment = false;
            }

            return result;
        }
    }
}
