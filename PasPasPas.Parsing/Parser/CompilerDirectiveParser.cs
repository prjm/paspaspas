using PasPasPas.Api;
using System.Collections.Generic;
using System;
using PasPasPas.Options.DataTypes;
using System.Globalization;
using PasPasPas.Options.Bundles;
using PasPasPas.Infrastructure.Service;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.Tokenizer;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     helper parser for compiler directives
    /// </summary>
    public class CompilerDirectiveParser : ParserBase {

        /// <summary>
        ///     create a new compiler directive parser
        /// </summary>
        public CompilerDirectiveParser(ServiceProvider environment)
            : base(new CompilerDirectiveTokenizerWithLookahead()) {
            this.environment = environment;
            BaseTokenizer = new CompilerDirectiveTokenizer();
        }

        /// <summary>
        ///     compiler opions
        /// </summary>
        public IOptionSet Options
            => environment.Resolve(StandardServices.CompilerConfigurationServiceClass) as IOptionSet;

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
            };

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
                PascalToken.MaxMemStackSizeSwitchLong
            };

        private ServiceProvider environment;

        /// <summary>
        ///     parse a compiler directive
        /// </summary>
        public bool ParseCompilerDirective() {
            var kind = CurrentToken().Kind;
            var result = false;

            if (switches.Contains(kind)) {
                result = ParseSwitch();
            }
            else if (longSwitches.Contains(kind)) {
                result = ParseLongSwitch();
            }
            else if (parameters.Contains(kind)) {
                result = ParseParameter();
            }

            if (!result) {
                // TODO
                // if (!ConditionalCompilation.Skip)
                // log error
                FetchNextToken();
            }

            return result;
        }

        private bool ParseParameter() {

            if (Match(PascalToken.IfDef)) {
                ParseIfDef();
                return true;
            }


            if (Match(PascalToken.EndIf)) {
                ParseEndIf();
                return true;
            }

            if (Match(PascalToken.ElseCd)) {
                ParseElse();
                return true;
            }

            if (Match(PascalToken.IfNDef)) {
                ParseIfNDef();
                return true;
            }

            if (ConditionalCompilation.Skip)
                return false;

            if (Match(PascalToken.Apptype)) {
                ParseApptypeParameter();
                return true;
            }

            if (Match(PascalToken.CodeAlign)) {
                ParseCodeAlignParameter();
                return true;
            }

            if (Match(PascalToken.Define)) {
                ParseDefine();
                return true;
            }

            if (Match(PascalToken.Undef)) {
                ParseUndef();
                return true;
            }

            if (Match(PascalToken.ExternalSym)) {
                ParseExternalSym();
                return true;
            }

            if (Match(PascalToken.HppEmit)) {
                ParseHppEmit();
                return true;
            }

            if (Match(PascalToken.ImageBase)) {
                ParseImageBase();
                return true;
            }

            if (Match(PascalToken.LibPrefix, PascalToken.LibSuffix, PascalToken.LibVersion)) {
                ParseLibParameter();
                return true;
            }

            if (Match(PascalToken.Warn)) {
                ParseWarnParameter();
                return true;
            }

            if (Match(PascalToken.Rtti)) {
                ParseRttiParameter();
                return true;
            }

            if (Match(PascalToken.Region)) {
                ParseRegion();
                return true;
            }

            if (Match(PascalToken.EndRegion)) {
                ParseEndRegion();
                return true;
            }

            if (Match(PascalToken.SetPEOsVersion)) {
                ParsePEOsVersion();
                return true;
            }

            if (Match(PascalToken.SetPESubsystemVersion)) {
                ParsePESubsystemVersion();
                return true;
            }


            if (Match(PascalToken.SetPEUserVersion)) {
                ParsePEUserVersion();
                return true;
            }

            if (Match(PascalToken.ObjTypeName)) {
                return ParseObjTypeNameSwitch();
            }

            if (Match(PascalToken.NoInclude)) {
                return ParseNoInclude();
            }

            if (Match(PascalToken.NoDefine)) {
                return ParseNoDefine();
            }

            if (Match(PascalToken.MessageCd)) {
                return ParseMessage();
            }

            if (Match(PascalToken.MinMemStackSizeSwitchLong, PascalToken.MaxMemStackSizeSwitchLong)) {
                return ParseStackSizeSwitch(false);
            }

            return false;
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
                Require(PascalToken.Comma);

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
            messageText = QuotedStringTokenValue.Unwrap(CurrentToken().Value);
            return true;
        }

        private bool ParseNoDefine() {
            Require(PascalToken.NoDefine);
            var typeName = Require(PascalToken.NoInclude).Value;
            var typeNameInUnion = string.Empty;
            if (Optional(PascalToken.Identifier)) {
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
                regionName = QuotedStringTokenValue.Unwrap(CurrentToken().Value);
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
            if (!RequireTokenKind(PascalToken.OpenParen))
                return null;

            if (!RequireTokenKind(PascalToken.OpenBraces))
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
            } while (Optional(PascalToken.Comma));

            if (!RequireTokenKind(PascalToken.CloseBraces))
                return null;

            if (!RequireTokenKind(PascalToken.CloseParen))
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
            var libInfo = QuotedStringTokenValue.Unwrap(Require(PascalToken.QuotedString).Value);

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

        private void ParseApptypeParameter() {
            Require(PascalToken.Apptype);
            var value = Require(PascalToken.Identifier).Value;

            if (string.Equals(value, "CONSOLE", StringComparison.OrdinalIgnoreCase)) {
                CompilerOptions.ApplicationType.Value = AppType.Console;
            }
            else if (string.Equals(value, "GUI", StringComparison.OrdinalIgnoreCase)) {
                CompilerOptions.ApplicationType.Value = AppType.Gui;
            }
            else {
                Unexpected();
            }
        }

        /// <summary>
        ///     parse a long switch
        /// </summary>
        private bool ParseLongSwitch() {

            if (ConditionalCompilation.Skip)
                return false;

            if (Optional(PascalToken.AlignSwitchLong)) {
                ParseAlignLongSwitch();
                return true;
            }

            if (Optional(PascalToken.BoolEvalSwitchLong)) {
                ParseLongBoolEvalSwitch();
                return true;
            }

            if (Optional(PascalToken.AssertSwitchLong)) {
                ParseLongAssertSwitch();
                return true;
            }

            if (Optional(PascalToken.DebugInfoSwitchLong)) {
                ParseLongDebugInfoSwitch();
                return true;
            }

            if (Optional(PascalToken.DenyPackageUnit)) {
                ParseDenyPackageUnitSwitch();
                return true;
            }

            if (Optional(PascalToken.DescriptionSwitchLong)) {
                ParseLongDescriptionSwitch();
                return true;
            }

            if (Optional(PascalToken.DesignOnly)) {
                ParseLongDesignOnlySwitch();
                return true;
            }

            if (Optional(PascalToken.ExtensionSwitchLong)) {
                ParseLongExtensionSwitch();
                return true;
            }

            if (Optional(PascalToken.ObjExportAll)) {
                ParseObjExportAllSwitch();
                return true;
            }

            if (Optional(PascalToken.ExtendedCompatibility)) {
                ParseExtendedCompatibilitySwitch();
                return true;
            }

            if (Optional(PascalToken.ExtendedSyntaxSwitchLong)) {
                ParseLongExtendedSyntaxSwitch();
                return true;
            }

            if (Optional(PascalToken.ExcessPrecision)) {
                ParseLongExcessPrecisionSwitch();
                return true;
            }

            if (Optional(PascalToken.HighCharUnicode)) {
                ParseLongHighCharUnicodeSwitch();
                return true;
            }

            if (Optional(PascalToken.Hints)) {
                ParseLongHintsSwitch();
                return true;
            }

            if (Optional(PascalToken.ImplicitBuild)) {
                ParseLongImplicitBuildSwitch();
                return true;
            }

            if (Optional(PascalToken.ImportedDataSwitchLong)) {
                ParseLongImportedDataSwitch();
                return true;
            }

            if (Optional(PascalToken.IncludeSwitchLong)) {
                ParseLongIncludeSwitch();
                return true;
            }

            if (Optional(PascalToken.IoChecks)) {
                ParseLongIoChecksSwitch();
                return true;
            }

            if (Optional(PascalToken.LocalSymbolSwithLong)) {
                ParseLongLocalSymbolSwitch();
                return true;
            }

            if (Optional(PascalToken.LongStringSwitchLong)) {
                ParseLongLongStringSwitch();
                return true;
            }

            if (Optional(PascalToken.OpenStringSwitchLong)) {
                ParseLongOpenStringSwitch();
                return true;
            }

            if (Optional(PascalToken.OptimizationSwitchLong)) {
                ParseLongOptimizationSwitch();
                return true;
            }

            if (Optional(PascalToken.OverflowSwitchLong)) {
                ParseLongOverflowSwitch();
                return true;
            }

            if (Optional(PascalToken.SaveDivideSwitchLong)) {
                ParseLongSaveDivideSwitch();
                return true;
            }

            if (Optional(PascalToken.RangeChecks)) {
                ParseLongRangeChecksSwitch();
                return true;
            }

            if (Optional(PascalToken.StackFramesSwitchLong)) {
                ParseLongStackFramesSwitch();
                return true;
            }

            if (Optional(PascalToken.ZeroBaseStrings)) {
                ParseZeroBasedStringSwitch();
                return true;
            }

            if (Optional(PascalToken.WritableConstSwitchLong)) {
                ParseLongWritableConstSwitch();
                return true;
            }

            if (Optional(PascalToken.WeakLinkRtti)) {
                ParseWeakLinkRttiSwitch();
                return true;
            }

            if (Optional(PascalToken.WeakPackageUnit)) {
                ParseWeakPackageUnitSwitch();
                return true;
            }

            if (Optional(PascalToken.Warnings)) {
                ParseWarningsSwitch();
                return true;
            }

            if (Optional(PascalToken.VarStringCheckSwitchLong)) {
                ParseLongVarStringCheckSwitch();
                return true;
            }

            if (Optional(PascalToken.TypedPointersSwitchLong)) {
                ParseLongTypedPointersSwitch();
                return true;
            }

            if (Optional(PascalToken.DefinitionInfo)) {
                ParseDefinitionInfoSwitch();
                return true;
            }

            if (Optional(PascalToken.ReferenceInfo)) {
                ParseReferenceInfoSwitch();
                return true;
            }

            if (Optional(PascalToken.StrongLinkTypes)) {
                ParseStrongLinkTypes();
                return true;
            }

            if (Optional(PascalToken.ScopedEnums)) {
                ParseScopedEnums();
                return true;
            }

            if (Optional(PascalToken.TypeInfoSwitchLong)) {
                ParseLongTypeInfoSwitch();
                return true;
            }

            if (Optional(PascalToken.RunOnly)) {
                ParseRunOnlyParameter();
                return true;
            }

            if (Match(PascalToken.IncludeRessourceLong)) {
                ParseLongIncludeRessourceSwitch();
                return true;
            }

            if (Optional(PascalToken.RealCompatibility)) {
                ParseRealCompatibilitySwitch();
                return true;
            }

            if (Optional(PascalToken.Pointermath)) {
                ParsePointermathSwitch();
                return true;
            }

            if (Optional(PascalToken.OldTypeLayout)) {
                ParseOldTypeLayoutSwitch();
                return true;
            }

            if (Optional(PascalToken.EnumSizeSwitchLong)) {
                ParseLongEnumSizeSwitch();
                return true;
            }

            if (Optional(PascalToken.MethodInfo)) {
                ParseMethodInfoSwitch();
                return true;
            }

            return false;
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
            var sourcePath = IncludeInput.CurrentFile?.Path ?? string.Empty;
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
            string filename = includeToken.Value;

            if (includeToken.Kind == PascalToken.QuotedString) {
                filename = QuotedStringTokenValue.Unwrap(includeToken.Value);
            }

            string sourcePath = IncludeInput.CurrentFile?.Path ?? string.Empty;
            string targetPath = Meta.IncludePathResolver.ResolvePath(sourcePath, filename).TargetPath;

            IFileAccess fileAccess = (IFileAccess)environment.Resolve(StandardServices.FileAccessServiceClass, this);
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
            var description = Require(PascalToken.QuotedString).Value;
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

        private void ParseLongAssertSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.Assertions.Value = AssertionMode.EnableAssertions;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.Assertions.Value = AssertionMode.DisableAssertions;
                return;
            }

            Unexpected();
        }

        private void ParseLongBoolEvalSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.BoolEval.Value = BooleanEvaluation.CompleteEvaluation;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.BoolEval.Value = BooleanEvaluation.ShortEvaluation;
                return;
            }

            Unexpected();
        }

        /// <summary>
        ///     
        /// </summary>
        private void ParseAlignLongSwitch() {
            if (Optional(PascalToken.On)) {
                CompilerOptions.Align.Value = Alignment.QuadWord;
                return;
            }

            if (Optional(PascalToken.Off)) {
                CompilerOptions.Align.Value = Alignment.Unaligned;
                return;
            }

            int value;
            if (Match(PascalToken.Integer) && int.TryParse(CurrentToken().Value, out value)) {
                switch (value) {
                    case 1:
                        CompilerOptions.Align.Value = Alignment.Unaligned;
                        return;
                    case 2:
                        CompilerOptions.Align.Value = Alignment.Word;
                        return;
                    case 4:
                        CompilerOptions.Align.Value = Alignment.DoubleWord;
                        return;
                    case 8:
                        CompilerOptions.Align.Value = Alignment.QuadWord;
                        return;
                    case 16:
                        CompilerOptions.Align.Value = Alignment.DoubleQuadWord;
                        return;
                }
            }

            Unexpected();
        }

        /// <summary>
        ///     parse a switch
        /// </summary>
        private bool ParseSwitch() {

            if (ConditionalCompilation.Skip)
                return false;

            if (Match(PascalToken.AlignSwitch, PascalToken.AlignSwitch1, PascalToken.AlignSwitch2, PascalToken.AlignSwitch4, PascalToken.AlignSwitch8, PascalToken.AlignSwitch16)) {
                ParseAlignSwitch();
                return true;
            }

            if (Match(PascalToken.BoolEvalSwitch)) {
                ParseBoolEvalSwitch();
                return true;
            }

            if (Match(PascalToken.AssertSwitch)) {
                ParseAssertSwitch();
                return true;
            }

            if (Match(PascalToken.DebugInfoOrDescriptionSwitch)) {
                return ParseDebugInfoOrDescriptionSwitch();
            }

            if (Match(PascalToken.ExtensionSwitch)) {
                return ParseExtensionSwitch();
            }

            if (Match(PascalToken.ExtendedSyntaxSwitch)) {
                return ParseExtendedSyntaxSwitch();
            }

            if (Match(PascalToken.ImportedDataSwitch)) {
                return ParseImportedDataSwitch();
            }

            if (Match(PascalToken.IncludeSwitch)) {
                return ParseIncludeSwitch();
            }

            if (Match(PascalToken.LinkOrLocalSymbolSwitch)) {
                return ParseLocalSymbolSwitch();
            }

            if (Match(PascalToken.LongStringSwitch)) {
                return ParseLongStringSwitch();
            }

            if (Match(PascalToken.OpenStringSwitch)) {
                return ParseOpenStringSwitch();
            }

            if (Match(PascalToken.OptimizationSwitch)) {
                return ParseOptimizationSwitch();
            }

            if (Match(PascalToken.OverflowSwitch)) {
                return ParseOverflowSwitch();
            }

            if (Match(PascalToken.SaveDivideSwitch)) {
                return ParseSaveDivideSwitch();
            }

            if (Match(PascalToken.IncludeRessource)) {
                return ParseIncludeRessource();
            }

            if (Match(PascalToken.StackFramesSwitch)) {
                return ParseStackFramesSwitch();
            }

            if (Match(PascalToken.WritableConstSwitch)) {
                return ParseWritableConstSwitch();
            }

            if (Match(PascalToken.VarStringCheckSwitch)) {
                return ParseVarStringCheckSwitch();
            }

            if (Match(PascalToken.TypedPointersSwitch)) {
                return ParseTypedPointersSwitch();
            }

            if (Match(PascalToken.SymbolDeclarationSwitch)) {
                return ParseSymbolDeclarationSwitch();
            }

            if (Match(PascalToken.SymbolDefinitionsOnlySwitch)) {
                return ParseSymbolDefinitionsOnlySwitch();
            }

            if (Match(PascalToken.TypeInfoSwitch)) {
                return ParseTypeInfoSwitch();
            }

            if (Match(PascalToken.EnumSizeSwitch, PascalToken.EnumSize1, PascalToken.EnumSize2, PascalToken.EnumSize4)) {
                return ParseEnumSizeSwitch();
            }

            return false;
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
                aliasName = QuotedStringTokenValue.Unwrap(CurrentToken().Value);
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
            var sourcePath = IncludeInput.CurrentFile?.Path ?? string.Empty;
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.RangeChecks.Value = RuntimeRangeChecks.EnableRangeChecks;
                return true;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.RangeChecks.Value = RuntimeRangeChecks.DisableRangeChecks;
                return true;
            }


            if (!ParseResourceFileName(sourcePath))
                return false;

            return true;
        }

        private string ParseFileName() {
            string filename = string.Empty;

            switch (CurrentToken().Kind) {
                case PascalToken.QuotedString:
                    filename = QuotedStringTokenValue.Unwrap(CurrentToken().Value);
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

        private bool ParseResourceFileName(string sourcePath) {
            var kind = CurrentToken().Kind;



            string rcFile = string.Empty;
            if (Optional(PascalToken.Identifier, PascalToken.QuotedString)) {
                rcFile = CurrentToken().Value;
                if (CurrentToken().Kind == PascalToken.QuotedString) {
                    rcFile = QuotedStringTokenValue.Unwrap(rcFile);
                }
            }

            var filename = ParseFileName();
            var resolvedFile = Meta.ResourceFilePathResolver.ResolvePath(sourcePath, filename);

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
            var resolvedFile = Meta.LinkedFileResolver.ResolvePath(string.Empty, filename);

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
            string filename = includeToken.Value;

            if (includeToken.Kind == PascalToken.QuotedString) {
                filename = QuotedStringTokenValue.Unwrap(includeToken.Value);
            }

            string sourcePath = IncludeInput.CurrentFile?.Path ?? string.Empty;
            string targetPath = Meta.IncludePathResolver.ResolvePath(sourcePath, filename).TargetPath;

            IFileAccess fileAccess = (IFileAccess)environment.Resolve(StandardServices.FileAccessServiceClass, this);
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
                Meta.Description.Value = QuotedStringTokenValue.Unwrap(CurrentToken().Value);
                return true;
            }

            Unexpected();
            return false;
        }

        private void ParseAssertSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.Assertions.Value = AssertionMode.EnableAssertions;
                return;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.Assertions.Value = AssertionMode.DisableAssertions;
                return;
            }

            Unexpected();
        }

        private void ParseBoolEvalSwitch() {
            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.BoolEval.Value = BooleanEvaluation.CompleteEvaluation;
                return;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.BoolEval.Value = BooleanEvaluation.ShortEvaluation;
                return;
            }

            Unexpected();
        }

        private void ParseAlignSwitch() {
            switch (CurrentToken().Kind) {

                case PascalToken.AlignSwitch1:
                    CompilerOptions.Align.Value = Alignment.Unaligned;
                    return;

                case PascalToken.AlignSwitch2:
                    CompilerOptions.Align.Value = Alignment.Word;
                    return;

                case PascalToken.AlignSwitch4:
                    CompilerOptions.Align.Value = Alignment.DoubleWord;
                    return;

                case PascalToken.AlignSwitch8:
                    CompilerOptions.Align.Value = Alignment.QuadWord;
                    return;

                case PascalToken.AlignSwitch16:
                    CompilerOptions.Align.Value = Alignment.DoubleQuadWord;
                    return;
            }

            FetchNextToken();

            if (Optional(PascalToken.Plus)) {
                CompilerOptions.Align.Value = Alignment.QuadWord;
                return;
            }

            if (Optional(PascalToken.Minus)) {
                CompilerOptions.Align.Value = Alignment.Unaligned;
                return;
            }

            Unexpected();
        }
    }
}
