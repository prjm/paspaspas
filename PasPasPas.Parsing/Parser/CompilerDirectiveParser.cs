using System.Collections.Generic;
using System;
using PasPasPas.Options.DataTypes;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;
using PasPasPas.Infrastructure.Log;

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
                PascalToken.LinkSwitchLong
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
                ParseIfOpt(parent);
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
                ParseLibParameter(parent);
            }
            else if (Match(PascalToken.Warn)) {
                ParseWarnParameter(parent);
            }
            else if (Match(PascalToken.Rtti)) {
                ParseRttiParameter(parent);
            }
            else if (Match(PascalToken.Region)) {
                ParseRegion(parent);
            }
            else if (Match(PascalToken.EndRegion)) {
                ParseEndRegion(parent);
            }
            else if (Match(PascalToken.SetPEOsVersion)) {
                ParsePEOsVersion(parent);
            }
            else if (Match(PascalToken.SetPESubsystemVersion)) {
                ParsePESubsystemVersion(parent);
            }
            else if (Match(PascalToken.SetPEUserVersion)) {
                ParsePEUserVersion(parent);
            }
            else if (Match(PascalToken.ObjTypeName)) {
                ParseObjTypeNameSwitch(parent);
            }
            else if (Match(PascalToken.NoInclude)) {
                ParseNoInclude(parent);
            }
            else if (Match(PascalToken.NoDefine)) {
                ParseNoDefine(parent);
            }
            else if (Match(PascalToken.MessageCd)) {
                ParseMessage(parent);
            }
            else if (Match(PascalToken.MinMemStackSizeSwitchLong, PascalToken.MaxMemStackSizeSwitchLong)) {
                ParseStackSizeSwitch(parent, false);
            }
        }

        private void ParseIfOpt(ISyntaxPart parent) {
            IfOpt result = CreateByTerminal<IfOpt>(parent);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                return;
            }

            result.SwitchKind = result.LastTerminal.Value;
            result.SwitchInfo = Options.GetSwitchInfo(result.LastTerminal.Value);

            if (!ContinueWith(result, TokenKind.Plus, TokenKind.Minus)) {
                return;
            }

            result.SwitchState = GetSwitchInfo(result.LastTerminal.Token.Kind);
        }

        private static SwitchInfo GetSwitchInfo(int switchState) {
            if (switchState == TokenKind.Plus)
                return SwitchInfo.Enabled;
            if (switchState == TokenKind.Minus)
                return SwitchInfo.Disabled;
            return SwitchInfo.Undefined;
        }

        private void ParseStackSizeSwitch(ISyntaxPart parent, bool mSwitch) {
            StackMemorySize result = CreateByTerminal<StackMemorySize>(parent);

            if (mSwitch || result.LastTerminal.Token.Kind == PascalToken.MinMemStackSizeSwitchLong) {

                if (!ContinueWith(result, TokenKind.Integer)) {
                    ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackMemSizeDirective, new[] { TokenKind.Integer });
                    return;
                }

                result.MinStackSize = DigitTokenGroupValue.Unwrap(result.LastTerminal.Token);
            }

            if (mSwitch)
                ContinueWith(result, TokenKind.Comma);

            if (mSwitch || result.LastTerminal.Token.Kind == PascalToken.MaxMemStackSizeSwitchLong) {

                if (!ContinueWith(result, TokenKind.Integer)) {
                    result.MinStackSize = null;
                    ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackMemSizeDirective, new[] { TokenKind.Integer });
                    return;
                }

                result.MaxStackSize = DigitTokenGroupValue.Unwrap(result.LastTerminal.Token);
            }
        }

        private void ParseMessage(ISyntaxPart parent) {
            Message result = CreateByTerminal<Message>(parent);

            string messageText = string.Empty;

            if (ContinueWith(result, TokenKind.Identifier)) {
                string messageType = result.LastTerminal.Value;

                if (string.Equals(messageType, "Hint", StringComparison.OrdinalIgnoreCase)) {
                    result.MessageType = MessageSeverity.Hint;
                }
                else if (string.Equals(messageType, "Warn", StringComparison.OrdinalIgnoreCase)) {
                    result.MessageType = MessageSeverity.Warning;
                }
                else if (string.Equals(messageType, "Error", StringComparison.OrdinalIgnoreCase)) {
                    result.MessageType = MessageSeverity.Error;
                }
                else if (string.Equals(messageType, "Fatal", StringComparison.OrdinalIgnoreCase)) {
                    result.MessageType = MessageSeverity.FatalError;
                }
                else {
                    ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidMessageDirective);
                    return;
                }
            }
            else {
                result.MessageType = MessageSeverity.Hint;
            }

            if (!ContinueWith(result, TokenKind.QuotedString)) {
                result.MessageType = MessageSeverity.Undefined;
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMessageDirective, new[] { TokenKind.QuotedString });
            }

            result.MessageText = QuotedStringTokenValue.Unwrap(CurrentToken());
        }

        private void ParseNoDefine(ISyntaxPart parent) {
            NoDefine result = CreateByTerminal<NoDefine>(parent);



            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidNoDefineDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.TypeName = result.LastTerminal.Value;

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.TypeNameInHpp = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);

                if (ContinueWith(result, TokenKind.QuotedString)) {
                    result.TypeNameInUnion = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
                }
            }
        }

        private void ParseNoInclude(ISyntaxPart parent) {
            NoInclude result = CreateByTerminal<NoInclude>(parent);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidNoIncludeDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.UnitName = result.LastTerminal.Value;
        }

        private void ParsePEUserVersion(ISyntaxPart parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent);
            result.Kind = PascalToken.SetPEUserVersion;
            ParsePEVersion(result);
        }

        private void ParsePESubsystemVersion(ISyntaxPart parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent);
            result.Kind = PascalToken.SetPESubsystemVersion;
            ParsePEVersion(result);
        }

        private void ParsePEOsVersion(ISyntaxPart parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent);
            result.Kind = PascalToken.SetPEOsVersion;
            ParsePEVersion(result);
        }

        private void ParsePEVersion(ParsedVersion result) {

            if (!ContinueWith(result, TokenKind.Real)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPEVersionDirective, new[] { TokenKind.Real });
                return;
            }

            var text = result.LastTerminal.Value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (text.Length != 2) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
                return;
            }

            int majorVersion;
            int minorVersion;

            if (!int.TryParse(text[0], out majorVersion) || !int.TryParse(text[1], out minorVersion)) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
                return;
            }

            result.MajorVersion = majorVersion;
            result.MinorVersion = minorVersion;

        }

        private void ParseEndRegion(ISyntaxPart parent) {
            CreateByTerminal<EndRegion>(parent);
        }

        private void ParseRegion(ISyntaxPart parent) {
            Region result = CreateByTerminal<Region>(parent);

            if (!ContinueWith(result, TokenKind.QuotedString)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRegionDirective, new[] { TokenKind.QuotedString });
                return;
            }

            result.RegionName = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
        }

        /// <summary>
        ///     parse the rtti parameter
        /// </summary>
        private void ParseRttiParameter(ISyntaxPart parent) {
            RttiControl result = CreateByTerminal<RttiControl>(parent);

            if (ContinueWith(result, PascalToken.Inherit)) {
                result.Mode = RttiGenerationMode.Inherit;
            }
            else if (ContinueWith(result, PascalToken.Explicit)) {
                result.Mode = RttiGenerationMode.Explicit;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { PascalToken.Inherit, PascalToken.Explicit });
                return;
            }

            if (!Match(new[] { PascalToken.Fields, PascalToken.Methods, PascalToken.Properties }))
                return;

            bool canContinue;

            do {
                canContinue = false;
                if (ContinueWith(result, PascalToken.Methods)) {
                    if (result.Methods != null) {
                        result.Mode = RttiGenerationMode.Undefined;
                        ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new int[0]);
                        return;
                    }
                    result.Methods = ParseRttiVisibility(result);
                    canContinue = result.Methods != null;
                }
                else if (ContinueWith(result, PascalToken.Properties)) {
                    if (result.Properties != null) {
                        result.Mode = RttiGenerationMode.Undefined;
                        ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new int[0]);
                        return;
                    }
                    result.Properties = ParseRttiVisibility(result);
                    canContinue = result.Properties != null;
                }
                else if (ContinueWith(result, PascalToken.Fields)) {
                    if (result.Fields != null) {
                        result.Mode = RttiGenerationMode.Undefined;
                        ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new int[0]);
                        return;
                    }
                    result.Fields = ParseRttiVisibility(result);
                    canContinue = result.Fields != null;
                }
            } while (canContinue);
        }

        private RttiForVisibility ParseRttiVisibility(RttiControl result) {
            if (!ContinueWith(result, TokenKind.OpenParen)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.OpenBraces });
                result.Mode = RttiGenerationMode.Undefined;
                return null;
            }

            if (!ContinueWith(result, TokenKind.OpenBraces)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.OpenBraces });
                result.Mode = RttiGenerationMode.Undefined;
                return null;
            }

            var visibility = new RttiForVisibility();

            do {
                if (!ContinueWith(result, PascalToken.VcPrivate, PascalToken.VcProtected, PascalToken.VcPublic, PascalToken.VcPublished)) {
                    break;
                }

                var kind = result.LastTerminal.Token.Kind;

                switch (kind) {
                    case PascalToken.VcPrivate:
                        visibility.ForPrivate = true;
                        break;
                    case PascalToken.VcProtected:
                        visibility.ForProtected = true;
                        break;
                    case PascalToken.VcPublic:
                        visibility.ForPublic = true;
                        break;
                    case PascalToken.VcPublished:
                        visibility.ForPublished = true;
                        break;
                    default: {
                            ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new int[0]);
                            result.Mode = RttiGenerationMode.Undefined;
                            return null;
                        }
                }
            } while (ContinueWith(result, TokenKind.Comma));

            if (!ContinueWith(result, TokenKind.CloseBraces)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.CloseBraces });
                result.Mode = RttiGenerationMode.Undefined;
                return null;
            }

            if (!ContinueWith(result, TokenKind.CloseParen)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.CloseParen });
                result.Mode = RttiGenerationMode.Undefined;
                return null;
            }

            return visibility;
        }

        private void ParseWarnParameter(ISyntaxPart parent) {
            WarnSwitch result = CreateByTerminal<WarnSwitch>(parent);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarnDirective, new[] { TokenKind.Identifier });
                return;
            }

            var warningType = result.LastTerminal.Value;
            var warningModes = new[] { TokenKind.On, PascalToken.Off, PascalToken.Error, TokenKind.Default };

            if (!ContinueWith(result, warningModes)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarnDirective, warningModes);
                return;
            }

            var warningMode = result.LastTerminal.Token.Kind;
            var parsedMode = WarningMode.Undefined;

            switch (warningMode) {
                case TokenKind.On:
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

        private void ParseLibParameter(ISyntaxPart parent) {
            LibInfo result = CreateByTerminal<LibInfo>(parent);
            int kind = result.LastTerminal.Token.Kind;

            if (!ContinueWith(result, TokenKind.QuotedString)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLibDirective, new[] { TokenKind.QuotedString });
                return;
            }

            var libInfo = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);

            switch (kind) {
                case PascalToken.LibPrefix:
                    result.LibPrefix = libInfo;
                    break;

                case PascalToken.LibSuffix:
                    result.LibSuffix = libInfo;
                    break;

                case PascalToken.LibVersion:
                    result.LibVersion = libInfo;
                    break;
            }
        }

        private void ParseImageBase(ISyntaxPart parent) {
            ImageBase result = CreateByTerminal<ImageBase>(parent);

            if (ContinueWith(result, TokenKind.Integer)) {
                result.BaseValue = DigitTokenGroupValue.Unwrap(result.LastTerminal.Token);
            }
            else if (ContinueWith(result, TokenKind.HexNumber)) {
                result.BaseValue = HexNumberTokenValue.Unwrap(result.LastTerminal.Token);
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidImageBaseDirective, new[] { TokenKind.Integer, TokenKind.HexNumber });
            }
        }

        private void ParseIfNDef(ISyntaxPart parent) {
            IfDef result = CreateByTerminal<IfDef>(parent);
            result.Negate = true;

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfNDefDirective, new[] { TokenKind.Identifier });
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
                if (ContinueWith(result, TokenKind.QuotedString)) {
                    result.EmitValue = result.LastTerminal.Value;
                }
                else {
                    result.Mode = HppEmitMode.Undefined;
                    ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidHppEmitDirective, new[] { TokenKind.QuotedString });
                }
            }
        }

        private void ParseExternalSym(ISyntaxPart parent) {
            ExternalSymbolDeclaration result = CreateByTerminal<ExternalSymbolDeclaration>(parent);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExternalSymbolDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.IdentifierName = result.LastTerminal.Value;

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.SymbolName = result.LastTerminal.Value;
            }

            if (ContinueWith(result, TokenKind.QuotedString)) {
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

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfDefDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseUndef(ISyntaxPart parent) {
            UnDefineSymbol result = CreateByTerminal<UnDefineSymbol>(parent);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidUnDefineDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseDefine(ISyntaxPart parent) {
            DefineSymbol result = CreateByTerminal<DefineSymbol>(parent);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidDefineDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseCodeAlignParameter(ISyntaxPart parent) {
            CodeAlignParameter result = CreateByTerminal<CodeAlignParameter>(parent);

            int value;
            if (ContinueWith(result, TokenKind.Integer) && int.TryParse(result.LastTerminal.Value, out value)) {
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

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidCodeAlignDirective, new[] { TokenKind.Integer });
        }

        private void ParseApptypeParameter(ISyntaxPart parent) {
            AppTypeParameter result = CreateByTerminal<AppTypeParameter>(parent);

            if (ContinueWith(result, TokenKind.Identifier)) {

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

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidApplicationType, new[] { TokenKind.Identifier });
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

            else if (Match(PascalToken.ImportedDataSwitchLong)) {
                ParseLongImportedDataSwitch(parent);
            }

            else if (Match(PascalToken.IncludeSwitchLong)) {
                ParseLongIncludeSwitch(parent);
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
                ParseWeakPackageUnitSwitch(parent);
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
                ParseLongIncludeRessourceSwitch(parent);
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

            else if (Match(PascalToken.EnumSizeSwitchLong)) {
                ParseLongEnumSizeSwitch(parent);
            }

            else if (Match(PascalToken.MethodInfo)) {
                ParseMethodInfoSwitch(parent);
            }

            else if (Match(PascalToken.LegacyIfEnd)) {
                ParseLegacyIfEndSwitch(parent);
            }

            else if (Match(PascalToken.LinkSwitchLong)) {
                ParseLongLinkSwitch(parent);
            }

        }

        private void ParseLongLinkSwitch(ISyntaxPart parent) {
            Link resultLink = CreateByTerminal<Link>(parent);
            ParseLinkParameter(resultLink);
        }

        private void ParseLegacyIfEndSwitch(ISyntaxPart parent) {
            LegacyIfEnd result = CreateByTerminal<LegacyIfEnd>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = EndIfMode.LegacyIfEnd;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = EndIfMode.Standard;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLegacyIfEndDirective, new[] { PascalToken.Off, TokenKind.On });
            }
        }

        private void ParseMethodInfoSwitch(ISyntaxPart parent) {
            MethodInfo result = CreateByTerminal<MethodInfo>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = MethodInfoRtti.EnableMethodInfo;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = MethodInfoRtti.DisableMethodInfo;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMethodInfoDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongEnumSizeSwitch(ISyntaxPart parent) {
            MinEnumSize result = CreateByTerminal<MinEnumSize>(parent);

            if (!ContinueWith(result, TokenKind.Integer)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective, new[] { TokenKind.Integer });
                return;
            }

            var size = DigitTokenGroupValue.Unwrap(result.LastTerminal.Token);

            switch (size) {
                case 1:
                    result.Size = EnumSize.OneByte;
                    break;
                case 2:
                    result.Size = EnumSize.TwoByte;
                    break;
                case 4:
                    result.Size = EnumSize.FourByte;
                    break;
                default:
                    result.Size = EnumSize.Undefined;
                    ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective);
                    break;
            }
        }

        private void ParseOldTypeLayoutSwitch(ISyntaxPart parent) {
            OldTypeLayout result = CreateByTerminal<OldTypeLayout>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = OldRecordTypes.EnableOldRecordPacking;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = OldRecordTypes.DisableOldRecordPacking;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOldTypeLayoutDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParsePointermathSwitch(ISyntaxPart parent) {
            PointerMath result = CreateByTerminal<PointerMath>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = PointerManipulation.EnablePointerMath;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = PointerManipulation.DisablePointerMath;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPointerMathDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseRealCompatibilitySwitch(ISyntaxPart parent) {
            RealCompatibility result = CreateByTerminal<RealCompatibility>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = Real48.EnableCompatibility;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = Real48.DisableCompatibility;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRealCompatibilityMode, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongIncludeRessourceSwitch(ISyntaxPart parent) {
            Resource result = CreateByTerminal<Resource>(parent);
            ParseResourceFileName(result);
        }

        private void ParseRunOnlyParameter(ISyntaxPart parent) {
            RunOnly result = CreateByTerminal<RunOnly>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RuntimePackageMode.RuntimeOnly;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RuntimePackageMode.Standard;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRunOnlyDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongTypeInfoSwitch(ISyntaxPart parent) {
            PublishedRtti result = CreateByTerminal<PublishedRtti>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RttiForPublishedProperties.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RttiForPublishedProperties.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseScopedEnums(ISyntaxPart parent) {
            ScopedEnums result = CreateByTerminal<ScopedEnums>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RequireScopedEnums.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RequireScopedEnums.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidScopedEnumsDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseStrongLinkTypes(ISyntaxPart parent) {
            StrongLinkTypes result = CreateByTerminal<StrongLinkTypes>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = StrongTypeLinking.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = StrongTypeLinking.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStrongLinkTypesDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseReferenceInfoSwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.ReferencesMode = SymbolReferenceInfo.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.ReferencesMode = SymbolReferenceInfo.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseDefinitionInfoSwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = SymbolDefinitionInfo.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = SymbolDefinitionInfo.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongTypedPointersSwitch(ISyntaxPart parent) {
            TypedPointers result = CreateByTerminal<TypedPointers>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = TypeCheckedPointers.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = TypeCheckedPointers.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongVarStringCheckSwitch(ISyntaxPart parent) {
            VarStringChecks result = CreateByTerminal<VarStringChecks>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ShortVarStringChecks.EnableChecks;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ShortVarStringChecks.DisableChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStringCheckDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseWarningsSwitch(ISyntaxPart parent) {
            Warnings result = CreateByTerminal<Warnings>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerWarnings.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = CompilerWarnings.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarningsDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseWeakPackageUnitSwitch(ISyntaxPart parent) {
            WeakPackageUnit result = CreateByTerminal<WeakPackageUnit>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = WeakPackaging.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = WeakPackaging.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWeakPackageUnitDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseWeakLinkRttiSwitch(ISyntaxPart parent) {
            WeakLinkRtti result = CreateByTerminal<WeakLinkRtti>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RttiLinkMode.LinkWeakRtti;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RttiLinkMode.LinkFullRtti;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWeakLinkRttiDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongWritableConstSwitch(ISyntaxPart parent) {
            WritableConsts result = CreateByTerminal<WritableConsts>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ConstantValues.Writable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ConstantValues.Constant;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseZeroBasedStringSwitch(ISyntaxPart parent) {
            ZeroBasedStrings result = CreateByTerminal<ZeroBasedStrings>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = FirstCharIndex.IsZero;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = FirstCharIndex.IsOne;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidZeroBasedStringsDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongStackFramesSwitch(ISyntaxPart parent) {
            StackFrames result = CreateByTerminal<StackFrames>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = StackFrameGeneration.EnableFrames;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = StackFrameGeneration.DisableFrames;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackFramesDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongRangeChecksSwitch(ISyntaxPart parent) {
            RangeChecks result = CreateByTerminal<RangeChecks>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RuntimeRangeChecks.EnableRangeChecks;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RuntimeRangeChecks.DisableRangeChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRangeCheckDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongSafeDivideSwitch(ISyntaxPart parent) {
            SafeDivide result = CreateByTerminal<SafeDivide>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = FDivSafeDivide.EnableSafeDivide;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = FDivSafeDivide.DisableSafeDivide;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidSafeDivide, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongOverflowSwitch(ISyntaxPart parent) {
            Overflow result = CreateByTerminal<Overflow>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RuntimeOverflowChecks.EnableChecks;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = RuntimeOverflowChecks.DisableChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongOptimizationSwitch(ISyntaxPart parent) {
            Optimization result = CreateByTerminal<Optimization>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerOptmization.EnableOptimization;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = CompilerOptmization.DisableOptimization;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOptimizationDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongOpenStringSwitch(ISyntaxPart parent) {
            OpenStrings result = CreateByTerminal<OpenStrings>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = OpenStringTypes.EnableOpenStrings;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = OpenStringTypes.DisableOpenStrings;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOpenStringsDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongLongStringSwitch(ISyntaxPart parent) {
            LongStrings result = CreateByTerminal<LongStrings>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = LongStringTypes.EnableLongStrings;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = LongStringTypes.DisableLongStrings;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongLocalSymbolSwitch(ISyntaxPart parent) {
            LocalSymbols result = CreateByTerminal<LocalSymbols>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = LocalDebugSymbols.EnableLocalSymbols;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = LocalDebugSymbols.DisableLocalSymbols;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLocalSymbolsDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongIoChecksSwitch(ISyntaxPart parent) {
            IoChecks result = CreateByTerminal<IoChecks>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = IoCallChecks.EnableIoChecks;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = IoCallChecks.DisableIoChecks;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongIncludeSwitch(ISyntaxPart parent) {
            Include result = CreateByTerminal<Include>(parent);
            ParseIncludeFileName(result);
        }

        private void ParseLongImportedDataSwitch(ISyntaxPart parent) {
            ImportedData result = CreateByTerminal<ImportedData>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ImportGlobalUnitData.DoImport;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ImportGlobalUnitData.NoImport;
                return;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidImportedDataDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongImplicitBuildSwitch(ISyntaxPart parent) {
            ImplicitBuild result = CreateByTerminal<ImplicitBuild>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ImplicitBuildUnit.EnableImplicitBuild;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ImplicitBuildUnit.DisableImplicitBuild;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidImplicitBuildDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongHintsSwitch(ISyntaxPart parent) {
            Hints result = CreateByTerminal<Hints>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerHints.EnableHints;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = CompilerHints.DisableHints;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidHintsDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongHighCharUnicodeSwitch(ISyntaxPart parent) {
            HighCharUnicodeSwitch result = CreateByTerminal<HighCharUnicodeSwitch>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = HighCharsUnicode.EnableHighChars;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = HighCharsUnicode.DisableHighChars;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidHighCharUnicodeDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongExcessPrecisionSwitch(ISyntaxPart parent) {
            ExcessPrecision result = CreateByTerminal<ExcessPrecision>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ExcessPrecisionForResults.EnableExcess;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ExcessPrecisionForResults.DisableExcess;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongExtendedSyntaxSwitch(ISyntaxPart parent) {
            ExtSyntax result = CreateByTerminal<ExtSyntax>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ExtendedSyntax.UseExtendedSyntax;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ExtendedSyntax.NoExtendedSyntax;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseExtendedCompatibilitySwitch(ISyntaxPart parent) {
            ExtendedCompatibility result = CreateByTerminal<ExtendedCompatibility>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ExtendedCompatiblityMode.Enabled;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ExtendedCompatiblityMode.Disabled;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseObjExportAllSwitch(ISyntaxPart parent) {
            ObjectExport result = CreateByTerminal<ObjectExport>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                CompilerOptions.ExportCppObjects.Value = ExportCppObjects.ExportAll;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                CompilerOptions.ExportCppObjects.Value = ExportCppObjects.DoNotExportAll;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidObjectExportDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongExtensionSwitch(ISyntaxPart parent) {
            Extension result = CreateByTerminal<Extension>(parent);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.ExecutableExtension = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseLongDesignOnlySwitch(ISyntaxPart parent) {
            DesignOnly result = CreateByTerminal<DesignOnly>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.DesignTimeOnly = DesignOnlyUnit.InDesignTimeOnly;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.DesignTimeOnly = DesignOnlyUnit.Alltimes;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongDescriptionSwitch(ISyntaxPart parent) {
            Description result = CreateByTerminal<Description>(parent);

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.DescriptionValue = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDescriptionDirective, new[] { TokenKind.QuotedString });
            }
        }

        private void ParseDenyPackageUnitSwitch(ISyntaxPart parent) {
            ParseDenyPackageUnit result = CreateByTerminal<ParseDenyPackageUnit>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.DenyUnit = DenyUnitInPackages.DenyUnit;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.DenyUnit = DenyUnitInPackages.AllowUnit;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongDebugInfoSwitch(ISyntaxPart parent) {
            DebugInfoSwitch result = CreateByTerminal<DebugInfoSwitch>(parent);
            if (ContinueWith(result, TokenKind.On)) {
                result.DebugInfo = DebugInformation.IncludeDebugInformation;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.DebugInfo = DebugInformation.NoDebugInfo;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongAssertSwitch(ISyntaxPart parent) {
            AssertSwitch result = CreateByTerminal<AssertSwitch>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.Assertions = AssertionMode.EnableAssertions;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Assertions = AssertionMode.DisableAssertions;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAssertDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongBoolEvalSwitch(ISyntaxPart parent) {
            var result = CreateByTerminal<BooleanEvaluationSwitch>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.BoolEval = BooleanEvaluation.CompleteEvaluation;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.BoolEval = BooleanEvaluation.ShortEvaluation;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidBoolEvalDirective, new[] { TokenKind.On, PascalToken.Off });
            }
        }

        private void ParseLongAlignSwitch(ISyntaxPart parent) {
            AlignSwitch result = CreateByTerminal<AlignSwitch>(parent);

            if (ContinueWith(result, TokenKind.On)) {
                result.AlignValue = Alignment.QuadWord;
                return;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.AlignValue = Alignment.Unaligned;
                return;
            }

            int value;
            if (ContinueWith(result, TokenKind.Integer) && int.TryParse(result.LastTerminal.Value, out value)) {

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

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAlignDirective, new[] { TokenKind.Integer });
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
                ParseImportedDataSwitch(parent);
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
                ParseEnumSizeSwitch(parent);
            }

        }

        private void ParseEnumSizeSwitch(ISyntaxPart parent) {
            MinEnumSize result = CreateByTerminal<MinEnumSize>(parent);
            var kind = result.LastTerminal.Token.Kind;

            if (kind == PascalToken.EnumSize1) {
                result.Size = EnumSize.OneByte;
            }
            else if (kind == PascalToken.EnumSize2) {
                result.Size = EnumSize.TwoByte;
            }
            else if (kind == PascalToken.EnumSize4) {
                result.Size = EnumSize.FourByte;
            }
            else if (ContinueWith(result, TokenKind.Plus)) {
                result.Size = EnumSize.FourByte;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Size = EnumSize.OneByte;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseObjTypeNameSwitch(ISyntaxPart parent) {
            ObjTypeName result = CreateByTerminal<ObjTypeName>(parent);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidObjTypeDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.TypeName = result.LastTerminal.Value;

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.AliasName = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
                if (string.IsNullOrWhiteSpace(result.AliasName)) {
                    result.AliasName = null;
                    result.TypeName = null;
                    ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
                    return;
                }


                string prefix = result.AliasName.Substring(0, 1);
                if (!string.Equals(prefix, "N", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(prefix, "B", StringComparison.OrdinalIgnoreCase)) {
                    result.AliasName = null;
                    result.TypeName = null;
                    ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
                    return;
                }
            }
        }

        private void ParseTypeInfoSwitch(ISyntaxPart parent) {

            if (LookAhead(1, TokenKind.Integer)) {
                ParseStackSizeSwitch(parent, true);
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
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { TokenKind.On, PascalToken.Off });
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

            Resource resultR = CreateByTerminal<Resource>(parent);
            ParseResourceFileName(resultR);
        }

        private string ParseFileName(SyntaxPartBase parent, bool allowTimes) {

            if (ContinueWith(parent, TokenKind.QuotedString)) {
                return QuotedStringTokenValue.Unwrap(parent.LastTerminal.Token);
            }

            else if (ContinueWith(parent, TokenKind.Identifier)) {
                return parent.LastTerminal.Value;
            }

            else if (allowTimes && ContinueWith(parent, TokenKind.Times)) {
                if (!ContinueWith(parent, TokenKind.Identifier)) {
                    ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidFileName, new[] { TokenKind.Identifier });
                    return null;
                }
                else {
                    return string.Concat("*", parent.LastTerminal.Value);
                }
            }

            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidFileName, new[] { TokenKind.Identifier });
                return null;
            }
        }

        private void ParseIncludeFileName(Include result) {
            result.FileName = ParseFileName(result, false);

            if (result.FileName == null) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidIncludeDirective, new[] { TokenKind.Identifier });
                return;
            }
        }

        private void ParseResourceFileName(Resource result) {
            result.FileName = ParseFileName(result, true);

            if (result.FileName == null) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidResourceDirective, new[] { TokenKind.Identifier });
                return;
            }

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.RcFile = result.LastTerminal.Value;
            }
            else if (ContinueWith(result, TokenKind.QuotedString)) {
                result.RcFile = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
            }
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

            Link resultLink = CreateByTerminal<Link>(parent);
            ParseLinkParameter(resultLink);
        }

        /// <summary>
        ///     parse a linked file parameter
        /// </summary>
        /// <returns></returns>
        private void ParseLinkParameter(Link result) {
            result.FileName = ParseFileName(result, false);
            if (result.FileName == null) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidLinkDirective);
            }
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
            else if (LookAhead(1, TokenKind.Identifier) || LookAhead(1, TokenKind.QuotedString)) {
                Include result = CreateByTerminal<Include>(parent);
                ParseIncludeFileName(result);
            }
            else {
                IoChecks result = CreateByTerminal<IoChecks>(parent);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseImportedDataSwitch(ISyntaxPart parent) {
            ImportedData result = CreateByTerminal<ImportedData>(parent);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = ImportGlobalUnitData.DoImport;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = ImportGlobalUnitData.NoImport;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidImportedDataDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
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

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.ExecutableExtension = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
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
            else if (LookAhead(1, TokenKind.QuotedString)) {
                Description result = CreateByTerminal<Description>(parent);
                ContinueWith(result, TokenKind.QuotedString);
                result.DescriptionValue = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
            }
            else {
                DebugInfoSwitch result = CreateByTerminal<DebugInfoSwitch>(parent);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.Plus, TokenKind.Minus, TokenKind.QuotedString });
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
