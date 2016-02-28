using PasPasPas.Api;
using PasPasPas.Internal.Input;
using System;
using System.Collections.Generic;

namespace PasPasPas.Internal.Tokenizer {

    /// <summary>
    ///     helper for compiler directives
    /// </summary>
    public class MacroProcessor : TokenizerBase {

        private PreprocessorPunctuators punctuators
            = new PreprocessorPunctuators();

        /// <summary>
        ///     known keywords
        /// </summary>
        public static IDictionary<string, int> Keywords { get; }
            = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                ["A"] = PascalToken.AlignSwitch,
                ["ALIGN"] = PascalToken.AlignSwitchLong,
                ["APPTYPE"] = PascalToken.Apptype,
                ["C"] = PascalToken.AssertSwitch,
                ["ASSERTIONS"] = PascalToken.AssertSwitchLong,
                ["B"] = PascalToken.BoolEvalSwitch,
                ["BOOLEVAL"] = PascalToken.BoolEvalSwitchLong,
                ["IFDEF"] = PascalToken.IfDef,
                ["IFNDEF"] = PascalToken.IfNDef,
                ["IF"] = PascalToken.IfCd,
                ["ELSEIF"] = PascalToken.ElseIf,
                ["ELSE"] = PascalToken.ElseCd,
                ["ENDIF"] = PascalToken.EndIf,
                ["IFEND"] = PascalToken.IfEnd,
                ["D"] = PascalToken.DebugInfoOrDescriptionSwitch,
                ["DEBUGINFO"] = PascalToken.DebugInfoSwitchLong,
                ["DEFINE"] = PascalToken.Define,
                ["DENYPACKAGEUNIT"] = PascalToken.DenyPackageUnit,
                ["DESIGNONLY"] = PascalToken.DesignOnly,
                ["E"] = PascalToken.ExtensionSwitch,
                ["EXTENSION"] = PascalToken.ExtensionSwitchLong,
                ["OBJEXPORTALL"] = PascalToken.ObjExportAll,
                ["X"] = PascalToken.ExtendedSyntaxSwitch,
                ["EXTENDEDSYNTAX"] = PascalToken.ExtendedSyntaxSwitchLong,
                ["EXTENDEDCOMPATIBILITY"] = PascalToken.ExtendedCompatibility,
                ["$EXCESSPRECISION"] = PascalToken.ExcessPrecision,
                ["$EXTERNALSYM"] = PascalToken.ExternalSym,
                ["$HIGHCHARUNICODE"] = PascalToken.HighCharUnicode,
                ["$HINTS"] = PascalToken.Hints,
                ["$HPPEMIT"] = PascalToken.HppEmit,
                ["$IFOPT"] = PascalToken.IfOpt,
                ["$IMAGEBASE"] = PascalToken.ImageBase,
                ["$IMPLICITBUILD"] = PascalToken.ImplicitBuild,
                ["$G"] = PascalToken.ImportedDataSwitch,
                ["$IMPORTEDDATA"] = PascalToken.ImportedDataSwitchLong,
                ["$I"] = PascalToken.IncludeSwitch,
                ["$INCLUDE"] = PascalToken.IncludeSwitchLong,
                ["$IOCHECKS"] = PascalToken.IoChecks,
                ["$LIBPREFIX"] = PascalToken.LibPrefix,
                ["$LIBSUFFIX"] = PascalToken.LibSuffix,
                ["$LIBVERSION"] = PascalToken.LibVersion,
                ["$LEGACYIFEND"] = PascalToken.LegacyIfEnd,
                ["$L"] = PascalToken.LinkOrLocalSymbolSwitch,
                ["$LINK"] = PascalToken.LinkSwitchLong,
                ["$LOCALSYMBOLS"] = PascalToken.LocalSymbolSwithLong,
                ["$H"] = PascalToken.LongStringSwitch,
                ["$LONGSTRINGS"] = PascalToken.LongStringSwitchLong,
                ["$M"] = PascalToken.MemStackSizeSwitch,
                ["$MINSTACKSIZE"] = PascalToken.MinMemStackSizeSwitchLong,
                ["$MAXSTACKSIZE"] = PascalToken.MaxMemStackSizeSwitchLong,
                ["$MESSAGE"] = PascalToken.MessageCd,
                ["$METHODINFO"] = PascalToken.MethodInfo,
                ["$Z"] = PascalToken.EnumSizeSwitch,
                ["$MINENUMSIZE"] = PascalToken.EnumSizeSwitchLong,
                ["$NODEFINE"] = PascalToken.NoDefine,
                ["$NOINCLUDE"] = PascalToken.NoInclude,
                ["$OBJTYPENAME"] = PascalToken.ObjTypeName,
                ["$OLDTYPELAYOUT"] = PascalToken.OldTypeLayout,
                ["$P"] = PascalToken.OpenStringSwitch,
                ["$OPENSTRINGS"] = PascalToken.OpenStringSwitchLong,
                ["$O"] = PascalToken.OptimizationSwitch,
                ["$OPTIMIZATION"] = PascalToken.OptimizationSwitchLong,
                ["$Q"] = PascalToken.OverflowSwitch,
                ["$OVERFLOWCHECKS"] = PascalToken.OverflowSwitchLong,
                ["$SETPEFLAGS"] = PascalToken.SetPeFlags,
                ["$SETPEOPTFLAGS"] = PascalToken.SetPeOptFlags,
                ["$SETPEOSVERSION"] = PascalToken.SetOsVersion,
                ["$SETPESUBSYSVERSION"] = PascalToken.SetPeSubsystemVerison,
                ["$SETPEUSERVERSION"] = PascalToken.SetPeUserVersion,
                ["$U"] = PascalToken.SaveDivideSwitch,
                ["$SAFEDIVIDE"] = PascalToken.SaveDivideSwitchLong,
                ["$POINTERMATH"] = PascalToken.Pointermath,
                ["$R"] = PascalToken.IncludeRessource,
                ["$RANGECHECKS"] = PascalToken.RangeChecks,
                ["$REALCOMPATIBILITY"] = PascalToken.RealCompatibility,
                ["$REGION"] = PascalToken.Region,
                ["$ENDREGION"] = PascalToken.EndRegion,
                ["$RESOURCE"] = PascalToken.IncludeRessourceLong,
                ["$RTTI"] = PascalToken.Rtti,
                ["$RUNONLY"] = PascalToken.RunOnly,
                ["$M"] = PascalToken.TypeInfoSwitch,
                ["$TYPEINFO"] = PascalToken.TypeInfoSwitchLong,
                ["$SCOPEDENUMS"] = PascalToken.ScopedEnums,
                ["$W"] = PascalToken.StackFramesSwitch,
                ["$STACKFRAMES"] = PascalToken.StackFramesSwitchLong,
                ["$STRONGLINKTYPES"] = PascalToken.StrongLinkTypes,
                ["$Y"] = PascalToken.SymbolDeclarationSwitch,
                ["$REFERENCEINFO"] = PascalToken.ReferenceInfo,
                ["DEFINITIONINFO"] = PascalToken.DefinitionInfo,
                ["$T"] = PascalToken.TypedPointersSwitch,
                ["$TYPEDADDRESS"] = PascalToken.TypedPointersSwitchLong,
                ["$UNDEF"] = PascalToken.Undef,
                ["$V"] = PascalToken.VarStringCheckSwitch,
                ["$VARSTRINGCHECKS"] = PascalToken.VarStringCheckSwitchLong,
                ["$WARN"] = PascalToken.Warn,
                ["$WARNINGS"] = PascalToken.Warnings,
                ["$WEAKPACKAGEUNIT"] = PascalToken.WeakPackageUnit,
                ["$WEAKLINKRTTI"] = PascalToken.WeakLinkRtti,
                ["$J"] = PascalToken.WriteableConstSwitch,
                ["$WRITEABLECONST"] = PascalToken.WriteableConstSwitchLong,
                ["$ZEROBASEDSTRINGS"] = PascalToken.ZeroBaseStrings
            };

        /// <summary>
        ///     register token types
        /// </summary>
        protected override Punctuators CharacterClasses
            => punctuators;

        private static string Unwrap(string value) {
            int startOffset = 0;
            var endOffset = 0;

            if (value.StartsWith("{$"))
                startOffset = 2;
            else if (value.StartsWith("(*$"))
                startOffset = 3;

            if (value.EndsWith("}"))
                endOffset = 1;
            else if (value.EndsWith("*)"))
                endOffset = 2;

            return value.Substring(startOffset, value.Length - startOffset - endOffset);
        }

        /// <summary>
        ///     process directive
        /// </summary>
        /// <param name="value"></param>
        public void ProcessValue(string value) {
            Input = new StringInput(Unwrap(value));
            ProcessValue();
        }

        /// <summary>
        ///     parsecompiler directive
        /// </summary>
        private void ProcessValue() {
            while (!Input.AtEof) {
                var token = FetchNextToken();
            }
        }
    }
}