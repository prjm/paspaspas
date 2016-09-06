using PasPasPas.Api;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.Parser;
using PasPasPas.Parsing.SyntaxTree;
using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     helper for compiler directives
    /// </summary>
    public class CompilerDirectiveTokenizer : TokenizerBase, ITokenizer {

        /// <summary>
        ///     create a new compiler directive tokenizer
        /// </summary>
        /// <param name="services">environment</param>
        /// <param name="input">file to parse</param>
        public CompilerDirectiveTokenizer(ParserServices services, StackedFileReader input) : base(services, input) { }

        private CompilerDirectivePatterns punctuators
            = new CompilerDirectivePatterns();

        /// <summary>
        ///     known keywords
        /// </summary>
        public static IDictionary<string, int> Keywords { get; }
            = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                ["A"] = PascalToken.AlignSwitch,
                ["A1"] = PascalToken.AlignSwitch1,
                ["A2"] = PascalToken.AlignSwitch2,
                ["A4"] = PascalToken.AlignSwitch4,
                ["A8"] = PascalToken.AlignSwitch8,
                ["A16"] = PascalToken.AlignSwitch16,
                ["ALIGN"] = PascalToken.AlignSwitchLong,
                ["APPTYPE"] = PascalToken.Apptype,
                ["C"] = PascalToken.AssertSwitch,
                ["ASSERTIONS"] = PascalToken.AssertSwitchLong,
                ["B"] = PascalToken.BoolEvalSwitch,
                ["BOOLEVAL"] = PascalToken.BoolEvalSwitchLong,
                ["CODEALIGN"] = PascalToken.CodeAlign,
                ["IFDEF"] = PascalToken.IfDef,
                ["IFNDEF"] = PascalToken.IfNDef,
                ["IF"] = PascalToken.IfCd,
                ["ELSEIF"] = PascalToken.ElseIf,
                ["ELSE"] = PascalToken.ElseCd,
                ["ENDIF"] = PascalToken.EndIf,
                ["IFEND"] = PascalToken.IfEnd,
                ["D"] = PascalToken.DebugInfoOrDescriptionSwitch,
                ["DESCRIPTION"] = PascalToken.DescriptionSwitchLong,
                ["DEBUGINFO"] = PascalToken.DebugInfoSwitchLong,
                ["DEFINE"] = PascalToken.Define,
                ["DEFAULT"] = TokenKind.Default,
                ["DENYPACKAGEUNIT"] = PascalToken.DenyPackageUnit,
                ["DESIGNONLY"] = PascalToken.DesignOnly,
                ["E"] = PascalToken.ExtensionSwitch,
                ["EXTENSION"] = PascalToken.ExtensionSwitchLong,
                ["OBJEXPORTALL"] = PascalToken.ObjExportAll,
                ["X"] = PascalToken.ExtendedSyntaxSwitch,
                ["EXTENDEDSYNTAX"] = PascalToken.ExtendedSyntaxSwitchLong,
                ["EXTENDEDCOMPATIBILITY"] = PascalToken.ExtendedCompatibility,
                ["EXCESSPRECISION"] = PascalToken.ExcessPrecision,
                ["EXTERNALSYM"] = PascalToken.ExternalSym,
                ["HIGHCHARUNICODE"] = PascalToken.HighCharUnicode,
                ["HINTS"] = PascalToken.Hints,
                ["HPPEMIT"] = PascalToken.HppEmit,
                ["IFOPT"] = PascalToken.IfOpt,
                ["IMAGEBASE"] = PascalToken.ImageBase,
                ["IMPLICITBUILD"] = PascalToken.ImplicitBuild,
                ["G"] = PascalToken.ImportedDataSwitch,
                ["IMPORTEDDATA"] = PascalToken.ImportedDataSwitchLong,
                ["I"] = PascalToken.IncludeSwitch,
                ["INCLUDE"] = PascalToken.IncludeSwitchLong,
                ["IOCHECKS"] = PascalToken.IoChecks,
                ["LIBPREFIX"] = PascalToken.LibPrefix,
                ["LIBSUFFIX"] = PascalToken.LibSuffix,
                ["LIBVERSION"] = PascalToken.LibVersion,
                ["LEGACYIFEND"] = PascalToken.LegacyIfEnd,
                ["L"] = PascalToken.LinkOrLocalSymbolSwitch,
                ["LINK"] = PascalToken.LinkSwitchLong,
                ["LOCALSYMBOLS"] = PascalToken.LocalSymbolSwithLong,
                ["H"] = PascalToken.LongStringSwitch,
                ["LONGSTRINGS"] = PascalToken.LongStringSwitchLong,
                ["MINSTACKSIZE"] = PascalToken.MinMemStackSizeSwitchLong,
                ["MAXSTACKSIZE"] = PascalToken.MaxMemStackSizeSwitchLong,
                ["MESSAGE"] = PascalToken.MessageCd,
                ["METHODINFO"] = PascalToken.MethodInfo,
                ["Z"] = PascalToken.EnumSizeSwitch,
                ["Z1"] = PascalToken.EnumSize1,
                ["Z2"] = PascalToken.EnumSize2,
                ["Z4"] = PascalToken.EnumSize4,
                ["MINENUMSIZE"] = PascalToken.EnumSizeSwitchLong,
                ["NODEFINE"] = PascalToken.NoDefine,
                ["NOINCLUDE"] = PascalToken.NoInclude,
                ["OBJTYPENAME"] = PascalToken.ObjTypeName,
                ["OLDTYPELAYOUT"] = PascalToken.OldTypeLayout,
                ["P"] = PascalToken.OpenStringSwitch,
                ["OPENSTRINGS"] = PascalToken.OpenStringSwitchLong,
                ["O"] = PascalToken.OptimizationSwitch,
                ["OPTIMIZATION"] = PascalToken.OptimizationSwitchLong,
                ["Q"] = PascalToken.OverflowSwitch,
                ["OVERFLOWCHECKS"] = PascalToken.OverflowSwitchLong,
                ["SETPEFLAGS"] = PascalToken.SetPEFlags,
                ["SETPEOPTFLAGS"] = PascalToken.SetPEOptFlags,
                ["SETPEOSVERSION"] = PascalToken.SetPEOsVersion,
                ["SETPESUBSYSVERSION"] = PascalToken.SetPESubsystemVersion,
                ["SETPEUSERVERSION"] = PascalToken.SetPEUserVersion,
                ["U"] = PascalToken.SaveDivideSwitch,
                ["SAFEDIVIDE"] = PascalToken.SafeDivideSwitchLong,
                ["POINTERMATH"] = PascalToken.Pointermath,
                ["R"] = PascalToken.IncludeRessource,
                ["RANGECHECKS"] = PascalToken.RangeChecks,
                ["REALCOMPATIBILITY"] = PascalToken.RealCompatibility,
                ["REGION"] = PascalToken.Region,
                ["ENDREGION"] = PascalToken.EndRegion,
                ["RESOURCE"] = PascalToken.IncludeRessourceLong,
                ["RTTI"] = PascalToken.Rtti,
                ["RUNONLY"] = PascalToken.RunOnly,
                ["M"] = PascalToken.TypeInfoSwitch,
                ["TYPEINFO"] = PascalToken.TypeInfoSwitchLong,
                ["SCOPEDENUMS"] = PascalToken.ScopedEnums,
                ["W"] = PascalToken.StackFramesSwitch,
                ["STACKFRAMES"] = PascalToken.StackFramesSwitchLong,
                ["STRONGLINKTYPES"] = PascalToken.StrongLinkTypes,
                ["Y"] = PascalToken.SymbolDeclarationSwitch,
                ["REFERENCEINFO"] = PascalToken.ReferenceInfo,
                ["DEFINITIONINFO"] = PascalToken.DefinitionInfo,
                ["T"] = PascalToken.TypedPointersSwitch,
                ["TYPEDADDRESS"] = PascalToken.TypedPointersSwitchLong,
                ["UNDEF"] = PascalToken.Undef,
                ["V"] = PascalToken.VarStringCheckSwitch,
                ["VARSTRINGCHECKS"] = PascalToken.VarStringCheckSwitchLong,
                ["WARN"] = PascalToken.Warn,
                ["WARNINGS"] = PascalToken.Warnings,
                ["WEAKPACKAGEUNIT"] = PascalToken.WeakPackageUnit,
                ["WEAKLINKRTTI"] = PascalToken.WeakLinkRtti,
                ["J"] = PascalToken.WritableConstSwitch,
                ["WRITEABLECONST"] = PascalToken.WritableConstSwitchLong,
                ["ZEROBASEDSTRINGS"] = PascalToken.ZeroBaseStrings,
                ["ON"] = TokenKind.On,
                ["OFF"] = PascalToken.Off,
                ["LINKUNIT"] = PascalToken.LinkUnit,
                ["OPENNAMESPACE"] = PascalToken.OpenNamespace,
                ["CLOSENAMESPACE"] = PascalToken.CloseNamepsace,
                ["NOUSINGNAMESPACE"] = PascalToken.NoUsingNamespace,
                ["END"] = TokenKind.End,
                ["ERROR"] = PascalToken.Error,
                ["YD"] = PascalToken.SymbolDefinitionsOnlySwitch,
                ["INHERIT"] = PascalToken.Inherit,
                ["EXPLICIT"] = PascalToken.Explicit,
                ["PROPERTIES"] = PascalToken.Properties,
                ["METHODS"] = PascalToken.Methods,
                ["FIELDS"] = PascalToken.Fields,
                ["VCPRIVATE"] = PascalToken.VcPrivate,
                ["VCPROTECTED"] = PascalToken.VcProtected,
                ["VCPUBLIC"] = PascalToken.VcPublic,
                ["VCPUBLISHED"] = PascalToken.VcPublished,
            };

        /// <summary>
        ///     register token types
        /// </summary>
        protected override InputPatterns CharacterClasses
            => punctuators;

        /// <summary>
        ///     unwrap a preprocessor command
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Unwrap(string value) {
            int startOffset = 0;
            var endOffset = 0;

            if (value.StartsWith("{", StringComparison.Ordinal))
                startOffset = 2;
            else if (value.StartsWith("(*", StringComparison.Ordinal))
                startOffset = 3;

            if (value.EndsWith("}", StringComparison.Ordinal))
                endOffset = 1;
            else if (value.EndsWith("*)", StringComparison.Ordinal))
                endOffset = 2;

            return value.Substring(startOffset, value.Length - startOffset - endOffset);
        }

    }
}