using System.Collections.Generic;
using System;
using PasPasPas.Options.DataTypes;
using System.Globalization;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     parser for compiler directives
    /// </summary>
    public class CompilerDirectiveParser : ParserBase {

        /// <summary>
        ///     create a new compiler directive parser
        /// </summary>
        /// <param name="environment">services</param>
        /// <param name="input">input file</param>
        public CompilerDirectiveParser(ParserServices environment, StackedFileReader input)
            : base(environment, new CompilerDirectiveTokenizerWithLookahead()) {
            BaseTokenizer = new CompilerDirectiveTokenizer(environment, input);
        }

        /// <summary>
        ///     compiler opions
        /// </summary>
        public IOptionSet Options
            => Environment.Options;

        /// <summary>
        ///     compiler options
        /// </summary>
        protected CompileOptions CompilerOptions
             => Options.CompilerOptions;

        /// <summary>
        ///     conditional compilation options
        /// </summary>
        protected ConditionalCompilationOptions ConditionalCompilation
            => Options.ConditionalCompilation;

        /// <summary>
        ///     metainformation
        /// </summary>
        protected MetaInformation Meta
            => Options.Meta;

        /// <summary>
        ///     reader to include files into
        /// </summary>
        public StackedFileReader IncludeInput { get; set; }

        /// <summary>
        ///     supported switches
        /// </summary>
        private static HashSet<int> switches
            = new HashSet<int>() {
                PascalToken.AlignSwitch, PascalToken.AlignSwitch1,PascalToken.AlignSwitch2,PascalToken.AlignSwitch4,PascalToken.AlignSwitch8,PascalToken.AlignSwitch16, /* A */
                PascalToken.BoolEvalSwitch, /* B */
                PascalToken.AssertSwitch, /* C */
                PascalToken.DebugInfoOrDescriptionSwitch, /* D */
                PascalToken.ExtensionSwitch, /* E */
                PascalToken.ExtendedSyntaxSwitch, /* X */
                PascalToken.ImportedDataSwitch,  /* G */
                PascalToken.IncludeSwitch, /* I */ /* IOCHECKS */
                PascalToken.LinkOrLocalSymbolSwitch,  /* L */
                PascalToken.LongStringSwitch, /* H */
                PascalToken.OpenStringSwitch, /* P */
                PascalToken.OptimizationSwitch, /* O */
                PascalToken.OverflowSwitch, /* Q */
                PascalToken.SaveDivideSwitch, /* U */
                PascalToken.IncludeRessource, /* R */ /* RANGECHECKS */
                PascalToken.StackFramesSwitch, /* W */
                PascalToken.WritableConstSwitch, /* J */
                PascalToken.VarStringCheckSwitch, /* V */
                PascalToken.TypedPointersSwitch, /* T */
                PascalToken.SymbolDeclarationSwitch, /* Y */
                PascalToken.SymbolDefinitionsOnlySwitch, /* YD */
                PascalToken.TypeInfoSwitch, /* M */
                PascalToken.EnumSize1, /* Z1 */
                PascalToken.EnumSize2, /* Z2 */
                PascalToken.EnumSize4, /* Z4 */
                PascalToken.EnumSizeSwitch, /* Z */
            };

        /// <summary>
        ///     supported long switches
        /// </summary>
        private static HashSet<int> longSwitches
            = new HashSet<int>() {
                PascalToken.AlignSwitchLong,
                PascalToken.BoolEvalSwitchLong,
                PascalToken.AssertSwitchLong,
                PascalToken.DebugInfoSwitchLong,
                PascalToken.DenyPackageUnit,
                PascalToken.DescriptionSwitchLong,
                PascalToken.DesignOnly,
                PascalToken.ExtensionSwitchLong,
                PascalToken.ObjExportAll,
                PascalToken.ExtendedCompatibility,
                PascalToken.ExtendedSyntaxSwitchLong,
                PascalToken.ExcessPrecision,
                PascalToken.HighCharUnicode,
                PascalToken.Hints,
                PascalToken.ImplicitBuild,
                PascalToken.ImportedDataSwitchLong,
                PascalToken.IncludeSwitchLong,
                PascalToken.IoChecks,
                PascalToken.LocalSymbolSwithLong,
                PascalToken.LongStringSwitchLong,
                PascalToken.OpenStringSwitchLong,
                PascalToken.OptimizationSwitchLong,
                PascalToken.OverflowSwitchLong,
                PascalToken.SaveDivideSwitchLong,
                PascalToken.RangeChecks,
                PascalToken.StackFramesSwitchLong,
                PascalToken.ZeroBaseStrings,
                PascalToken.WritableConstSwitchLong,
                PascalToken.WeakLinkRtti,
                PascalToken.WeakPackageUnit,
                PascalToken.Warnings,
                PascalToken.VarStringCheckSwitchLong,
                PascalToken.TypedPointersSwitchLong,
                PascalToken.ReferenceInfo,
                PascalToken.DefinitionInfo,
                PascalToken.StrongLinkTypes,
                PascalToken.ScopedEnums,
                PascalToken.TypeInfoSwitchLong,
                PascalToken.RunOnly,
                PascalToken.IncludeRessourceLong,
                PascalToken.RealCompatibility,
                PascalToken.Pointermath,
                PascalToken.OldTypeLayout,
                PascalToken.EnumSizeSwitchLong,
                PascalToken.MethodInfo,
                PascalToken.LegacyIfEnd,
            };

        /// <summary>
        ///     supported parameters and directives
        /// </summary>
        private static HashSet<int> parameters
            = new HashSet<int>() {
                PascalToken.Apptype,
                PascalToken.CodeAlign,
                PascalToken.Define,
                PascalToken.Undef,
                PascalToken.IfDef,
                PascalToken.EndIf,
                PascalToken.ElseCd,
                PascalToken.ExternalSym,
                PascalToken.HppEmit,
                PascalToken.IfNDef,
                PascalToken.ImageBase,
                PascalToken.LibPrefix,
                PascalToken.LibSuffix,
                PascalToken.LibVersion,
                PascalToken.Warn,
                PascalToken.Rtti,
                PascalToken.Region,
                PascalToken.EndRegion,
                PascalToken.SetPEOsVersion,
                PascalToken.SetPESubsystemVersion,
                PascalToken.SetPEUserVersion,
                PascalToken.ObjTypeName,
                PascalToken.NoInclude,
                PascalToken.NoDefine,
                PascalToken.MessageCd,
                PascalToken.MinMemStackSizeSwitchLong,
                PascalToken.MaxMemStackSizeSwitchLong,
                PascalToken.IfOpt,
            };

        private void ParseParameter(ISyntaxPart parent) {

            if (Match(PascalToken.IfDef)) {
                ParseIfDef();
            }
            else if (Match(PascalToken.IfOpt)) {
                ParseIfOpt();
            }
            else if (Match(PascalToken.EndIf)) {
                ParseEndIf();
            }
            else if (Match(PascalToken.ElseCd)) {
                ParseElse();
            }
            else if (Match(PascalToken.IfNDef)) {
                ParseIfNDef();
            }
            else if (Match(PascalToken.Apptype)) {
                ParseApptypeParameter(parent);
            }
            else if (Match(PascalToken.CodeAlign)) {
                ParseCodeAlignParameter();
            }
            else if (Match(PascalToken.Define)) {
                ParseDefine();
            }
            else if (Match(PascalToken.Undef)) {
                ParseUndef();
            }
            else if (Match(PascalToken.ExternalSym)) {
                ParseExternalSym();
            }
            else if (Match(PascalToken.HppEmit)) {
                ParseHppEmit();
            }
            else if (Match(PascalToken.ImageBase)) {
                ParseImageBase();
            }
            else if (Match(PascalToken.LibPrefix, PascalToken.LibSuffix, PascalToken.LibVersion)) {
                ParseLibParameter();
            }
            else if (Match(PascalToken.Warn)) {
                ParseWarnParameter();
            }
            else if (Match(PascalToken.Rtti)) {
                ParseRttiParameter();
            }
            else if (Match(PascalToken.Region)) {
                ParseRegion();
            }
            else if (Match(PascalToken.EndRegion)) {
                ParseEndRegion();
            }
            else if (Match(PascalToken.SetPEOsVersion)) {
                ParsePEOsVersion();
            }
            else if (Match(PascalToken.SetPESubsystemVersion)) {
                ParsePESubsystemVersion();
            }
            else if (Match(PascalToken.SetPEUserVersion)) {
                ParsePEUserVersion();
            }
            else if (Match(PascalToken.ObjTypeName)) {
                ParseObjTypeNameSwitch();
            }
            else if (Match(PascalToken.NoInclude)) {
                ParseNoInclude();
            }
            else if (Match(PascalToken.NoDefine)) {
                ParseNoDefine();
            }
            else if (Match(PascalToken.MessageCd)) {
                ParseMessage();
            }
            else if (Match(PascalToken.MinMemStackSizeSwitchLong, PascalToken.MaxMemStackSizeSwitchLong)) {
                ParseStackSizeSwitch(false);
            }
        }

        private void ParseIfOpt() {
            Require(PascalToken.IfOpt);
            var switchKind = Require(PascalToken.Identifier).Value;
            var switchState = Require(PascalToken.Plus, PascalToken.Minus).Kind;
            var requiredInfo = GetSwitchInfo(switchState);
            var switchInfo = Options.GetSwitchInfo(switchKind);
            ConditionalCompilation.AddIfOptCondition(switchKind, requiredInfo, switchInfo);
        }

        private static SwitchInfo GetSwitchInfo(int switchState) {
            if (switchState == PascalToken.Plus)
                return SwitchInfo.Enabled;
            if (switchState == PascalToken.Minus)
                return SwitchInfo.Disabled;
            return SwitchInfo.Undefined;
        }

        private bool ParseStackSizeSwitch(bool mSwitch) {
            int sizeValue;
            string size;

            if (mSwitch || Optional(PascalToken.MinMemStackSizeSwitchLong)) {
                size = Require(PascalToken.Integer).Value;
                if (int.TryParse(size, out sizeValue)) {
                    CompilerOptions.MinimumStackMemSize.Value = sizeValue;
                    if (!mSwitch)
                        return true;
                }
                if (!mSwitch)
                    return false;
            }

            if (mSwitch)
                Require(TokenKind.Comma);

            if (mSwitch || Optional(PascalToken.MaxMemStackSizeSwitchLong)) {
                size = Require(PascalToken.Integer).Value;
                if (int.TryParse(size, out sizeValue)) {
                    CompilerOptions.MaximumStackMemSize.Value = sizeValue;
                    if (!mSwitch)
                        return true;
                }
                if (!mSwitch)
                    return false;
            }

            return mSwitch;
        }

        private bool ParseMessage() {
            Require(PascalToken.MessageCd);
            string messageText = string.Empty;
            string messageType = string.Empty;

            if (Optional(PascalToken.Identifier)) {
                messageType = CurrentToken().Value;
            }

            Require(PascalToken.QuotedString);
            messageText = QuotedStringTokenValue.Unwrap(CurrentToken());
            return true;
        }

        private bool ParseNoDefine() {
            Require(PascalToken.NoDefine);
            var typeName = Require(PascalToken.NoInclude).Value;
            var typeNameInUnion = string.Empty;
            if (Match(PascalToken.Identifier)) {
                typeNameInUnion = CurrentToken().Value;
            }

            Meta.AddNoDefine(typeName, typeNameInUnion);
            return true;
        }

        private bool ParseNoInclude() {
            Require(PascalToken.NoInclude);
            var unitName = Require(PascalToken.NoInclude).Value;
            Meta.AddNoInclude(unitName);
            return true;
        }

        private void ParsePEUserVersion() {
            Require(PascalToken.SetPEUserVersion);
            var version = ParsePEVersion();
            if (version != null) {
                Meta.PEUserVersion.MajorVersion.Value = version.Item1;
                Meta.PEUserVersion.MinorVersion.Value = version.Item2;
            }
        }

        private void ParsePESubsystemVersion() {
            Require(PascalToken.SetPESubsystemVersion);
            var version = ParsePEVersion();
            if (version != null) {
                Meta.PESubsystemVersion.MajorVersion.Value = version.Item1;
                Meta.PESubsystemVersion.MinorVersion.Value = version.Item2;
            }
        }

        private void ParsePEOsVersion() {
            Require(PascalToken.SetPEOsVersion);
            var version = ParsePEVersion();
            if (version != null) {
                Meta.PEOsVersion.MajorVersion.Value = version.Item1;
                Meta.PEOsVersion.MinorVersion.Value = version.Item2;
            }
        }

        private Tuple<int, int> ParsePEVersion() {
            var text = (Require(PascalToken.Identifier).Value ?? string.Empty).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var major = text.Length > 0 ? text[0] : string.Empty;
            var minor = text.Length > 1 ? text[1] : string.Empty;

            int majorVersion;
            int minorVersion;

            if (int.TryParse(major, out majorVersion) && int.TryParse(minor, out minorVersion)) {
                return Tuple.Create(majorVersion, minorVersion);
            }
            else {
                return null;
            }
        }

        private void ParseEndRegion() {
            Require(PascalToken.EndRegion);
            Meta.StopRegion();
        }

        private void ParseRegion() {
            var regionName = "";
            Require(PascalToken.Region);

            if (Match(PascalToken.QuotedString)) {
                regionName = QuotedStringTokenValue.Unwrap(CurrentToken());
            }

            Meta.StartRegion(regionName);
        }

        /// <summary>
        ///     parse the rtti parameter
        /// </summary>
        private void ParseRttiParameter() {
            Require(PascalToken.Rtti);
            RttiGenerationMode mode;

            if (Optional(PascalToken.Inherit)) {
                mode = RttiGenerationMode.Inherit;
            }
            else if (Optional(PascalToken.Explicit)) {
                mode = RttiGenerationMode.Explicit;
            }
            else {
                Unexpected();
                return;
            }

            RttiForVisibility methods = null;
            RttiForVisibility properties = null;
            RttiForVisibility fields = null;
            bool canContinue;

            do {
                canContinue = false;
                if (Optional(PascalToken.Methods)) {
                    if (methods != null) {
                        Unexpected();
                        return;
                    }
                    methods = ParseRttiVisibility();
                    canContinue = methods != null;
                }
                else if (Optional(PascalToken.Properties)) {
                    if (properties != null) {
                        Unexpected();
                        return;
                    }
                    properties = ParseRttiVisibility();
                    canContinue = properties != null;
                }
                else if (Optional(PascalToken.Fields)) {
                    if (fields != null) {
                        Unexpected();
                        return;
                    }
                    fields = ParseRttiVisibility();
                    canContinue = fields != null;
                }
            } while (canContinue);

            CompilerOptions.Rtti.Mode = mode;
            CompilerOptions.Rtti.AssignVisibility(properties, methods, fields);
        }

        private RttiForVisibility ParseRttiVisibility() {
            if (!RequireTokenKind(TokenKind.OpenParen))
                return null;

            if (!RequireTokenKind(TokenKind.OpenBraces))
                return null;

            var result = new RttiForVisibility();

            do {
                var kind = Require(PascalToken.VcPrivate, PascalToken.VcProtected, PascalToken.VcPublic, PascalToken.VcPublished).Kind;

                switch (kind) {
                    case PascalToken.VcPrivate:
                        result.ForPrivate = true;
                        break;
                    case PascalToken.VcProtected:
                        result.ForProtected = true;
                        break;
                    case PascalToken.VcPublic:
                        result.ForPublic = true;
                        break;
                    case PascalToken.VcPublished:
                        result.ForPublished = true;
                        break;
                    default:
                        return null;
                }
            } while (Optional(TokenKind.Comma));

            if (!RequireTokenKind(TokenKind.CloseBraces))
                return null;

            if (!RequireTokenKind(TokenKind.CloseParen))
                return null;

            return result;
        }

        private void ParseWarnParameter() {
            Require(PascalToken.Warn);
            var warningType = Require(PascalToken.Identifier).Value;
            var warningMode = Require(PascalToken.On, PascalToken.Off, PascalToken.Error, PascalToken.Default).Kind;
            var parsedMode = WarningMode.Undefined;

            switch (warningMode) {
                case PascalToken.On:
                    parsedMode = WarningMode.On;
                    break;

                case PascalToken.Off:
                    parsedMode = WarningMode.Off;
                    break;

                case PascalToken.Default:
                    parsedMode = WarningMode.Default;
                    break;

                case PascalToken.Error:
                    parsedMode = WarningMode.Error;
                    break;
            }

            if (parsedMode != WarningMode.Undefined && Options.Warnings.HasWarningIdent(warningType))
                Options.Warnings.SetModeByIdentifier(warningType, parsedMode);
            else
                Unexpected();
        }

        private void ParseLibParameter() {
            int kind = Require(PascalToken.LibPrefix, PascalToken.LibSuffix, PascalToken.LibVersion).Kind;
            var libInfo = QuotedStringTokenValue.Unwrap(Require(PascalToken.QuotedString));

            switch (kind) {
                case PascalToken.LibPrefix:
                    Meta.LibPrefix.Value = libInfo;
                    break;

                case PascalToken.LibSuffix:
                    Meta.LibSuffix.Value = libInfo;
                    break;

                case PascalToken.LibVersion:
                    Meta.LibVersion.Value = libInfo;
                    break;
            }
        }

        private void ParseImageBase() {
            Require(PascalToken.ImageBase);
            var number = Require(PascalToken.Integer, PascalToken.HexNumber);
            int value;

            if ((number.Kind == PascalToken.Integer && int.TryParse(number.Value, out value)) ||
                (number.Kind == PascalToken.HexNumber && int.TryParse(number.Value.Substring(1), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))) {
                CompilerOptions.ImageBase.Value = value;
            }
        }

        private void ParseIfNDef() {
            Require(PascalToken.IfNDef);
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                ConditionalCompilation.AddIfNDefCondition(value);
            }
        }

        private void ParseHppEmit() {
            Require(PascalToken.HppEmit);
            HppEmitMode mode = HppEmitMode.Standard;
            if (Optional(PascalToken.End))
                mode = HppEmitMode.AtEnd;
            else if (Optional(PascalToken.LinkUnit))
                mode = HppEmitMode.LinkUnit;
            else if (Optional(PascalToken.OpenNamespace))
                mode = HppEmitMode.OpenNamespace;
            else if (Optional(PascalToken.CloseNamepsace))
                mode = HppEmitMode.CloseNamespace;
            else if (Optional(PascalToken.NoUsingNamespace))
                mode = HppEmitMode.NoUsingNamespace;

            string emitValue = null;
            if (mode == HppEmitMode.AtEnd || mode == HppEmitMode.Standard) {
                emitValue = Require(PascalToken.QuotedString).Value;
            }

            Meta.HeaderEmit(mode, emitValue);
        }

        private void ParseExternalSym() {
            Require(PascalToken.ExternalSym);
            var identiferName = Require(PascalToken.Identifier).Value;
            string symbolName = null, unionName = null;

            if (Optional(PascalToken.QuotedString)) {
                symbolName = Require(PascalToken.QuotedString).Value;
            }

            if (Optional(PascalToken.QuotedString)) {
                unionName = Require(PascalToken.QuotedString).Value;
            }

            Meta.RegisterExternalSymbol(identiferName, symbolName, unionName);
        }

        private void ParseElse() {
            Require(PascalToken.ElseCd);
            ConditionalCompilation.AddElseCondition();
        }

        private void ParseEndIf() {
            Require(PascalToken.EndIf);
            ConditionalCompilation.RemoveIfDefCondition();
        }

        private void ParseIfDef() {
            Require(PascalToken.IfDef);
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                ConditionalCompilation.AddIfDefCondition(value);
            }
        }

        private void ParseUndef() {
            Require(PascalToken.Undef);
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                ConditionalCompilation.UndefineSymbol(value);
            }
        }

        private void ParseDefine() {
            Require(PascalToken.Define);
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                ConditionalCompilation.DefineSymbol(value);
            }
        }

        private void ParseCodeAlignParameter() {
            Require(PascalToken.CodeAlign);
            var value = Require(PascalToken.Integer).Value;
            int align;

            if (!int.TryParse(value, out align)) {
                Unexpected();
                return;
            }


            switch (align) {
                case 1:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.OneByte;
                    return;
                case 2:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.TwoByte;
                    return;
                case 4:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.FourByte;
                    return;
                case 8:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.EightByte;
                    return;
                case 16:
                    CompilerOptions.CodeAlign.Value = CodeAlignment.SixteenByte;
                    return;
            }

            Unexpected();
        }

        private void ParseApptypeParameter(ISyntaxPart parent) {
            AppTypeParameter result = CreateByTerminal<AppTypeParameter>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {

                var value = result.LastTerminal.Value;

                if (string.Equals(value, "CONSOLE", StringComparison.OrdinalIgnoreCase)) {
                    result.ApplicationType = AppType.Console;
                    return;
                }
                else if (string.Equals(value, "GUI", StringComparison.OrdinalIgnoreCase)) {
                    result.ApplicationType = AppType.Gui;
                    return;
                }

                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidApplicationType, result.LastTerminal.Value);
                return;
            }

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidApplicationType);
        }

        /// <summary>
        ///     parse a long switch
        /// </summary>
        private void ParseLongSwitch(ISyntaxPart parent) {

            if (Match(PascalToken.AlignSwitchLong)) {
                ParseLongAlignSwitch(parent);
            }
            else if (Match(PascalToken.BoolEvalSwitchLong)) {
                ParseLongBoolEvalSwitch(parent);
            }
            else if (Match(PascalToken.AssertSwitchLong)) {
                ParseLongAssertSwitch(parent);
            }

            else if (Optional(PascalToken.DebugInfoSwitchLong)) {
                ParseLongDebugInfoSwitch();
            }

            else if (Optional(PascalToken.DenyPackageUnit)) {
                ParseDenyPackageUnitSwitch();
            }

            else if (Optional(PascalToken.DescriptionSwitchLong)) {
                ParseLongDescriptionSwitch();
            }

            else if (Optional(PascalToken.DesignOnly)) {
                ParseLongDesignOnlySwitch();
            }

            else if (Optional(PascalToken.ExtensionSwitchLong)) {
                ParseLongExtensionSwitch();
            }

            else if (Optional(PascalToken.ObjExportAll)) {
                ParseObjExportAllSwitch();
            }

            else if (Optional(PascalToken.ExtendedCompatibility)) {
                ParseExtendedCompatibilitySwitch();
            }

            else if (Optional(PascalToken.ExtendedSyntaxSwitchLong)) {
                ParseLongExtendedSyntaxSwitch();
            }

            else if (Optional(PascalToken.ExcessPrecision)) {
                ParseLongExcessPrecisionSwitch();
            }

            else if (Optional(PascalToken.HighCharUnicode)) {
                ParseLongHighCharUnicodeSwitch();
            }

            else if (Optional(PascalToken.Hints)) {
                ParseLongHintsSwitch();
            }

            else if (Optional(PascalToken.ImplicitBuild)) {
                ParseLongImplicitBuildSwitch();
            }

            else if (Optional(PascalToken.ImportedDataSwitchLong)) {
                ParseLongImportedDataSwitch();
            }

            else if (Optional(PascalToken.IncludeSwitchLong)) {
                ParseLongIncludeSwitch();
            }

            else if (Optional(PascalToken.IoChecks)) {
                ParseLongIoChecksSwitch();
            }

            else if (Optional(PascalToken.LocalSymbolSwithLong)) {
                ParseLongLocalSymbolSwitch();
            }

            else if (Optional(PascalToken.LongStringSwitchLong)) {
                ParseLongLongStringSwitch();
            }

            else if (Optional(PascalToken.OpenStringSwitchLong)) {
                ParseLongOpenStringSwitch();
            }

            else if (Optional(PascalToken.OptimizationSwitchLong)) {
                ParseLongOptimizationSwitch();
            }

            else if (Optional(PascalToken.OverflowSwitchLong)) {
                ParseLongOverflowSwitch();
            }

            else if (Optional(PascalToken.SaveDivideSwitchLong)) {
                ParseLongSaveDivideSwitch();
            }

            else if (Optional(PascalToken.RangeChecks)) {
                ParseLongRangeChecksSwitch();
            }

            else if (Optional(PascalToken.StackFramesSwitchLong)) {
                ParseLongStackFramesSwitch();
            }

            else if (Optional(PascalToken.ZeroBaseStrings)) {
                ParseZeroBasedStringSwitch();
            }

            else if (Optional(PascalToken.WritableConstSwitchLong)) {
                ParseLongWritableConstSwitch();
            }

            else if (Optional(PascalToken.WeakLinkRtti)) {
                ParseWeakLinkRttiSwitch();
            }

            else if (Optional(PascalToken.WeakPackageUnit)) {
                ParseWeakPackageUnitSwitch();
            }

            else if (Optional(PascalToken.Warnings)) {
                ParseWarningsSwitch();
            }

            else if (Optional(PascalToken.VarStringCheckSwitchLong)) {
                ParseLongVarStringCheckSwitch();
            }

            else if (Optional(PascalToken.TypedPointersSwitchLong)) {
                ParseLongTypedPointersSwitch();
            }

            else if (Optional(PascalToken.DefinitionInfo)) {
                ParseDefinitionInfoSwitch();
            }

            else if (Optional(PascalToken.ReferenceInfo)) {
                ParseReferenceInfoSwitch();
            }

            else if (Optional(PascalToken.StrongLinkTypes)) {
                ParseStrongLinkTypes();
            }

            else if (Optional(PascalToken.ScopedEnums)) {
                ParseScopedEnums();
            }

            else if (Optional(PascalToken.TypeInfoSwitchLong)) {
                ParseLongTypeInfoSwitch();
            }

            else if (Optional(PascalToken.RunOnly)) {
                ParseRunOnlyParameter();
            }

            else if (Match(PascalToken.IncludeRessourceLong)) {
                ParseLongIncludeRessourceSwitch();
            }

            else if (Optional(PascalToken.RealCompatibility)) {
                ParseRealCompatibilitySwitch();
            }

            else if (Optional(PascalToken.Pointermath)) {
                ParsePointermathSwitch();
            }

            else if (Optional(PascalToken.OldTypeLayout)) {
                ParseOldTypeLayoutSwitch();
            }

            else if (Optional(PascalToken.EnumSizeSwitchLong)) {
                ParseLongEnumSizeSwitch();
            }

            else if (Optional(PascalToken.MethodInfo)) {
                ParseMethodInfoSwitch();
            }
            else if (Optional(PascalToken.LegacyIfEnd)) {
                ParseLegacyIfEndSwitch();
            }

        }

        private void ParseLegacyIfEndSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.LegacyIfEnd.Value = EndIfMode.LegacyIfEnd;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.LegacyIfEnd.Value = EndIfMode.Standard;
                return;
            }
            Unexpected();
        }

        private void ParseMethodInfoSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.MethodInfo.Value = MethodInfoRtti.EnableMethodInfo;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.MethodInfo.Value = MethodInfoRtti.DisableMethodInfo;
                return;
            }
            Unexpected();
        }

        private void ParseLongEnumSizeSwitch() {
            var size = Require(PascalToken.Integer);
            int intSize;

            if (int.TryParse(size.Value, out intSize)) {
                switch (intSize) {
                    case 1:
                        CompilerOptions.MinumEnumSize.Value = EnumSize.OneByte;
                        break;
                    case 2:
                        CompilerOptions.MinumEnumSize.Value = EnumSize.TwoByte;
                        break;
                    case 4:
                        CompilerOptions.MinumEnumSize.Value = EnumSize.FourByte;
                        break;
                    default:
                        Unexpected();
                        break;
                }
            }
        }

        private void ParseOldTypeLayoutSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.OldTypeLayout.Value = OldRecordTypes.EnableOldRecordPacking;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.OldTypeLayout.Value = OldRecordTypes.DisableOldRecordPacking;
                return;
            }
            Unexpected();
        }

        private void ParsePointermathSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.PointerMath.Value = PointerManipulation.EnablePointerMath;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.PointerMath.Value = PointerManipulation.DisablePointerMath;
                return;
            }
            Unexpected();
        }

        private void ParseRealCompatibilitySwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.RealCompatiblity.Value = Real48.EnableCompatibility;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.RealCompatiblity.Value = Real48.DisableCompatibility;
                return;
            }
            Unexpected();
        }

        private void ParseLongIncludeRessourceSwitch() {
            var sourcePath = CurrentToken().FilePath;
            FetchNextToken();
            ParseResourceFileName(sourcePath);
        }

        private void ParseRunOnlyParameter() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.RuntimeOnlyPackage.Value = RuntimePackageMode.RuntimeOnly;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.RuntimeOnlyPackage.Value = RuntimePackageMode.Standard;
                return;
            }
            Unexpected();
        }



        private void ParseLongTypeInfoSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.PublishedRtti.Value = RttiForPublishedProperties.Enable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.PublishedRtti.Value = RttiForPublishedProperties.Disable;
                return;
            }
            Unexpected();
        }

        private void ParseScopedEnums() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.ScopedEnums.Value = RequireScopedEnums.Enable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.ScopedEnums.Value = RequireScopedEnums.Disable;
                return;
            }
            Unexpected();
        }

        private void ParseStrongLinkTypes() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.LinkAllTypes.Value = StrongTypeLinking.Enable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.LinkAllTypes.Value = StrongTypeLinking.Disable;
                return;
            }
            Unexpected();
        }

        private void ParseReferenceInfoSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.SymbolReferences.Value = SymbolReferenceInfo.Enable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.SymbolReferences.Value = SymbolReferenceInfo.Disable;
                return;
            }
            Unexpected();
        }

        private void ParseDefinitionInfoSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.SymbolDefinitions.Value = SymbolDefinitionInfo.Enable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.SymbolDefinitions.Value = SymbolDefinitionInfo.Disable;
                return;
            }
            Unexpected();
        }

        private void ParseLongTypedPointersSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.TypedPointers.Value = TypeCheckedPointers.Enable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.TypedPointers.Value = TypeCheckedPointers.Disable;
                return;
            }
            Unexpected();
        }

        private void ParseLongVarStringCheckSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.VarStringChecks.Value = ShortVarStringChecks.EnableChecks;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.VarStringChecks.Value = ShortVarStringChecks.DisableChecks;
                return;
            }
            Unexpected();
        }

        private void ParseWarningsSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.Warnings.Value = CompilerWarnings.Enable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.Warnings.Value = CompilerWarnings.Disable;
                return;
            }
            Unexpected();
        }

        private void ParseWeakPackageUnitSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.WeakPackageUnit.Value = WeakPackaging.Enable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.WeakPackageUnit.Value = WeakPackaging.Disable;
                return;
            }
            Unexpected();
        }

        private void ParseWeakLinkRttiSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.WeakLinkRtti.Value = RttiLinkMode.LinkWeakRtti;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.WeakLinkRtti.Value = RttiLinkMode.LinkFullRtti;
                return;
            }
            Unexpected();
        }

        private void ParseLongWritableConstSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.WritableConstants.Value = ConstantValues.Writable;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.WritableConstants.Value = ConstantValues.Constant;
                return;
            }
            Unexpected();
        }

        private void ParseZeroBasedStringSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.IndexOfFirstCharInString.Value = FirstCharIndex.IsZero;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.IndexOfFirstCharInString.Value = FirstCharIndex.IsOne;
                return;
            }
            Unexpected();
        }

        private void ParseLongStackFramesSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.StackFrames.Value = StackFrameGeneration.EnableFrames;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.StackFrames.Value = StackFrameGeneration.DisableFrames;
                return;
            }
            Unexpected();
        }

        private void ParseLongRangeChecksSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.RangeChecks.Value = RuntimeRangeChecks.EnableRangeChecks;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.RangeChecks.Value = RuntimeRangeChecks.DisableRangeChecks;
                return;
            }
            Unexpected();
        }

        private void ParseLongSaveDivideSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.SafeDivide.Value = FDivSafeDivide.EnableSafeDivide;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.SafeDivide.Value = FDivSafeDivide.DisableSafeDivide;
                return;
            }
            Unexpected();
        }

        private void ParseLongOverflowSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.CheckOverflows.Value = RuntimeOverflowChecks.EnableChecks;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.CheckOverflows.Value = RuntimeOverflowChecks.DisableChecks;
                return;
            }
            Unexpected();
        }

        private void ParseLongOptimizationSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.Optimization.Value = CompilerOptmization.EnableOptimization;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.Optimization.Value = CompilerOptmization.DisableOptimization;
                return;
            }
            Unexpected();
        }

        private void ParseLongOpenStringSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.OpenStrings.Value = OpenStringTypes.EnableOpenStrings;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.OpenStrings.Value = OpenStringTypes.DisableOpenStrings;
                return;
            }
            Unexpected();
        }

        private void ParseLongLongStringSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.LongStrings.Value = LongStringTypes.EnableLongStrings;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.LongStrings.Value = LongStringTypes.DisableLongStrings;
                return;
            }
            Unexpected();
        }

        private void ParseLongLocalSymbolSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.LocalSymbols.Value = LocalDebugSymbols.EnableLocalSymbols;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.LocalSymbols.Value = LocalDebugSymbols.DisableLocalSymbols;
                return;
            }
            Unexpected();
        }

        private void ParseLongIoChecksSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.IoChecks.Value = IoCallChecks.EnableIoChecks;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.IoChecks.Value = IoCallChecks.DisableIoChecks;
                return;
            }
            Unexpected();
        }

        private void ParseLongIncludeSwitch() {
            var includeToken = Require(PascalToken.Identifier, PascalToken.QuotedString);
            var sourcePath = includeToken.FilePath;
            string filename = includeToken.Value;

            if (includeToken.Kind == PascalToken.QuotedString) {
                filename = QuotedStringTokenValue.Unwrap(includeToken);
            }

            var targetPath = Meta.IncludePathResolver.ResolvePath(sourcePath, new FileReference(filename)).TargetPath;

            var fileAccess = Options.Files;
            IncludeInput.AddFile(fileAccess.OpenFileForReading(targetPath));
        }

        private void ParseLongImportedDataSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.ImportedData.Value = ImportGlobalUnitData.DoImport;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.ImportedData.Value = ImportGlobalUnitData.NoImport;
                return;
            }
            Unexpected();
        }

        private void ParseLongImplicitBuildSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.ImplicitBuild.Value = ImplicitBuildUnit.EnableImplicitBuild;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.ImplicitBuild.Value = ImplicitBuildUnit.DisableImplicitBuild;
                return;
            }
            Unexpected();
        }

        private void ParseLongHintsSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.Hints.Value = CompilerHints.EnableHints;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.Hints.Value = CompilerHints.DisableHints;
                return;
            }
            Unexpected();
        }

        private void ParseLongHighCharUnicodeSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.HighCharUnicode.Value = HighCharsUnicode.EnableHighChars;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.HighCharUnicode.Value = HighCharsUnicode.DisableHighChars;
                return;
            }
            Unexpected();
        }

        private void ParseLongExcessPrecisionSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.ExcessPrecision.Value = ExcessPrecisionForResults.EnableExcess;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.ExcessPrecision.Value = ExcessPrecisionForResults.DisableExcess;
                return;
            }
            Unexpected();
        }

        private void ParseLongExtendedSyntaxSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.UseExtendedSyntax.Value = ExtendedSyntax.UseExtendedSyntax;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.UseExtendedSyntax.Value = ExtendedSyntax.NoExtendedSyntax;
                return;
            }
            Unexpected();
        }

        private void ParseExtendedCompatibilitySwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.ExtendedCompatibility.Value = ExtendedCompatiblityMode.Enabled;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.ExtendedCompatibility.Value = ExtendedCompatiblityMode.Disabled;
                return;
            }
            Unexpected();
        }

        private void ParseObjExportAllSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.ExportCppObjects.Value = ExportCppObjects.ExportAll;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.ExportCppObjects.Value = ExportCppObjects.DoNotExportAll;
                return;
            }

            Unexpected();
        }

        private void ParseLongExtensionSwitch() {
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                Meta.FileExtension.Value = value;
            }
        }

        private void ParseLongDesignOnlySwitch() {
            if (Optional(PascalToken.On)) {
                ConditionalCompilation.DesignOnly.Value = DesignOnlyUnit.InDesignTimeOnly;
                return;
            }

            if (Optional(PascalToken.Off)) {
                ConditionalCompilation.DesignOnly.Value = DesignOnlyUnit.Alltimes;
                return;
            }

            Unexpected();
        }

        private void ParseLongDescriptionSwitch() {
            var description = Require(PascalToken.QuotedString);
            Meta.Description.Value = QuotedStringTokenValue.Unwrap(description);
        }

        private void ParseDenyPackageUnitSwitch() {
            if (Optional(PascalToken.On)) {
                ConditionalCompilation.DenyInPackages.Value = DenyUnitInPackages.DenyUnit;
                return;
            }

            if (Optional(PascalToken.Off)) {
                ConditionalCompilation.DenyInPackages.Value = DenyUnitInPackages.AllowUnit;
                return;
            }

            Unexpected();
        }

        private void ParseLongDebugInfoSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.DebugInfo.Value = DebugInformation.IncludeDebugInformation;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.DebugInfo.Value = DebugInformation.NoDebugInfo;
                return;
            }

            Unexpected();
        }

        private void ParseLongAssertSwitch(ISyntaxPart parent) {
            AssertSwitch result = CreateByTerminal<AssertSwitch>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Assertions = AssertionMode.EnableAssertions;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Assertions = AssertionMode.DisableAssertions;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAssertDirective);
            }
        }

        private void ParseLongBoolEvalSwitch(ISyntaxPart parent) {
            var result = CreateByTerminal<BooleanEvaluationSwitch>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.BoolEval = BooleanEvaluation.CompleteEvaluation;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.BoolEval = BooleanEvaluation.ShortEvaluation;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidBoolEvalDirective);
            }
        }

        private void ParseLongAlignSwitch(ISyntaxPart parent) {
            AlignSwitch result = CreateByTerminal<AlignSwitch>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.AlignValue = Alignment.QuadWord;
                return;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.AlignValue = Alignment.Unaligned;
                return;
            }

            int value;
            if (ContinueWith(result, PascalToken.Integer) && int.TryParse(result.LastTerminal.Value, out value)) {

                switch (value) {
                    case 1:
                        result.AlignValue = Alignment.Unaligned;
                        return;
                    case 2:
                        result.AlignValue = Alignment.Word;
                        return;
                    case 4:
                        result.AlignValue = Alignment.DoubleWord;
                        return;
                    case 8:
                        result.AlignValue = Alignment.QuadWord;
                        return;
                    case 16:
                        result.AlignValue = Alignment.DoubleQuadWord;
                        return;
                }

                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidAlignDirective, result.LastTerminal.Value);
                return;
            }

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAlignDirective);
        }

        /// <summary>
        ///     parse a switch
        /// </summary>
        private void ParseSwitch(ISyntaxPart parent) {

            if (Match(PascalToken.AlignSwitch, PascalToken.AlignSwitch1, PascalToken.AlignSwitch2, PascalToken.AlignSwitch4, PascalToken.AlignSwitch8, PascalToken.AlignSwitch16)) {
                ParseAlignSwitch(parent);
            }
            else if (Match(PascalToken.BoolEvalSwitch)) {
                ParseBoolEvalSwitch(parent);
            }
            else if (Match(PascalToken.AssertSwitch)) {
                ParseAssertSwitch(parent);
            }
            else if (Match(PascalToken.DebugInfoOrDescriptionSwitch)) {
                ParseDebugInfoOrDescriptionSwitch();
            }
            else if (Match(PascalToken.ExtensionSwitch)) {
                ParseExtensionSwitch();
            }
            else if (Match(PascalToken.ExtendedSyntaxSwitch)) {
                ParseExtendedSyntaxSwitch();

            }
            else if (Match(PascalToken.ImportedDataSwitch)) {
                ParseImportedDataSwitch();
            }
            else if (Match(PascalToken.IncludeSwitch)) {
                ParseIncludeSwitch();
            }
            else if (Match(PascalToken.LinkOrLocalSymbolSwitch)) {
                ParseLocalSymbolSwitch();
            }
            else if (Match(PascalToken.LongStringSwitch)) {
                ParseLongStringSwitch();
            }
            else if (Match(PascalToken.OpenStringSwitch)) {
                ParseOpenStringSwitch();
            }
            else if (Match(PascalToken.OptimizationSwitch)) {
                ParseOptimizationSwitch();
            }
            else if (Match(PascalToken.OverflowSwitch)) {
                ParseOverflowSwitch();
            }
            else if (Match(PascalToken.SaveDivideSwitch)) {
                ParseSaveDivideSwitch();
            }
            else if (Match(PascalToken.IncludeRessource)) {
                ParseIncludeRessource();
            }
            else if (Match(PascalToken.StackFramesSwitch)) {
                ParseStackFramesSwitch();
            }
            else if (Match(PascalToken.WritableConstSwitch)) {
                ParseWritableConstSwitch();
            }
            else if (Match(PascalToken.VarStringCheckSwitch)) {
                ParseVarStringCheckSwitch();
            }
            else if (Match(PascalToken.TypedPointersSwitch)) {
                ParseTypedPointersSwitch();
            }
            else if (Match(PascalToken.SymbolDeclarationSwitch)) {
                ParseSymbolDeclarationSwitch();
            }
            else if (Match(PascalToken.SymbolDefinitionsOnlySwitch)) {
                ParseSymbolDefinitionsOnlySwitch();
            }
            else if (Match(PascalToken.TypeInfoSwitch)) {
                ParseTypeInfoSwitch();
            }
            else if (Match(PascalToken.EnumSizeSwitch, PascalToken.EnumSize1, PascalToken.EnumSize2, PascalToken.EnumSize4)) {
                ParseEnumSizeSwitch();
            }

        }

        private bool ParseEnumSizeSwitch() {
            var kind = CurrentToken().Kind;

            if (kind == PascalToken.EnumSize1) {
                CompilerOptions.MinumEnumSize.Value = EnumSize.OneByte;
                return true;
            }

            if (kind == PascalToken.EnumSize2) {
                CompilerOptions.MinumEnumSize.Value = EnumSize.TwoByte;
                return true;
            }

            if (kind == PascalToken.EnumSize4) {
                CompilerOptions.MinumEnumSize.Value = EnumSize.FourByte;
                return true;
            }

            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.MinumEnumSize.Value = EnumSize.FourByte;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.MinumEnumSize.Value = EnumSize.OneByte;
                return true;
            }

            return false;
        }

        private bool ParseObjTypeNameSwitch() {
            FetchNextToken();
            var typeName = Require(PascalToken.Identifier).Value;
            var aliasName = string.Empty;
            if (Optional(PascalToken.QuotedString)) {
                aliasName = QuotedStringTokenValue.Unwrap(CurrentToken());
            }

            Meta.AddObjectFileTypeName(typeName, aliasName);
            return true;
        }

        private bool ParseTypeInfoSwitch() {
            FetchNextToken();

            if (CurrentToken().Kind == PascalToken.Integer) {
                return ParseStackSizeSwitch(true);
            }

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.PublishedRtti.Value = RttiForPublishedProperties.Enable;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.PublishedRtti.Value = RttiForPublishedProperties.Disable;
                return true;
            }

            return false;
        }

        private bool ParseSymbolDefinitionsOnlySwitch() {
            CompilerOptions.SymbolReferences.Value = SymbolReferenceInfo.Disable;
            CompilerOptions.SymbolDefinitions.Value = SymbolDefinitionInfo.Enable;
            return true;
        }

        private bool ParseSymbolDeclarationSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.SymbolReferences.Value = SymbolReferenceInfo.Enable;
                CompilerOptions.SymbolDefinitions.Value = SymbolDefinitionInfo.Enable;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.SymbolReferences.Value = SymbolReferenceInfo.Disable;
                CompilerOptions.SymbolDefinitions.Value = SymbolDefinitionInfo.Disable;
                return true;
            }

            return false;
        }

        private bool ParseTypedPointersSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.TypedPointers.Value = TypeCheckedPointers.Enable;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.TypedPointers.Value = TypeCheckedPointers.Disable;
                return true;
            }

            return false;
        }

        private bool ParseVarStringCheckSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.VarStringChecks.Value = ShortVarStringChecks.EnableChecks;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.VarStringChecks.Value = ShortVarStringChecks.DisableChecks;
                return true;
            }

            return false;
        }

        private bool ParseWritableConstSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.WritableConstants.Value = ConstantValues.Writable;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.WritableConstants.Value = ConstantValues.Constant;
                return true;
            }

            return false;
        }

        private bool ParseStackFramesSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.StackFrames.Value = StackFrameGeneration.EnableFrames;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.StackFrames.Value = StackFrameGeneration.DisableFrames;
                return true;
            }

            return false;
        }

        private bool ParseIncludeRessource() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.RangeChecks.Value = RuntimeRangeChecks.EnableRangeChecks;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.RangeChecks.Value = RuntimeRangeChecks.DisableRangeChecks;
                return true;
            }

            var sourcePath = CurrentToken().FilePath;
            if (!ParseResourceFileName(sourcePath))
                return false;

            return true;
        }

        private string ParseFileName() {
            string filename = string.Empty;

            switch (CurrentToken().Kind) {
                case PascalToken.QuotedString:
                    filename = QuotedStringTokenValue.Unwrap(CurrentToken());
                    break;

                case PascalToken.Identifier:
                    filename = CurrentToken().Value;
                    break;

                case PascalToken.Times:
                    filename = Require(PascalToken.Identifier).Value;
                    break;


                default:
                    Unexpected();
                    return string.Empty;

            }

            return filename;
        }

        private bool ParseResourceFileName(IFileReference sourcePath) {
            var kind = CurrentToken().Kind;
            string rcFile = string.Empty;
            string filename = ParseFileName();

            if (Optional(PascalToken.Identifier, PascalToken.QuotedString)) {
                rcFile = CurrentToken().Value;
                if (CurrentToken().Kind == PascalToken.QuotedString) {
                    rcFile = QuotedStringTokenValue.Unwrap(CurrentToken());
                }
            }

            var resolvedFile = Meta.ResourceFilePathResolver.ResolvePath(sourcePath, new FileReference(filename));

            if (resolvedFile.IsResolved) {
                var resourceReference = new ResourceReference();
                resourceReference.OriginalFileName = filename;
                resourceReference.TargetPath = resolvedFile.TargetPath;
                resourceReference.RcFile = rcFile;
                Meta.AddResourceReference(resourceReference);
            }
            return true;
        }

        private bool ParseSaveDivideSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.SafeDivide.Value = FDivSafeDivide.EnableSafeDivide;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.SafeDivide.Value = FDivSafeDivide.DisableSafeDivide;
                return true;
            }

            return false;
        }

        private bool ParseOverflowSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.CheckOverflows.Value = RuntimeOverflowChecks.EnableChecks;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.CheckOverflows.Value = RuntimeOverflowChecks.DisableChecks;
                return true;
            }

            return false;
        }

        private bool ParseOptimizationSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.Optimization.Value = CompilerOptmization.EnableOptimization;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.Optimization.Value = CompilerOptmization.DisableOptimization;
                return true;
            }

            return false;
        }

        private bool ParseOpenStringSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.OpenStrings.Value = OpenStringTypes.EnableOpenStrings;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.OpenStrings.Value = OpenStringTypes.DisableOpenStrings;
                return true;
            }

            return false;
        }

        private bool ParseLongStringSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.LongStrings.Value = LongStringTypes.EnableLongStrings;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.LongStrings.Value = LongStringTypes.DisableLongStrings;
                return true;
            }

            return false;
        }

        private bool ParseLocalSymbolSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.LocalSymbols.Value = LocalDebugSymbols.EnableLocalSymbols;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.LocalSymbols.Value = LocalDebugSymbols.DisableLocalSymbols;
                return true;
            }

            return ParseLinkParameter();
        }

        /// <summary>
        ///     parse a linked file parameter
        /// </summary>
        /// <returns></returns>
        private bool ParseLinkParameter() {
            var filename = ParseFileName();
            var resolvedFile = Meta.LinkedFileResolver.ResolvePath(new FileReference(string.Empty), new FileReference(filename));

            if (resolvedFile.IsResolved) {
                var linkedFile = new LinkedFile();
                linkedFile.OriginalFileName = filename;
                linkedFile.TargetPath = resolvedFile.TargetPath;
                Meta.AddLinkedFile(linkedFile);
                return true;
            }

            return false;
        }

        private bool ParseIncludeSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.IoChecks.Value = IoCallChecks.EnableIoChecks;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.IoChecks.Value = IoCallChecks.DisableIoChecks;
                return true;
            }

            var includeToken = Require(PascalToken.Identifier, PascalToken.QuotedString);
            var sourcePath = includeToken.FilePath;
            string filename = includeToken.Value;

            if (includeToken.Kind == PascalToken.QuotedString) {
                filename = QuotedStringTokenValue.Unwrap(includeToken);
            }

            var targetPath = Meta.IncludePathResolver.ResolvePath(sourcePath, new FileReference(filename)).TargetPath;

            IFileAccess fileAccess = Options.Files;
            IncludeInput.AddFile(fileAccess.OpenFileForReading(targetPath));
            return true;
        }

        private bool ParseImportedDataSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.ImportedData.Value = ImportGlobalUnitData.DoImport;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.ImportedData.Value = ImportGlobalUnitData.NoImport;
                return true;
            }

            Unexpected();
            return false;
        }

        private bool ParseExtendedSyntaxSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.UseExtendedSyntax.Value = ExtendedSyntax.UseExtendedSyntax;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.UseExtendedSyntax.Value = ExtendedSyntax.NoExtendedSyntax;
                return true;
            }

            Unexpected();
            return false;
        }

        private bool ParseExtensionSwitch() {
            Require(PascalToken.ExtensionSwitch);
            var value = Require(CurrentToken().Kind).Value;
            if (!string.IsNullOrEmpty(value)) {
                Meta.FileExtension.Value = value;
            }
            return true;
        }

        private bool ParseDebugInfoOrDescriptionSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.DebugInfo.Value = DebugInformation.IncludeDebugInformation;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.DebugInfo.Value = DebugInformation.NoDebugInfo;
                return true;
            }

            if (Match(PascalToken.QuotedString)) {
                Meta.Description.Value = QuotedStringTokenValue.Unwrap(CurrentToken());
                return true;
            }

            Unexpected();
            return false;
        }

        private void ParseAssertSwitch(ISyntaxPart parent) {
            AssertSwitch result = CreateByTerminal<AssertSwitch>(parent);

            if (ContinueWith(result, PascalToken.Plus)) {
                result.Assertions = AssertionMode.EnableAssertions;
            }
            else if (ContinueWith(result, PascalToken.Minus)) {
                result.Assertions = AssertionMode.DisableAssertions;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAssertDirective);
            }
        }

        private void ParseBoolEvalSwitch(ISyntaxPart parent) {
            BooleanEvaluationSwitch result = CreateByTerminal<BooleanEvaluationSwitch>(parent);

            if (ContinueWith(result, PascalToken.Plus)) {
                result.BoolEval = BooleanEvaluation.CompleteEvaluation;
            }
            else if (ContinueWith(result, PascalToken.Minus)) {
                result.BoolEval = BooleanEvaluation.ShortEvaluation;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidBoolEvalDirective);
            }
        }

        private ISyntaxPart ParseAlignSwitch(ISyntaxPart parent) {
            AlignSwitch result;

            if (OptionalPart(parent, out result, PascalToken.AlignSwitch1)) {
                result.AlignValue = Alignment.Unaligned;
            }
            else if (OptionalPart(parent, out result, PascalToken.AlignSwitch2)) {
                result.AlignValue = Alignment.Word;
            }
            else if (OptionalPart(parent, out result, PascalToken.AlignSwitch4)) {
                result.AlignValue = Alignment.DoubleWord;
            }
            else if (OptionalPart(parent, out result, PascalToken.AlignSwitch8)) {
                result.AlignValue = Alignment.QuadWord;
            }
            else if (OptionalPart(parent, out result, PascalToken.AlignSwitch16)) {
                result.AlignValue = Alignment.DoubleQuadWord;
            }
            else {
                result = CreateByTerminal<AlignSwitch>(parent);

                if (ContinueWith(result, PascalToken.Plus)) {
                    result.AlignValue = Alignment.QuadWord;
                }
                else if (ContinueWith(result, PascalToken.Minus)) {
                    result.AlignValue = Alignment.Unaligned;
                }
                else {
                    ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAlignDirective);
                }
            }

            return result;
        }

        /// <summary>
        ///     parse a compiler directive
        /// </summary>
        /// <returns>parsed syntax tree</returns>
        public override ISyntaxPart Parse() {
            var kind = CurrentToken().Kind;
            ISyntaxPart result = CreateChild<CompilerDirective>(null);

            if (switches.Contains(kind)) {
                ParseSwitch(result);
            }
            else if (longSwitches.Contains(kind)) {
                ParseLongSwitch(result);
            }
            else if (parameters.Contains(kind)) {
                ParseParameter(result);
            }

            if (result == null) {
                // TODO
                // if (!ConditionalCompilation.Skip)
                // log error
                FetchNextToken();
            }

            return result;

        }
    }
}
