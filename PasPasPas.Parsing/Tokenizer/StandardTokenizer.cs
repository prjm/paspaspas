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
                ["asm"] = TokenKind.Asm,
                ["begin"] = TokenKind.Begin,
                ["case"] = TokenKind.Case,
                ["class"] = TokenKind.Class,
                ["const"] = TokenKind.Const,
                ["constructor"] = TokenKind.Constructor,
                ["destructor"] = TokenKind.Destructor,
                ["dispinterface"] = TokenKind.DispInterface,
                ["div"] = TokenKind.Div,
                ["do"] = PascalToken.Do,
                ["downto"] = TokenKind.DownTo,
                ["else"] = PascalToken.Else,
                ["end"] = TokenKind.End,
                ["except"] = PascalToken.Except,
                ["exports"] = TokenKind.Exports,
                ["file"] = TokenKind.File,
                ["finalization"] = TokenKind.Finalization,
                ["finally"] = PascalToken.Finally,
                ["for"] = TokenKind.For,
                ["function"] = TokenKind.Function,
                ["goto"] = PascalToken.GoToKeyword,
                ["if"] = PascalToken.If,
                ["implementation"] = TokenKind.Implementation,
                ["in"] = TokenKind.In,
                ["inherited"] = PascalToken.Inherited,
                ["initialization"] = TokenKind.Initialization,
                ["inline"] = TokenKind.Inline,
                ["interface"] = TokenKind.Interface,
                ["is"] = PascalToken.Is,
                ["label"] = TokenKind.Label,
                ["library"] = TokenKind.Library,
                ["mod"] = PascalToken.Mod,
                ["nil"] = PascalToken.Nil,
                ["not"] = PascalToken.Not,
                ["of"] = TokenKind.Of,
                ["or"] = PascalToken.Or,
                ["packed"] = TokenKind.Packed,
                ["procedure"] = TokenKind.Procedure,
                ["program"] = TokenKind.Program,
                ["property"] = TokenKind.Property,
                ["raise"] = PascalToken.Raise,
                ["record"] = TokenKind.Record,
                ["repeat"] = PascalToken.Repeat,
                ["resourcestring"] = TokenKind.Resourcestring,
                ["set"] = TokenKind.Set,
                ["shl"] = PascalToken.Shl,
                ["shr"] = PascalToken.Shr,
                ["string"] = TokenKind.String,
                ["then"] = PascalToken.Then,
                ["threadvar"] = TokenKind.ThreadVar,
                ["to"] = TokenKind.To,
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
