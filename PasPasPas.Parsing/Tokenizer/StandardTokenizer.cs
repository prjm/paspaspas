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
    public class StandardTokenizer : TokenizerBase, IPascalTokenizer {

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

    }
}
