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

        private void ParseParameter(ISyntaxTreeNode parent) {

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

        private void ParseIfOpt(ISyntaxTreeNode parent) {
            IfOpt result = CreateByTerminal<IfOpt>(parent);

            if (!ContinueWith(result, PascalToken.Identifier)) {
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

        private void ParseStackSizeSwitch(ISyntaxTreeNode parent, bool mSwitch) {
            StackMemSize result = CreateByTerminal<StackMemSize>(parent);

            if (mSwitch || result.LastTerminal.Token.Kind == PascalToken.MinMemStackSizeSwitchLong) {

                if (!ContinueWith(result, PascalToken.Integer)) {
                    ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackMemSizeDirective, new[] { PascalToken.Integer });
                    return;
                }

                result.MinStackSize = DigitTokenGroupValue.Unwrap(result.LastTerminal.Token);
            }

            if (mSwitch)
                ContinueWith(result, TokenKind.Comma);

            if (mSwitch || result.LastTerminal.Token.Kind == PascalToken.MaxMemStackSizeSwitchLong) {

                if (!ContinueWith(result, PascalToken.Integer)) {
                    result.MinStackSize = null;
                    ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackMemSizeDirective, new[] { PascalToken.Integer });
                    return;
                }

                result.MaxStackSize = DigitTokenGroupValue.Unwrap(result.LastTerminal.Token);
            }
        }

        private void ParseMessage(ISyntaxTreeNode parent) {
            Message result = CreateByTerminal<Message>(parent);

            string messageText = string.Empty;

            if (ContinueWith(result, PascalToken.Identifier)) {
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

            if (!ContinueWith(result, PascalToken.QuotedString)) {
                result.MessageType = MessageSeverity.Undefined;
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMessageDirective, new[] { PascalToken.QuotedString });
            }

            result.MessageText = QuotedStringTokenValue.Unwrap(CurrentToken());
        }

        private void ParseNoDefine(ISyntaxTreeNode parent) {
            NoDefine result = CreateByTerminal<NoDefine>(parent);


            if (!ContinueWith(result, PascalToken.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidNoDefineDirective, new[] { PascalToken.Identifier });
                return;
            }

            result.TypeName = result.LastTerminal.Value;

            if (ContinueWith(result, PascalToken.QuotedString)) {
                result.TypeNameInHpp = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);

                if (ContinueWith(result, PascalToken.QuotedString)) {
                    result.TypeNameInUnion = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
                }
            }
        }

        private void ParseNoInclude(ISyntaxTreeNode parent) {
            NoInclude result = CreateByTerminal<NoInclude>(parent);

            if (!ContinueWith(result, PascalToken.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidNoIncludeDirective, new[] { PascalToken.Identifier });
                return;
            }

            result.UnitName = result.LastTerminal.Value;
        }

        private void ParsePEUserVersion(ISyntaxTreeNode parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent);
            result.Kind = PascalToken.SetPEUserVersion;
            ParsePEVersion(result);
        }

        private void ParsePESubsystemVersion(ISyntaxTreeNode parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent);
            result.Kind = PascalToken.SetPESubsystemVersion;
            ParsePEVersion(result);
        }

        private void ParsePEOsVersion(ISyntaxTreeNode parent) {
            ParsedVersion result = CreateByTerminal<ParsedVersion>(parent);
            result.Kind = PascalToken.SetPEOsVersion;
            ParsePEVersion(result);
        }

        private void ParsePEVersion(ParsedVersion result) {

            if (!ContinueWith(result, PascalToken.Real)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPEVersionDirective, new[] { PascalToken.Real });
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

        private void ParseEndRegion(ISyntaxTreeNode parent) {
            CreateByTerminal<EndRegion>(parent);
        }

        private void ParseRegion(ISyntaxTreeNode parent) {
            Region result = CreateByTerminal<Region>(parent);

            if (!ContinueWith(result, PascalToken.QuotedString)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRegionDirective, new[] { PascalToken.QuotedString });
                return;
            }

            result.RegionName = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
        }

        /// <summary>
        ///     parse the rtti parameter
        /// </summary>
        private void ParseRttiParameter(ISyntaxTreeNode parent) {
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

        private void ParseWarnParameter(ISyntaxTreeNode parent) {
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

        private void ParseLibParameter(ISyntaxTreeNode parent) {
            LibInfo result = CreateByTerminal<LibInfo>(parent);
            int kind = result.LastTerminal.Token.Kind;

            if (!ContinueWith(result, PascalToken.QuotedString)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLibDirective, new[] { PascalToken.QuotedString });
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

        private void ParseImageBase(ISyntaxTreeNode parent) {
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

        private void ParseIfNDef(ISyntaxTreeNode parent) {
            IfDef result = CreateByTerminal<IfDef>(parent);
            result.Negate = true;

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfNDefDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseHppEmit(ISyntaxTreeNode parent) {
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

        private void ParseExternalSym(ISyntaxTreeNode parent) {
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

        private void ParseElse(ISyntaxTreeNode parent) {
            CreateByTerminal<ElseDirective>(parent);
        }

        private void ParseEndIf(ISyntaxTreeNode parent) {
            CreateByTerminal<EndIf>(parent);
        }

        private void ParseIfDef(ISyntaxTreeNode parent) {
            IfDef result = CreateByTerminal<IfDef>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfDefDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseUndef(ISyntaxTreeNode parent) {
            UnDefineSymbol result = CreateByTerminal<UnDefineSymbol>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidUnDefineDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseDefine(ISyntaxTreeNode parent) {
            DefineSymbol result = CreateByTerminal<DefineSymbol>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.SymbolName = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidDefineDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseCodeAlignParameter(ISyntaxTreeNode parent) {
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

        private void ParseApptypeParameter(ISyntaxTreeNode parent) {
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
        private void ParseLongSwitch(ISyntaxTreeNode parent) {

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

        private void ParseLongLinkSwitch(ISyntaxTreeNode parent) {
            Link resultLink = CreateByTerminal<Link>(parent);
            ParseLinkParameter(resultLink);
        }

        private void ParseLegacyIfEndSwitch(ISyntaxTreeNode parent) {
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

        private void ParseMethodInfoSwitch(ISyntaxTreeNode parent) {
            MethodInfo result = CreateByTerminal<MethodInfo>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = MethodInfoRtti.EnableMethodInfo;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = MethodInfoRtti.DisableMethodInfo;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMethodInfoDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongEnumSizeSwitch(ISyntaxTreeNode parent) {
            MinEnumSize result = CreateByTerminal<MinEnumSize>(parent);

            if (!ContinueWith(result, PascalToken.Integer)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective, new[] { PascalToken.Integer });
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

        private void ParseOldTypeLayoutSwitch(ISyntaxTreeNode parent) {
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

        private void ParsePointermathSwitch(ISyntaxTreeNode parent) {
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

        private void ParseRealCompatibilitySwitch(ISyntaxTreeNode parent) {
            RealCompatibility result = CreateByTerminal<RealCompatibility>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = Real48.EnableCompatibility;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = Real48.DisableCompatibility;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRealCompatibilityMode, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongIncludeRessourceSwitch(ISyntaxTreeNode parent) {
            Resource result = CreateByTerminal<Resource>(parent);
            ParseResourceFileName(result);
        }

        private void ParseRunOnlyParameter(ISyntaxTreeNode parent) {
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

        private void ParseLongTypeInfoSwitch(ISyntaxTreeNode parent) {
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

        private void ParseScopedEnums(ISyntaxTreeNode parent) {
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

        private void ParseStrongLinkTypes(ISyntaxTreeNode parent) {
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

        private void ParseReferenceInfoSwitch(ISyntaxTreeNode parent) {
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

        private void ParseDefinitionInfoSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongTypedPointersSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongVarStringCheckSwitch(ISyntaxTreeNode parent) {
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

        private void ParseWarningsSwitch(ISyntaxTreeNode parent) {
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

        private void ParseWeakPackageUnitSwitch(ISyntaxTreeNode parent) {
            WeakPackageUnit result = CreateByTerminal<WeakPackageUnit>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = WeakPackaging.Enable;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = WeakPackaging.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWeakPackageUnitDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseWeakLinkRttiSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongWritableConstSwitch(ISyntaxTreeNode parent) {
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

        private void ParseZeroBasedStringSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongStackFramesSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongRangeChecksSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongSafeDivideSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongOverflowSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongOptimizationSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongOpenStringSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongLongStringSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongLocalSymbolSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongIoChecksSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongIncludeSwitch(ISyntaxTreeNode parent) {
            Include result = CreateByTerminal<Include>(parent);
            ParseIncludeFileName(result);
        }

        private void ParseLongImportedDataSwitch(ISyntaxTreeNode parent) {
            ImportedData result = CreateByTerminal<ImportedData>(parent);

            if (ContinueWith(result, PascalToken.On)) {
                result.Mode = ImportGlobalUnitData.DoImport;
            }
            else if (ContinueWith(result, PascalToken.Off)) {
                result.Mode = ImportGlobalUnitData.NoImport;
                return;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidImportedDataDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseLongImplicitBuildSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongHintsSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongHighCharUnicodeSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongExcessPrecisionSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongExtendedSyntaxSwitch(ISyntaxTreeNode parent) {
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

        private void ParseExtendedCompatibilitySwitch(ISyntaxTreeNode parent) {
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

        private void ParseObjExportAllSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongExtensionSwitch(ISyntaxTreeNode parent) {
            Extension result = CreateByTerminal<Extension>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.ExecutableExtension = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseLongDesignOnlySwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongDescriptionSwitch(ISyntaxTreeNode parent) {
            Description result = CreateByTerminal<Description>(parent);

            if (ContinueWith(result, PascalToken.QuotedString)) {
                result.DescriptionValue = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDescriptionDirective, new[] { PascalToken.QuotedString });
            }
        }

        private void ParseDenyPackageUnitSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongDebugInfoSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongAssertSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongBoolEvalSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongAlignSwitch(ISyntaxTreeNode parent) {
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
        private void ParseSwitch(ISyntaxTreeNode parent) {

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

        private void ParseEnumSizeSwitch(ISyntaxTreeNode parent) {
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

        private void ParseObjTypeNameSwitch(ISyntaxTreeNode parent) {
            ObjTypeName result = CreateByTerminal<ObjTypeName>(parent);

            if (!ContinueWith(result, PascalToken.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidObjTypeDirective, new[] { PascalToken.Identifier });
                return;
            }

            result.TypeName = result.LastTerminal.Value;

            if (ContinueWith(result, PascalToken.QuotedString)) {
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

        private void ParseTypeInfoSwitch(ISyntaxTreeNode parent) {

            if (LookAhead(1, PascalToken.Integer)) {
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
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { PascalToken.On, PascalToken.Off });
            }
        }

        private void ParseSymbolDefinitionsOnlySwitch(ISyntaxTreeNode parent) {
            SymbolDefinitions result = CreateByTerminal<SymbolDefinitions>(parent);
            result.ReferencesMode = SymbolReferenceInfo.Disable;
            result.Mode = SymbolDefinitionInfo.Enable;
        }

        private void ParseSymbolDeclarationSwitch(ISyntaxTreeNode parent) {
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

        private void ParseTypedPointersSwitch(ISyntaxTreeNode parent) {
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

        private void ParseVarStringCheckSwitch(ISyntaxTreeNode parent) {
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

        private void ParseWritableConstSwitch(ISyntaxTreeNode parent) {
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

        private void ParseStackFramesSwitch(ISyntaxTreeNode parent) {
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

        private void ParseIncludeRessource(ISyntaxTreeNode parent) {

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

            if (ContinueWith(parent, PascalToken.QuotedString)) {
                return QuotedStringTokenValue.Unwrap(parent.LastTerminal.Token);
            }

            else if (ContinueWith(parent, PascalToken.Identifier)) {
                return parent.LastTerminal.Value;
            }

            else if (allowTimes && ContinueWith(parent, TokenKind.Times)) {
                if (!ContinueWith(parent, PascalToken.Identifier)) {
                    ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidFileName, new[] { PascalToken.Identifier });
                    return null;
                }
                else {
                    return string.Concat("*", parent.LastTerminal.Value);
                }
            }

            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidFileName, new[] { PascalToken.Identifier });
                return null;
            }
        }

        private void ParseIncludeFileName(Include result) {
            result.Filename = ParseFileName(result, false);

            if (result.Filename == null) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidIncludeDirective, new[] { PascalToken.Identifier });
                return;
            }
        }

        private void ParseResourceFileName(Resource result) {
            result.Filename = ParseFileName(result, true);

            if (result.Filename == null) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidResourceDirective, new[] { PascalToken.Identifier });
                return;
            }

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.RcFile = result.LastTerminal.Value;
            }
            else if (ContinueWith(result, PascalToken.QuotedString)) {
                result.RcFile = QuotedStringTokenValue.Unwrap(result.LastTerminal.Token);
            }
        }

        private void ParseSaveDivideSwitch(ISyntaxTreeNode parent) {
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

        private void ParseOverflowSwitch(ISyntaxTreeNode parent) {
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

        private void ParseOptimizationSwitch(ISyntaxTreeNode parent) {
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

        private void ParseOpenStringSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLongStringSwitch(ISyntaxTreeNode parent) {
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

        private void ParseLocalSymbolSwitch(ISyntaxTreeNode parent) {
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
            result.Filename = ParseFileName(result, false);
            if (result.Filename == null) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidLinkDirective);
            }
        }

        private void ParseIncludeSwitch(ISyntaxTreeNode parent) {

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
                Include result = CreateByTerminal<Include>(parent);
                ParseIncludeFileName(result);
            }
            else {
                IoChecks result = CreateByTerminal<IoChecks>(parent);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseImportedDataSwitch(ISyntaxTreeNode parent) {
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

        private void ParseExtendedSyntaxSwitch(ISyntaxTreeNode parent) {
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

        private void ParseExtensionSwitch(ISyntaxTreeNode parent) {
            Extension result = CreateByTerminal<Extension>(parent);

            if (ContinueWith(result, PascalToken.Identifier)) {
                result.ExecutableExtension = result.LastTerminal.Value;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { PascalToken.Identifier });
            }
        }

        private void ParseDebugInfoOrDescriptionSwitch(ISyntaxTreeNode parent) {
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

        private void ParseAssertSwitch(ISyntaxTreeNode parent) {
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

        private void ParseBoolEvalSwitch(ISyntaxTreeNode parent) {
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

        private ISyntaxTreeNode ParseAlignSwitch(ISyntaxTreeNode parent) {
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
        public override ISyntaxTreeNode Parse() {
            var kind = CurrentToken().Kind;
            ISyntaxTreeNode result = CreateChild<CompilerDirective>(null);

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
