using PasPasPas.Api;
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
            = new Guid("{489C8577-E50F-43AE-83F8-F418343C5271}");

        /// <summary>
        ///     message id: incomplete identifier
        /// </summary>
        public static readonly Guid IncompleteIdentifier
            = new Guid("{988D22EB-E405-4391-BD7E-7DF3D4E553D3}");

        /// <summary>
        ///     get punctuators
        /// </summary>
        protected override InputPatterns CharacterClasses
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
                ["package"] = TokenKind.Package,
                ["unit"] = TokenKind.Unit,
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
                ["array"] = TokenKind.Array,
                ["as"] = PascalToken.As,
                ["asm"] = PascalToken.Asm,
                ["begin"] = TokenKind.Begin,
                ["case"] = PascalToken.Case,
                ["class"] = TokenKind.Class,
                ["const"] = TokenKind.Const,
                ["constructor"] = TokenKind.Constructor,
                ["destructor"] = TokenKind.Destructor,
                ["dispinterface"] = PascalToken.DispInterface,
                ["div"] = PascalToken.Div,
                ["do"] = PascalToken.Do,
                ["downto"] = PascalToken.DownTo,
                ["else"] = PascalToken.Else,
                ["end"] = TokenKind.End,
                ["except"] = PascalToken.Except,
                ["exports"] = PascalToken.Exports,
                ["file"] = TokenKind.File,
                ["finalization"] = PascalToken.Finalization,
                ["finally"] = PascalToken.Finally,
                ["for"] = PascalToken.For,
                ["function"] = TokenKind.Function,
                ["goto"] = PascalToken.GoToKeyword,
                ["if"] = PascalToken.If,
                ["implementation"] = PascalToken.Implementation,
                ["in"] = TokenKind.In,
                ["inherited"] = PascalToken.Inherited,
                ["initialization"] = PascalToken.Initialization,
                ["inline"] = PascalToken.Inline,
                ["interface"] = PascalToken.Interface,
                ["is"] = PascalToken.Is,
                ["label"] = PascalToken.Label,
                ["library"] = TokenKind.Library,
                ["mod"] = PascalToken.Mod,
                ["nil"] = PascalToken.Nil,
                ["not"] = PascalToken.Not,
                ["of"] = TokenKind.Of,
                ["or"] = PascalToken.Or,
                ["packed"] = TokenKind.Packed,
                ["procedure"] = TokenKind.Procedure,
                ["program"] = TokenKind.Program,
                ["property"] = PascalToken.Property,
                ["raise"] = PascalToken.Raise,
                ["record"] = TokenKind.Record,
                ["repeat"] = PascalToken.Repeat,
                ["resourcestring"] = TokenKind.Resourcestring,
                ["set"] = TokenKind.Set,
                ["shl"] = PascalToken.Shl,
                ["shr"] = PascalToken.Shr,
                ["string"] = PascalToken.String,
                ["then"] = PascalToken.Then,
                ["threadvar"] = PascalToken.ThreadVar,
                ["to"] = PascalToken.To,
                ["try"] = PascalToken.Try,
                ["unit"] = TokenKind.Unit,
                ["until"] = PascalToken.Until,
                ["uses"] = TokenKind.Uses,
                ["var"] = TokenKind.Var,
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
                ["at"] = TokenKind.At,
                ["on"] = PascalToken.On
            };

    }
}
