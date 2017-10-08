using System.Collections.Generic;
using System;
using PasPasPas.Options.DataTypes;
using PasPasPas.Options.Bundles;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.Tokenizer.Patterns;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     parser for compiler directives
    /// </summary>
    public class CompilerDirectiveParser : ParserBase {

        private static InputPatterns GetPatternsFromFactory(StaticEnvironment environment)
            => environment.Require<PatternFactory>(StaticDependency.TokenizerPatternFactory).CompilerDirectivePatterns;

        private static Tokenizer.Tokenizer CreateTokenizer(StaticEnvironment environment, StackedFileReader reader)
            => new Tokenizer.Tokenizer(environment, GetPatternsFromFactory(environment), reader);

        private static TokenizerWithLookahead CreateTokenizer(StaticEnvironment environment, StackedFileReader reader, OptionSet options)
            => new TokenizerWithLookahead(environment, options, CreateTokenizer(environment, reader), TokenizerMode.CompilerDirective);

        /// <summary>
        ///     create a new compiler directive parser 
        /// </summary>
        /// <param name="environment">services</param>
        /// <param name="input">input file</param>
        public CompilerDirectiveParser(StaticEnvironment staticEnvironment, OptionSet options, StackedFileReader input)
            : base(staticEnvironment, options, CreateTokenizer(staticEnvironment, input, options)) {
        }

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
                TokenKind.IfCd,
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
                TokenKind.IfEnd,
            };

        private void ParseParameter(IExtendableSyntaxPart parent) {

            if (Match(TokenKind.IfDef)) {
                ParseIfDef(parent);
            }
            else if (Match(TokenKind.IfOpt)) {
                ParseIfOpt(parent);
            }
            else if (Match(TokenKind.EndIf)) {
                ParseEndIf(parent);
            }
            else if (Match(TokenKind.IfEnd)) {
                ParseIfEnd(parent);
            }
            else if (Match(TokenKind.ElseCd)) {
                ParseElse(parent);
            }
            else if (Match(TokenKind.IfNDef)) {
                ParseIfNDef(parent);
            }
            else if (Match(TokenKind.IfCd)) {
                ParseIf(parent);
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

        private void ParseIfOpt(IExtendableSyntaxPart parent) {
            var result = new IfOpt();
            InitByTerminal(result, parent, TokenKind.IfOpt);

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

        private void ParseStackSizeSwitch(IExtendableSyntaxPart parent, bool mSwitch) {
            var result = new StackMemorySize();
            InitByTerminal(result, parent, TokenKind.MinMemStackSizeSwitchLong, TokenKind.MaxMemStackSizeSwitchLong, TokenKind.TypeInfoSwitch);

            if (mSwitch || result.LastTerminalKind == TokenKind.MinMemStackSizeSwitchLong) {

                if (!ContinueWith(result, TokenKind.Integer)) {
                    ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                    return;
                }

                result.MinStackSize = 1; // DigitTokenGroupValue.Unwrap(result.LastTerminalToken);
            }

            if (mSwitch)
                ContinueWith(result, TokenKind.Comma);

            if (mSwitch || result.LastTerminalKind == TokenKind.MaxMemStackSizeSwitchLong) {

                if (!ContinueWith(result, TokenKind.Integer)) {
                    result.MinStackSize = null;
                    ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                    return;
                }

                result.MaxStackSize = 0; // DigitTokenGroupValue.Unwrap(result.LastTerminalToken);
            }
        }

        private void ParseMessage(IExtendableSyntaxPart parent) {
            var result = new Message();
            InitByTerminal(result, parent, TokenKind.MessageCd);

            if (ContinueWith(result, TokenKind.Identifier)) {
                var messageType = result.LastTerminalValue;

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

            result.MessageText = string.Empty;// QuotedStringTokenValue.Unwrap(CurrentToken());
        }

        private void ParseNoDefine(IExtendableSyntaxPart parent) {
            var result = new NoDefine();
            InitByTerminal(result, parent, TokenKind.NoDefine);



            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidNoDefineDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.TypeName = result.LastTerminalValue;

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.TypeNameInHpp = string.Empty; // QuotedStringTokenValue.Unwrap(result.LastTerminalToken);

                if (ContinueWith(result, TokenKind.QuotedString)) {
                    result.TypeNameInUnion = string.Empty; // QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
                }
            }
        }

        private void ParseNoInclude(IExtendableSyntaxPart parent) {
            var result = new NoInclude();
            InitByTerminal(result, parent, TokenKind.NoInclude);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidNoIncludeDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.UnitName = result.LastTerminalValue;
        }

        private void ParsePEUserVersion(IExtendableSyntaxPart parent) {
            var result = new ParsedVersion();
            InitByTerminal(result, parent, TokenKind.SetPEUserVersion);
            result.Kind = TokenKind.SetPEUserVersion;
            ParsePEVersion(result);
        }

        private void ParsePESubsystemVersion(IExtendableSyntaxPart parent) {
            var result = new ParsedVersion();
            InitByTerminal(result, parent, TokenKind.SetPESubsystemVersion);
            result.Kind = TokenKind.SetPESubsystemVersion;
            ParsePEVersion(result);
        }

        private void ParsePEOsVersion(IExtendableSyntaxPart parent) {
            var result = new ParsedVersion();
            InitByTerminal(result, parent, TokenKind.SetPEOsVersion);
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


            if (!int.TryParse(text[0], out var majorVersion) || !int.TryParse(text[1], out var minorVersion)) {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
                return;
            }

            result.MajorVersion = majorVersion;
            result.MinorVersion = minorVersion;

        }

        private void ParseEndRegion(IExtendableSyntaxPart parent) {
            var result = new EndRegion();
            InitByTerminal(result, parent, TokenKind.EndRegion);
        }

        private void ParseRegion(IExtendableSyntaxPart parent) {
            var result = new Region();
            InitByTerminal(result, parent, TokenKind.Region);

            if (!ContinueWith(result, TokenKind.QuotedString)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidRegionDirective, new[] { TokenKind.QuotedString });
                return;
            }

            result.RegionName = string.Empty; // QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
        }

        /// <summary>
        ///     parse the rtti parameter
        /// </summary>
        private void ParseRttiParameter(IExtendableSyntaxPart parent) {
            var result = new RttiControl();
            InitByTerminal(result, parent, TokenKind.Rtti);

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

        private void ParseWarnParameter(IExtendableSyntaxPart parent) {
            var result = new WarnSwitch();
            InitByTerminal(result, parent, TokenKind.Warn);

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

        private void ParseLibParameter(IExtendableSyntaxPart parent) {
            var result = new LibInfo();
            InitByTerminal(result, parent, TokenKind.LibPrefix, TokenKind.LibSuffix, TokenKind.LibVersion);
            var kind = result.LastTerminalKind;

            if (!ContinueWith(result, TokenKind.QuotedString)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidLibDirective, new[] { TokenKind.QuotedString });
                return;
            }

            var libInfo = string.Empty; // QuotedStringTokenValue.Unwrap(result.LastTerminalToken);

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

        private void ParseImageBase(IExtendableSyntaxPart parent) {
            var result = new ImageBase();
            InitByTerminal(result, parent, TokenKind.ImageBase);

            if (ContinueWith(result, TokenKind.Integer)) {
                result.BaseValue = 0;// DigitTokenGroupValue.Unwrap(result.LastTerminalToken);
            }
            else if (ContinueWith(result, TokenKind.HexNumber)) {
                result.BaseValue = 0; // HexNumberTokenValue.Unwrap(result.LastTerminalToken);
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidImageBaseDirective, new[] { TokenKind.Integer, TokenKind.HexNumber });
            }
        }

        private void ParseIf(IExtendableSyntaxPart parent) {
            var result = new IfDirective();
            InitByTerminal(result, parent, TokenKind.IfCd);
        }

        private void ParseIfNDef(IExtendableSyntaxPart parent) {
            var result = new IfDef();
            InitByTerminal(result, parent, TokenKind.IfNDef);
            result.Negate = true;

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfNDefDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseHppEmit(IExtendableSyntaxPart parent) {
            var result = new HppEmit();
            InitByTerminal(result, parent, TokenKind.HppEmit);
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

        private void ParseExternalSym(IExtendableSyntaxPart parent) {
            var result = new ExternalSymbolDeclaration();
            InitByTerminal(result, parent, TokenKind.ExternalSym);

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

        private void ParseElse(IExtendableSyntaxPart parent) {
            var result = new ElseDirective();
            InitByTerminal(result, parent, TokenKind.ElseCd);
        }

        private void ParseEndIf(IExtendableSyntaxPart parent) {
            var result = new EndIf();
            new EndIf();
            InitByTerminal(result, parent, TokenKind.EndIf);
        }

        private void ParseIfEnd(IExtendableSyntaxPart parent) {
            var result = new EndIf();
            InitByTerminal(result, parent, TokenKind.IfEnd);
        }

        private void ParseIfDef(IExtendableSyntaxPart parent) {
            var result = new IfDef();
            InitByTerminal(result, parent, TokenKind.IfDef);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIfDefDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseUndef(IExtendableSyntaxPart parent) {
            var result = new UnDefineSymbol();
            InitByTerminal(result, parent, TokenKind.Undef);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidUnDefineDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseDefine(IExtendableSyntaxPart parent) {
            var result = new DefineSymbol();
            InitByTerminal(result, parent, TokenKind.Define);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.SymbolName = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidDefineDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseCodeAlignParameter(IExtendableSyntaxPart parent) {
            var result = new CodeAlignParameter();
            InitByTerminal(result, parent, TokenKind.CodeAlign);

            if (ContinueWith(result, TokenKind.Integer) && int.TryParse(result.LastTerminalValue, out var value)) {
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

        private void ParseApptypeParameter(IExtendableSyntaxPart parent) {
            var result = new AppTypeParameter();
            InitByTerminal(result, parent, TokenKind.Apptype);
            result = new AppTypeParameter();
            InitByTerminal(result, parent, TokenKind.Apptype);

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
        private void ParseLongSwitch(IExtendableSyntaxPart parent) {

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

        private void ParseLongLinkSwitch(IExtendableSyntaxPart parent) {
            var result = new Link();
            InitByTerminal(result, parent, TokenKind.LinkSwitchLong);
            ParseLinkParameter(result);
        }

        private void ParseLegacyIfEndSwitch(IExtendableSyntaxPart parent) {
            var result = new LegacyIfEnd();
            InitByTerminal(result, parent, TokenKind.LegacyIfEnd);

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

        private void ParseMethodInfoSwitch(IExtendableSyntaxPart parent) {
            var result = new MethodInfo();
            InitByTerminal(result, parent, TokenKind.MethodInfo);

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

        private void ParseLongEnumSizeSwitch(IExtendableSyntaxPart parent) {
            var result = new MinEnumSize();
            InitByTerminal(result, parent, TokenKind.EnumSizeSwitchLong);

            if (!ContinueWith(result, TokenKind.Integer)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective, new[] { TokenKind.Integer });
                return;
            }

            var size = 99; // DigitTokenGroupValue.Unwrap(result.LastTerminalToken);

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

        private void ParseOldTypeLayoutSwitch(IExtendableSyntaxPart parent) {
            var result = new OldTypeLayout();
            InitByTerminal(result, parent, TokenKind.OldTypeLayout);

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

        private void ParsePointermathSwitch(IExtendableSyntaxPart parent) {
            var result = new PointerMath();
            InitByTerminal(result, parent, TokenKind.Pointermath);

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

        private void ParseRealCompatibilitySwitch(IExtendableSyntaxPart parent) {
            var result = new RealCompatibility();
            InitByTerminal(result, parent, TokenKind.RealCompatibility);

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

        private void ParseLongIncludeRessourceSwitch(IExtendableSyntaxPart parent) {
            var result = new Resource();
            InitByTerminal(result, parent, TokenKind.IncludeRessourceLong);
            ParseResourceFileName(result);
        }

        private void ParseRunOnlyParameter(IExtendableSyntaxPart parent) {
            var result = new RunOnly();
            InitByTerminal(result, parent, TokenKind.RunOnly);

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

        private void ParseLongTypeInfoSwitch(IExtendableSyntaxPart parent) {
            var result = new PublishedRtti();
            InitByTerminal(result, parent, TokenKind.TypeInfoSwitchLong);

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

        private void ParseScopedEnums(IExtendableSyntaxPart parent) {
            var result = new ScopedEnums();
            InitByTerminal(result, parent, TokenKind.ScopedEnums);

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

        private void ParseStrongLinkTypes(IExtendableSyntaxPart parent) {
            var result = new StrongLinkTypes();
            InitByTerminal(result, parent, TokenKind.StrongLinkTypes);

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

        private void ParseReferenceInfoSwitch(IExtendableSyntaxPart parent) {
            var result = new SymbolDefinitions();
            InitByTerminal(result, parent, TokenKind.ReferenceInfo);

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

        private void ParseDefinitionInfoSwitch(IExtendableSyntaxPart parent) {
            var result = new SymbolDefinitions();
            InitByTerminal(result, parent, TokenKind.DefinitionInfo);

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

        private void ParseLongTypedPointersSwitch(IExtendableSyntaxPart parent) {
            var result = new TypedPointers();
            InitByTerminal(result, parent, TokenKind.TypedPointersSwitchLong);

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

        private void ParseLongVarStringCheckSwitch(IExtendableSyntaxPart parent) {
            var result = new VarStringChecks();
            InitByTerminal(result, parent, TokenKind.VarStringCheckSwitchLong);

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

        private void ParseWarningsSwitch(IExtendableSyntaxPart parent) {
            var result = new Warnings();
            InitByTerminal(result, parent, TokenKind.Warnings);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerWarning.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = CompilerWarning.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWarningsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseWeakPackageUnitSwitch(IExtendableSyntaxPart parent) {
            var result = new WeakPackageUnit();
            InitByTerminal(result, parent, TokenKind.WeakPackageUnit);

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

        private void ParseWeakLinkRttiSwitch(IExtendableSyntaxPart parent) {
            var result = new WeakLinkRtti();
            InitByTerminal(result, parent, TokenKind.WeakLinkRtti);

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

        private void ParseLongWritableConstSwitch(IExtendableSyntaxPart parent) {
            var result = new WritableConsts();
            InitByTerminal(result, parent, TokenKind.WritableConstSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ConstantValue.Writable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ConstantValue.Constant;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseZeroBasedStringSwitch(IExtendableSyntaxPart parent) {
            var result = new ZeroBasedStrings();
            InitByTerminal(result, parent, TokenKind.ZeroBaseStrings);

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

        private void ParseLongStackFramesSwitch(IExtendableSyntaxPart parent) {
            var result = new StackFrames();
            InitByTerminal(result, parent, TokenKind.StackFramesSwitchLong);

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

        private void ParseLongRangeChecksSwitch(IExtendableSyntaxPart parent) {
            var result = new RangeChecks();
            InitByTerminal(result, parent, TokenKind.RangeChecks);

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

        private void ParseLongSafeDivideSwitch(IExtendableSyntaxPart parent) {
            var result = new SafeDivide();
            InitByTerminal(result, parent, TokenKind.SafeDivideSwitchLong);

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

        private void ParseLongOverflowSwitch(IExtendableSyntaxPart parent) {
            var result = new Overflow();
            InitByTerminal(result, parent, TokenKind.OverflowSwitchLong);

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

        private void ParseLongOptimizationSwitch(IExtendableSyntaxPart parent) {
            var result = new Optimization();
            InitByTerminal(result, parent, TokenKind.OptimizationSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerOptimization.EnableOptimization;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = CompilerOptimization.DisableOptimization;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOptimizationDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongOpenStringSwitch(IExtendableSyntaxPart parent) {
            var result = new OpenStrings();
            InitByTerminal(result, parent, TokenKind.OpenStringSwitchLong);

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

        private void ParseLongLongStringSwitch(IExtendableSyntaxPart parent) {
            var result = new LongStrings();
            InitByTerminal(result, parent, TokenKind.LongStringSwitchLong);

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

        private void ParseLongLocalSymbolSwitch(IExtendableSyntaxPart parent) {
            var result = new LocalSymbols();
            InitByTerminal(result, parent, TokenKind.LocalSymbolSwithLong);

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

        private void ParseLongIoChecksSwitch(IExtendableSyntaxPart parent) {
            var result = new IoChecks();
            InitByTerminal(result, parent, TokenKind.IoChecks);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = IoCallCheck.EnableIoChecks;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = IoCallCheck.DisableIoChecks;
            }
            else {
                ErrorAndSkip(parent, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongIncludeSwitch(IExtendableSyntaxPart parent) {
            var result = new Include();
            InitByTerminal(result, parent, TokenKind.IncludeSwitchLong);
            ParseIncludeFileName(result);
        }

        private void ParseLongImportedDataSwitch(IExtendableSyntaxPart parent) {
            var result = new ImportedData();
            InitByTerminal(result, parent, TokenKind.ImportedDataSwitchLong);

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

        private void ParseLongImplicitBuildSwitch(IExtendableSyntaxPart parent) {
            var result = new ImplicitBuild();
            InitByTerminal(result, parent, TokenKind.ImplicitBuild);

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

        private void ParseLongHintsSwitch(IExtendableSyntaxPart parent) {
            var result = new Hints();
            InitByTerminal(result, parent, TokenKind.Hints);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = CompilerHint.EnableHints;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = CompilerHint.DisableHints;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidHintsDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongHighCharUnicodeSwitch(IExtendableSyntaxPart parent) {
            var result = new HighCharUnicodeSwitch();
            InitByTerminal(result, parent, TokenKind.HighCharUnicode);

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

        private void ParseLongExcessPrecisionSwitch(IExtendableSyntaxPart parent) {
            var result = new ExcessPrecision();
            InitByTerminal(result, parent, TokenKind.ExcessPrecision);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ExcessPrecisionForResult.EnableExcess;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ExcessPrecisionForResult.DisableExcess;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongExtendedSyntaxSwitch(IExtendableSyntaxPart parent) {
            var result = new ExtSyntax();
            InitByTerminal(result, parent, TokenKind.ExtendedSyntaxSwitchLong);

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

        private void ParseExtendedCompatibilitySwitch(IExtendableSyntaxPart parent) {
            var result = new ExtendedCompatibility();
            InitByTerminal(result, parent, TokenKind.ExtendedCompatibility);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ExtendedCompatibilityMode.Enabled;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ExtendedCompatibilityMode.Disabled;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseObjExportAllSwitch(IExtendableSyntaxPart parent) {
            var result = new ObjectExport();
            InitByTerminal(result, parent, TokenKind.ObjExportAll);

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

        private void ParseLongExtensionSwitch(IExtendableSyntaxPart parent) {
            var result = new Extension();
            InitByTerminal(result, parent, TokenKind.ExtensionSwitchLong);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.ExecutableExtension = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseLongDesignOnlySwitch(IExtendableSyntaxPart parent) {
            var result = new DesignOnly();
            InitByTerminal(result, parent, TokenKind.DesignOnly);

            if (ContinueWith(result, TokenKind.On)) {
                result.DesignTimeOnly = DesignOnlyUnit.InDesignTimeOnly;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.DesignTimeOnly = DesignOnlyUnit.AllTimes;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongDescriptionSwitch(IExtendableSyntaxPart parent) {
            var result = new Description();
            InitByTerminal(result, parent, TokenKind.DescriptionSwitchLong);

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.DescriptionValue = string.Empty;// QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDescriptionDirective, new[] { TokenKind.QuotedString });
            }
        }

        private void ParseDenyPackageUnitSwitch(IExtendableSyntaxPart parent) {
            var result = new ParseDenyPackageUnit();
            InitByTerminal(result, parent, TokenKind.DenyPackageUnit);

            if (ContinueWith(result, TokenKind.On)) {
                result.DenyUnit = DenyUnitInPackage.DenyUnit;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.DenyUnit = DenyUnitInPackage.AllowUnit;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongDebugInfoSwitch(IExtendableSyntaxPart parent) {
            var result = new DebugInfoSwitch();
            InitByTerminal(result, parent, TokenKind.DebugInfoSwitchLong);
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

        private void ParseLongAssertSwitch(IExtendableSyntaxPart parent) {
            var result = new AssertSwitch();
            InitByTerminal(result, parent, TokenKind.AssertSwitchLong);

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

        private void ParseLongBoolEvalSwitch(IExtendableSyntaxPart parent) {
            var result = new BooleanEvaluationSwitch();
            InitByTerminal(result, parent, TokenKind.BoolEvalSwitchLong);

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

        private void ParseLongAlignSwitch(IExtendableSyntaxPart parent) {
            var result = new AlignSwitch();
            InitByTerminal(result, parent, TokenKind.AlignSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.AlignValue = Alignment.QuadWord;
                return;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.AlignValue = Alignment.Unaligned;
                return;
            }

            if (ContinueWith(result, TokenKind.Integer) && int.TryParse(result.LastTerminalValue, out var value)) {

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
        private void ParseSwitch(IExtendableSyntaxPart parent) {

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

        private void ParseEnumSizeSwitch(IExtendableSyntaxPart parent) {
            var result = new MinEnumSize();
            InitByTerminal(result, parent, TokenKind.EnumSize1, TokenKind.EnumSize2, TokenKind.EnumSize4, TokenKind.EnumSizeSwitch);
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

        private void ParseObjTypeNameSwitch(IExtendableSyntaxPart parent) {
            var result = new ObjTypeName();
            InitByTerminal(result, parent, TokenKind.ObjTypeName);

            if (!ContinueWith(result, TokenKind.Identifier)) {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidObjTypeDirective, new[] { TokenKind.Identifier });
                return;
            }

            result.TypeName = result.LastTerminalValue;

            if (ContinueWith(result, TokenKind.QuotedString)) {
                result.AliasName = string.Empty; // QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
                if (string.IsNullOrWhiteSpace(result.AliasName)) {
                    result.AliasName = null;
                    result.TypeName = null;
                    ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
                    return;
                }


                var prefix = result.AliasName.Substring(0, 1);
                if (!string.Equals(prefix, "N", StringComparison.OrdinalIgnoreCase) &&
                    !string.Equals(prefix, "B", StringComparison.OrdinalIgnoreCase)) {
                    result.AliasName = null;
                    result.TypeName = null;
                    ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
                    return;
                }
            }
        }

        private void ParseTypeInfoSwitch(IExtendableSyntaxPart parent) {

            if (LookAhead(1, TokenKind.Integer)) {
                ParseStackSizeSwitch(parent, true);
                return;
            }

            var result = new PublishedRtti();
            InitByTerminal(result, parent, TokenKind.TypeInfoSwitch);

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

        private void ParseSymbolDefinitionsOnlySwitch(IExtendableSyntaxPart parent) {
            var result = new SymbolDefinitions();
            InitByTerminal(result, parent, TokenKind.SymbolDefinitionsOnlySwitch);
            result.ReferencesMode = SymbolReferenceInfo.Disable;
            result.Mode = SymbolDefinitionInfo.Enable;
        }

        private void ParseSymbolDeclarationSwitch(IExtendableSyntaxPart parent) {
            var result = new SymbolDefinitions();
            InitByTerminal(result, parent, TokenKind.SymbolDeclarationSwitch);

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

        private void ParseTypedPointersSwitch(IExtendableSyntaxPart parent) {
            var result = new TypedPointers();
            InitByTerminal(result, parent, TokenKind.TypedPointersSwitch);

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

        private void ParseVarStringCheckSwitch(IExtendableSyntaxPart parent) {
            var result = new VarStringChecks();
            InitByTerminal(result, parent, TokenKind.VarStringCheckSwitch);

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

        private void ParseWritableConstSwitch(IExtendableSyntaxPart parent) {
            var result = new WritableConsts();
            InitByTerminal(result, parent, TokenKind.WritableConstSwitch);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = ConstantValue.Writable;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = ConstantValue.Constant;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseStackFramesSwitch(IExtendableSyntaxPart parent) {
            var result = new StackFrames();
            InitByTerminal(result, parent, TokenKind.StackFramesSwitch);

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

        private void ParseIncludeRessource(IExtendableSyntaxPart parent) {

            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                var result = new RangeChecks();
                InitByTerminal(result, parent, TokenKind.IncludeRessource);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.Mode = RuntimeRangeChecks.EnableRangeChecks;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.Mode = RuntimeRangeChecks.DisableRangeChecks;
                }
                return;
            }

            var resultR = new Resource();
            InitByTerminal(resultR, parent, TokenKind.IncludeRessource);
            ParseResourceFileName(resultR);
        }

        private string ParseFileName(SyntaxPartBase parent, bool allowTimes) {

            if (ContinueWith(parent, TokenKind.QuotedString)) {
                return string.Empty; // QuotedStringTokenValue.Unwrap(parent.LastTerminalToken);
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
                result.RcFile = string.Empty; // QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
            }
        }

        private void ParseSaveDivideSwitch(IExtendableSyntaxPart parent) {
            var result = new SafeDivide();
            InitByTerminal(result, parent, TokenKind.SaveDivideSwitch);

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

        private void ParseOverflowSwitch(IExtendableSyntaxPart parent) {
            var result = new Overflow();
            InitByTerminal(result, parent, TokenKind.OverflowSwitch);

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

        private void ParseOptimizationSwitch(IExtendableSyntaxPart parent) {
            var result = new Optimization();
            InitByTerminal(result, parent, TokenKind.OptimizationSwitch);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = CompilerOptimization.EnableOptimization;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = CompilerOptimization.DisableOptimization;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidOptimizationDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseOpenStringSwitch(IExtendableSyntaxPart parent) {
            var result = new OpenStrings();
            InitByTerminal(result, parent, TokenKind.OpenStringSwitch);

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

        private void ParseLongStringSwitch(IExtendableSyntaxPart parent) {
            var result = new LongStrings();
            InitByTerminal(result, parent, TokenKind.LongStringSwitch);

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

        private void ParseLocalSymbolSwitch(IExtendableSyntaxPart parent) {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                var result = new LocalSymbols();
                InitByTerminal(result, parent, TokenKind.LinkOrLocalSymbolSwitch);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.Mode = LocalDebugSymbols.EnableLocalSymbols;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.Mode = LocalDebugSymbols.DisableLocalSymbols;
                }
                return;
            }

            var resultLink = new Link();
            InitByTerminal(resultLink, parent, TokenKind.LinkOrLocalSymbolSwitch);
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

        private void ParseIncludeSwitch(IExtendableSyntaxPart parent) {

            if (LookAhead(1, TokenKind.Plus)) {
                var result = new IoChecks();
                InitByTerminal(result, parent, TokenKind.IncludeSwitch);
                ContinueWith(result, TokenKind.Plus);
                result.Mode = IoCallCheck.EnableIoChecks;
            }
            else if (LookAhead(1, TokenKind.Minus)) {
                var result = new IoChecks();
                InitByTerminal(result, parent, TokenKind.IncludeSwitch);
                ContinueWith(result, TokenKind.Minus);
                result.Mode = IoCallCheck.DisableIoChecks;
            }
            else if (LookAhead(1, TokenKind.Identifier) || LookAhead(1, TokenKind.QuotedString)) {
                var result = new Include();
                InitByTerminal(result, parent, TokenKind.IncludeSwitch);
                ParseIncludeFileName(result);
            }
            else {
                var result = new IoChecks();
                InitByTerminal(result, parent, TokenKind.IncludeSwitch);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }
        }

        private void ParseImportedDataSwitch(IExtendableSyntaxPart parent) {
            var result = new ImportedData();
            InitByTerminal(result, parent, TokenKind.ImportedDataSwitch);

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

        private void ParseExtendedSyntaxSwitch(IExtendableSyntaxPart parent) {
            var result = new ExtSyntax();
            InitByTerminal(result, parent, TokenKind.ExtendedSyntaxSwitch);

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

        private void ParseExtensionSwitch(IExtendableSyntaxPart parent) {
            var result = new Extension();
            InitByTerminal(result, parent, TokenKind.ExtensionSwitch);

            if (ContinueWith(result, TokenKind.Identifier)) {
                result.ExecutableExtension = result.LastTerminalValue;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
            }
        }

        private void ParseDebugInfoOrDescriptionSwitch(IExtendableSyntaxPart parent) {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                var result = new DebugInfoSwitch();
                InitByTerminal(result, parent, TokenKind.DebugInfoOrDescriptionSwitch);

                if (ContinueWith(result, TokenKind.Plus)) {
                    result.DebugInfo = DebugInformation.IncludeDebugInformation;
                }
                else if (ContinueWith(result, TokenKind.Minus)) {
                    result.DebugInfo = DebugInformation.NoDebugInfo;
                }
            }
            else if (LookAhead(1, TokenKind.QuotedString)) {
                var result = new Description();
                InitByTerminal(result, parent, TokenKind.DebugInfoOrDescriptionSwitch);
                ContinueWith(result, TokenKind.QuotedString);
                result.DescriptionValue = string.Empty; // QuotedStringTokenValue.Unwrap(result.LastTerminalToken);
            }
            else {
                var result = new DebugInfoSwitch();
                InitByTerminal(result, parent, TokenKind.DebugInfoOrDescriptionSwitch);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.Plus, TokenKind.Minus, TokenKind.QuotedString });
            }
        }

        private void ParseAssertSwitch(IExtendableSyntaxPart parent) {
            var result = new AssertSwitch();
            InitByTerminal(result, parent, TokenKind.AssertSwitch);

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

        private void ParseBoolEvalSwitch(IExtendableSyntaxPart parent) {
            var result = new BooleanEvaluationSwitch();
            InitByTerminal(result, parent, TokenKind.BoolEvalSwitch);

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

        private ISyntaxPart ParseAlignSwitch(IExtendableSyntaxPart parent) {
            var result = new AlignSwitch();

            if (Match(TokenKind.AlignSwitch1)) {
                InitByTerminal(result, parent, TokenKind.AlignSwitch1);
                result.AlignValue = Alignment.Unaligned;
            }
            else if (Match(TokenKind.AlignSwitch2)) {
                InitByTerminal(result, parent, TokenKind.AlignSwitch2);
                result.AlignValue = Alignment.Word;
            }
            else if (Match(TokenKind.AlignSwitch4)) {
                InitByTerminal(result, parent, TokenKind.AlignSwitch4);
                result.AlignValue = Alignment.DoubleWord;
            }
            else if (Match(TokenKind.AlignSwitch8)) {
                InitByTerminal(result, parent, TokenKind.AlignSwitch8);
                result.AlignValue = Alignment.QuadWord;
            }
            else if (Match(TokenKind.AlignSwitch16)) {
                InitByTerminal(result, parent, TokenKind.AlignSwitch16);
                result.AlignValue = Alignment.DoubleQuadWord;
            }
            else {
                InitByTerminal(result, parent, TokenKind.AlignSwitch);

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
            IExtendableSyntaxPart result = new CompilerDirective();

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
            => Tokenizer.Keywords.ContainsKey(CurrentToken().Value);

    }

}
