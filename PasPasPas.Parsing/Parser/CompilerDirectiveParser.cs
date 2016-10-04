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
                TokenKind.AlignSwitch, TokenKind.AlignSwitch1,TokenKind.AlignSwitch2,TokenKind.AlignSwitch4,TokenKind.AlignSwitch8,TokenKind.AlignSwitch16, /* A */
                TokenKind.BoolEvalSwitch, /* B */
                TokenKind.AssertSwitch, /* C */
                TokenKind.DebugInfoOrDescriptionSwitch, /* D */
                TokenKind.ExtensionSwitch, /* E */
                TokenKind.ExtendedSyntaxSwitch, /* X */
                TokenKind.ImportedDataSwitch,  /* G */
                TokenKind.IncludeSwitch, /* I */ /* IOCHECKS */
                TokenKind.LinkOrLocalSymbolSwitch,  /* L */
                TokenKind.LongStringSwitch, /* H */
                TokenKind.OpenStringSwitch, /* P */
                TokenKind.OptimizationSwitch, /* O */
                TokenKind.OverflowSwitch, /* Q */
                TokenKind.SaveDivideSwitch, /* U */
                TokenKind.IncludeRessource, /* R */ /* RANGECHECKS */
                TokenKind.StackFramesSwitch, /* W */
                TokenKind.WritableConstSwitch, /* J */
                TokenKind.VarStringCheckSwitch, /* V */
                TokenKind.TypedPointersSwitch, /* T */
                TokenKind.SymbolDeclarationSwitch, /* Y */
                TokenKind.SymbolDefinitionsOnlySwitch, /* YD */
                TokenKind.TypeInfoSwitch, /* M */
                TokenKind.EnumSize1, /* Z1 */
                TokenKind.EnumSize2, /* Z2 */
                TokenKind.EnumSize4, /* Z4 */
                TokenKind.EnumSizeSwitch, /* Z */
            };

        /// <summary>
        ///     supported long switches
        /// </summary>
        private static HashSet<int> longSwitches
            = new HashSet<int>() {
                TokenKind.AlignSwitchLong,
                TokenKind.BoolEvalSwitchLong,
                TokenKind.AssertSwitchLong,
                TokenKind.DebugInfoSwitchLong,
                TokenKind.DenyPackageUnit,
                TokenKind.DescriptionSwitchLong,
                TokenKind.DesignOnly,
                TokenKind.ExtensionSwitchLong,
                TokenKind.ObjExportAll,
                TokenKind.ExtendedCompatibility,
                TokenKind.ExtendedSyntaxSwitchLong,
                TokenKind.ExcessPrecision,
                TokenKind.HighCharUnicode,
                TokenKind.Hints,
                TokenKind.ImplicitBuild,
                TokenKind.ImportedDataSwitchLong,
                TokenKind.IncludeSwitchLong,
                TokenKind.IoChecks,
                TokenKind.LocalSymbolSwithLong,
                TokenKind.LongStringSwitchLong,
                TokenKind.OpenStringSwitchLong,
                TokenKind.OptimizationSwitchLong,
                TokenKind.OverflowSwitchLong,
                TokenKind.SafeDivideSwitchLong,
                TokenKind.RangeChecks,
                TokenKind.StackFramesSwitchLong,
                TokenKind.ZeroBaseStrings,
                TokenKind.WritableConstSwitchLong,
                TokenKind.WeakLinkRtti,
                TokenKind.WeakPackageUnit,
                TokenKind.Warnings,
                TokenKind.VarStringCheckSwitchLong,
                TokenKind.TypedPointersSwitchLong,
                TokenKind.ReferenceInfo,
                TokenKind.DefinitionInfo,
                TokenKind.StrongLinkTypes,
                TokenKind.ScopedEnums,
                TokenKind.TypeInfoSwitchLong,
                TokenKind.RunOnly,
                TokenKind.IncludeRessourceLong,
                TokenKind.RealCompatibility,
                TokenKind.Pointermath,
                TokenKind.OldTypeLayout,
                TokenKind.EnumSizeSwitchLong,
                TokenKind.MethodInfo,
                TokenKind.LegacyIfEnd,
                TokenKind.LinkSwitchLong
            };

        /// <summary>
        ///     supported parameters and directives
        /// </summary>
        private static HashSet<int> parameters
            = new HashSet<int>() {
                TokenKind.Apptype,
                TokenKind.CodeAlign,
                TokenKind.Define,
                TokenKind.Undef,
                TokenKind.IfDef,
                TokenKind.EndIf,
                TokenKind.ElseCd,
                TokenKind.ExternalSym,
                TokenKind.HppEmit,
                TokenKind.IfNDef,
                TokenKind.ImageBase,
                TokenKind.LibPrefix,
                TokenKind.LibSuffix,
                TokenKind.LibVersion,
                TokenKind.Warn,
                TokenKind.Rtti,
                TokenKind.Region,
                TokenKind.EndRegion,
                TokenKind.SetPEOsVersion,
                TokenKind.SetPESubsystemVersion,
                TokenKind.SetPEUserVersion,
                TokenKind.ObjTypeName,
                TokenKind.NoInclude,
                TokenKind.NoDefine,
                TokenKind.MessageCd,
                TokenKind.MinMemStackSizeSwitchLong,
                TokenKind.MaxMemStackSizeSwitchLong,
                TokenKind.IfOpt,
            };

        private void ParseParameter(ISyntaxPart parent) {

            if (Match(TokenKind.IfDef)) {
                ParseIfDef(parent);
            }
            else if (Match(TokenKind.IfOpt)) {
                ParseIfOpt(parent);
            }
            else if (Match(TokenKind.EndIf)) {
                ParseEndIf(parent);
            }
            else if (Match(TokenKind.ElseCd)) {
                ParseElse(parent);
            }
            else if (Match(TokenKind.IfNDef)) {
                ParseIfNDef(parent);
            }
            else if (Match(TokenKind.Apptype)) {
                ParseApptypeParameter(parent);
            }
            else if (Match(TokenKind.CodeAlign)) {
                ParseCodeAlignParameter(parent);
            }
            else if (Match(TokenKind.Define)) {
                ParseDefine(parent);
            }
            else if (Match(TokenKind.Undef)) {
                ParseUndef(parent);
            }
            else if (Match(TokenKind.ExternalSym)) {
                ParseExternalSym(parent);
            }
            else if (Match(TokenKind.HppEmit)) {
                ParseHppEmit(parent);
            }
            else if (Match(TokenKind.ImageBase)) {
                ParseImageBase(parent);
            }
            else if (Match(TokenKind.LibPrefix, TokenKind.LibSuffix, TokenKind.LibVersion)) {
                ParseLibParameter(parent);
            }
            else if (Match(TokenKind.Warn)) {
                ParseWarnParameter(parent);
            }
            else if (Match(TokenKind.Rtti)) {
                ParseRttiParameter(parent);
            }
            else if (Match(TokenKind.Region)) {
                ParseRegion(parent);
            }
            else if (Match(TokenKind.EndRegion)) {
                ParseEndRegion(parent);
            }
            else if (Match(TokenKind.SetPEOsVersion)) {
                ParsePEOsVersion(parent);
            }
            else if (Match(TokenKind.SetPESubsystemVersion)) {
                ParsePESubsystemVersion(parent);
            }
            else if (Match(TokenKind.SetPEUserVersion)) {
                ParsePEUserVersion(parent);
            }
            else if (Match(TokenKind.ObjTypeName)) {
                ParseObjTypeNameSwitch(parent);
            }
            else if (Match(TokenKind.NoInclude)) {
                ParseNoInclude(parent);
            }
            else if (Match(TokenKind.NoDefine)) {
                ParseNoDefine(parent);
            }
            else if (Match(TokenKind.MessageCd)) {
                ParseMessage(parent);
            }
            else if (Match(TokenKind.MinMemStackSizeSwitchLong, TokenKind.MaxMemStackSizeSwitchLong)) {
                ParseStackSizeSwitch(parent, false);
            }
        }

        private void ParseIfOpt(ISyntaxPart parent) {
            IfOpt result = CreateByTerminal<IfOpt>(parent, TokenKind.IfOpt);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                return;
            }

            result.SwitchKind = result.LastTerminalValue;
            result.SwitchInfo = Options.GetSwitchInfo(result.LastTerminalValue);

            if (!ContinueWith(result, TokenKind.Plus, TokenKind.Minus)) {
                return;
            }

            result.SwitchState = GetSwitchInfo(result.LastTerminalKind);
        }

        private static SwitchInfo GetSwitchInfo(int switchState) {
            if (switchState == TokenKind.Plus)
                return SwitchInfo.Enabled;
            if (switchState == TokenKind.Minus)
                return SwitchInfo.Disabled;
            return SwitchInfo.Undefined;
        }

        private void ParseStackSizeSwitch(ISyntaxPart parent, bool mSwitch) {
            StackMemorySize result = CreateByTerminal<StackMemorySize>(parent, TokenKind.MinMemStackSizeSwitchLong, TokenKind.MaxMemStackSizeSwitchLong, TokenKind.TypeInfoSwitch);

            if (mSwitch || result.LastTerminalKind == TokenKind.MinMemStackSizeSwitchLong) {

                if (!ContinueWith(result, TokenKind.Integer)) {
                    ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                    return;
                }

                result.MinStackSize = DigitTokenGroupValue.Unwrap(result.LastTerminalToken);
            }

            if (mSwitch)
                ContinueWith(result, TokenKind.Comma);

            if (mSwitch || result.LastTerminalKind == TokenKind.MaxMemStackSizeSwitchLong) {

                if (!ContinueWith(result, TokenKind.Integer)) {
                    result.MinStackSize = null;
                    ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                    return;
                }

                result.MaxStackSize = DigitTokenGroupValue.Unwrap(result.LastTerminalToken);
            }
        }

        private void ParseMessage(ISyntaxPart parent) {
            Message result = CreateByTerminal<Message>(parent, TokenKind.MessageCd);

            if (ContinueWith(result, TokenKind.Identifier)) {
                string messageType = result.LastTerminalValue;

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
            NoDefine result = CreateByTerminal<NoDefine>(parent, TokenKind.NoDefine);



            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidNoDefineDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.TypeName = result.LastTerminalValue;

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.TypeNameInHpp = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);

                if (ContinueWith(result, TokenKind.QuotedString)) {
                    result.TypeNameInUnion = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
                }
            }
        }

        private void ParseNoInclude(ISyntaxPart parent) {
            NoInclude result = CreateByTerminal<NoInclude>(parent, TokenKind.NoInclude);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidNoIncludeDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.UnitName = result.LastTerminalValue;
        }

        private void ParsePEUserVersion(ISyntaxPart parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent, TokenKind.SetPEUserVersion);
            result.Kind = TokenKind.SetPEUserVersion;
            ParsePEVersion(result);
        }

        private void ParsePESubsystemVersion(ISyntaxPart parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent, TokenKind.SetPESubsystemVersion);
            result.Kind = TokenKind.SetPESubsystemVersion;
            ParsePEVersion(result);
        }

        private void ParsePEOsVersion(ISyntaxPart parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent, TokenKind.SetPEOsVersion);
            result.Kind = TokenKind.SetPEOsVersion;
            ParsePEVersion(result);
        }

        private void ParsePEVersion(ParsedVersion result) {

            if (!ContinueWith(result, TokenKind.Real)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPEVersionDirective, new[] { TokenKind.Real });
                return;
            }

            var text = result.LastTerminalValue.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

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
            CreateByTerminal<EndRegion>(parent, TokenKind.EndRegion);
        }

        private void ParseRegion(ISyntaxPart parent) {
            Region result = CreateByTerminal<Region>(parent, TokenKind.Region);

            if (!ContinueWith(result, TokenKind.QuotedString)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRegionDirective, new[] { TokenKind.QuotedString });
                return;
            }

            result.RegionName = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
        }

        /// <summary>
        ///     parse the rtti parameter
        /// </summary>
        private void ParseRttiParameter(ISyntaxPart parent) {
            RttiControl result = CreateByTerminal<RttiControl>(parent, TokenKind.Rtti);

            if (ContinueWith(result, TokenKind.Inherit)) {
                result.Mode = RttiGenerationMode.Inherit;
            }
            else if (ContinueWith(result, TokenKind.Explicit)) {
                result.Mode = RttiGenerationMode.Explicit;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.Inherit, TokenKind.Explicit });
                return;
            }

            if (!Match(TokenKind.Fields, TokenKind.Methods, TokenKind.Properties))
                return;

            bool canContinue;

            do {
                canContinue = false;
                if (ContinueWith(result, TokenKind.Methods)) {
                    if (result.Methods != null) {
                        result.Mode = RttiGenerationMode.Undefined;
                        ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new int[0]);
                        return;
                    }
                    result.Methods = ParseRttiVisibility(result);
                    canContinue = result.Methods != null;
                }
                else if (ContinueWith(result, TokenKind.Properties)) {
                    if (result.Properties != null) {
                        result.Mode = RttiGenerationMode.Undefined;
                        ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRttiDirective, new int[0]);
                        return;
                    }
                    result.Properties = ParseRttiVisibility(result);
                    canContinue = result.Properties != null;
                }
                else if (ContinueWith(result, TokenKind.Fields)) {
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
                if (!ContinueWith(result, TokenKind.VcPrivate, TokenKind.VcProtected, TokenKind.VcPublic, TokenKind.VcPublished)) {
                    break;
                }

                var kind = result.LastTerminalKind;

                switch (kind) {
                    case TokenKind.VcPrivate:
                        visibility.ForPrivate = true;
                        break;
                    case TokenKind.VcProtected:
                        visibility.ForProtected = true;
                        break;
                    case TokenKind.VcPublic:
                        visibility.ForPublic = true;
                        break;
                    case TokenKind.VcPublished:
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
            WarnSwitch result = CreateByTerminal<WarnSwitch>(parent, TokenKind.Warn);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarnDirective, new[] { TokenKind.Identifier });
                return;
            }

            var warningType = result.LastTerminalValue;
            var warningModes = new[] { TokenKind.On, TokenKind.Off, TokenKind.Error, TokenKind.Default };

            if (!ContinueWith(result, TokenKind.On, TokenKind.Off, TokenKind.Error, TokenKind.Default)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarnDirective, warningModes);
                return;
            }

            var warningMode = result.LastTerminalKind;
            var parsedMode = WarningMode.Undefined;

            switch (warningMode) {
                case TokenKind.On:
                    parsedMode = WarningMode.On;
                    break;

                case TokenKind.Off:
                    parsedMode = WarningMode.Off;
                    break;

                case TokenKind.Default:
                    parsedMode = WarningMode.Default;
                    break;

                case TokenKind.Error:
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
            LibInfo result = CreateByTerminal<LibInfo>(parent, TokenKind.LibPrefix, TokenKind.LibSuffix, TokenKind.LibVersion);
            int kind = result.LastTerminalKind;

            if (!ContinueWith(result, TokenKind.QuotedString)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLibDirective, new[] { TokenKind.QuotedString });
                return;
            }

            var libInfo = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);

            switch (kind) {
                case TokenKind.LibPrefix:
                    result.LibPrefix = libInfo;
                    break;

                case TokenKind.LibSuffix:
                    result.LibSuffix = libInfo;
                    break;

                case TokenKind.LibVersion:
                    result.LibVersion = libInfo;
                    break;
            }
        }

        private void ParseImageBase(ISyntaxPart parent) {
            ImageBase result = CreateByTerminal<ImageBase>(parent, TokenKind.ImageBase);

            if (ContinueWith(result, TokenKind.Integer)) {
                result.BaseValue = DigitTokenGroupValue.Unwrap(result.LastTerminalToken);
            }
            else if (ContinueWith(result, TokenKind.HexNumber)) {
                result.BaseValue = HexNumberTokenValue.Unwrap(result.LastTerminalToken);
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidImageBaseDirective, new[] { TokenKind.Integer, TokenKind.HexNumber });
            }
        }

        private void ParseIfNDef(ISyntaxPart parent) {
            IfDef result = CreateByTerminal<IfDef>(parent, TokenKind.IfNDef);
            result.Negate = true;

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfNDefDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseHppEmit(ISyntaxPart parent) {
            HppEmit result = CreateByTerminal<HppEmit>(parent, TokenKind.HppEmit);
            result.Mode = HppEmitMode.Standard;

            if (ContinueWith(result, TokenKind.End))
                result.Mode = HppEmitMode.AtEnd;
            else if (ContinueWith(result, TokenKind.LinkUnit))
                result.Mode = HppEmitMode.LinkUnit;
            else if (ContinueWith(result, TokenKind.OpenNamespace))
                result.Mode = HppEmitMode.OpenNamespace;
            else if (ContinueWith(result, TokenKind.CloseNamepsace))
                result.Mode = HppEmitMode.CloseNamespace;
            else if (ContinueWith(result, TokenKind.NoUsingNamespace))
                result.Mode = HppEmitMode.NoUsingNamespace;

            if (result.Mode == HppEmitMode.AtEnd || result.Mode == HppEmitMode.Standard) {
                if (ContinueWith(result, TokenKind.QuotedString)) {
                    result.EmitValue = result.LastTerminalValue;
                }
                else {
                    result.Mode = HppEmitMode.Undefined;
                    ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidHppEmitDirective, new[] { TokenKind.QuotedString });
                }
            }
        }

        private void ParseExternalSym(ISyntaxPart parent) {
            ExternalSymbolDeclaration result = CreateByTerminal<ExternalSymbolDeclaration>(parent, TokenKind.ExternalSym);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExternalSymbolDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.IdentifierName = result.LastTerminalValue;

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.SymbolName = result.LastTerminalValue;
            }

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.UnionName = result.LastTerminalValue;
            }
        }

        private void ParseElse(ISyntaxPart parent) {
            CreateByTerminal<ElseDirective>(parent, TokenKind.ElseCd);
        }

        private void ParseEndIf(ISyntaxPart parent) {
            CreateByTerminal<EndIf>(parent, TokenKind.EndIf);
        }

        private void ParseIfDef(ISyntaxPart parent) {
            IfDef result = CreateByTerminal<IfDef>(parent, TokenKind.IfDef);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfDefDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseUndef(ISyntaxPart parent) {
            UnDefineSymbol result = CreateByTerminal<UnDefineSymbol>(parent, TokenKind.Undef);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidUnDefineDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseDefine(ISyntaxPart parent) {
            DefineSymbol result = CreateByTerminal<DefineSymbol>(parent, TokenKind.Define);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidDefineDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseCodeAlignParameter(ISyntaxPart parent) {
            CodeAlignParameter result = CreateByTerminal<CodeAlignParameter>(parent, TokenKind.CodeAlign);

            int value;
            if (ContinueWith(result, TokenKind.Integer) && int.TryParse(result.LastTerminalValue, out value)) {
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

                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidCodeAlignDirective, result.LastTerminalValue);
                return;
            }

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidCodeAlignDirective, new[] { TokenKind.Integer });
        }

        private void ParseApptypeParameter(ISyntaxPart parent) {
            AppTypeParameter result = CreateByTerminal<AppTypeParameter>(parent, TokenKind.Apptype);

            if (ContinueWith(result, TokenKind.Identifier)) {

                var value = result.LastTerminalValue;

                if (string.Equals(value, "CONSOLE", StringComparison.OrdinalIgnoreCase)) {
                    result.ApplicationType = AppType.Console;
                    return;
                }
                else if (string.Equals(value, "GUI", StringComparison.OrdinalIgnoreCase)) {
                    result.ApplicationType = AppType.Gui;
                    return;
                }

                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidApplicationType, result.LastTerminalValue);
                return;
            }

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidApplicationType, new[] { TokenKind.Identifier });
        }

        /// <summary>
        ///     parse a long switch
        /// </summary>
        private void ParseLongSwitch(ISyntaxPart parent) {

            if (Match(TokenKind.AlignSwitchLong)) {
                ParseLongAlignSwitch(parent);
            }
            else if (Match(TokenKind.BoolEvalSwitchLong)) {
                ParseLongBoolEvalSwitch(parent);
            }
            else if (Match(TokenKind.AssertSwitchLong)) {
                ParseLongAssertSwitch(parent);
            }
            else if (Match(TokenKind.DebugInfoSwitchLong)) {
                ParseLongDebugInfoSwitch(parent);
            }

            else if (Match(TokenKind.DenyPackageUnit)) {
                ParseDenyPackageUnitSwitch(parent);
            }

            else if (Match(TokenKind.DescriptionSwitchLong)) {
                ParseLongDescriptionSwitch(parent);
            }

            else if (Match(TokenKind.DesignOnly)) {
                ParseLongDesignOnlySwitch(parent);
            }

            else if (Match(TokenKind.ExtensionSwitchLong)) {
                ParseLongExtensionSwitch(parent);
            }

            else if (Match(TokenKind.ObjExportAll)) {
                ParseObjExportAllSwitch(parent);
            }

            else if (Match(TokenKind.ExtendedCompatibility)) {
                ParseExtendedCompatibilitySwitch(parent);
            }

            else if (Match(TokenKind.ExtendedSyntaxSwitchLong)) {
                ParseLongExtendedSyntaxSwitch(parent);
            }

            else if (Match(TokenKind.ExcessPrecision)) {
                ParseLongExcessPrecisionSwitch(parent);
            }

            else if (Match(TokenKind.HighCharUnicode)) {
                ParseLongHighCharUnicodeSwitch(parent);
            }

            else if (Match(TokenKind.Hints)) {
                ParseLongHintsSwitch(parent);
            }

            else if (Match(TokenKind.ImplicitBuild)) {
                ParseLongImplicitBuildSwitch(parent);
            }

            else if (Match(TokenKind.ImportedDataSwitchLong)) {
                ParseLongImportedDataSwitch(parent);
            }

            else if (Match(TokenKind.IncludeSwitchLong)) {
                ParseLongIncludeSwitch(parent);
            }

            else if (Match(TokenKind.IoChecks)) {
                ParseLongIoChecksSwitch(parent);
            }

            else if (Match(TokenKind.LocalSymbolSwithLong)) {
                ParseLongLocalSymbolSwitch(parent);
            }

            else if (Match(TokenKind.LongStringSwitchLong)) {
                ParseLongLongStringSwitch(parent);
            }

            else if (Match(TokenKind.OpenStringSwitchLong)) {
                ParseLongOpenStringSwitch(parent);
            }

            else if (Match(TokenKind.OptimizationSwitchLong)) {
                ParseLongOptimizationSwitch(parent);
            }

            else if (Match(TokenKind.OverflowSwitchLong)) {
                ParseLongOverflowSwitch(parent);
            }

            else if (Match(TokenKind.SafeDivideSwitchLong)) {
                ParseLongSafeDivideSwitch(parent);
            }

            else if (Match(TokenKind.RangeChecks)) {
                ParseLongRangeChecksSwitch(parent);
            }

            else if (Match(TokenKind.StackFramesSwitchLong)) {
                ParseLongStackFramesSwitch(parent);
            }

            else if (Match(TokenKind.ZeroBaseStrings)) {
                ParseZeroBasedStringSwitch(parent);
            }

            else if (Match(TokenKind.WritableConstSwitchLong)) {
                ParseLongWritableConstSwitch(parent);
            }

            else if (Match(TokenKind.WeakLinkRtti)) {
                ParseWeakLinkRttiSwitch(parent);
            }

            else if (Match(TokenKind.WeakPackageUnit)) {
                ParseWeakPackageUnitSwitch(parent);
            }

            else if (Match(TokenKind.Warnings)) {
                ParseWarningsSwitch(parent);
            }

            else if (Match(TokenKind.VarStringCheckSwitchLong)) {
                ParseLongVarStringCheckSwitch(parent);
            }

            else if (Match(TokenKind.TypedPointersSwitchLong)) {
                ParseLongTypedPointersSwitch(parent);
            }

            else if (Match(TokenKind.DefinitionInfo)) {
                ParseDefinitionInfoSwitch(parent);
            }

            else if (Match(TokenKind.ReferenceInfo)) {
                ParseReferenceInfoSwitch(parent);
            }

            else if (Match(TokenKind.StrongLinkTypes)) {
                ParseStrongLinkTypes(parent);
            }

            else if (Match(TokenKind.ScopedEnums)) {
                ParseScopedEnums(parent);
            }

            else if (Match(TokenKind.TypeInfoSwitchLong)) {
                ParseLongTypeInfoSwitch(parent);
            }

            else if (Match(TokenKind.RunOnly)) {
                ParseRunOnlyParameter(parent);
            }

            else if (Match(TokenKind.IncludeRessourceLong)) {
                ParseLongIncludeRessourceSwitch(parent);
            }

            else if (Match(TokenKind.RealCompatibility)) {
                ParseRealCompatibilitySwitch(parent);
            }

            else if (Match(TokenKind.Pointermath)) {
                ParsePointermathSwitch(parent);
            }

            else if (Match(TokenKind.OldTypeLayout)) {
                ParseOldTypeLayoutSwitch(parent);
            }

            else if (Match(TokenKind.EnumSizeSwitchLong)) {
                ParseLongEnumSizeSwitch(parent);
            }

            else if (Match(TokenKind.MethodInfo)) {
                ParseMethodInfoSwitch(parent);
            }

            else if (Match(TokenKind.LegacyIfEnd)) {
                ParseLegacyIfEndSwitch(parent);
            }

            else if (Match(TokenKind.LinkSwitchLong)) {
                ParseLongLinkSwitch(parent);
            }

        }

        private void ParseLongLinkSwitch(ISyntaxPart parent) {
            Link resultLink = CreateByTerminal<Link>(parent, TokenKind.LinkSwitchLong);
            ParseLinkParameter(resultLink);
        }

        private void ParseLegacyIfEndSwitch(ISyntaxPart parent) {
            LegacyIfEnd result = CreateByTerminal<LegacyIfEnd>(parent, TokenKind.LegacyIfEnd);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = EndIfMode.LegacyIfEnd;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = EndIfMode.Standard;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLegacyIfEndDirective, new[] { TokenKind.Off, TokenKind.On });
            }
        }

        private void ParseMethodInfoSwitch(ISyntaxPart parent) {
            MethodInfo result = CreateByTerminal<MethodInfo>(parent, TokenKind.MethodInfo);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = MethodInfoRtti.EnableMethodInfo;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = MethodInfoRtti.DisableMethodInfo;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMethodInfoDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongEnumSizeSwitch(ISyntaxPart parent) {
            MinEnumSize result = CreateByTerminal<MinEnumSize>(parent, TokenKind.EnumSizeSwitchLong);

            if (!ContinueWith(result, TokenKind.Integer)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective, new[] { TokenKind.Integer });
                return;
            }

            var size = DigitTokenGroupValue.Unwrap(result.LastTerminalToken);

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
            OldTypeLayout result = CreateByTerminal<OldTypeLayout>(parent, TokenKind.OldTypeLayout);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = OldRecordTypes.EnableOldRecordPacking;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = OldRecordTypes.DisableOldRecordPacking;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOldTypeLayoutDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParsePointermathSwitch(ISyntaxPart parent) {
            PointerMath result = CreateByTerminal<PointerMath>(parent, TokenKind.Pointermath);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = PointerManipulation.EnablePointerMath;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = PointerManipulation.DisablePointerMath;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPointerMathDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseRealCompatibilitySwitch(ISyntaxPart parent) {
            RealCompatibility result = CreateByTerminal<RealCompatibility>(parent, TokenKind.RealCompatibility);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = Real48.EnableCompatibility;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = Real48.DisableCompatibility;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRealCompatibilityMode, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongIncludeRessourceSwitch(ISyntaxPart parent) {
            Resource result = CreateByTerminal<Resource>(parent, TokenKind.IncludeRessourceLong);
            ParseResourceFileName(result);
        }

        private void ParseRunOnlyParameter(ISyntaxPart parent) {
            RunOnly result = CreateByTerminal<RunOnly>(parent, TokenKind.RunOnly);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RuntimePackageMode.RuntimeOnly;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = RuntimePackageMode.Standard;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRunOnlyDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongTypeInfoSwitch(ISyntaxPart parent) {
            PublishedRtti result = CreateByTerminal<PublishedRtti>(parent, TokenKind.TypeInfoSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RttiForPublishedProperties.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = RttiForPublishedProperties.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseScopedEnums(ISyntaxPart parent) {
            ScopedEnums result = CreateByTerminal<ScopedEnums>(parent, TokenKind.ScopedEnums);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RequireScopedEnums.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = RequireScopedEnums.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidScopedEnumsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseStrongLinkTypes(ISyntaxPart parent) {
            StrongLinkTypes result = CreateByTerminal<StrongLinkTypes>(parent, TokenKind.StrongLinkTypes);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = StrongTypeLinking.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = StrongTypeLinking.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStrongLinkTypesDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseReferenceInfoSwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent, TokenKind.ReferenceInfo);

            if (ContinueWith(result, TokenKind.On)) {
                result.ReferencesMode = SymbolReferenceInfo.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.ReferencesMode = SymbolReferenceInfo.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseDefinitionInfoSwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent, TokenKind.DefinitionInfo);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = SymbolDefinitionInfo.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = SymbolDefinitionInfo.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongTypedPointersSwitch(ISyntaxPart parent) {
            TypedPointers result = CreateByTerminal<TypedPointers>(parent, TokenKind.TypedPointersSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = TypeCheckedPointers.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = TypeCheckedPointers.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongVarStringCheckSwitch(ISyntaxPart parent) {
            VarStringChecks result = CreateByTerminal<VarStringChecks>(parent, TokenKind.VarStringCheckSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ShortVarStringChecks.EnableChecks;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ShortVarStringChecks.DisableChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStringCheckDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseWarningsSwitch(ISyntaxPart parent) {
            Warnings result = CreateByTerminal<Warnings>(parent, TokenKind.Warnings);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerWarnings.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = CompilerWarnings.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarningsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseWeakPackageUnitSwitch(ISyntaxPart parent) {
            WeakPackageUnit result = CreateByTerminal<WeakPackageUnit>(parent, TokenKind.WeakPackageUnit);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = WeakPackaging.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = WeakPackaging.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWeakPackageUnitDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseWeakLinkRttiSwitch(ISyntaxPart parent) {
            WeakLinkRtti result = CreateByTerminal<WeakLinkRtti>(parent, TokenKind.WeakLinkRtti);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RttiLinkMode.LinkWeakRtti;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = RttiLinkMode.LinkFullRtti;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWeakLinkRttiDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongWritableConstSwitch(ISyntaxPart parent) {
            WritableConsts result = CreateByTerminal<WritableConsts>(parent, TokenKind.WritableConstSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ConstantValues.Writable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ConstantValues.Constant;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseZeroBasedStringSwitch(ISyntaxPart parent) {
            ZeroBasedStrings result = CreateByTerminal<ZeroBasedStrings>(parent, TokenKind.ZeroBaseStrings);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = FirstCharIndex.IsZero;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = FirstCharIndex.IsOne;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidZeroBasedStringsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongStackFramesSwitch(ISyntaxPart parent) {
            StackFrames result = CreateByTerminal<StackFrames>(parent, TokenKind.StackFramesSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = StackFrameGeneration.EnableFrames;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = StackFrameGeneration.DisableFrames;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackFramesDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongRangeChecksSwitch(ISyntaxPart parent) {
            RangeChecks result = CreateByTerminal<RangeChecks>(parent, TokenKind.RangeChecks);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RuntimeRangeChecks.EnableRangeChecks;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = RuntimeRangeChecks.DisableRangeChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRangeCheckDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongSafeDivideSwitch(ISyntaxPart parent) {
            SafeDivide result = CreateByTerminal<SafeDivide>(parent, TokenKind.SafeDivideSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = FDivSafeDivide.EnableSafeDivide;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = FDivSafeDivide.DisableSafeDivide;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidSafeDivide, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongOverflowSwitch(ISyntaxPart parent) {
            Overflow result = CreateByTerminal<Overflow>(parent, TokenKind.OverflowSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = RuntimeOverflowChecks.EnableChecks;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = RuntimeOverflowChecks.DisableChecks;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongOptimizationSwitch(ISyntaxPart parent) {
            Optimization result = CreateByTerminal<Optimization>(parent, TokenKind.OptimizationSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerOptmization.EnableOptimization;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = CompilerOptmization.DisableOptimization;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOptimizationDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongOpenStringSwitch(ISyntaxPart parent) {
            OpenStrings result = CreateByTerminal<OpenStrings>(parent, TokenKind.OpenStringSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = OpenStringTypes.EnableOpenStrings;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = OpenStringTypes.DisableOpenStrings;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOpenStringsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongLongStringSwitch(ISyntaxPart parent) {
            LongStrings result = CreateByTerminal<LongStrings>(parent, TokenKind.LongStringSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = LongStringTypes.EnableLongStrings;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = LongStringTypes.DisableLongStrings;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongLocalSymbolSwitch(ISyntaxPart parent) {
            LocalSymbols result = CreateByTerminal<LocalSymbols>(parent, TokenKind.LocalSymbolSwithLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = LocalDebugSymbols.EnableLocalSymbols;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = LocalDebugSymbols.DisableLocalSymbols;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLocalSymbolsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongIoChecksSwitch(ISyntaxPart parent) {
            IoChecks result = CreateByTerminal<IoChecks>(parent, TokenKind.IoChecks);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = IoCallChecks.EnableIoChecks;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = IoCallChecks.DisableIoChecks;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongIncludeSwitch(ISyntaxPart parent) {
            Include result = CreateByTerminal<Include>(parent, TokenKind.IncludeSwitchLong);
            ParseIncludeFileName(result);
        }

        private void ParseLongImportedDataSwitch(ISyntaxPart parent) {
            ImportedData result = CreateByTerminal<ImportedData>(parent, TokenKind.ImportedDataSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ImportGlobalUnitData.DoImport;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ImportGlobalUnitData.NoImport;
                return;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidImportedDataDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongImplicitBuildSwitch(ISyntaxPart parent) {
            ImplicitBuild result = CreateByTerminal<ImplicitBuild>(parent, TokenKind.ImplicitBuild);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ImplicitBuildUnit.EnableImplicitBuild;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ImplicitBuildUnit.DisableImplicitBuild;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidImplicitBuildDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongHintsSwitch(ISyntaxPart parent) {
            Hints result = CreateByTerminal<Hints>(parent, TokenKind.Hints);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerHints.EnableHints;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = CompilerHints.DisableHints;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidHintsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongHighCharUnicodeSwitch(ISyntaxPart parent) {
            HighCharUnicodeSwitch result = CreateByTerminal<HighCharUnicodeSwitch>(parent, TokenKind.HighCharUnicode);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = HighCharsUnicode.EnableHighChars;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = HighCharsUnicode.DisableHighChars;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidHighCharUnicodeDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongExcessPrecisionSwitch(ISyntaxPart parent) {
            ExcessPrecision result = CreateByTerminal<ExcessPrecision>(parent, TokenKind.ExcessPrecision);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ExcessPrecisionForResults.EnableExcess;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ExcessPrecisionForResults.DisableExcess;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongExtendedSyntaxSwitch(ISyntaxPart parent) {
            ExtSyntax result = CreateByTerminal<ExtSyntax>(parent, TokenKind.ExtendedSyntaxSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ExtendedSyntax.UseExtendedSyntax;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ExtendedSyntax.NoExtendedSyntax;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseExtendedCompatibilitySwitch(ISyntaxPart parent) {
            ExtendedCompatibility result = CreateByTerminal<ExtendedCompatibility>(parent, TokenKind.ExtendedCompatibility);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ExtendedCompatiblityMode.Enabled;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ExtendedCompatiblityMode.Disabled;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseObjExportAllSwitch(ISyntaxPart parent) {
            ObjectExport result = CreateByTerminal<ObjectExport>(parent, TokenKind.ObjExportAll);

            if (ContinueWith(result, TokenKind.On)) {
                CompilerOptions.ExportCppObjects.Value = ExportCppObjects.ExportAll;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                CompilerOptions.ExportCppObjects.Value = ExportCppObjects.DoNotExportAll;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidObjectExportDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongExtensionSwitch(ISyntaxPart parent) {
            Extension result = CreateByTerminal<Extension>(parent, TokenKind.ExtensionSwitchLong);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.ExecutableExtension = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseLongDesignOnlySwitch(ISyntaxPart parent) {
            DesignOnly result = CreateByTerminal<DesignOnly>(parent, TokenKind.DesignOnly);

            if (ContinueWith(result, TokenKind.On)) {
                result.DesignTimeOnly = DesignOnlyUnit.InDesignTimeOnly;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.DesignTimeOnly = DesignOnlyUnit.Alltimes;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongDescriptionSwitch(ISyntaxPart parent) {
            Description result = CreateByTerminal<Description>(parent, TokenKind.DescriptionSwitchLong);

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.DescriptionValue = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDescriptionDirective, new[] { TokenKind.QuotedString });
            }
        }

        private void ParseDenyPackageUnitSwitch(ISyntaxPart parent) {
            ParseDenyPackageUnit result = CreateByTerminal<ParseDenyPackageUnit>(parent, TokenKind.DenyPackageUnit);

            if (ContinueWith(result, TokenKind.On)) {
                result.DenyUnit = DenyUnitInPackages.DenyUnit;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.DenyUnit = DenyUnitInPackages.AllowUnit;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongDebugInfoSwitch(ISyntaxPart parent) {
            DebugInfoSwitch result = CreateByTerminal<DebugInfoSwitch>(parent, TokenKind.DebugInfoSwitchLong);
            if (ContinueWith(result, TokenKind.On)) {
                result.DebugInfo = DebugInformation.IncludeDebugInformation;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.DebugInfo = DebugInformation.NoDebugInfo;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongAssertSwitch(ISyntaxPart parent) {
            AssertSwitch result = CreateByTerminal<AssertSwitch>(parent, TokenKind.AssertSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Assertions = AssertionMode.EnableAssertions;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Assertions = AssertionMode.DisableAssertions;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAssertDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongBoolEvalSwitch(ISyntaxPart parent) {
            var result = CreateByTerminal<BooleanEvaluationSwitch>(parent, TokenKind.BoolEvalSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.BoolEval = BooleanEvaluation.CompleteEvaluation;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.BoolEval = BooleanEvaluation.ShortEvaluation;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidBoolEvalDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongAlignSwitch(ISyntaxPart parent) {
            AlignSwitch result = CreateByTerminal<AlignSwitch>(parent, TokenKind.AlignSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.AlignValue = Alignment.QuadWord;
                return;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.AlignValue = Alignment.Unaligned;
                return;
            }

            int value;
            if (ContinueWith(result, TokenKind.Integer) && int.TryParse(result.LastTerminalValue, out value)) {

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

                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidAlignDirective, result.LastTerminalValue);
                return;
            }

            ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidAlignDirective, new[] { TokenKind.Integer });
        }

        /// <summary>
        ///     parse a switch
        /// </summary>
        private void ParseSwitch(ISyntaxPart parent) {

            if (Match(TokenKind.AlignSwitch, TokenKind.AlignSwitch1, TokenKind.AlignSwitch2, TokenKind.AlignSwitch4, TokenKind.AlignSwitch8, TokenKind.AlignSwitch16)) {
                ParseAlignSwitch(parent);
            }
            else if (Match(TokenKind.BoolEvalSwitch)) {
                ParseBoolEvalSwitch(parent);
            }
            else if (Match(TokenKind.AssertSwitch)) {
                ParseAssertSwitch(parent);
            }
            else if (Match(TokenKind.DebugInfoOrDescriptionSwitch)) {
                ParseDebugInfoOrDescriptionSwitch(parent);
            }
            else if (Match(TokenKind.ExtensionSwitch)) {
                ParseExtensionSwitch(parent);
            }
            else if (Match(TokenKind.ExtendedSyntaxSwitch)) {
                ParseExtendedSyntaxSwitch(parent);
            }
            else if (Match(TokenKind.ImportedDataSwitch)) {
                ParseImportedDataSwitch(parent);
            }
            else if (Match(TokenKind.IncludeSwitch)) {
                ParseIncludeSwitch(parent);
            }
            else if (Match(TokenKind.LinkOrLocalSymbolSwitch)) {
                ParseLocalSymbolSwitch(parent);
            }
            else if (Match(TokenKind.LongStringSwitch)) {
                ParseLongStringSwitch(parent);
            }
            else if (Match(TokenKind.OpenStringSwitch)) {
                ParseOpenStringSwitch(parent);
            }
            else if (Match(TokenKind.OptimizationSwitch)) {
                ParseOptimizationSwitch(parent);
            }
            else if (Match(TokenKind.OverflowSwitch)) {
                ParseOverflowSwitch(parent);
            }
            else if (Match(TokenKind.SaveDivideSwitch)) {
                ParseSaveDivideSwitch(parent);
            }
            else if (Match(TokenKind.IncludeRessource)) {
                ParseIncludeRessource(parent);
            }
            else if (Match(TokenKind.StackFramesSwitch)) {
                ParseStackFramesSwitch(parent);
            }
            else if (Match(TokenKind.WritableConstSwitch)) {
                ParseWritableConstSwitch(parent);
            }
            else if (Match(TokenKind.VarStringCheckSwitch)) {
                ParseVarStringCheckSwitch(parent);
            }
            else if (Match(TokenKind.TypedPointersSwitch)) {
                ParseTypedPointersSwitch(parent);
            }
            else if (Match(TokenKind.SymbolDeclarationSwitch)) {
                ParseSymbolDeclarationSwitch(parent);
            }
            else if (Match(TokenKind.SymbolDefinitionsOnlySwitch)) {
                ParseSymbolDefinitionsOnlySwitch(parent);
            }
            else if (Match(TokenKind.TypeInfoSwitch)) {
                ParseTypeInfoSwitch(parent);
            }
            else if (Match(TokenKind.EnumSizeSwitch, TokenKind.EnumSize1, TokenKind.EnumSize2, TokenKind.EnumSize4)) {
                ParseEnumSizeSwitch(parent);
            }

        }

        private void ParseEnumSizeSwitch(ISyntaxPart parent) {
            MinEnumSize result = CreateByTerminal<MinEnumSize>(parent, TokenKind.EnumSize1, TokenKind.EnumSize2, TokenKind.EnumSize4, TokenKind.EnumSizeSwitch);
            var kind = result.LastTerminalKind;

            if (kind == TokenKind.EnumSize1) {
                result.Size = EnumSize.OneByte;
            }
            else if (kind == TokenKind.EnumSize2) {
                result.Size = EnumSize.TwoByte;
            }
            else if (kind == TokenKind.EnumSize4) {
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
            ObjTypeName result = CreateByTerminal<ObjTypeName>(parent, TokenKind.ObjTypeName);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidObjTypeDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.TypeName = result.LastTerminalValue;

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.AliasName = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
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

            PublishedRtti result = CreateByTerminal<PublishedRtti>(parent, TokenKind.TypeInfoSwitch);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = RttiForPublishedProperties.Enable;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = RttiForPublishedProperties.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseSymbolDefinitionsOnlySwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent, TokenKind.SymbolDefinitionsOnlySwitch);
            result.ReferencesMode = SymbolReferenceInfo.Disable;
            result.Mode = SymbolDefinitionInfo.Enable;
        }

        private void ParseSymbolDeclarationSwitch(ISyntaxPart parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent, TokenKind.SymbolDeclarationSwitch);

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
            TypedPointers result = CreateByTerminal<TypedPointers>(parent, TokenKind.TypedPointersSwitch);

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
            VarStringChecks result = CreateByTerminal<VarStringChecks>(parent, TokenKind.VarStringCheckSwitch);

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
            WritableConsts result = CreateByTerminal<WritableConsts>(parent, TokenKind.WritableConstSwitch);

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
            StackFrames result = CreateByTerminal<StackFrames>(parent, TokenKind.StackFramesSwitch);

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
                RangeChecks result = CreateByTerminal<RangeChecks>(parent, TokenKind.IncludeRessource);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.Mode = RuntimeRangeChecks.EnableRangeChecks;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.Mode = RuntimeRangeChecks.DisableRangeChecks;
                }
                return;
            }

            Resource resultR = CreateByTerminal<Resource>(parent, TokenKind.IncludeRessource);
            ParseResourceFileName(resultR);
        }

        private string ParseFileName(SyntaxPartBase parent, bool allowTimes) {

            if (ContinueWith(parent, TokenKind.QuotedString)) {
                return QuotedStringTokenValue.Unwrap(parent.LastTerminalToken);
            }

            else if (ContinueWith(parent, TokenKind.Identifier)) {
                return parent.LastTerminalValue;
            }

            else if (allowTimes && ContinueWith(parent, TokenKind.Times)) {
                if (!ContinueWith(parent, TokenKind.Identifier)) {
                    ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidFileName, new[] { TokenKind.Identifier });
                    return null;
                }
                else {
                    return string.Concat("*", parent.LastTerminalValue);
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
                result.RcFile = result.LastTerminalValue;
            }
            else if (ContinueWith(result, TokenKind.QuotedString)) {
                result.RcFile = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
            }
        }

        private void ParseSaveDivideSwitch(ISyntaxPart parent) {
            SafeDivide result = CreateByTerminal<SafeDivide>(parent, TokenKind.SaveDivideSwitch);

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
            Overflow result = CreateByTerminal<Overflow>(parent, TokenKind.OverflowSwitch);

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
            Optimization result = CreateByTerminal<Optimization>(parent, TokenKind.OptimizationSwitch);

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
            OpenStrings result = CreateByTerminal<OpenStrings>(parent, TokenKind.OpenStringSwitch);

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
            LongStrings result = CreateByTerminal<LongStrings>(parent, TokenKind.LongStringSwitch);

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
                LocalSymbols result = CreateByTerminal<LocalSymbols>(parent, TokenKind.LinkOrLocalSymbolSwitch);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.Mode = LocalDebugSymbols.EnableLocalSymbols;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.Mode = LocalDebugSymbols.DisableLocalSymbols;
                }
                return;
            }

            Link resultLink = CreateByTerminal<Link>(parent, TokenKind.LinkOrLocalSymbolSwitch);
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
                IoChecks result = CreateByTerminal<IoChecks>(parent, TokenKind.IncludeSwitch);
                ContinueWith(result, TokenKind.Plus);
                result.Mode = IoCallChecks.EnableIoChecks;
            }
            else if (LookAhead(1, TokenKind.Minus)) {
                IoChecks result = CreateByTerminal<IoChecks>(parent, TokenKind.IncludeSwitch);
                ContinueWith(result, TokenKind.Minus);
                result.Mode = IoCallChecks.DisableIoChecks;
            }
            else if (LookAhead(1, TokenKind.Identifier) || LookAhead(1, TokenKind.QuotedString)) {
                Include result = CreateByTerminal<Include>(parent, TokenKind.IncludeSwitch);
                ParseIncludeFileName(result);
            }
            else {
                IoChecks result = CreateByTerminal<IoChecks>(parent, TokenKind.IncludeSwitch);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseImportedDataSwitch(ISyntaxPart parent) {
            ImportedData result = CreateByTerminal<ImportedData>(parent, TokenKind.ImportedDataSwitch);

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
            ExtSyntax result = CreateByTerminal<ExtSyntax>(parent, TokenKind.ExtendedSyntaxSwitch);

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
            Extension result = CreateByTerminal<Extension>(parent, TokenKind.ExtensionSwitch);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.ExecutableExtension = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseDebugInfoOrDescriptionSwitch(ISyntaxPart parent) {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                DebugInfoSwitch result = CreateByTerminal<DebugInfoSwitch>(parent, TokenKind.DebugInfoOrDescriptionSwitch);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.DebugInfo = DebugInformation.IncludeDebugInformation;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.DebugInfo = DebugInformation.NoDebugInfo;
                }
            }
            else if (LookAhead(1, TokenKind.QuotedString)) {
                Description result = CreateByTerminal<Description>(parent, TokenKind.DebugInfoOrDescriptionSwitch);
                ContinueWith(result, TokenKind.QuotedString);
                result.DescriptionValue = QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
            }
            else {
                DebugInfoSwitch result = CreateByTerminal<DebugInfoSwitch>(parent, TokenKind.DebugInfoOrDescriptionSwitch);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.Plus, TokenKind.Minus, TokenKind.QuotedString });
            }
        }

        private void ParseAssertSwitch(ISyntaxPart parent) {
            AssertSwitch result = CreateByTerminal<AssertSwitch>(parent, TokenKind.AssertSwitch);

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
            BooleanEvaluationSwitch result = CreateByTerminal<BooleanEvaluationSwitch>(parent, TokenKind.BoolEvalSwitch);

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

            if (OptionalPart(parent, out result, TokenKind.AlignSwitch1)) {
                result.AlignValue = Alignment.Unaligned;
            }
            else if (OptionalPart(parent, out result, TokenKind.AlignSwitch2)) {
                result.AlignValue = Alignment.Word;
            }
            else if (OptionalPart(parent, out result, TokenKind.AlignSwitch4)) {
                result.AlignValue = Alignment.DoubleWord;
            }
            else if (OptionalPart(parent, out result, TokenKind.AlignSwitch8)) {
                result.AlignValue = Alignment.QuadWord;
            }
            else if (OptionalPart(parent, out result, TokenKind.AlignSwitch16)) {
                result.AlignValue = Alignment.DoubleQuadWord;
            }
            else {
                result = CreateByTerminal<AlignSwitch>(parent, TokenKind.AlignSwitch);

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
