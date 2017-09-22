using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.Tokenizer.CharClass;
using PasPasPas.Parsing.Tokenizer.TokenGroups;

namespace PasPasPas.Parsing.Tokenizer.Patterns {

    /// <summary>
    ///     provides tokenizer helpers delphi compiler directives
    /// </summary>
    public class CompilerDirectivePatterns : InputPatterns {

        /// <summary>
        ///     known keywords
        /// </summary>
        public static IDictionary<string, int> Keywords { get; }
            = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
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
                ["ELSEIF"] = TokenKind.ElseIf,
                ["ELSE"] = TokenKind.ElseCd,
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

        /// <summary>
        ///     create the preprocessor punctuators
        /// </summary>
        public CompilerDirectivePatterns() {

            AddPattern('+', TokenKind.Plus);
            AddPattern('-', TokenKind.Minus);
            AddPattern('(', TokenKind.OpenParen);
            AddPattern(')', TokenKind.CloseParen);
            AddPattern('[', TokenKind.OpenBraces);
            AddPattern(']', TokenKind.CloseBraces);
            AddPattern(',', TokenKind.Comma);
            AddPattern('*', TokenKind.Times);
            AddPattern(new WhiteSpaceCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.WhiteSpace, new WhiteSpaceCharacterClass()));
            AddPattern(new IdentifierCharacterClass() { AllowDots = true }, new IdentifierTokenGroupValue(Keywords) { AllowDots = true });
            AddPattern(new DigitCharClass(false), new NumberTokenGroupValue() { AllowIdents = true });
            AddPattern('$', new CharacterClassTokenGroupValue(TokenKind.HexNumber, new DigitCharClass(true), 2, LiteralValues.Literals.ParsedHexNumbers, Tokenizer.IncompleteHexNumber));
            AddPattern(new ControlCharacterClass(), new CharacterClassTokenGroupValue(TokenKind.ControlChar, new ControlCharacterClass()));
            AddPattern('"', new QuotedStringTokenValue(TokenKind.QuotedString, '"'));
        }
    }
}