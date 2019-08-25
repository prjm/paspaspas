using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.TokenGroups;

namespace PasPasPas.Parsing.Tokenizer.Patterns {

    /// <summary>
    ///     helper class to create tokenizer patterns
    /// </summary>
    public class PatternFactory : IEnvironmentItem {

        /// <summary>
        ///     create a new pattern factory
        /// </summary>
        /// <param name="pool"></param>
        public PatternFactory(IStringPool pool) {
            StandardPatterns = CreateStandardPatterns();
            CompilerDirectivePatterns = CreateCompilerDirectivePatterns();

            foreach (var key in CompilerDirectivePatterns.Keywords.Keys)
                pool.AddString(key);

            foreach (var key in StandardPatterns.Keywords.Keys)
                pool.AddString(key);
        }

        /// <summary>
        ///     standard tokenizer patterns
        /// </summary>
        public InputPatterns StandardPatterns { get; }

        /// <summary>
        ///     input patters for compiler directives
        /// </summary>
        public InputPatterns CompilerDirectivePatterns { get; }

        /// <summary>
        ///     dummy
        /// </summary>
        public int Count
            => -1;

        /// <summary>
        ///     caption
        /// </summary>
        public static string Caption
            => "TokenizerPatternFactory";

        private static InputPatterns CreateCompilerDirectivePatterns() {
            var keywords = new Dictionary<string, int>(130, StringComparer.OrdinalIgnoreCase) {
                ["A"] = TokenKind.AlignSwitch,
                ["A1"] = TokenKind.AlignSwitch1,
                ["A2"] = TokenKind.AlignSwitch2,
                ["A4"] = TokenKind.AlignSwitch4,
                ["A8"] = TokenKind.AlignSwitch8,
                ["A16"] = TokenKind.AlignSwitch16,
                ["ALIGN"] = TokenKind.AlignSwitchLong,
                ["APPTYPE"] = TokenKind.Apptype,
                ["C"] = TokenKind.AssertSwitch,
                ["ASSERTIONS"] = TokenKind.AssertSwitchLong,
                ["B"] = TokenKind.BoolEvalSwitch,
                ["BOOLEVAL"] = TokenKind.BoolEvalSwitchLong,
                ["CODEALIGN"] = TokenKind.CodeAlign,
                ["IFDEF"] = TokenKind.IfDef,
                ["IFNDEF"] = TokenKind.IfNDef,
                ["IF"] = TokenKind.IfCd,
                ["ELSE"] = TokenKind.ElseCd,
                ["ELSEIF"] = TokenKind.ElseIf,
                ["ENDIF"] = TokenKind.EndIf,
                ["IFEND"] = TokenKind.IfEnd,
                ["D"] = TokenKind.DebugInfoOrDescriptionSwitch,
                ["DESCRIPTION"] = TokenKind.DescriptionSwitchLong,
                ["DEBUGINFO"] = TokenKind.DebugInfoSwitchLong,
                ["DEFINE"] = TokenKind.Define,
                ["DEFAULT"] = TokenKind.Default,
                ["DENYPACKAGEUNIT"] = TokenKind.DenyPackageUnit,
                ["DESIGNONLY"] = TokenKind.DesignOnly,
                ["E"] = TokenKind.ExtensionSwitch,
                ["EXTENSION"] = TokenKind.ExtensionSwitchLong,
                ["OBJEXPORTALL"] = TokenKind.ObjExportAll,
                ["X"] = TokenKind.ExtendedSyntaxSwitch,
                ["EXTENDEDSYNTAX"] = TokenKind.ExtendedSyntaxSwitchLong,
                ["EXTENDEDCOMPATIBILITY"] = TokenKind.ExtendedCompatibility,
                ["EXCESSPRECISION"] = TokenKind.ExcessPrecision,
                ["EXTERNALSYM"] = TokenKind.ExternalSym,
                ["HIGHCHARUNICODE"] = TokenKind.HighCharUnicode,
                ["HINTS"] = TokenKind.Hints,
                ["HPPEMIT"] = TokenKind.HppEmit,
                ["IFOPT"] = TokenKind.IfOpt,
                ["IMAGEBASE"] = TokenKind.ImageBase,
                ["IMPLICITBUILD"] = TokenKind.ImplicitBuild,
                ["G"] = TokenKind.ImportedDataSwitch,
                ["IMPORTEDDATA"] = TokenKind.ImportedDataSwitchLong,
                ["I"] = TokenKind.IncludeSwitch,
                ["INCLUDE"] = TokenKind.IncludeSwitchLong,
                ["IOCHECKS"] = TokenKind.IoChecks,
                ["LIBPREFIX"] = TokenKind.LibPrefix,
                ["LIBSUFFIX"] = TokenKind.LibSuffix,
                ["LIBVERSION"] = TokenKind.LibVersion,
                ["LEGACYIFEND"] = TokenKind.LegacyIfEnd,
                ["VARPROPSETTER"] = TokenKind.VarPropSetter,
                ["L"] = TokenKind.LinkOrLocalSymbolSwitch,
                ["LINK"] = TokenKind.LinkSwitchLong,
                ["LOCALSYMBOLS"] = TokenKind.LocalSymbolSwithLong,
                ["H"] = TokenKind.LongStringSwitch,
                ["LONGSTRINGS"] = TokenKind.LongStringSwitchLong,
                ["MINSTACKSIZE"] = TokenKind.MinMemStackSizeSwitchLong,
                ["MAXSTACKSIZE"] = TokenKind.MaxMemStackSizeSwitchLong,
                ["MESSAGE"] = TokenKind.MessageCd,
                ["METHODINFO"] = TokenKind.MethodInfo,
                ["Z"] = TokenKind.EnumSizeSwitch,
                ["Z1"] = TokenKind.EnumSize1,
                ["Z2"] = TokenKind.EnumSize2,
                ["Z4"] = TokenKind.EnumSize4,
                ["MINENUMSIZE"] = TokenKind.EnumSizeSwitchLong,
                ["NODEFINE"] = TokenKind.NoDefine,
                ["NOINCLUDE"] = TokenKind.NoInclude,
                ["OBJTYPENAME"] = TokenKind.ObjTypeName,
                ["OLDTYPELAYOUT"] = TokenKind.OldTypeLayout,
                ["P"] = TokenKind.OpenStringSwitch,
                ["OPENSTRINGS"] = TokenKind.OpenStringSwitchLong,
                ["O"] = TokenKind.OptimizationSwitch,
                ["OPTIMIZATION"] = TokenKind.OptimizationSwitchLong,
                ["Q"] = TokenKind.OverflowSwitch,
                ["OVERFLOWCHECKS"] = TokenKind.OverflowSwitchLong,
                ["SETPEFLAGS"] = TokenKind.SetPEFlags,
                ["SETPEOPTFLAGS"] = TokenKind.SetPEOptFlags,
                ["SETPEOSVERSION"] = TokenKind.SetPEOsVersion,
                ["SETPESUBSYSVERSION"] = TokenKind.SetPESubsystemVersion,
                ["SETPEUSERVERSION"] = TokenKind.SetPEUserVersion,
                ["U"] = TokenKind.SaveDivideSwitch,
                ["SAFEDIVIDE"] = TokenKind.SafeDivideSwitchLong,
                ["POINTERMATH"] = TokenKind.Pointermath,
                ["R"] = TokenKind.IncludeRessource,
                ["RANGECHECKS"] = TokenKind.RangeChecks,
                ["REALCOMPATIBILITY"] = TokenKind.RealCompatibility,
                ["REGION"] = TokenKind.Region,
                ["ENDREGION"] = TokenKind.EndRegion,
                ["RESOURCE"] = TokenKind.IncludeRessourceLong,
                ["RTTI"] = TokenKind.Rtti,
                ["RUNONLY"] = TokenKind.RunOnly,
                ["M"] = TokenKind.TypeInfoSwitch,
                ["TYPEINFO"] = TokenKind.TypeInfoSwitchLong,
                ["SCOPEDENUMS"] = TokenKind.ScopedEnums,
                ["W"] = TokenKind.StackFramesSwitch,
                ["STACKFRAMES"] = TokenKind.StackFramesSwitchLong,
                ["STRONGLINKTYPES"] = TokenKind.StrongLinkTypes,
                ["Y"] = TokenKind.SymbolDeclarationSwitch,
                ["REFERENCEINFO"] = TokenKind.ReferenceInfo,
                ["DEFINITIONINFO"] = TokenKind.DefinitionInfo,
                ["T"] = TokenKind.TypedPointersSwitch,
                ["TYPEDADDRESS"] = TokenKind.TypedPointersSwitchLong,
                ["UNDEF"] = TokenKind.Undef,
                ["V"] = TokenKind.VarStringCheckSwitch,
                ["VARSTRINGCHECKS"] = TokenKind.VarStringCheckSwitchLong,
                ["WARN"] = TokenKind.Warn,
                ["WARNINGS"] = TokenKind.Warnings,
                ["WEAKPACKAGEUNIT"] = TokenKind.WeakPackageUnit,
                ["WEAKLINKRTTI"] = TokenKind.WeakLinkRtti,
                ["J"] = TokenKind.WritableConstSwitch,
                ["WRITEABLECONST"] = TokenKind.WritableConstSwitchLong,
                ["ZEROBASEDSTRINGS"] = TokenKind.ZeroBaseStrings,
                ["ON"] = TokenKind.On,
                ["OFF"] = TokenKind.Off,
                ["LINKUNIT"] = TokenKind.LinkUnit,
                ["OPENNAMESPACE"] = TokenKind.OpenNamespace,
                ["CLOSENAMESPACE"] = TokenKind.CloseNamepsace,
                ["NOUSINGNAMESPACE"] = TokenKind.NoUsingNamespace,
                ["END"] = TokenKind.End,
                ["ERROR"] = TokenKind.Error,
                ["YD"] = TokenKind.SymbolDefinitionsOnlySwitch,
                ["INHERIT"] = TokenKind.Inherit,
                ["EXPLICIT"] = TokenKind.Explicit,
                ["PROPERTIES"] = TokenKind.Properties,
                ["METHODS"] = TokenKind.Methods,
                ["FIELDS"] = TokenKind.Fields,
                ["VCPRIVATE"] = TokenKind.VcPrivate,
                ["VCPROTECTED"] = TokenKind.VcProtected,
                ["VCPUBLIC"] = TokenKind.VcPublic,
                ["VCPUBLISHED"] = TokenKind.VcPublished,
            };

            var result = new InputPatterns(keywords);
            result.AddPattern('+', TokenKind.Plus);
            result.AddPattern('-', TokenKind.Minus);
            result.AddPattern('(', TokenKind.OpenParen);
            result.AddPattern(')', TokenKind.CloseParen);
            result.AddPattern('[', TokenKind.OpenBraces);
            result.AddPattern(']', TokenKind.CloseBraces);
            result.AddPattern(',', TokenKind.Comma);
            result.AddPattern('*', TokenKind.Times);
            result.AddPattern(new WhiteSpaceCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.WhiteSpace, new WhiteSpaceCharacterClass()));
            result.AddPattern(new IdentifierCharacterClass(dots: true), new IdentifierTokenGroupValue(keywords, allowDots: true));
            result.AddPattern(new DigitCharClass(false), new NumberTokenGroupValue() { AllowIdents = true });
            result.AddPattern('$', new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, LiteralParserKind.HexNumbers, MessageNumbers.IncompleteHexNumber));
            result.AddPattern(new ControlCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.ControlChar, new ControlCharacterClass()));
            result.AddPattern('"', new QuotedStringTokenValue(TokenKind.QuotedString, '"'));
            result.AddPattern('\'', new QuotedStringTokenValue(TokenKind.QuotedString, '\''));
            return result;
        }

        private static InputPatterns CreateStandardPatterns() {

            var keywords = new Dictionary<string, int>(170, StringComparer.OrdinalIgnoreCase) {
                ["program"] = TokenKind.Program,
                ["pointer"] = TokenKind.PointerKeyword,
                ["type"] = TokenKind.TypeKeyword,
                ["experimental"] = TokenKind.Experimental,
                ["deprecated"] = TokenKind.Deprecated,
                ["platform"] = TokenKind.Platform,
                ["sealed"] = TokenKind.Sealed,
                ["strict"] = TokenKind.Strict,
                ["private"] = TokenKind.Private,
                ["published"] = TokenKind.Published,
                ["protected"] = TokenKind.Protected,
                ["automated"] = TokenKind.Automated,
                ["public"] = TokenKind.Public,
                ["out"] = TokenKind.Out,
                ["reintroduce"] = TokenKind.Reintroduce,
                ["overload"] = TokenKind.Overload,
                ["static"] = TokenKind.Static,
                ["override"] = TokenKind.Override,
                ["virtual"] = TokenKind.Virtual,
                ["final"] = TokenKind.Final,
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
                ["index"] = TokenKind.Index,
                ["read"] = TokenKind.Read,
                ["add"] = TokenKind.Add,
                ["remove"] = TokenKind.Remove,
                ["readonly"] = TokenKind.ReadOnly,
                ["writeonly"] = TokenKind.WriteOnly,
                ["dynamic"] = TokenKind.Dynamic,
                ["write"] = TokenKind.Write,
                ["dispid"] = TokenKind.DispId,
                ["default"] = TokenKind.Default,
                ["nodefault"] = TokenKind.NoDefault,
                ["stored"] = TokenKind.Stored,
                ["implements"] = TokenKind.Implements,
                ["contains"] = TokenKind.Contains,
                ["requires"] = TokenKind.Requires,
                ["package"] = TokenKind.Package,
                ["unit"] = TokenKind.Unit,
                ["asm"] = TokenKind.Asm,
                ["assembly"] = TokenKind.Assembly,
                ["operator"] = TokenKind.Operator,
                ["absolute"] = TokenKind.Absolute,
                ["abstract"] = TokenKind.Abstract,
                ["name"] = TokenKind.Name,
                ["resident"] = TokenKind.Resident,
                ["shortstring"] = TokenKind.ShortString,
                ["string"] = TokenKind.StringKeyword,
                ["widestring"] = TokenKind.WideString,
                ["unicodestring"] = TokenKind.UnicodeString,
                ["reference"] = TokenKind.Reference,
                ["helper"] = TokenKind.Helper,
                ["and"] = TokenKind.And,
                ["ansistring"] = TokenKind.AnsiString,
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
                ["message"] = TokenKind.Message,
                ["to"] = TokenKind.To,
                ["try"] = TokenKind.Try,
                ["until"] = TokenKind.Until,
                ["uses"] = TokenKind.Uses,
                ["var"] = TokenKind.Var,
                ["while"] = TokenKind.While,
                ["with"] = TokenKind.With,
                ["xor"] = TokenKind.Xor,
                ["object"] = TokenKind.ObjectKeyword,
                ["varargs"] = TokenKind.VarArgs,
                ["external"] = TokenKind.External,
                ["forward"] = TokenKind.Forward,
                ["unsafe"] = TokenKind.Unsafe,
                ["true"] = TokenKind.True,
                ["false"] = TokenKind.False,
                ["exit"] = TokenKind.Exit,
                ["break"] = TokenKind.Break,
                ["continue"] = TokenKind.Continue,
                ["at"] = TokenKind.AtWord,
                ["on"] = TokenKind.On,
                ["dependency"] = TokenKind.Dependency,
                ["delayed"] = TokenKind.Delayed
            };

            var result = new InputPatterns(keywords);
            result.AddPattern(',', TokenKind.Comma);

            var dot = result.AddPattern('.', TokenKind.Dot);
            dot.Add('.', TokenKind.DotDot);
            dot.Add(')', TokenKind.CloseBraces);

            var lparen = result.AddPattern('(', TokenKind.OpenParen);
            lparen.Add('.', TokenKind.OpenBraces);
            lparen.Add('*', new SequenceGroupTokenValue(TokenKind.Comment, "*)")).Add('$', new SequenceGroupTokenValue(TokenKind.Preprocessor, "*)", true));

            result.AddPattern(')', TokenKind.CloseParen);
            result.AddPattern(';', TokenKind.Semicolon);
            result.AddPattern('=', TokenKind.EqualsSign);
            result.AddPattern('[', TokenKind.OpenBraces);
            result.AddPattern(']', TokenKind.CloseBraces);
            result.AddPattern(':', TokenKind.Colon).Add('=', TokenKind.Assignment);
            result.AddPattern('^', TokenKind.Circumflex);
            result.AddPattern('+', TokenKind.Plus);
            result.AddPattern('-', TokenKind.Minus);
            result.AddPattern('*', TokenKind.Times);

            result.AddPattern('/', TokenKind.Slash).Add('/', new EndOfLineCommentTokenGroupValue());

            result.AddPattern('@', TokenKind.At);
            result.AddPattern('>', TokenKind.GreaterThen).Add('=', TokenKind.GreaterThenEquals);

            var lt = result.AddPattern('<', TokenKind.LessThen);
            lt.Add('=', TokenKind.LessThenEquals);
            lt.Add('>', TokenKind.NotEquals);

            result.AddPattern('{', new SequenceGroupTokenValue(TokenKind.Comment, "}")).Add('$', new SequenceGroupTokenValue(TokenKind.Preprocessor, "}", true));
            result.AddPattern('$', new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, LiteralParserKind.HexNumbers, MessageNumbers.IncompleteHexNumber));
            result.AddPattern(new WhiteSpaceCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.WhiteSpace, new WhiteSpaceCharacterClass()));
            result.AddPattern(new IdentifierCharacterClass(), new IdentifierTokenGroupValue(keywords, allowAmpersand: true));

            result.AddPattern(new DigitCharClass(false), new NumberTokenGroupValue());
            result.AddPattern(new ControlCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.ControlChar, new ControlCharacterClass()));
            result.AddPattern('\'', new StringGroupTokenValue());
            result.AddPattern('"', new QuotedStringTokenValue(TokenKind.DoubleQuotedString, '"'));
            result.AddPattern('#', new StringGroupTokenValue());
            result.AddPattern('\x001A', new SoftEofTokenValue());
            return result;
        }
    }
}
