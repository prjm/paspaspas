using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.CompilerDirectives;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     parser for compiler directives
    /// </summary>
    public class CompilerDirectiveParser : ParserBase {

        private static InputPatterns GetPatternsFromFactory(IParserEnvironment environment)
            => environment.Patterns.CompilerDirectivePatterns;

        private static Tokenizer.TokenizerBase CreateTokenizer(IParserEnvironment env, StackedFileReader reader)
            => new Tokenizer.TokenizerBase(env, GetPatternsFromFactory(env), reader);

        private static TokenizerWithLookahead CreateTokenizer(IParserEnvironment env, StackedFileReader reader, OptionSet options)
            => new TokenizerWithLookahead(env, options, CreateTokenizer(env, reader), TokenizerMode.CompilerDirective);

        /// <summary>
        ///     create a new compiler directive parser
        /// </summary>
        /// <param name="env">services</param>
        /// <param name="input">input file</param>
        /// <param name="options">options</param>
        public CompilerDirectiveParser(IParserEnvironment env, OptionSet options, StackedFileReader input)
            : base(env, options, CreateTokenizer(env, input, options)) {
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
        ///     meta information
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
        private static readonly HashSet<int> switches
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
        private static readonly HashSet<int> longSwitches
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
        private static readonly HashSet<int> parameters
            = new HashSet<int>() {
                TokenKind.Apptype,
                TokenKind.CodeAlign,
                TokenKind.Define,
                TokenKind.Undef,
                TokenKind.IfDef,
                TokenKind.EndIf,
                TokenKind.ElseCd,
                TokenKind.ElseIf,
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

        private ISyntaxPart ParseParameter() {

            if (Match(TokenKind.IfDef))
                return ParseIfDef();

            if (Match(TokenKind.IfOpt))
                return ParseIfOpt();

            if (Match(TokenKind.EndIf))
                return ParseEndIf();

            if (Match(TokenKind.IfEnd))
                return ParseIfEnd();

            if (Match(TokenKind.ElseCd))
                return ParseElse();

            if (Match(TokenKind.ElseIf))
                return ParseElseIf();

            if (Match(TokenKind.IfNDef))
                return ParseIfNDef();

            if (Match(TokenKind.IfCd))
                return ParseIf();

            if (Match(TokenKind.Apptype))
                return ParseApptypeParameter();

            if (Match(TokenKind.CodeAlign))
                return ParseCodeAlignParameter();

            if (Match(TokenKind.Define))
                return ParseDefine();

            if (Match(TokenKind.Undef))
                return ParseUndef();

            if (Match(TokenKind.ExternalSym))
                return ParseExternalSym();

            if (Match(TokenKind.HppEmit))
                return ParseHppEmit();

            if (Match(TokenKind.ImageBase))
                return ParseImageBase();

            if (Match(TokenKind.LibPrefix, TokenKind.LibSuffix, TokenKind.LibVersion))
                return ParseLibParameter();

            if (Match(TokenKind.Warn))
                return ParseWarnParameter();

            if (Match(TokenKind.Rtti))
                return ParseRttiParameter();

            if (Match(TokenKind.Region))
                return ParseRegion();

            if (Match(TokenKind.EndRegion))
                return ParseEndRegion();

            if (Match(TokenKind.SetPEOsVersion))
                return ParsePEOsVersion();

            if (Match(TokenKind.SetPESubsystemVersion))
                return ParsePESubsystemVersion();

            if (Match(TokenKind.SetPEUserVersion))
                return ParsePEUserVersion();

            if (Match(TokenKind.ObjTypeName))
                return ParseObjTypeNameSwitch();

            if (Match(TokenKind.NoInclude))
                return ParseNoInclude();

            if (Match(TokenKind.NoDefine))
                return ParseNoDefine();

            if (Match(TokenKind.MessageCd))
                return ParseMessage();

            if (Match(TokenKind.MinMemStackSizeSwitchLong, TokenKind.MaxMemStackSizeSwitchLong))
                return ParseStackSizeSwitch(false);

            return null;
        }

        /// <summary>
        ///     parse a switch
        /// </summary>
        private ISyntaxPart ParseSwitch() {

            if (Match(TokenKind.AlignSwitch, TokenKind.AlignSwitch1, TokenKind.AlignSwitch2, TokenKind.AlignSwitch4, TokenKind.AlignSwitch8, TokenKind.AlignSwitch16))
                return ParseAlignSwitch();

            if (Match(TokenKind.BoolEvalSwitch))
                return ParseBoolEvalSwitch();

            if (Match(TokenKind.AssertSwitch))
                return ParseAssertSwitch();

            if (Match(TokenKind.DebugInfoOrDescriptionSwitch))
                return ParseDebugInfoOrDescriptionSwitch();

            if (Match(TokenKind.ExtensionSwitch))
                return ParseExtensionSwitch();

            if (Match(TokenKind.ExtendedSyntaxSwitch))
                return ParseExtendedSyntaxSwitch();

            if (Match(TokenKind.ImportedDataSwitch))
                return ParseImportedDataSwitch();

            if (Match(TokenKind.IncludeSwitch))
                return ParseIncludeSwitch();

            if (Match(TokenKind.LinkOrLocalSymbolSwitch))
                return ParseLocalSymbolSwitch();

            if (Match(TokenKind.LongStringSwitch))
                return ParseLongStringSwitch();

            if (Match(TokenKind.OpenStringSwitch))
                return ParseOpenStringSwitch();

            if (Match(TokenKind.OptimizationSwitch))
                return ParseOptimizationSwitch();

            if (Match(TokenKind.OverflowSwitch))
                return ParseOverflowSwitch();

            if (Match(TokenKind.SaveDivideSwitch))
                return ParseSaveDivideSwitch();

            if (Match(TokenKind.IncludeRessource))
                return ParseIncludeRessource();

            if (Match(TokenKind.StackFramesSwitch))
                return ParseStackFramesSwitch();

            if (Match(TokenKind.WritableConstSwitch))
                return ParseWritableConstSwitch();

            if (Match(TokenKind.VarStringCheckSwitch))
                return ParseVarStringCheckSwitch();

            if (Match(TokenKind.TypedPointersSwitch))
                return ParseTypedPointersSwitch();

            if (Match(TokenKind.SymbolDeclarationSwitch))
                return ParseSymbolDeclarationSwitch();

            if (Match(TokenKind.SymbolDefinitionsOnlySwitch))
                return ParseSymbolDefinitionsOnlySwitch();

            if (Match(TokenKind.TypeInfoSwitch))
                return ParseTypeInfoSwitch();

            if (Match(TokenKind.EnumSizeSwitch, TokenKind.EnumSize1, TokenKind.EnumSize2, TokenKind.EnumSize4))
                return ParseEnumSizeSwitch();

            return default;
        }

        /// <summary>
        ///     parse a long switch
        /// </summary>
        private ISyntaxPart ParseLongSwitch() {

            if (Match(TokenKind.AlignSwitchLong))
                return ParseLongAlignSwitch();

            if (Match(TokenKind.BoolEvalSwitchLong))
                return ParseLongBoolEvalSwitch();

            if (Match(TokenKind.AssertSwitchLong))
                return ParseLongAssertSwitch();

            if (Match(TokenKind.DebugInfoSwitchLong))
                return ParseLongDebugInfoSwitch();

            if (Match(TokenKind.DenyPackageUnit))
                return ParseDenyPackageUnitSwitch();

            if (Match(TokenKind.DescriptionSwitchLong))
                return ParseLongDescriptionSwitch();

            if (Match(TokenKind.DesignOnly))
                return ParseLongDesignOnlySwitch();

            if (Match(TokenKind.ExtensionSwitchLong))
                return ParseLongExtensionSwitch();

            if (Match(TokenKind.ObjExportAll))
                return ParseObjExportAllSwitch();

            if (Match(TokenKind.ExtendedCompatibility))
                return ParseExtendedCompatibilitySwitch();

            if (Match(TokenKind.ExtendedSyntaxSwitchLong))
                return ParseLongExtendedSyntaxSwitch();

            if (Match(TokenKind.ExcessPrecision))
                return ParseLongExcessPrecisionSwitch();

            if (Match(TokenKind.HighCharUnicode))
                return ParseLongHighCharUnicodeSwitch();

            if (Match(TokenKind.Hints))
                return ParseLongHintsSwitch();

            if (Match(TokenKind.ImplicitBuild))
                return ParseLongImplicitBuildSwitch();

            if (Match(TokenKind.ImportedDataSwitchLong))
                return ParseLongImportedDataSwitch();

            if (Match(TokenKind.IncludeSwitchLong))
                return ParseLongIncludeSwitch();

            if (Match(TokenKind.IoChecks))
                return ParseLongIoChecksSwitch();

            if (Match(TokenKind.LocalSymbolSwithLong))
                return ParseLongLocalSymbolSwitch();

            if (Match(TokenKind.LongStringSwitchLong))
                return ParseLongLongStringSwitch();

            if (Match(TokenKind.OpenStringSwitchLong))
                return ParseLongOpenStringSwitch();

            if (Match(TokenKind.OptimizationSwitchLong))
                return ParseLongOptimizationSwitch();

            if (Match(TokenKind.OverflowSwitchLong))
                return ParseLongOverflowSwitch();

            if (Match(TokenKind.SafeDivideSwitchLong))
                return ParseLongSafeDivideSwitch();

            if (Match(TokenKind.RangeChecks))
                return ParseLongRangeChecksSwitch();

            if (Match(TokenKind.StackFramesSwitchLong))
                return ParseLongStackFramesSwitch();

            if (Match(TokenKind.ZeroBaseStrings))
                return ParseZeroBasedStringSwitch();

            if (Match(TokenKind.WritableConstSwitchLong))
                return ParseLongWritableConstSwitch();

            if (Match(TokenKind.WeakLinkRtti))
                return ParseWeakLinkRttiSwitch();

            if (Match(TokenKind.WeakPackageUnit))
                return ParseWeakPackageUnitSwitch();

            if (Match(TokenKind.Warnings))
                return ParseWarningsSwitch();

            if (Match(TokenKind.VarStringCheckSwitchLong))
                return ParseLongVarStringCheckSwitch();

            if (Match(TokenKind.TypedPointersSwitchLong))
                return ParseLongTypedPointersSwitch();

            if (Match(TokenKind.DefinitionInfo))
                return ParseDefinitionInfoSwitch();

            if (Match(TokenKind.ReferenceInfo))
                return ParseReferenceInfoSwitch();

            if (Match(TokenKind.StrongLinkTypes))
                return ParseStrongLinkTypes();

            if (Match(TokenKind.ScopedEnums))
                return ParseScopedEnums();

            if (Match(TokenKind.TypeInfoSwitchLong))
                return ParseLongTypeInfoSwitch();

            if (Match(TokenKind.RunOnly))
                return ParseRunOnlyParameter();

            if (Match(TokenKind.IncludeRessourceLong))
                return ParseLongIncludeRessourceSwitch();

            if (Match(TokenKind.RealCompatibility))
                return ParseRealCompatibilitySwitch();

            if (Match(TokenKind.Pointermath))
                return ParsePointermathSwitch();

            if (Match(TokenKind.OldTypeLayout))
                return ParseOldTypeLayoutSwitch();

            if (Match(TokenKind.EnumSizeSwitchLong))
                return ParseLongEnumSizeSwitch();

            if (Match(TokenKind.MethodInfo))
                return ParseMethodInfoSwitch();

            if (Match(TokenKind.LegacyIfEnd))
                return ParseLegacyIfEndSwitch();

            if (Match(TokenKind.LinkSwitchLong))
                return ParseLongLinkSwitch();

            return default;
        }

        private StackMemorySize ParseStackSizeSwitch(bool mSwitch) {
            var symbol = ContinueWithOrMissing(TokenKind.MinMemStackSizeSwitchLong, TokenKind.MaxMemStackSizeSwitchLong, TokenKind.TypeInfoSwitch);
            var size1 = default(Terminal);
            var size2 = default(Terminal);
            var minStackSize = 0ul;
            var maxStackSize = 0ul;
            var comma = default(Terminal);

            if (mSwitch || symbol.GetSymbolKind() == TokenKind.MinMemStackSizeSwitchLong) {

                size1 = ContinueWith(TokenKind.Integer);

                if (size1 == default) {
                    size1 = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                }
                else if (size1?.Token.ParsedValue is IIntegerValue intValue && !intValue.IsNegative) {
                    minStackSize = intValue.UnsignedValue;
                }
                else {
                    size1 = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                }
            }

            if (mSwitch)
                comma = ContinueWith(TokenKind.Comma);

            if (mSwitch || symbol.GetSymbolKind() == TokenKind.MaxMemStackSizeSwitchLong) {

                size2 = ContinueWith(TokenKind.Integer);

                if (size2 == default) {
                    size2 = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                }
                else if (size2?.Token.ParsedValue is IIntegerValue intValue && !intValue.IsNegative) {
                    maxStackSize = intValue.UnsignedValue;
                }
                else {
                    size2 = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                }
            }

            return new StackMemorySize(symbol, size1, comma, size2, minStackSize, maxStackSize);
        }

        /// <summary>
        ///     get a switch info
        /// </summary>
        /// <param name="switchState"></param>
        /// <returns></returns>
        public static SwitchInfo GetSwitchInfo(int switchState) {
            if (switchState == TokenKind.Plus)
                return SwitchInfo.Enabled;
            if (switchState == TokenKind.Minus)
                return SwitchInfo.Disabled;
            return SwitchInfo.Undefined;
        }

        private IfOpt ParseIfOpt() {
            var ifOpt = ContinueWithOrMissing(TokenKind.IfOpt);
            var switchKind = ContinueWithOrMissing(TokenKind.Identifier);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);

            return new IfOpt(ifOpt, switchKind, mode, Options.GetSwitchInfo(switchKind?.Value));
        }


        private Message ParseMessage() {
            var symbol = ContinueWithOrMissing(TokenKind.MessageCd);
            var kind = ContinueWith(TokenKind.Identifier);
            var message = MessageSeverity.Undefined;
            var hasError = false;

            if (kind != default) {
                var messageType = kind.Token.Value;

                if (string.Equals(messageType, "Hint", StringComparison.OrdinalIgnoreCase)) {
                    message = MessageSeverity.Hint;
                }
                else if (string.Equals(messageType, "Warn", StringComparison.OrdinalIgnoreCase)) {
                    message = MessageSeverity.Warning;
                }
                else if (string.Equals(messageType, "Error", StringComparison.OrdinalIgnoreCase)) {
                    message = MessageSeverity.Error;
                }
                else if (string.Equals(messageType, "Fatal", StringComparison.OrdinalIgnoreCase)) {
                    message = MessageSeverity.FatalError;
                }
                else {
                    hasError = true;
                    ErrorLastPart(CompilerDirectiveParserErrors.InvalidMessageDirective);
                }
            }
            else {
                message = MessageSeverity.Hint;
            }

            var text = ContinueWith(TokenKind.QuotedString);

            if (text == default) {
                message = MessageSeverity.Undefined;

                if (!hasError) {
                    text = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidMessageDirective, new[] { TokenKind.QuotedString });
                    hasError = true;
                }
            }

            var messageText = string.Empty;
            var textValue = text?.Token.ParsedValue ?? default;

            if (textValue is IStringValue stringValue)
                messageText = stringValue.AsUnicodeString;
            else if (!hasError)
                ErrorLastPart(CompilerDirectiveParserErrors.InvalidMessageDirective);

            return new Message(symbol, kind, text, message, messageText);
        }

        private NoDefine ParseNoDefine() {
            var symbol = ContinueWithOrMissing(TokenKind.NoDefine);
            var typeName = ContinueWith(TokenKind.Identifier);

            if (typeName == default) {
                typeName = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidNoDefineDirective, new[] { TokenKind.Identifier });
            }

            var hppTypeName = ContinueWith(TokenKind.QuotedString);
            var parsedHppTypeName = string.Empty;
            var unionName = default(Terminal);
            var parsedUnionName = string.Empty;

            if (hppTypeName != default && hppTypeName.Token.ParsedValue is IStringValue hppTypeNameValue) {
                parsedHppTypeName = hppTypeNameValue.AsUnicodeString;

                unionName = ContinueWith(TokenKind.QuotedString);
                if (unionName != default && unionName.Token.ParsedValue is IStringValue unionNameValue) {
                    parsedUnionName = unionNameValue.AsUnicodeString;
                }
            }

            return new NoDefine(symbol, typeName, hppTypeName, unionName, parsedHppTypeName, parsedUnionName);
        }

        private NoInclude ParseNoInclude() {
            var symbol = ContinueWithOrMissing(TokenKind.NoInclude);
            var name = ContinueWith(TokenKind.Identifier);

            if (name == default) {
                name = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidNoIncludeDirective, new[] { TokenKind.Identifier });
            }

            return new NoInclude(symbol, name);
        }

        private ParsedVersion ParsePEUserVersion() {
            var symbol = ContinueWithOrMissing(TokenKind.SetPEUserVersion);
            return ParsePEVersion(symbol);
        }

        private ParsedVersion ParsePESubsystemVersion() {
            var symbol = ContinueWithOrMissing(TokenKind.SetPESubsystemVersion);
            return ParsePEVersion(symbol);
        }

        private ParsedVersion ParsePEOsVersion() {
            var symbol = ContinueWithOrMissing(TokenKind.SetPEOsVersion);
            return ParsePEVersion(symbol);
        }

        private ParsedVersion ParsePEVersion(Terminal symbol) {
            var hasError = false;
            var number = ContinueWith(TokenKind.Real);

            if (number == default) {
                hasError = true;
                number = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidPEVersionDirective, new[] { TokenKind.Real });
            }

            var text = (number?.Token.Value ?? string.Empty).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (text.Length != 2 && !hasError) {
                ErrorLastPart(CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            }

            var majorVersion = 0;
            var minorVersion = 0;

            if (!hasError && text.Length == 2 && (!int.TryParse(text[0], out majorVersion) || !int.TryParse(text[1], out minorVersion))) {
                ErrorLastPart(CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            }

            return new ParsedVersion(symbol, number, majorVersion, minorVersion);
        }

        private EndRegion ParseEndRegion()
            => new EndRegion(ContinueWithOrMissing(TokenKind.EndRegion));

        private Region ParseRegion() {
            var symbol = ContinueWithOrMissing(TokenKind.Region);
            var regionName = ContinueWith(TokenKind.QuotedString);
            var region = default(string);

            if (regionName == default || !(regionName.Token.ParsedValue is IStringValue name)) {
                regionName = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRegionDirective, new[] { TokenKind.QuotedString });
            }
            else {
                region = name.AsUnicodeString;
            }

            return new Region(symbol, regionName, region);
        }

        /// <summary>
        ///     parse the rtti parameter
        /// </summary>
        private RttiControl ParseRttiParameter() {
            var symbol = ContinueWithOrMissing(TokenKind.Rtti);
            var mode = ContinueWith(TokenKind.Inherit, TokenKind.Explicit);
            var parsedMode = RttiGenerationMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Inherit) {
                parsedMode = RttiGenerationMode.Inherit;
            }
            else if (mode.GetSymbolKind() == TokenKind.Explicit) {
                parsedMode = RttiGenerationMode.Explicit;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.Inherit, TokenKind.Explicit });
            }

            if (!Match(TokenKind.Fields, TokenKind.Methods, TokenKind.Properties))
                return new RttiControl(symbol, mode, parsedMode, ImmutableArray<RttiControlSpecifier>.Empty);

            var methods = default(RttiControlSpecifier);
            var properties = default(RttiControlSpecifier);
            var fields = default(RttiControlSpecifier);
            var specifier = default(Terminal);

            using (var list = GetList<RttiControlSpecifier>()) {
                do {
                    specifier = ContinueWith(TokenKind.Methods, TokenKind.Properties, TokenKind.Fields);

                    if (specifier.GetSymbolKind() == TokenKind.Methods) {
                        if (methods != null) {
                            parsedMode = RttiGenerationMode.Undefined;
                            ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRttiDirective, Array.Empty<int>());
                        }
                        methods = AddToList(list, ParseRttiVisibility());
                    }

                    if (specifier.GetSymbolKind() == TokenKind.Properties) {
                        if (properties != default) {
                            parsedMode = RttiGenerationMode.Undefined;
                            ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRttiDirective, Array.Empty<int>());
                        }
                        properties = ParseRttiVisibility();
                    }

                    if (specifier.GetSymbolKind() == TokenKind.Fields) {
                        if (fields != default) {
                            parsedMode = RttiGenerationMode.Undefined;
                            ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRttiDirective, Array.Empty<int>());
                        }
                        fields = ParseRttiVisibility();
                    }
                } while (specifier != default);

                return new RttiControl(symbol, mode, parsedMode, GetFixedArray(list));
            }
        }

        private RttiControlSpecifier ParseRttiVisibility() {
            var openParen = ContinueWith(TokenKind.OpenParen);
            var openBraces = ContinueWith(TokenKind.OpenBraces);

            if (openParen == default)
                openParen = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.OpenBraces });

            if (openBraces == default)
                openBraces = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.OpenBraces });

            using (var list = GetList<RttiVisibilityItem>()) {
                var comma = default(Terminal);

                do {
                    var mode = ContinueWith(TokenKind.VcPrivate, TokenKind.VcProtected, TokenKind.VcPublic, TokenKind.VcPublished);
                    var kind = mode.GetSymbolKind();

                    if (mode == default)
                        break;

                    comma = ContinueWith(TokenKind.Comma);
                    list.Item.Add(new RttiVisibilityItem(mode, comma));

                } while (comma != default);

                var closeBraces = ContinueWith(TokenKind.CloseBraces);
                if (closeBraces == default)
                    closeBraces = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.CloseBraces });

                var closeParen = ContinueWith(TokenKind.CloseParen);
                if (closeParen == default)
                    closeParen = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.CloseParen });

                return new RttiControlSpecifier(openParen, openBraces, GetFixedArray(list), closeBraces, closeParen);
            }
        }

        private WarnSwitch ParseWarnParameter() {
            var symbol = ContinueWithOrMissing(TokenKind.Warn);
            var id = ContinueWith(TokenKind.Identifier);
            var warningType = default(string);
            var warningMode = default(Terminal);
            var invalid = false;

            if (id == default) {
                invalid = true;
                id = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidWarnDirective, new[] { TokenKind.Identifier });
            }
            else {
                warningType = id.Token.Value;
            }

            warningMode = ContinueWith(TokenKind.On, TokenKind.Off, TokenKind.Error, TokenKind.Default);

            if (warningMode == default) {
                if (!invalid)
                    warningMode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidWarnDirective, new[] { TokenKind.On, TokenKind.Off, TokenKind.Error, TokenKind.Default });
                invalid = true;
            }

            var warningKind = warningMode.GetSymbolKind();
            var parsedMode = WarningMode.Undefined;

            switch (warningKind) {
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
                //..
            }
            else {
                warningType = null;
                parsedMode = WarningMode.Undefined;
                if (!invalid)
                    ErrorLastPart(CompilerDirectiveParserErrors.InvalidWarnDirective, Array.Empty<object>());
            }

            return new WarnSwitch(symbol, id, warningMode, warningType, parsedMode);
        }

        private LibInfo ParseLibParameter() {
            var symbol = ContinueWithOrMissing(TokenKind.LibPrefix, TokenKind.LibSuffix, TokenKind.LibVersion);
            var kind = symbol.GetSymbolKind();
            var libParam = ContinueWith(TokenKind.QuotedString);
            var libInfo = default(string);
            var libPrefix = default(string);
            var libSuffix = default(string);
            var libVersion = default(string);

            if (libParam == default || !(libParam.Token.ParsedValue is IStringValue libInfoValue)) {
                libParam = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidLibDirective, new[] { TokenKind.QuotedString });
            }
            else {
                libInfo = libInfoValue.AsUnicodeString;
            }

            switch (kind) {
                case TokenKind.LibPrefix:
                    libPrefix = libInfo;
                    break;

                case TokenKind.LibSuffix:
                    libSuffix = libInfo;
                    break;

                case TokenKind.LibVersion:
                    libVersion = libInfo;
                    break;
            }

            return new LibInfo(symbol, libParam, libPrefix, libSuffix, libVersion);
        }

        private ImageBase ParseImageBase() {
            var symbol = ContinueWithOrMissing(TokenKind.ImageBase);
            var value = ContinueWith(TokenKind.Integer, TokenKind.HexNumber);
            var baseValue = 0ul;

            if (value != default) {
                if (value.Token.ParsedValue is IIntegerValue hexValue && !hexValue.IsNegative)
                    baseValue = hexValue.UnsignedValue;
                else
                    ErrorLastPart(CompilerDirectiveParserErrors.InvalidImageBaseDirective, new[] { TokenKind.Integer, TokenKind.HexNumber });
            }
            else {
                value = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidImageBaseDirective, new[] { TokenKind.Integer, TokenKind.HexNumber });
            }

            return new ImageBase(symbol, value, baseValue);
        }

        private IfDirective ParseIf()
            => new IfDirective(ContinueWithOrMissing(TokenKind.IfCd));

        private IfDef ParseIfNDef() {
            var symbol = ContinueWithOrMissing(TokenKind.IfNDef);
            var conditional = ContinueWith(TokenKind.Identifier);

            if (conditional == default) {
                conditional = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidIfNDefDirective, new[] { TokenKind.Identifier });
            }

            return new IfDef(symbol, true, conditional);
        }

        private HppEmit ParseHppEmit() {
            var symbol = ContinueWithOrMissing(TokenKind.HppEmit);
            var mode = HppEmitMode.Standard;
            var modeSymbol = ContinueWith(TokenKind.End, TokenKind.LinkUnit, TokenKind.OpenNamespace, TokenKind.CloseNamepsace, TokenKind.NoUsingNamespace);
            var modeValue = default(Terminal);

            if (modeSymbol.GetSymbolKind() == TokenKind.End)
                mode = HppEmitMode.AtEnd;
            else if (modeSymbol.GetSymbolKind() == TokenKind.LinkUnit)
                mode = HppEmitMode.LinkUnit;
            else if (modeSymbol.GetSymbolKind() == TokenKind.OpenNamespace)
                mode = HppEmitMode.OpenNamespace;
            else if (modeSymbol.GetSymbolKind() == TokenKind.CloseNamepsace)
                mode = HppEmitMode.CloseNamespace;
            else if (modeSymbol.GetSymbolKind() == TokenKind.NoUsingNamespace)
                mode = HppEmitMode.NoUsingNamespace;

            if (mode == HppEmitMode.AtEnd || mode == HppEmitMode.Standard) {
                modeValue = ContinueWith(TokenKind.QuotedString);
                if (modeValue == default) {
                    mode = HppEmitMode.Undefined;
                    modeValue = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidHppEmitDirective, new[] { TokenKind.QuotedString });
                }
            }

            return new HppEmit(symbol, modeSymbol, modeValue, mode);
        }

        private ExternalSymbolDeclaration ParseExternalSym() {
            var symbol = ContinueWithOrMissing(TokenKind.ExternalSym);
            var identifier = ContinueWith(TokenKind.Identifier);
            var symbolName = ContinueWith(TokenKind.QuotedString);
            var unionName = ContinueWith(TokenKind.QuotedString);

            if (identifier == default) {
                identifier = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidExternalSymbolDirective, new[] { TokenKind.Identifier });
            }

            return new ExternalSymbolDeclaration(symbol, identifier, symbolName, unionName);
        }

        private ElseDirective ParseElse()
            => new ElseDirective(ContinueWithOrMissing(TokenKind.ElseCd));

        private ElseIfDirective ParseElseIf()
            => new ElseIfDirective(ContinueWithOrMissing(TokenKind.ElseIf));

        private EndIf ParseEndIf()
            => new EndIf(ContinueWithOrMissing(TokenKind.EndIf));

        private EndIf ParseIfEnd()
            => new EndIf(ContinueWithOrMissing(TokenKind.EndIf));

        private IfDef ParseIfDef() {
            var ifDef = ContinueWithOrMissing(TokenKind.IfDef);
            var conditional = ContinueWith(TokenKind.Identifier);

            if (conditional == default) {
                conditional = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidIfDefDirective, new[] { TokenKind.Identifier });
            }

            return new IfDef(ifDef, false, conditional);
        }

        private UnDefineSymbol ParseUndef() {
            var symbol = ContinueWith(TokenKind.Undef);
            var conditional = ContinueWith(TokenKind.Identifier);

            if (conditional == default) {
                conditional = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidUnDefineDirective, new[] { TokenKind.Identifier });
            }

            return new UnDefineSymbol(symbol, conditional);
        }

        private DefineSymbol ParseDefine() {
            var symbol = ContinueWithOrMissing(TokenKind.Define);
            var name = ContinueWith(TokenKind.Identifier);

            if (name == default)
                name = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDefineDirective, new[] { TokenKind.Identifier });

            return new DefineSymbol(symbol, name);
        }

        private CodeAlignParameter ParseCodeAlignParameter() {
            var symbol = ContinueWithOrMissing(TokenKind.CodeAlign);
            var value = ContinueWith(TokenKind.Integer);
            var codeAlign = CodeAlignment.Undefined;

            if (value != default && value.Token.ParsedValue is IIntegerValue intValue) {
                switch (intValue.SignedValue) {

                    case 1:
                        codeAlign = CodeAlignment.OneByte;
                        break;

                    case 2:
                        codeAlign = CodeAlignment.TwoByte;
                        break;

                    case 4:
                        codeAlign = CodeAlignment.FourByte;
                        break;

                    case 8:
                        codeAlign = CodeAlignment.EightByte;
                        break;

                    case 16:
                        codeAlign = CodeAlignment.SixteenByte;
                        break;

                    default:
                        ErrorLastPart(CompilerDirectiveParserErrors.InvalidCodeAlignDirective, value);
                        break;
                }

            }
            else {
                value = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidCodeAlignDirective, new[] { TokenKind.Integer });
            }

            return new CodeAlignParameter(symbol, value, codeAlign);
        }

        /// <summary>
        ///     parse an application type parameter
        /// </summary>
        /// <returns></returns>
        public AppTypeParameter ParseApptypeParameter() {
            var appTypeSymbol = ContinueWithOrMissing(TokenKind.Apptype);
            var appTypeInfo = ContinueWith(TokenKind.Identifier);
            var appType = AppType.Undefined;

            if (appTypeInfo != default && appTypeInfo.Token.Value != default) {

                var value = appTypeInfo.Token.Value;

                if (string.Equals(value, "CONSOLE", StringComparison.OrdinalIgnoreCase)) {
                    appType = AppType.Console;
                }
                else if (string.Equals(value, "GUI", StringComparison.OrdinalIgnoreCase)) {
                    appType = AppType.Gui;
                }
                else {
                    ErrorLastPart(CompilerDirectiveParserErrors.InvalidApplicationType, null);
                }
            }
            else {
                appTypeInfo = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidApplicationType, new[] { TokenKind.Identifier });
            }

            return new AppTypeParameter(appTypeSymbol, appTypeInfo, appType); ;
        }

        private Link ParseLongLinkSwitch()
            => ParseLinkParameter(ContinueWithOrMissing(TokenKind.LinkSwitchLong));

        private LegacyIfEnd ParseLegacyIfEndSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.LegacyIfEnd);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off); ;
            var parsedMode = EndIfMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = EndIfMode.LegacyIfEnd;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = EndIfMode.Standard;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidLegacyIfEndDirective, new[] { TokenKind.Off, TokenKind.On });
            }

            return new LegacyIfEnd(symbol, mode, parsedMode);
        }

        private MethodInfo ParseMethodInfoSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.MethodInfo);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off); ;
            var parsedMode = MethodInfoRttiMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = MethodInfoRttiMode.EnableMethodInfo;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = MethodInfoRttiMode.DisableMethodInfo;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidMethodInfoDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new MethodInfo(symbol, mode, parsedMode);
        }

        private MinEnumSize ParseLongEnumSizeSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.EnumSizeSwitchLong);
            var sizeSymbol = ContinueWith(TokenKind.Integer);
            var hasError = false;

            if (sizeSymbol == default) {
                sizeSymbol = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective, new[] { TokenKind.Integer });
                hasError = true;
            }

            var size = 0ul;
            var parsedSize = EnumSize.Undefined;

            if (sizeSymbol?.Token.ParsedValue is IIntegerValue intValue && !intValue.IsNegative) {
                size = intValue.UnsignedValue;
            }
            else if (!hasError) {
                ErrorLastPart(default, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective);
                hasError = true;
            }

            switch (size) {
                case 1:
                    parsedSize = EnumSize.OneByte;
                    break;

                case 2:
                    parsedSize = EnumSize.TwoByte;
                    break;

                case 4:
                    parsedSize = EnumSize.FourByte;
                    break;

                default:
                    parsedSize = EnumSize.Undefined;
                    if (!hasError)
                        ErrorLastPart(default, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective);
                    break;
            }

            return new MinEnumSize(symbol, sizeSymbol, parsedSize);
        }

        private OldTypeLayout ParseOldTypeLayoutSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.OldTypeLayout);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off); ;
            var parsedMode = OldRecordTypes.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = OldRecordTypes.EnableOldRecordPacking;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = OldRecordTypes.DisableOldRecordPacking;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidOldTypeLayoutDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new OldTypeLayout(symbol, mode, parsedMode);
        }

        private PointerMath ParsePointermathSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.Pointermath);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off); ;
            var parsedMode = PointerManipulation.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = PointerManipulation.EnablePointerMath;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = PointerManipulation.DisablePointerMath;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidPointerMathDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new PointerMath(symbol, mode, parsedMode);
        }

        private RealCompatibility ParseRealCompatibilitySwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.RealCompatibility);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off); ;
            var parsedMode = Real48.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = Real48.EnableCompatibility;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = Real48.DisableCompatibility;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRealCompatibilityMode, new[] { TokenKind.On, TokenKind.Off });
            }

            return new RealCompatibility(symbol, mode, parsedMode);
        }

        private Resource ParseLongIncludeRessourceSwitch()
            => ParseResourceFileName(ContinueWithOrMissing(TokenKind.IncludeRessourceLong));

        private RunOnly ParseRunOnlyParameter() {
            var symbol = ContinueWithOrMissing(TokenKind.RunOnly);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off); ;
            var parsedMode = RuntimePackageMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = RuntimePackageMode.RuntimeOnly;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = RuntimePackageMode.Standard;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRunOnlyDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new RunOnly(symbol, mode, parsedMode);
        }

        private PublishedRtti ParseLongTypeInfoSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.TypeInfoSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off); ;
            var parsedMode = RttiForPublishedProperties.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = RttiForPublishedProperties.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = RttiForPublishedProperties.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new PublishedRtti(symbol, mode, parsedMode);
        }

        private ScopedEnums ParseScopedEnums() {
            var symbol = ContinueWithOrMissing(TokenKind.ScopedEnums);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off); ;
            var parsedMode = RequireScopedEnums.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = RequireScopedEnums.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = RequireScopedEnums.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidScopedEnumsDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ScopedEnums(symbol, mode, parsedMode);
        }

        private StrongLinkTypes ParseStrongLinkTypes() {
            var symbol = ContinueWithOrMissing(TokenKind.StrongLinkTypes);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = StrongTypeLinking.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = StrongTypeLinking.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = StrongTypeLinking.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStrongLinkTypesDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new StrongLinkTypes(symbol, mode, parsedMode);
        }

        private SymbolDefinitions ParseReferenceInfoSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ReferenceInfo);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = SymbolReferenceInfo.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = SymbolReferenceInfo.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = SymbolReferenceInfo.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new SymbolDefinitions(symbol, mode, parsedMode);
        }

        private SymbolDefinitions ParseDefinitionInfoSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.DefinitionInfo);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = SymbolDefinitionInfo.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = SymbolDefinitionInfo.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = SymbolDefinitionInfo.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new SymbolDefinitions(symbol, mode, SymbolReferenceInfo.Undefined, parsedMode);
        }

        private TypedPointers ParseLongTypedPointersSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.TypedPointersSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = UsePointersWithTypeChecking.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = UsePointersWithTypeChecking.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = UsePointersWithTypeChecking.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new TypedPointers(symbol, mode, parsedMode);
        }

        private VarStringChecks ParseLongVarStringCheckSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.VarStringCheckSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = ShortVarStringCheck.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = ShortVarStringCheck.EnableChecks;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = ShortVarStringCheck.DisableChecks;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStringCheckDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new VarStringChecks(symbol, mode, parsedMode);
        }

        private Warnings ParseWarningsSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.Warnings);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = CompilerWarning.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = CompilerWarning.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = CompilerWarning.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidWarningsDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new Warnings(symbol, mode, parsedMode);
        }

        private WeakPackageUnit ParseWeakPackageUnitSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.WeakPackageUnit);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = WeakPackaging.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = WeakPackaging.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = WeakPackaging.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidWeakPackageUnitDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new WeakPackageUnit(symbol, mode, parsedMode);
        }

        private WeakLinkRtti ParseWeakLinkRttiSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.WeakLinkRtti);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = RttiLinkMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = RttiLinkMode.LinkWeakRtti;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = RttiLinkMode.LinkFullRtti;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidWeakLinkRttiDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new WeakLinkRtti(symbol, mode, parsedMode);
        }

        private WritableConsts ParseLongWritableConstSwitch() {
            var symbol = ContinueWith(TokenKind.WritableConstSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = ConstantValue.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = ConstantValue.Writable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = ConstantValue.Constant;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidWritableConstantsDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new WritableConsts(symbol, mode, parsedMode);
        }

        private ZeroBasedStrings ParseZeroBasedStringSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ZeroBaseStrings);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = FirstCharIndex.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = FirstCharIndex.IsZero;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = FirstCharIndex.IsOne;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidZeroBasedStringsDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ZeroBasedStrings(symbol, mode, parsedMode);
        }

        private StackFrames ParseLongStackFramesSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.StackFramesSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = StackFrameGeneration.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = StackFrameGeneration.EnableFrames;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = StackFrameGeneration.DisableFrames;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStackFramesDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new StackFrames(symbol, mode, parsedMode);
        }

        private RangeChecks ParseLongRangeChecksSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.RangeChecks);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = RuntimeRangeChecks.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = RuntimeRangeChecks.EnableRangeChecks;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = RuntimeRangeChecks.DisableRangeChecks;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidRangeCheckDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new RangeChecks(symbol, mode, parsedMode);
        }

        private SafeDivide ParseLongSafeDivideSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.SafeDivideSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = FDivSafeDivide.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = FDivSafeDivide.EnableSafeDivide;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = FDivSafeDivide.DisableSafeDivide;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidSafeDivide, new[] { TokenKind.On, TokenKind.Off });
            }

            return new SafeDivide(symbol, mode, parsedMode);
        }

        private Overflow ParseLongOverflowSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.OverflowSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = RuntimeOverflowCheck.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = RuntimeOverflowCheck.EnableChecks;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = RuntimeOverflowCheck.DisableChecks;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidOverflowCheckDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new Overflow(symbol, mode, parsedMode);
        }

        private Optimization ParseLongOptimizationSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.OptimizationSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = CompilerOptimization.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = CompilerOptimization.EnableOptimization;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = CompilerOptimization.DisableOptimization;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidOptimizationDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new Optimization(symbol, mode, parsedMode);
        }

        private OpenStrings ParseLongOpenStringSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.OpenStringSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = OpenStringTypes.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = OpenStringTypes.EnableOpenStrings;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = OpenStringTypes.DisableOpenStrings;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidOpenStringsDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new OpenStrings(symbol, mode, parsedMode);
        }

        private LongStrings ParseLongLongStringSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.LongStringSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = LongStringTypes.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = LongStringTypes.EnableLongStrings;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = LongStringTypes.DisableLongStrings;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new LongStrings(symbol, mode, parsedMode);
        }

        private LocalSymbols ParseLongLocalSymbolSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.LocalSymbolSwithLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = LocalDebugSymbolMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = LocalDebugSymbolMode.EnableLocalSymbols;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = LocalDebugSymbolMode.DisableLocalSymbols;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidLocalSymbolsDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new LocalSymbols(symbol, mode, parsedMode);
        }

        private IoChecks ParseLongIoChecksSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.IoChecks);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = IoCallCheck.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = IoCallCheck.EnableIoChecks;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = IoCallCheck.DisableIoChecks;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new IoChecks(symbol, mode, parsedMode);
        }

        private Include ParseLongIncludeSwitch()
            => ParseIncludeFileName(ContinueWithOrMissing(TokenKind.IncludeSwitchLong));

        private ImportedData ParseLongImportedDataSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ImportedDataSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = ImportGlobalUnitData.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = ImportGlobalUnitData.DoImport;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = ImportGlobalUnitData.NoImport;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidImportedDataDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ImportedData(symbol, mode, parsedMode);
        }

        private ImplicitBuild ParseLongImplicitBuildSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ImplicitBuild);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = ImplicitBuildUnit.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = ImplicitBuildUnit.EnableImplicitBuild;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = ImplicitBuildUnit.DisableImplicitBuild;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidImplicitBuildDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ImplicitBuild(symbol, mode, parsedMode);
        }

        private Hints ParseLongHintsSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.Hints);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = CompilerHint.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = CompilerHint.EnableHints;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = CompilerHint.DisableHints;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidHintsDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new Hints(symbol, mode, parsedMode);
        }

        private HighCharUnicodeSwitch ParseLongHighCharUnicodeSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.HighCharUnicode);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = HighCharsUnicode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = HighCharsUnicode.EnableHighChars;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = HighCharsUnicode.DisableHighChars;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidHighCharUnicodeDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new HighCharUnicodeSwitch(symbol, mode, parsedMode);
        }

        private ExcessPrecision ParseLongExcessPrecisionSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ExcessPrecision);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = ExcessPrecisionForResult.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = ExcessPrecisionForResult.EnableExcess;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = ExcessPrecisionForResult.DisableExcess;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ExcessPrecision(symbol, mode, parsedMode);
        }

        private ExtSyntax ParseLongExtendedSyntaxSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ExtendedSyntaxSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = ExtendedSyntax.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = ExtendedSyntax.UseExtendedSyntax;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = ExtendedSyntax.NoExtendedSyntax;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ExtSyntax(symbol, mode, parsedMode);
        }

        private ExtendedCompatibility ParseExtendedCompatibilitySwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ExtendedCompatibility);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = ExtendedCompatibilityMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = ExtendedCompatibilityMode.Enabled;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = ExtendedCompatibilityMode.Disabled;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ExtendedCompatibility(symbol, mode, parsedMode);
        }

        private ObjectExport ParseObjExportAllSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ObjExportAll);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = ExportCppObjectMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = ExportCppObjectMode.ExportAll;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = ExportCppObjectMode.DoNotExportAll;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidObjectExportDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ObjectExport(symbol, mode, parsedMode);
        }

        private Extension ParseLongExtensionSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ExtensionSwitchLong);
            var extension = ContinueWith(TokenKind.Identifier);
            var parsedExtension = default(string);

            if (extension.GetSymbolKind() == TokenKind.Identifier) {
                parsedExtension = extension.Token.Value;
            }
            else {
                extension = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
            }

            return new Extension(symbol, extension, parsedExtension);
        }

        private DesignOnly ParseLongDesignOnlySwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.DesignOnly);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var designTimeOnly = DesignOnlyUnit.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                designTimeOnly = DesignOnlyUnit.InDesignTimeOnly;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                designTimeOnly = DesignOnlyUnit.AllTimes;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new DesignOnly(symbol, mode, designTimeOnly);
        }

        private Description ParseLongDescriptionSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.DescriptionSwitchLong);
            var value = ContinueWith(TokenKind.QuotedString);
            var descriptionValue = default(string);

            if (value != default && value.Token.ParsedValue is IStringValue description) {
                descriptionValue = description.AsUnicodeString;
            }
            else {
                value = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDescriptionDirective, new[] { TokenKind.QuotedString });
            }

            return new Description(symbol, value, descriptionValue);
        }

        private ParseDenyPackageUnit ParseDenyPackageUnitSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.DenyPackageUnit);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var denyUnit = DenyUnitInPackage.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                denyUnit = DenyUnitInPackage.DenyUnit;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                denyUnit = DenyUnitInPackage.AllowUnit;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ParseDenyPackageUnit(symbol, mode, denyUnit);
        }

        private DebugInfoSwitch ParseLongDebugInfoSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.DebugInfoSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var debugInfo = DebugInformation.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                debugInfo = DebugInformation.IncludeDebugInformation;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                debugInfo = DebugInformation.NoDebugInfo;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new DebugInfoSwitch(symbol, mode, debugInfo);
        }

        private AssertSwitch ParseLongAssertSwitch() {
            var assert = ContinueWithOrMissing(TokenKind.AssertSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var option = AssertionMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                option = AssertionMode.EnableAssertions;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                option = AssertionMode.DisableAssertions;
            }
            else if (mode == default) {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidAssertDirective, new int[] { TokenKind.On, TokenKind.Off });
            }

            return new AssertSwitch(assert, mode, option);
        }

        private BooleanEvaluationSwitch ParseLongBoolEvalSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.BoolEvalSwitchLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var evalMode = BooleanEvaluation.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                evalMode = BooleanEvaluation.CompleteEvaluation;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                evalMode = BooleanEvaluation.ShortEvaluation;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidBoolEvalDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new BooleanEvaluationSwitch(symbol, mode, evalMode);
        }

        private AlignSwitch ParseLongAlignSwitch() {
            var alignSymbol = ContinueWithOrMissing(TokenKind.AlignSwitchLong);
            var alignSwitch = ContinueWith(TokenKind.On, TokenKind.Off);

            if (alignSwitch.GetSymbolKind() == TokenKind.On)
                return new AlignSwitch(alignSymbol, alignSwitch, Alignment.QuadWord);

            if (alignSwitch.GetSymbolKind() == TokenKind.Off)
                return new AlignSwitch(alignSymbol, alignSwitch, Alignment.Unaligned);

            alignSwitch = ContinueWith(TokenKind.Integer);

            if (alignSwitch != default && alignSwitch.Token.ParsedValue is IIntegerValue intValue) {

                switch (intValue.UnsignedValue) {

                    case 1:
                        return new AlignSwitch(alignSymbol, alignSwitch, Alignment.Unaligned);

                    case 2:
                        return new AlignSwitch(alignSymbol, alignSwitch, Alignment.Word);

                    case 4:
                        return new AlignSwitch(alignSymbol, alignSwitch, Alignment.DoubleWord);

                    case 8:
                        return new AlignSwitch(alignSymbol, alignSwitch, Alignment.QuadWord);

                    case 16:
                        return new AlignSwitch(alignSymbol, alignSwitch, Alignment.DoubleQuadWord);

                }

                ErrorLastPart(CompilerDirectiveParserErrors.InvalidAlignDirective);
                return new AlignSwitch(alignSymbol, alignSwitch, Alignment.Undefined);
            }

            alignSwitch = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidAlignDirective, new[] { TokenKind.Integer });
            return new AlignSwitch(alignSymbol, alignSwitch, Alignment.Undefined);
        }

        private MinEnumSize ParseEnumSizeSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.EnumSize1, TokenKind.EnumSize2, TokenKind.EnumSize4, TokenKind.EnumSizeSwitch);
            var kind = symbol?.Token.Kind;
            var size = EnumSize.Undefined;
            var mode = default(Terminal);

            if (kind == TokenKind.EnumSize1) {
                size = EnumSize.OneByte;
            }
            else if (kind == TokenKind.EnumSize2) {
                size = EnumSize.TwoByte;
            }
            else if (kind == TokenKind.EnumSize4) {
                size = EnumSize.FourByte;
            }
            else if (Match(TokenKind.Plus)) {
                mode = ContinueWith(TokenKind.Plus);
                size = EnumSize.FourByte;
            }
            else if (Match(TokenKind.Minus)) {
                mode = ContinueWith(TokenKind.Minus);
                size = EnumSize.OneByte;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new MinEnumSize(symbol, mode, size);
        }

        private ObjTypeName ParseObjTypeNameSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ObjTypeName);
            var name = ContinueWith(TokenKind.Identifier);
            var typeName = name?.Token.Value ?? string.Empty;

            if (name == default) {
                name = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidObjTypeDirective, new[] { TokenKind.Identifier });
            }

            var aliasName = ContinueWith(TokenKind.QuotedString);
            var prefix = string.Empty;
            var parsedAliasName = string.Empty;

            if (aliasName != default && aliasName.Token.ParsedValue is IStringValue alias) {
                parsedAliasName = alias.AsUnicodeString;
                if (string.IsNullOrWhiteSpace(parsedAliasName)) {
                    parsedAliasName = null;
                    typeName = null;
                    ErrorLastPart(CompilerDirectiveParserErrors.InvalidObjTypeDirective);
                }
                else {
                    prefix = parsedAliasName.Substring(0, 1);
                    if (!string.Equals(prefix, "N", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(prefix, "B", StringComparison.OrdinalIgnoreCase)) {
                        parsedAliasName = null;
                        typeName = null;
                        ErrorLastPart(CompilerDirectiveParserErrors.InvalidObjTypeDirective);
                    }
                }
            }

            return new ObjTypeName(symbol, name, aliasName, typeName, parsedAliasName);
        }

        private ISyntaxPart ParseTypeInfoSwitch() {

            if (LookAhead(1, TokenKind.Integer)) {
                return ParseStackSizeSwitch(true);
            }

            var symbol = ContinueWithOrMissing(TokenKind.TypeInfoSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = RttiForPublishedProperties.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = RttiForPublishedProperties.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = RttiForPublishedProperties.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new PublishedRtti(symbol, mode, parsedMode);
        }

        private SymbolDefinitions ParseSymbolDefinitionsOnlySwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.SymbolDefinitionsOnlySwitch);
            var referencesMode = SymbolReferenceInfo.Disable;
            var mode = SymbolDefinitionInfo.Enable;
            return new SymbolDefinitions(symbol, referencesMode, mode);
        }

        private SymbolDefinitions ParseSymbolDeclarationSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.SymbolDeclarationSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var refMode = SymbolReferenceInfo.Undefined;
            var parsedMode = SymbolDefinitionInfo.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                refMode = SymbolReferenceInfo.Enable;
                parsedMode = SymbolDefinitionInfo.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                refMode = SymbolReferenceInfo.Disable;
                parsedMode = SymbolDefinitionInfo.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new SymbolDefinitions(symbol, mode, refMode, parsedMode);
        }

        private TypedPointers ParseTypedPointersSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.TypedPointersSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = UsePointersWithTypeChecking.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = UsePointersWithTypeChecking.Enable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = UsePointersWithTypeChecking.Disable;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective, new[] { TokenKind.Minus, TokenKind.Platform });
            }

            return new TypedPointers(symbol, mode, parsedMode);
        }

        private VarStringChecks ParseVarStringCheckSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.VarStringCheckSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = ShortVarStringCheck.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = ShortVarStringCheck.EnableChecks;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = ShortVarStringCheck.DisableChecks;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStringCheckDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new VarStringChecks(symbol, mode, parsedMode);
        }

        private WritableConsts ParseWritableConstSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.WritableConstSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = ConstantValue.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = ConstantValue.Writable;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = ConstantValue.Constant;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidWritableConstantsDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new WritableConsts(symbol, mode, parsedMode);
        }

        private StackFrames ParseStackFramesSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.StackFramesSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = StackFrameGeneration.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = StackFrameGeneration.EnableFrames;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = StackFrameGeneration.DisableFrames;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidStackFramesDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new StackFrames(symbol, mode, parsedMode);
        }

        private ISyntaxPart ParseIncludeRessource() {

            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                var symbol = ContinueWithOrMissing(TokenKind.IncludeRessource);
                var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
                var parsedMode = RuntimeRangeChecks.Undefined;

                if (mode.GetSymbolKind() == TokenKind.Plus) {
                    parsedMode = RuntimeRangeChecks.EnableRangeChecks;
                }
                else if (mode.GetSymbolKind() == TokenKind.Minus) {
                    parsedMode = RuntimeRangeChecks.DisableRangeChecks;
                }

                return new RangeChecks(symbol, mode, parsedMode);
            }

            return ParseResourceFileName(ContinueWithOrMissing(TokenKind.IncludeRessource));
        }

        private FilenameSymbol ParseFileName(bool allowTimes, out string name) {
            var filename = ContinueWith(TokenKind.QuotedString);

            if (filename != default && filename.Token.ParsedValue is IStringValue fileName) {
                name = fileName.AsUnicodeString;
                return new FilenameSymbol(filename, default);
            }

            filename = ContinueWith(TokenKind.Identifier);

            if (filename != default) {
                name = filename.Token.Value;
                return new FilenameSymbol(filename, default);
            }

            if (allowTimes && Match(TokenKind.Times)) {

                filename = ContinueWith(TokenKind.Times);
                var ident = ContinueWith(TokenKind.Identifier);

                if (ident == default) {
                    ident = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidFileName, new[] { TokenKind.Identifier });
                    name = default;
                    return new FilenameSymbol(filename, ident);
                }

                name = string.Concat("*", ident.Token.Value);
                return new FilenameSymbol(filename, ident);
            }

            filename = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidFileName, new[] { TokenKind.Identifier });
            name = default;
            return new FilenameSymbol(filename, default);
        }

        private Include ParseIncludeFileName(Terminal symbol) {
            var fileName = ParseFileName(false, out var name);

            if (name == default)
                ErrorLastPart(CompilerDirectiveParserErrors.InvalidIncludeDirective, new[] { TokenKind.Identifier });

            return new Include(symbol, fileName, name);
        }

        private Resource ParseResourceFileName(Terminal symbol) {
            var fileName = ParseFileName(true, out var name);

            if (name == null)
                ErrorLastPart(CompilerDirectiveParserErrors.InvalidResourceDirective, new[] { TokenKind.Identifier });

            var rcFile = ContinueWith(TokenKind.Identifier);
            var rcFileName = default(string);

            if (rcFile != default) {
                rcFileName = rcFile.Token.Value;
            }
            else {
                rcFile = ContinueWith(TokenKind.QuotedString);

                if (rcFile != default && rcFile.Token.ParsedValue is IStringValue rcName) {
                    rcFileName = rcName.AsUnicodeString;
                }
            }

            return new Resource(symbol, fileName, rcFile, name, rcFileName);
        }

        private SafeDivide ParseSaveDivideSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.SaveDivideSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = FDivSafeDivide.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = FDivSafeDivide.EnableSafeDivide;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = FDivSafeDivide.DisableSafeDivide;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidSafeDivide, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new SafeDivide(symbol, mode, parsedMode);
        }

        private Overflow ParseOverflowSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.OverflowSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = RuntimeOverflowCheck.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = RuntimeOverflowCheck.EnableChecks;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = RuntimeOverflowCheck.DisableChecks;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidOverflowCheckDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new Overflow(symbol, mode, parsedMode);
        }

        private Optimization ParseOptimizationSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.OptimizationSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = CompilerOptimization.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = CompilerOptimization.EnableOptimization;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = CompilerOptimization.DisableOptimization;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidOptimizationDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new Optimization(symbol, mode, parsedMode);
        }

        private OpenStrings ParseOpenStringSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.OpenStringSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = OpenStringTypes.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = OpenStringTypes.EnableOpenStrings;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = OpenStringTypes.DisableOpenStrings;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidOpenStringsDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new OpenStrings(symbol, mode, parsedMode);
        }

        private LongStrings ParseLongStringSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.LongStringSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = LongStringTypes.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = LongStringTypes.EnableLongStrings;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = LongStringTypes.DisableLongStrings;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new LongStrings(symbol, mode, parsedMode);
        }

        private ISyntaxPart ParseLocalSymbolSwitch() {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                var symbol = ContinueWithOrMissing(TokenKind.LinkOrLocalSymbolSwitch);
                var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
                var parsedMode = LocalDebugSymbolMode.Undefined;

                if (mode.GetSymbolKind() == TokenKind.Plus) {
                    parsedMode = LocalDebugSymbolMode.EnableLocalSymbols;
                }
                else if (mode.GetSymbolKind() == TokenKind.Minus) {
                    parsedMode = LocalDebugSymbolMode.DisableLocalSymbols;
                }
                return new LocalSymbols(symbol, mode, parsedMode);
            }

            return ParseLinkParameter(ContinueWithOrMissing(TokenKind.LinkOrLocalSymbolSwitch));
        }

        /// <summary>
        ///     parse a linked file parameter
        /// </summary>
        /// <returns></returns>
        private Link ParseLinkParameter(Terminal symbol) {
            var fileName = ParseFileName(false, out var name);

            if (name == null) {
                ErrorLastPart(CompilerDirectiveParserErrors.InvalidLinkDirective);
            }

            return new Link(symbol, fileName, name);
        }

        private SyntaxPartBase ParseIncludeSwitch() {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                var symbol = ContinueWithOrMissing(TokenKind.IncludeSwitch);
                var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
                var parsedMode = IoCallCheck.Undefined;

                if (mode.GetSymbolKind() == TokenKind.Plus)
                    parsedMode = IoCallCheck.EnableIoChecks;
                else if (mode.GetSymbolKind() == TokenKind.Minus)
                    parsedMode = IoCallCheck.DisableIoChecks;

                return new IoChecks(symbol, mode, parsedMode);
            }

            if (LookAhead(1, TokenKind.Identifier) || LookAhead(1, TokenKind.QuotedString)) {
                return ParseIncludeFileName(ContinueWith(TokenKind.IncludeSwitch));
            }

            var symbol1 = ContinueWithOrMissing(TokenKind.IncludeSwitch);
            var mode1 = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            return new IoChecks(symbol1, mode1, IoCallCheck.Undefined);
        }


        private ImportedData ParseImportedDataSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ImportedDataSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = ImportGlobalUnitData.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = ImportGlobalUnitData.DoImport;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = ImportGlobalUnitData.NoImport;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidImportedDataDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new ImportedData(symbol, mode, parsedMode);
        }

        private ExtSyntax ParseExtendedSyntaxSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ExtendedSyntaxSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var parsedMode = ExtendedSyntax.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                parsedMode = ExtendedSyntax.UseExtendedSyntax;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                parsedMode = ExtendedSyntax.NoExtendedSyntax;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new ExtSyntax(symbol, mode, parsedMode);
        }

        private Extension ParseExtensionSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ExtensionSwitch);
            var extension = ContinueWith(TokenKind.Identifier);
            var parsedExtension = default(string);

            if (extension.GetSymbolKind() == TokenKind.Identifier) {
                parsedExtension = extension.Token.Value;
            }
            else {
                extension = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
            }

            return new Extension(symbol, extension, parsedExtension);
        }

        private ISyntaxPart ParseDebugInfoOrDescriptionSwitch() {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                var symbol = ContinueWithOrMissing(TokenKind.DebugInfoOrDescriptionSwitch);
                var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
                var debugInfo = DebugInformation.Undefined;

                if (mode.GetSymbolKind() == TokenKind.Plus) {
                    debugInfo = DebugInformation.IncludeDebugInformation;
                }
                else if (mode.GetSymbolKind() == TokenKind.Minus) {
                    debugInfo = DebugInformation.NoDebugInfo;
                }
                return new DebugInfoSwitch(symbol, mode, debugInfo);
            }
            else if (LookAhead(1, TokenKind.QuotedString)) {
                var symbol = ContinueWithOrMissing(TokenKind.DebugInfoOrDescriptionSwitch);
                var value = ContinueWith(TokenKind.QuotedString);
                var description = (value?.Token.ParsedValue as IStringValue)?.AsUnicodeString;
                return new Description(symbol, value, description);
            }
            else {
                var symbol = ContinueWithOrMissing(TokenKind.DebugInfoOrDescriptionSwitch);
                var mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.Plus, TokenKind.Minus, TokenKind.QuotedString });
                return new DebugInfoSwitch(symbol, mode, DebugInformation.Undefined);
            }
        }

        private AssertSwitch ParseAssertSwitch() {
            var assert = ContinueWithOrMissing(TokenKind.AssertSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var option = AssertionMode.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                option = AssertionMode.EnableAssertions;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                option = AssertionMode.DisableAssertions;
            }
            else if (mode == default) {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidAssertDirective, new int[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new AssertSwitch(assert, mode, option);
        }

        private BooleanEvaluationSwitch ParseBoolEvalSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.BoolEvalSwitch);
            var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
            var evalMode = BooleanEvaluation.Undefined;

            if (mode.GetSymbolKind() == TokenKind.Plus) {
                evalMode = BooleanEvaluation.CompleteEvaluation;
            }
            else if (mode.GetSymbolKind() == TokenKind.Minus) {
                evalMode = BooleanEvaluation.ShortEvaluation;
            }
            else {
                mode = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidBoolEvalDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new BooleanEvaluationSwitch(symbol, mode, evalMode);
        }

        /// <summary>
        ///     parse an align switch
        /// </summary>
        /// <returns></returns>
        public AlignSwitch ParseAlignSwitch() {
            var alignSymbol = default(Terminal);
            var alignSwitch = default(Terminal);
            var alignValue = Alignment.Undefined;

            if (Match(TokenKind.AlignSwitch1)) {
                alignSymbol = ContinueWith(TokenKind.AlignSwitch1);
                alignValue = Alignment.Unaligned;
            }
            else if (Match(TokenKind.AlignSwitch2)) {
                alignSymbol = ContinueWith(TokenKind.AlignSwitch2);
                alignValue = Alignment.Word;
            }
            else if (Match(TokenKind.AlignSwitch4)) {
                alignSymbol = ContinueWith(TokenKind.AlignSwitch4);
                alignValue = Alignment.DoubleWord;
            }
            else if (Match(TokenKind.AlignSwitch8)) {
                alignSymbol = ContinueWith(TokenKind.AlignSwitch8);
                alignValue = Alignment.QuadWord;
            }
            else if (Match(TokenKind.AlignSwitch16)) {
                alignSymbol = ContinueWith(TokenKind.AlignSwitch16);
                alignValue = Alignment.DoubleQuadWord;
            }
            else {
                alignSymbol = ContinueWithOrMissing(TokenKind.AlignSwitch);
                alignSwitch = ContinueWith(TokenKind.Plus, TokenKind.Minus);

                if (alignSwitch.GetSymbolKind() == TokenKind.Plus) {
                    alignValue = Alignment.QuadWord;
                }
                else if (alignSwitch.GetSymbolKind() == TokenKind.Minus) {
                    alignValue = Alignment.Unaligned;
                }
                else {
                    alignSwitch = ErrorAndSkip(CompilerDirectiveParserErrors.InvalidAlignDirective, new[] { TokenKind.Plus, TokenKind.Minus });
                }
            }

            return new AlignSwitch(alignSymbol, alignSwitch, alignValue);
        }

        /// <summary>
        ///     parse a compiler directive
        /// </summary>
        /// <returns>parsed syntax tree</returns>
        public override ISyntaxPart Parse() {
            var kind = CurrentToken().Kind;

            if (switches.Contains(kind))
                return ParseSwitch();

            if (longSwitches.Contains(kind))
                return ParseLongSwitch();

            if (parameters.Contains(kind))
                return ParseParameter();

            // TODO
            // if (!ConditionalCompilation.Skip)
            // log error
            FetchNextToken();
            return null;
        }

        /// <summary>
        ///     allow an identifier if it was tokenized as a keyword
        /// </summary>
        /// <returns></returns>
        protected override bool AllowIdentifier() {
            var token = CurrentToken().Value;
            return (!string.IsNullOrEmpty(token)) && (Tokenizer.Keywords.ContainsKey(token));
        }

    }

}
