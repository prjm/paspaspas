using System.Collections.Generic;
using System;
using PasPasPas.Options.DataTypes;
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
                PascalToken.SafeDivideSwitchLong,
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
                ParseIfDef(parent);
            }
            else if (Match(PascalToken.IfOpt)) {
                ParseIfOpt();
            }
            else if (Match(PascalToken.EndIf)) {
                ParseEndIf(parent);
            }
            else if (Match(PascalToken.ElseCd)) {
                ParseElse(parent);
            }
            else if (Match(PascalToken.IfNDef)) {
                ParseIfNDef(parent);
            }
            else if (Match(PascalToken.Apptype)) {
                ParseApptypeParameter(parent);
            }
            else if (Match(PascalToken.CodeAlign)) {
                ParseCodeAlignParameter(parent);
            }
            else if (Match(PascalToken.Define)) {
                ParseDefine(parent);
            }
            else if (Match(PascalToken.Undef)) {
                ParseUndef(parent);
            }
            else if (Match(PascalToken.ExternalSym)) {
                ParseExternalSym(parent);
            }
            else if (Match(PascalToken.HppEmit)) {
                ParseHppEmit(parent);
            }
            else if (Match(PascalToken.ImageBase)) {
                ParseImageBase(parent);
            }
            else if (Match(PascalToken.LibPrefix, PascalToken.LibSuffix, PascalToken.LibVersion)) {
                ParseLibParameter();
            }
            else if (Match(PascalToken.Warn)) {
                ParseWarnParameter(parent);
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
            var switchState = Require(TokenKind.Plus, TokenKind.Minus).Kind;
            var requiredInfo = GetSwitchInfo(switchState);
            var switchInfo = Options.GetSwitchInfo(switchKind);
            ConditionalCompilation.AddIfOptCondition(switchKind, requiredInfo, switchInfo);
        }

        private static SwitchInfo GetSwitchInfo(int switchState) {
            if (switchState == TokenKind.Plus)
                return SwitchInfo.Enabled;
            if (switchState == TokenKind.Minus)
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

        private void ParseWarnParameter(ISyntaxPart parent) {
            WarnSwitch result = CreateByTerminal<WarnSwitch>(parent);

            if (!ContinueWith(result, PascalToken.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarnDirective, new[] { PascalToken.Identifier });
                return;
            }

            var warningType = result.LastTerminal.Value;
            var warningModes = new[] { PascalToken.On, PascalToken.Off, PascalToken.Error, TokenKind.Default };

            if (!ContinueWith(result, warningModes)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarnDirective, warningModes);
                return;
            }

            var warningMode = result.LastTerminal.Token.Kind;
            var parsedMode = WarningMode.Undefined;

            switch (warningMode) {
                case PascalToken.On:
                    parsedMode = WarningMode.On;
                    break;

                case PascalToken.Off:
                    parsedMode = WarningMode.Off;
                    break;

                case TokenKind.Default:
                    parsedMode = WarningMode.Default;
                    break;

                case PascalToken.Error:
                    parsedMode = WarningMode.Error;
                    break;
            }

            if (parsedMode != WarningMode.Undefined && Options.Warnings.HasWarningIdent(warningType)) {
                result.WarningType = warningType;
                result.Mode = parsedMode;
            }
            else {
                result.WarningType = null;
                result.Mode = WarningMode.Undefined;
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidWarnDirective, new object[] { });
            }
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

        private void ParseImageBase(ISyntaxPart parent) {
            ImageBase result = CreateByTerminal<ImageBase>(parent);

            if (ContinueWith(result, PascalToken.Integer)) {
                result.BaseValue = DigitTokenGroupValue.Unwrap(result.LastTerminal.Token);
            }
            else if (ContinueWith(result, PascalToken.HexNumber)) {
                result.BaseValue = HexNumberTokenValue.Unwrap(result.LastTerminal.Token);
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidImageBaseDirective, new[] { PascalToken.Integer, PascalToken.HexNumber });
            }
        }

        private void ParseIfNDef(ISyntaxPart parent) {
            IfDef result = CreateByTerminal<IfDef>(parent);
            result.Negate = true;

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfNDefDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseHppEmit(ISyntaxPart parent) {
            HppEmit result = CreateByTerminal<HppEmit>(parent);
            result.Mode = HppEmitMode.Standard;

            if (ContinueWith(result, TokenKind.End))
                result.Mode = HppEmitMode.AtEnd;
            else if (ContinueWith(result, PascalToken.LinkUnit))
                result.Mode = HppEmitMode.LinkUnit;
            else if (ContinueWith(result, PascalToken.OpenNamespace))
                result.Mode = HppEmitMode.OpenNamespace;
            else if (ContinueWith(result, PascalToken.CloseNamepsace))
                result.Mode = HppEmitMode.CloseNamespace;
            else if (ContinueWith(result, PascalToken.NoUsingNamespace))
                result.Mode = HppEmitMode.NoUsingNamespace;

            if (result.Mode == HppEmitMode.AtEnd || result.Mode == HppEmitMode.Standard) {
                if (ContinueWith(result, PascalToken.QuotedString)) {
                    result.EmitValue = result.LastTerminal.Value;
                }
                else {
                    result.Mode = HppEmitMode.Undefined;
                    ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidHppEmitDirective, new[] { PascalToken.QuotedString });
                }
            }
        }

        private void ParseExternalSym(ISyntaxPart parent) {
            ExternalSymbolDeclaration result = CreateByTerminal<ExternalSymbolDeclaration>(parent);

            if (!ContinueWith(result, PascalToken.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExternalSymbolDirective, new[] { PascalToken.Identifier });
                return;
            }

            result.IdentifierName = result.LastTerminal.Value;

            if (ContinueWith(result, PascalToken.QuotedString)) {
                result.SymbolName = result.LastTerminal.Value;
            }

            if (ContinueWith(result, PascalToken.QuotedString)) {
                result.UnionName = result.LastTerminal.Value;
            }
        }

        private void ParseElse(ISyntaxPart parent) {
            CreateByTerminal<ElseDirective>(parent);
        }

        private void ParseEndIf(ISyntaxPart parent) {
            CreateByTerminal<EndIf>(parent);
        }

        private void ParseIfDef(ISyntaxPart parent) {
            IfDef result = CreateByTerminal<IfDef>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfDefDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseUndef(ISyntaxPart parent) {
            UnDefineSymbol result = CreateByTerminal<UnDefineSymbol>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidUnDefineDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseDefine(ISyntaxPart parent) {
            DefineSymbol result = CreateByTerminal<DefineSymbol>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidDefineDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseCodeAlignParameter(ISyntaxPart parent) {
            CodeAlignParameter result = CreateByTerminal<CodeAlignParameter>(parent);

            int value;
            if (ContinueWith(result, PascalToken.Integer) && int.TryParse(result.LastTerminal.Value, out value)) {
                switch (value) {
                    case 1:
                        result.CodeAlign = CodeAlignment.OneByte;
                        return;
                    case 2:
                        result.CodeAlign = CodeAlignment.TwoByte;
                        return;
                    case 4:
                        result.CodeAlign = CodeAlignment.FourByte;
                        return;
                    case 8:
                        result.CodeAlign = CodeAlignment.EightByte;
                        return;
                    case 16:
                        result.CodeAlign = CodeAlignment.SixteenByte;
                        return;
                }

                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidCodeAlignDirective, result.LastTerminal.Value);
                return;
            }

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidCodeAlignDirective, new[] { PascalToken.Integer });
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

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidApplicationType, new[] { PascalToken.Identifier });
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
            else if (Match(PascalToken.DebugInfoSwitchLong)) {
                ParseLongDebugInfoSwitch(parent);
            }

            else if (Match(PascalToken.DenyPackageUnit)) {
                ParseDenyPackageUnitSwitch(parent);
            }

            else if (Match(PascalToken.DescriptionSwitchLong)) {
                ParseLongDescriptionSwitch(parent);
            }

            else if (Match(PascalToken.DesignOnly)) {
                ParseLongDesignOnlySwitch(parent);
            }

            else if (Match(PascalToken.ExtensionSwitchLong)) {
                ParseLongExtensionSwitch(parent);
            }

            else if (Match(PascalToken.ObjExportAll)) {
                ParseObjExportAllSwitch(parent);
            }

            else if (Match(PascalToken.ExtendedCompatibility)) {
                ParseExtendedCompatibilitySwitch(parent);
            }

            else if (Match(PascalToken.ExtendedSyntaxSwitchLong)) {
                ParseLongExtendedSyntaxSwitch(parent);
            }

            else if (Match(PascalToken.ExcessPrecision)) {
                ParseLongExcessPrecisionSwitch(parent);
            }

            else if (Match(PascalToken.HighCharUnicode)) {
                ParseLongHighCharUnicodeSwitch(parent);
            }

            else if (Match(PascalToken.Hints)) {
                ParseLongHintsSwitch(parent);
            }

            else if (Match(PascalToken.ImplicitBuild)) {
                ParseLongImplicitBuildSwitch(parent);
            }

            else if (Optional(PascalToken.ImportedDataSwitchLong)) {
                ParseLongImportedDataSwitch();
            }

            else if (Optional(PascalToken.IncludeSwitchLong)) {
                ParseLongIncludeSwitch();
            }

            else if (Match(PascalToken.IoChecks)) {
                ParseLongIoChecksSwitch(parent);
            }

            else if (Match(PascalToken.LocalSymbolSwithLong)) {
                ParseLongLocalSymbolSwitch(parent);
            }

            else if (Match(PascalToken.LongStringSwitchLong)) {
                ParseLongLongStringSwitch(parent);
            }

            else if (Match(PascalToken.OpenStringSwitchLong)) {
                ParseLongOpenStringSwitch(parent);
            }

            else if (Match(PascalToken.OptimizationSwitchLong)) {
                ParseLongOptimizationSwitch(parent);
            }

            else if (Match(PascalToken.OverflowSwitchLong)) {
                ParseLongOverflowSwitch(parent);
            }

            else if (Match(PascalToken.SafeDivideSwitchLong)) {
                ParseLongSafeDivideSwitch(parent);
            }

            else if (Match(PascalToken.RangeChecks)) {
                ParseLongRangeChecksSwitch(parent);
            }

            else if (Match(PascalToken.StackFramesSwitchLong)) {
                ParseLongStackFramesSwitch(parent);
            }

            else if (Match(PascalToken.ZeroBaseStrings)) {
                ParseZeroBasedStringSwitch(parent);
            }

            else if (Match(PascalToken.WritableConstSwitchLong)) {
                ParseLongWritableConstSwitch(parent);
            }

            else if (Match(PascalToken.WeakLinkRtti)) {
                ParseWeakLinkRttiSwitch(parent);
            }

            else if (Match(PascalToken.WeakPackageUnit)) {
                ParseWeakPackageUnitSwitch();
            }

            else if (Match(PascalToken.Warnings)) {
                ParseWarningsSwitch(parent);
            }

            else if (Match(PascalToken.VarStringCheckSwitchLong)) {
                ParseLongVarStringCheckSwitch(parent);
            }

            else if (Match(PascalToken.TypedPointersSwitchLong)) {
                ParseLongTypedPointersSwitch(parent);
            }

            else if (Match(PascalToken.DefinitionInfo)) {
                ParseDefinitionInfoSwitch(parent);
            }

            else if (Match(PascalToken.ReferenceInfo)) {
                ParseReferenceInfoSwitch(parent);
            }

            else if (Match(PascalToken.StrongLinkTypes)) {
                ParseStrongLinkTypes(parent);
            }

            else if (Match(PascalToken.ScopedEnums)) {
                ParseScopedEnums(parent);
            }

            else if (Match(PascalToken.TypeInfoSwitchLong)) {
                ParseLongTypeInfoSwitch(parent);
            }

            else if (Match(PascalToken.RunOnly)) {
                ParseRunOnlyParameter(parent);
            }

            else if (Match(PascalToken.IncludeRessourceLong)) {
                ParseLongIncludeRessourceSwitch();
            }

            else if (Match(PascalToken.RealCompatibility)) {
                ParseRealCompatibilitySwitch(parent);
            }

            else if (Match(PascalToken.Pointermath)) {
                ParsePointermathSwitch(parent);
            }

            else if (Match(PascalToken.OldTypeLayout)) {
                ParseOldTypeLayoutSwitch(parent);
            }

            else if (Optional(PascalToken.EnumSizeSwitchLong)) {
                ParseLongEnumSizeSwitch();
            }

            else if (Optional(PascalToken.MethodInfo)) {
                ParseMethodInfoSwitch();
            }
            else if (Match(PascalToken.LegacyIfEnd)) {
                ParseLegacyIfEndSwitch(parent);
            }

        }

        private void ParseLegacyIfEndSwitch(ISyntaxPart parent) {
            LegacyIfEnd result = CreateByTerminal<LegacyIfEnd>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = EndIfMode.LegacyIfEnd;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = EndIfMode.Standard;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLegacyIfEndDirective, new[] { PascalToken.Off, PascalToken.On });
            }
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

        private void ParseOldTypeLayoutSwitch(ISyntaxPart parent) {
            OldTypeLayout result = CreateByTerminal<OldTypeLayout>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = OldRecordTypes.EnableOldRecordPacking;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = OldRecordTypes.DisableOldRecordPacking;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOldTypeLayoutDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParsePointermathSwitch(ISyntaxPart parent) {
            PointerMath result = CreateByTerminal<PointerMath>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = PointerManipulation.EnablePointerMath;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = PointerManipulation.DisablePointerMath;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPointerMathDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseRealCompatibilitySwitch(ISyntaxPart parent) {
            RealCompatibility result = CreateByTerminal<RealCompatibility>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = Real48.EnableCompatibility;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = Real48.DisableCompatibility;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRealCompitibilityMode, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongIncludeRessourceSwitch() {
            var sourcePath = CurrentToken().FilePath;
            FetchNextToken();
            ParseResourceFileName(sourcePath);
        }

        private void ParseRunOnlyParameter(ISyntaxPart parent) {
            RunOnly result = CreateByTerminal<RunOnly>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = RuntimePackageMode.RuntimeOnly;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RuntimePackageMode.Standard;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRunOnlyDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongTypeInfoSwitch(ISyntaxPart parent) {
            PublishedRtti result = CreateByTerminal<PublishedRtti>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = RttiForPublishedProperties.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RttiForPublishedProperties.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseScopedEnums(ISyntaxPart parent) {
            ScopedEnums result = CreateByTerminal<ScopedEnums>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = RequireScopedEnums.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RequireScopedEnums.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidScopedEnumsDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseStrongLinkTypes(ISyntaxPart parent) {
            StrongLinkTypes result = CreateByTerminal<StrongLinkTypes>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = StrongTypeLinking.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = StrongTypeLinking.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStrongLinkTypesDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseReferenceInfoSwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.ReferencesMode = SymbolReferenceInfo.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.ReferencesMode = SymbolReferenceInfo.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseDefinitionInfoSwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = SymbolDefinitionInfo.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = SymbolDefinitionInfo.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongTypedPointersSwitch(ISyntaxPart parent) {
            TypedPointers result = CreateByTerminal<TypedPointers>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = TypeCheckedPointers.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = TypeCheckedPointers.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongVarStringCheckSwitch(ISyntaxPart parent) {
            VarStringChecks result = CreateByTerminal<VarStringChecks>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = ShortVarStringChecks.EnableChecks;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ShortVarStringChecks.DisableChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStringCheckDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseWarningsSwitch(ISyntaxPart parent) {
            Warnings result = CreateByTerminal<Warnings>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = CompilerWarnings.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = CompilerWarnings.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarningsDirective, new[] { PascalToken.On, PascalToken.Off });
            }
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

        private void ParseWeakLinkRttiSwitch(ISyntaxPart parent) {
            WeakLinkRtti result = CreateByTerminal<WeakLinkRtti>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = RttiLinkMode.LinkWeakRtti;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RttiLinkMode.LinkFullRtti;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWeakLinkRttiDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongWritableConstSwitch(ISyntaxPart parent) {
            WritableConsts result = CreateByTerminal<WritableConsts>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = ConstantValues.Writable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ConstantValues.Constant;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseZeroBasedStringSwitch(ISyntaxPart parent) {
            ZeroBasedStrings result = CreateByTerminal<ZeroBasedStrings>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = FirstCharIndex.IsZero;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = FirstCharIndex.IsOne;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidZeroBasedStringsDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongStackFramesSwitch(ISyntaxPart parent) {
            StackFrames result = CreateByTerminal<StackFrames>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = StackFrameGeneration.EnableFrames;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = StackFrameGeneration.DisableFrames;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackFramesDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongRangeChecksSwitch(ISyntaxPart parent) {
            RangeChecks result = CreateByTerminal<RangeChecks>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = RuntimeRangeChecks.EnableRangeChecks;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RuntimeRangeChecks.DisableRangeChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRangeCheckDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongSafeDivideSwitch(ISyntaxPart parent) {
            SafeDivide result = CreateByTerminal<SafeDivide>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = FDivSafeDivide.EnableSafeDivide;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = FDivSafeDivide.DisableSafeDivide;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidSafeDivide, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongOverflowSwitch(ISyntaxPart parent) {
            Overflow result = CreateByTerminal<Overflow>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = RuntimeOverflowChecks.EnableChecks;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RuntimeOverflowChecks.DisableChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongOptimizationSwitch(ISyntaxPart parent) {
            Optimization result = CreateByTerminal<Optimization>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = CompilerOptmization.EnableOptimization;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = CompilerOptmization.DisableOptimization;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOptimizationDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongOpenStringSwitch(ISyntaxPart parent) {
            OpenStrings result = CreateByTerminal<OpenStrings>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = OpenStringTypes.EnableOpenStrings;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = OpenStringTypes.DisableOpenStrings;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOpenStringsDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongLongStringSwitch(ISyntaxPart parent) {
            LongStrings result = CreateByTerminal<LongStrings>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = LongStringTypes.EnableLongStrings;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = LongStringTypes.DisableLongStrings;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongLocalSymbolSwitch(ISyntaxPart parent) {
            LocalSymbols result = CreateByTerminal<LocalSymbols>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = LocalDebugSymbols.EnableLocalSymbols;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = LocalDebugSymbols.DisableLocalSymbols;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLocalSymbolsDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongIoChecksSwitch(ISyntaxPart parent) {
            IoChecks result = CreateByTerminal<IoChecks>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = IoCallChecks.EnableIoChecks;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = IoCallChecks.DisableIoChecks;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { PascalToken.On, PascalToken.Off });
            }
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

        private void ParseLongImplicitBuildSwitch(ISyntaxPart parent) {
            ImplicitBuild result = CreateByTerminal<ImplicitBuild>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = ImplicitBuildUnit.EnableImplicitBuild;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ImplicitBuildUnit.DisableImplicitBuild;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidImplicitBuildDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongHintsSwitch(ISyntaxPart parent) {
            Hints result = CreateByTerminal<Hints>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = CompilerHints.EnableHints;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = CompilerHints.DisableHints;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidHintsDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongHighCharUnicodeSwitch(ISyntaxPart parent) {
            HighCharUnicodeSwitch result = CreateByTerminal<HighCharUnicodeSwitch>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = HighCharsUnicode.EnableHighChars;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = HighCharsUnicode.DisableHighChars;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidHighCharUnicodeDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongExcessPrecisionSwitch(ISyntaxPart parent) {
            ExcessPrecision result = CreateByTerminal<ExcessPrecision>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = ExcessPrecisionForResults.EnableExcess;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ExcessPrecisionForResults.DisableExcess;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongExtendedSyntaxSwitch(ISyntaxPart parent) {
            ExtSyntax result = CreateByTerminal<ExtSyntax>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = ExtendedSyntax.UseExtendedSyntax;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ExtendedSyntax.NoExtendedSyntax;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseExtendedCompatibilitySwitch(ISyntaxPart parent) {
            ExtendedCompatibility result = CreateByTerminal<ExtendedCompatibility>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = ExtendedCompatiblityMode.Enabled;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ExtendedCompatiblityMode.Disabled;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseObjExportAllSwitch(ISyntaxPart parent) {
            ObjectExport result = CreateByTerminal<ObjectExport>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                CompilerOptions.ExportCppObjects.Value = ExportCppObjects.ExportAll;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                CompilerOptions.ExportCppObjects.Value = ExportCppObjects.DoNotExportAll;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidObjectExportDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongExtensionSwitch(ISyntaxPart parent) {
            Extension result = CreateByTerminal<Extension>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.ExecutableExtension = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseLongDesignOnlySwitch(ISyntaxPart parent) {
            DesignOnly result = CreateByTerminal<DesignOnly>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.DesignTimeOnly = DesignOnlyUnit.InDesignTimeOnly;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.DesignTimeOnly = DesignOnlyUnit.Alltimes;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongDescriptionSwitch(ISyntaxPart parent) {
            Description result = CreateByTerminal<Description>(parent);

            if (ContinueWith(result, PascalToken.QuotedString)) {
                result.DescriptionValue = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDescriptionDirective, new[] { PascalToken.QuotedString });
            }
        }

        private void ParseDenyPackageUnitSwitch(ISyntaxPart parent) {
            ParseDenyPackageUnit result = CreateByTerminal<ParseDenyPackageUnit>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.DenyUnit = DenyUnitInPackages.DenyUnit;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.DenyUnit = DenyUnitInPackages.AllowUnit;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongDebugInfoSwitch(ISyntaxPart parent) {
            DebugInfoSwitch result = CreateByTerminal<DebugInfoSwitch>(parent);
            if (ContinueWith(result, PascalToken.On)) {
                result.DebugInfo = DebugInformation.IncludeDebugInformation;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.DebugInfo = DebugInformation.NoDebugInfo;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { PascalToken.On, PascalToken.Off });
            }
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
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAssertDirective, new[] { PascalToken.On, PascalToken.Off });
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
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidBoolEvalDirective, new[] { PascalToken.On, PascalToken.Off });
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

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAlignDirective, new[] { PascalToken.Integer });
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
                ParseDebugInfoOrDescriptionSwitch(parent);
            }
            else if (Match(PascalToken.ExtensionSwitch)) {
                ParseExtensionSwitch(parent);
            }
            else if (Match(PascalToken.ExtendedSyntaxSwitch)) {
                ParseExtendedSyntaxSwitch(parent);
            }
            else if (Match(PascalToken.ImportedDataSwitch)) {
                ParseImportedDataSwitch();
            }
            else if (Match(PascalToken.IncludeSwitch)) {
                ParseIncludeSwitch(parent);
            }
            else if (Match(PascalToken.LinkOrLocalSymbolSwitch)) {
                ParseLocalSymbolSwitch(parent);
            }
            else if (Match(PascalToken.LongStringSwitch)) {
                ParseLongStringSwitch(parent);
            }
            else if (Match(PascalToken.OpenStringSwitch)) {
                ParseOpenStringSwitch(parent);
            }
            else if (Match(PascalToken.OptimizationSwitch)) {
                ParseOptimizationSwitch(parent);
            }
            else if (Match(PascalToken.OverflowSwitch)) {
                ParseOverflowSwitch(parent);
            }
            else if (Match(PascalToken.SaveDivideSwitch)) {
                ParseSaveDivideSwitch(parent);
            }
            else if (Match(PascalToken.IncludeRessource)) {
                ParseIncludeRessource(parent);
            }
            else if (Match(PascalToken.StackFramesSwitch)) {
                ParseStackFramesSwitch(parent);
            }
            else if (Match(PascalToken.WritableConstSwitch)) {
                ParseWritableConstSwitch(parent);
            }
            else if (Match(PascalToken.VarStringCheckSwitch)) {
                ParseVarStringCheckSwitch(parent);
            }
            else if (Match(PascalToken.TypedPointersSwitch)) {
                ParseTypedPointersSwitch(parent);
            }
            else if (Match(PascalToken.SymbolDeclarationSwitch)) {
                ParseSymbolDeclarationSwitch(parent);
            }
            else if (Match(PascalToken.SymbolDefinitionsOnlySwitch)) {
                ParseSymbolDefinitionsOnlySwitch(parent);
            }
            else if (Match(PascalToken.TypeInfoSwitch)) {
                ParseTypeInfoSwitch(parent);
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

            if (Optional(TokenKind.Plus)) {
                CompilerOptions.MinumEnumSize.Value = EnumSize.FourByte;
                return true;
            }

            if (Optional(TokenKind.Minus)) {
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

        private void ParseTypeInfoSwitch(ISyntaxPart parent) {

            if (LookAhead(1, PascalToken.Integer)) {
                FetchNextToken();

                if (CurrentToken().Kind == PascalToken.Integer) {
                    ParseStackSizeSwitch(true);
                }

                return;
            }

            PublishedRtti result = CreateByTerminal<PublishedRtti>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = RttiForPublishedProperties.Enable;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = RttiForPublishedProperties.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseSymbolDefinitionsOnlySwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent);
            result.ReferencesMode = SymbolReferenceInfo.Disable;
            result.Mode = SymbolDefinitionInfo.Enable;
        }

        private void ParseSymbolDeclarationSwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.ReferencesMode = SymbolReferenceInfo.Enable;
                result.Mode = SymbolDefinitionInfo.Enable;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.ReferencesMode = SymbolReferenceInfo.Disable;
                result.Mode = SymbolDefinitionInfo.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseTypedPointersSwitch(ISyntaxPart parent) {
            TypedPointers result = CreateByTerminal<TypedPointers>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = TypeCheckedPointers.Enable;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = TypeCheckedPointers.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective, new[] { TokenKind.Minus, TokenKind.Platform });
            }
        }

        private void ParseVarStringCheckSwitch(ISyntaxPart parent) {
            VarStringChecks result = CreateByTerminal<VarStringChecks>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = ShortVarStringChecks.EnableChecks;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = ShortVarStringChecks.DisableChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStringCheckDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseWritableConstSwitch(ISyntaxPart parent) {
            WritableConsts result = CreateByTerminal<WritableConsts>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = ConstantValues.Writable;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = ConstantValues.Constant;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseStackFramesSwitch(ISyntaxPart parent) {
            StackFrames result = CreateByTerminal<StackFrames>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = StackFrameGeneration.EnableFrames;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = StackFrameGeneration.DisableFrames;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackFramesDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseIncludeRessource(ISyntaxPart parent) {

            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                RangeChecks result = CreateByTerminal<RangeChecks>(parent);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.Mode = RuntimeRangeChecks.EnableRangeChecks;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.Mode = RuntimeRangeChecks.DisableRangeChecks;
                }
                return;
            }

            FetchNextToken();
            var sourcePath = CurrentToken().FilePath;
            if (!ParseResourceFileName(sourcePath))
                return;

            return;
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

                case TokenKind.Times:
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

        private void ParseSaveDivideSwitch(ISyntaxPart parent) {
            SafeDivide result = CreateByTerminal<SafeDivide>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = FDivSafeDivide.EnableSafeDivide;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = FDivSafeDivide.DisableSafeDivide;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidSafeDivide, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseOverflowSwitch(ISyntaxPart parent) {
            Overflow result = CreateByTerminal<Overflow>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = RuntimeOverflowChecks.EnableChecks;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = RuntimeOverflowChecks.DisableChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseOptimizationSwitch(ISyntaxPart parent) {
            Optimization result = CreateByTerminal<Optimization>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = CompilerOptmization.EnableOptimization;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = CompilerOptmization.DisableOptimization;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOptimizationDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseOpenStringSwitch(ISyntaxPart parent) {
            OpenStrings result = CreateByTerminal<OpenStrings>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = OpenStringTypes.EnableOpenStrings;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = OpenStringTypes.DisableOpenStrings;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOpenStringsDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseLongStringSwitch(ISyntaxPart parent) {
            LongStrings result = CreateByTerminal<LongStrings>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = LongStringTypes.EnableLongStrings;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = LongStringTypes.DisableLongStrings;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseLocalSymbolSwitch(ISyntaxPart parent) {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                LocalSymbols result = CreateByTerminal<LocalSymbols>(parent);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.Mode = LocalDebugSymbols.EnableLocalSymbols;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.Mode = LocalDebugSymbols.DisableLocalSymbols;
                }
                return;
            }

            FetchNextToken();
            ParseLinkParameter();
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

        private void ParseIncludeSwitch(ISyntaxPart parent) {

            if (LookAhead(1, TokenKind.Plus)) {
                IoChecks result = CreateByTerminal<IoChecks>(parent);
                ContinueWith(result, TokenKind.Plus);
                result.Mode = IoCallChecks.EnableIoChecks;
            }
            else if (LookAhead(1, TokenKind.Minus)) {
                IoChecks result = CreateByTerminal<IoChecks>(parent);
                ContinueWith(result, TokenKind.Minus);
                result.Mode = IoCallChecks.DisableIoChecks;
            }
            else if (LookAhead(1, PascalToken.Identifier) || LookAhead(1, PascalToken.QuotedString)) {
                var includeToken = Require(PascalToken.Identifier, PascalToken.QuotedString);
                var sourcePath = includeToken.FilePath;
                string filename = includeToken.Value;

                if (includeToken.Kind == PascalToken.QuotedString) {
                    filename = QuotedStringTokenValue.Unwrap(includeToken);
                }

                var targetPath = Meta.IncludePathResolver.ResolvePath(sourcePath, new FileReference(filename)).TargetPath;

                IFileAccess fileAccess = Options.Files;
                IncludeInput.AddFile(fileAccess.OpenFileForReading(targetPath));
            }
            else {
                IoChecks result = CreateByTerminal<IoChecks>(parent);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private bool ParseImportedDataSwitch() {
            FetchNextToken();

            if (Optional(TokenKind.Plus)) {
                CompilerOptions.ImportedData.Value = ImportGlobalUnitData.DoImport;
                return true;
            }

            if (Optional(TokenKind.Minus)) {
                CompilerOptions.ImportedData.Value = ImportGlobalUnitData.NoImport;
                return true;
            }

            Unexpected();
            return false;
        }

        private void ParseExtendedSyntaxSwitch(ISyntaxPart parent) {
            ExtSyntax result = CreateByTerminal<ExtSyntax>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = ExtendedSyntax.UseExtendedSyntax;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = ExtendedSyntax.NoExtendedSyntax;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseExtensionSwitch(ISyntaxPart parent) {
            Extension result = CreateByTerminal<Extension>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.ExecutableExtension = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseDebugInfoOrDescriptionSwitch(ISyntaxPart parent) {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                DebugInfoSwitch result = CreateByTerminal<DebugInfoSwitch>(parent);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.DebugInfo = DebugInformation.IncludeDebugInformation;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.DebugInfo = DebugInformation.NoDebugInfo;
                }
            }
            else if (LookAhead(1, PascalToken.QuotedString)) {
                Description result = CreateByTerminal<Description>(parent);
                ContinueWith(result, PascalToken.QuotedString);
                result.DescriptionValue = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
            }
            else {
                DebugInfoSwitch result = CreateByTerminal<DebugInfoSwitch>(parent);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.Plus, TokenKind.Minus, PascalToken.QuotedString });
            }
        }

        private void ParseAssertSwitch(ISyntaxPart parent) {
            AssertSwitch result = CreateByTerminal<AssertSwitch>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Assertions = AssertionMode.EnableAssertions;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Assertions = AssertionMode.DisableAssertions;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAssertDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseBoolEvalSwitch(ISyntaxPart parent) {
            BooleanEvaluationSwitch result = CreateByTerminal<BooleanEvaluationSwitch>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.BoolEval = BooleanEvaluation.CompleteEvaluation;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.BoolEval = BooleanEvaluation.ShortEvaluation;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidBoolEvalDirective, new[] { TokenKind.Plus, TokenKind.Minus });
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

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.AlignValue = Alignment.QuadWord;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.AlignValue = Alignment.Unaligned;
                }
                else {
                    ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAlignDirective, new[] { TokenKind.Plus, TokenKind.Minus });
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

        /// <summary>
        ///     allow an identifier if it was tokenized as a keyword
        /// </summary>
        /// <returns></returns>
        protected override bool AllowIdentifier()
            => CompilerDirectiveTokenizer.Keywords.ContainsKey(CurrentToken()?.Value);

    }

}
