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

        private static Tokenizer.Tokenizer CreateTokenizer(IParserEnvironment env, StackedFileReader reader)
            => new Tokenizer.Tokenizer(env, GetPatternsFromFactory(env), reader);

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
            IExtendableSyntaxPart parent = new CompilerDirective();

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
                return ParseIncludeSwitch(parent);

            if (Match(TokenKind.LinkOrLocalSymbolSwitch))
                return ParseLocalSymbolSwitch();

            if (Match(TokenKind.LongStringSwitch))
                return ParseLongStringSwitch();

            if (Match(TokenKind.OpenStringSwitch))
                return ParseOpenStringSwitch();

            if (Match(TokenKind.OptimizationSwitch)) {
                ParseOptimizationSwitch(parent);
            }
            else if (Match(TokenKind.OverflowSwitch)) {
                ParseOverflowSwitch(parent);
            }
            else if (Match(TokenKind.SaveDivideSwitch)) {
                ParseSaveDivideSwitch(parent);
            }
            else if (Match(TokenKind.IncludeRessource)) {
                return ParseIncludeRessource();
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
                return ParseTypeInfoSwitch();
            }
            else if (Match(TokenKind.EnumSizeSwitch, TokenKind.EnumSize1, TokenKind.EnumSize2, TokenKind.EnumSize4)) {
                ParseEnumSizeSwitch(parent);
            }

            return parent;
        }

        /// <summary>
        ///     parse a long switch
        /// </summary>
        private ISyntaxPart ParseLongSwitch() {
            var parent = new CompilerDirective();

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
                return ParseLongDesignOnlySwitch(parent);

            if (Match(TokenKind.ExtensionSwitchLong))
                return ParseLongExtensionSwitch();

            if (Match(TokenKind.ObjExportAll)) {
                ParseObjExportAllSwitch(parent);
            }

            else if (Match(TokenKind.ExtendedCompatibility)) {
                ParseExtendedCompatibilitySwitch(parent);
            }

            if (Match(TokenKind.ExtendedSyntaxSwitchLong))
                return ParseLongExtendedSyntaxSwitch();


            if (Match(TokenKind.ExcessPrecision)) {
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
                return ParseLongImportedDataSwitch();
            }

            else if (Match(TokenKind.IncludeSwitchLong)) {
                return ParseLongIncludeSwitch();
            }

            else if (Match(TokenKind.IoChecks)) {
                ParseLongIoChecksSwitch(parent);
            }

            else if (Match(TokenKind.LocalSymbolSwithLong)) {
                return ParseLongLocalSymbolSwitch();
            }

            else if (Match(TokenKind.LongStringSwitchLong)) {
                return ParseLongLongStringSwitch();
            }

            else if (Match(TokenKind.OpenStringSwitchLong)) {
                return ParseLongOpenStringSwitch();
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
                return ParseLongRangeChecksSwitch();
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
                return ParseLongTypeInfoSwitch();
            }

            else if (Match(TokenKind.RunOnly)) {
                ParseRunOnlyParameter(parent);
            }

            else if (Match(TokenKind.IncludeRessourceLong)) {
                return ParseLongIncludeRessourceSwitch();
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
                return ParseLongLinkSwitch();
            }

            return parent;
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
                    size1 = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                }
                else if (size1?.Token.ParsedValue is IIntegerValue intValue && !intValue.IsNegative) {
                    minStackSize = intValue.UnsignedValue;
                }
                else {
                    size1 = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                }
            }

            if (mSwitch)
                comma = ContinueWith(TokenKind.Comma);

            if (mSwitch || symbol.GetSymbolKind() == TokenKind.MaxMemStackSizeSwitchLong) {

                size2 = ContinueWith(TokenKind.Integer);

                if (size2 == default) {
                    size2 = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                }
                else if (size2?.Token.ParsedValue is IIntegerValue intValue && !intValue.IsNegative) {
                    maxStackSize = intValue.UnsignedValue;
                }
                else {
                    size2 = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective, new[] { TokenKind.Integer });
                }
            }

            return new StackMemorySize(symbol, size1, comma, size2, minStackSize, maxStackSize);
        }

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
                    ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidMessageDirective);
                }
            }
            else {
                message = MessageSeverity.Hint;
            }

            var text = ContinueWith(TokenKind.QuotedString);

            if (text == default) {
                message = MessageSeverity.Undefined;

                if (!hasError) {
                    text = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidMessageDirective, new[] { TokenKind.QuotedString });
                    hasError = true;
                }
            }

            var messageText = string.Empty;
            var textValue = text?.Token.ParsedValue ?? default;

            if (textValue is IStringValue stringValue)
                messageText = stringValue.AsUnicodeString;
            else if (!hasError)
                ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidMessageDirective);

            return new Message(symbol, kind, text, message, messageText);
        }

        private NoDefine ParseNoDefine() {
            var symbol = ContinueWithOrMissing(TokenKind.NoDefine);
            var typeName = ContinueWith(TokenKind.Identifier);

            if (typeName == default) {
                typeName = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidNoDefineDirective, new[] { TokenKind.Identifier });
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
                name = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidNoIncludeDirective, new[] { TokenKind.Identifier });
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
                number = ErrorAndSkip(number, CompilerDirectiveParserErrors.InvalidPEVersionDirective, new[] { TokenKind.Real });
            }

            var text = (number?.Token.Value ?? string.Empty).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (text.Length != 2 && !hasError) {
                ErrorLastPart(number, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            }

            var majorVersion = 0;
            var minorVersion = 0;

            if (!hasError && text.Length == 2 && (!int.TryParse(text[0], out majorVersion) || !int.TryParse(text[1], out minorVersion))) {
                ErrorLastPart(number, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
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
                regionName = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRegionDirective, new[] { TokenKind.QuotedString });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.Inherit, TokenKind.Explicit });
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
                            ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRttiDirective, Array.Empty<int>());
                        }
                        methods = AddToList(list, ParseRttiVisibility());
                    }

                    if (specifier.GetSymbolKind() == TokenKind.Properties) {
                        if (properties != default) {
                            parsedMode = RttiGenerationMode.Undefined;
                            ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRttiDirective, Array.Empty<int>());
                        }
                        properties = ParseRttiVisibility();
                    }

                    if (specifier.GetSymbolKind() == TokenKind.Fields) {
                        if (fields != default) {
                            parsedMode = RttiGenerationMode.Undefined;
                            ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRttiDirective, Array.Empty<int>());
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
                openParen = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.OpenBraces });

            if (openBraces == default)
                openBraces = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.OpenBraces });

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
                    closeBraces = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.CloseBraces });

                var closeParen = ContinueWith(TokenKind.CloseParen);
                if (closeParen == default)
                    closeParen = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRttiDirective, new[] { TokenKind.CloseParen });

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
                id = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidWarnDirective, new[] { TokenKind.Identifier });
            }
            else {
                warningType = id.Token.Value;
            }

            warningMode = ContinueWith(TokenKind.On, TokenKind.Off, TokenKind.Error, TokenKind.Default);

            if (warningMode == default) {
                if (!invalid)
                    warningMode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidWarnDirective, new[] { TokenKind.On, TokenKind.Off, TokenKind.Error, TokenKind.Default });
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
                    ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidWarnDirective, Array.Empty<object>());
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
                libParam = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidLibDirective, new[] { TokenKind.QuotedString });
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
                    ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidImageBaseDirective, new[] { TokenKind.Integer, TokenKind.HexNumber });
            }
            else {
                value = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidImageBaseDirective, new[] { TokenKind.Integer, TokenKind.HexNumber });
            }

            return new ImageBase(symbol, value, baseValue);
        }

        private IfDirective ParseIf()
            => new IfDirective(ContinueWithOrMissing(TokenKind.IfCd));

        private IfDef ParseIfNDef() {
            var symbol = ContinueWithOrMissing(TokenKind.IfNDef);
            var conditional = ContinueWith(TokenKind.Identifier);

            if (conditional == default) {
                conditional = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidIfNDefDirective, new[] { TokenKind.Identifier });
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
                    modeValue = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidHppEmitDirective, new[] { TokenKind.QuotedString });
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
                identifier = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidExternalSymbolDirective, new[] { TokenKind.Identifier });
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
                conditional = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidIfDefDirective, new[] { TokenKind.Identifier });
            }

            return new IfDef(ifDef, false, conditional);
        }

        private UnDefineSymbol ParseUndef() {
            var symbol = ContinueWith(TokenKind.Undef);
            var conditional = ContinueWith(TokenKind.Identifier);

            if (conditional == default) {
                conditional = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidUnDefineDirective, new[] { TokenKind.Identifier });
            }

            return new UnDefineSymbol(symbol, conditional);
        }

        private DefineSymbol ParseDefine() {
            var symbol = ContinueWithOrMissing(TokenKind.Define);
            var name = ContinueWith(TokenKind.Identifier);

            if (name == default)
                name = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidDefineDirective, new[] { TokenKind.Identifier });

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
                        ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidCodeAlignDirective, value);
                        break;
                }

            }
            else {
                value = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidCodeAlignDirective, new[] { TokenKind.Integer });
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
                    ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidApplicationType, null);
                }
            }
            else {
                appTypeInfo = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidApplicationType, new[] { TokenKind.Identifier });
            }

            return new AppTypeParameter(appTypeSymbol, appTypeInfo, appType); ;
        }

        private Link ParseLongLinkSwitch()
            => ParseLinkParameter(ContinueWithOrMissing(TokenKind.LinkSwitchLong));

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

            ulong size;

            if (result.LastTerminalToken.ParsedValue is IIntegerValue intValue && !intValue.IsNegative) {
                size = intValue.UnsignedValue;
            }
            else {
                ErrorLastPart(result, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective);
                return;
            }

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

        private Resource ParseLongIncludeRessourceSwitch()
            => ParseResourceFileName(ContinueWithOrMissing(TokenKind.IncludeRessourceLong));

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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new PublishedRtti(symbol, mode, parsedMode);
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
                result.Mode = UsePointersWithTypeChecking.Enable;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = UsePointersWithTypeChecking.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective, new[] { TokenKind.On, TokenKind.Off });
            }
        }

        private void ParseLongVarStringCheckSwitch(IExtendableSyntaxPart parent) {
            var result = new VarStringChecks();
            InitByTerminal(result, parent, TokenKind.VarStringCheckSwitchLong);

            if (ContinueWith(result, TokenKind.On)) {
                result.Mode = ShortVarStringCheck.EnableChecks;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = ShortVarStringCheck.DisableChecks;
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidRangeCheckDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new RangeChecks(symbol, mode, parsedMode);
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
                result.Mode = RuntimeOverflowCheck.EnableChecks;
            }
            else if (ContinueWith(result, TokenKind.Off)) {
                result.Mode = RuntimeOverflowCheck.DisableChecks;
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidOpenStringsDirective, new[] { TokenKind.On, TokenKind.Off });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new LongStrings(symbol, mode, parsedMode);
        }

        private LocalSymbols ParseLongLocalSymbolSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.LocalSymbolSwithLong);
            var mode = ContinueWith(TokenKind.On, TokenKind.Off);
            var parsedMode = LocalDebugSymbols.Undefined;

            if (mode.GetSymbolKind() == TokenKind.On) {
                parsedMode = LocalDebugSymbols.EnableLocalSymbols;
            }
            else if (mode.GetSymbolKind() == TokenKind.Off) {
                parsedMode = LocalDebugSymbols.DisableLocalSymbols;
            }
            else {
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidLocalSymbolsDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new LocalSymbols(symbol, mode, parsedMode);
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidImportedDataDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ImportedData(symbol, mode, parsedMode);
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective, new[] { TokenKind.On, TokenKind.Off });
            }

            return new ExtSyntax(symbol, mode, parsedMode);
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

        private Extension ParseLongExtensionSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ExtensionSwitchLong);
            var extension = ContinueWith(TokenKind.Identifier);
            var parsedExtension = default(string);

            if (extension.GetSymbolKind() == TokenKind.Identifier) {
                parsedExtension = extension.Token.Value;
            }
            else {
                extension = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
            }

            return new Extension(symbol, extension, parsedExtension);
        }

        private DesignOnly ParseLongDesignOnlySwitch(IExtendableSyntaxPart parent) {
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective, new[] { TokenKind.On, TokenKind.Off });
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
                value = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidDescriptionDirective, new[] { TokenKind.QuotedString });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective, new[] { TokenKind.On, TokenKind.Off });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.On, TokenKind.Off });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidAssertDirective, new int[] { TokenKind.On, TokenKind.Off });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidBoolEvalDirective, new[] { TokenKind.On, TokenKind.Off });
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

                ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidAlignDirective);
                return new AlignSwitch(alignSymbol, alignSwitch, Alignment.Undefined);
            }

            alignSwitch = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidAlignDirective, new[] { TokenKind.Integer });
            return new AlignSwitch(alignSymbol, alignSwitch, Alignment.Undefined);
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

        private ObjTypeName ParseObjTypeNameSwitch() {
            var symbol = ContinueWithOrMissing(TokenKind.ObjTypeName);
            var name = ContinueWith(TokenKind.Identifier);
            var typeName = name?.Token.Value ?? string.Empty;

            if (name == default) {
                name = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidObjTypeDirective, new[] { TokenKind.Identifier });
            }

            var aliasName = ContinueWith(TokenKind.QuotedString);
            var prefix = string.Empty;
            var parsedAliasName = string.Empty;

            if (aliasName != default && aliasName.Token.ParsedValue is IStringValue alias) {
                parsedAliasName = alias.AsUnicodeString;
                if (string.IsNullOrWhiteSpace(parsedAliasName)) {
                    parsedAliasName = null;
                    typeName = null;
                    ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
                }
                else {
                    prefix = parsedAliasName.Substring(0, 1);
                    if (!string.Equals(prefix, "N", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(prefix, "B", StringComparison.OrdinalIgnoreCase)) {
                        parsedAliasName = null;
                        typeName = null;
                        ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new PublishedRtti(symbol, mode, parsedMode);
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
                result.Mode = UsePointersWithTypeChecking.Enable;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = UsePointersWithTypeChecking.Disable;
            }
            else {
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective, new[] { TokenKind.Minus, TokenKind.Platform });
            }
        }

        private void ParseVarStringCheckSwitch(IExtendableSyntaxPart parent) {
            var result = new VarStringChecks();
            InitByTerminal(result, parent, TokenKind.VarStringCheckSwitch);

            if (ContinueWith(result, TokenKind.Plus)) {
                result.Mode = ShortVarStringCheck.EnableChecks;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = ShortVarStringCheck.DisableChecks;
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
                    ident = ErrorAndSkip(ident, CompilerDirectiveParserErrors.InvalidFileName, new[] { TokenKind.Identifier });
                    name = default;
                    return new FilenameSymbol(filename, ident);
                }

                name = string.Concat("*", ident.Token.Value);
                return new FilenameSymbol(filename, ident);
            }

            filename = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidFileName, new[] { TokenKind.Identifier });
            name = default;
            return new FilenameSymbol(filename, default);
        }

        private Include ParseIncludeFileName(Terminal symbol) {
            var fileName = ParseFileName(false, out var name);

            if (name == default)
                ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidIncludeDirective, new[] { TokenKind.Identifier });

            return new Include(symbol, fileName, name);
        }

        private Resource ParseResourceFileName(Terminal symbol) {
            var fileName = ParseFileName(true, out var name);

            if (name == null)
                ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidResourceDirective, new[] { TokenKind.Identifier });

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
                result.Mode = RuntimeOverflowCheck.EnableChecks;
            }
            else if (ContinueWith(result, TokenKind.Minus)) {
                result.Mode = RuntimeOverflowCheck.DisableChecks;
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidOpenStringsDirective, new[] { TokenKind.Plus, TokenKind.Minus });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective, new[] { TokenKind.Plus, TokenKind.Minus });
            }

            return new LongStrings(symbol, mode, parsedMode);
        }

        private ISyntaxPart ParseLocalSymbolSwitch() {
            if (LookAhead(1, TokenKind.Plus, TokenKind.Minus)) {
                var symbol = ContinueWithOrMissing(TokenKind.LinkOrLocalSymbolSwitch);
                var mode = ContinueWith(TokenKind.Plus, TokenKind.Minus);
                var parsedMode = LocalDebugSymbols.Undefined;

                if (mode.GetSymbolKind() == TokenKind.Plus) {
                    parsedMode = LocalDebugSymbols.EnableLocalSymbols;
                }
                else if (mode.GetSymbolKind() == TokenKind.Minus) {
                    parsedMode = LocalDebugSymbols.DisableLocalSymbols;
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
                ErrorLastPart(null, CompilerDirectiveParserErrors.InvalidLinkDirective);
            }

            return new Link(symbol, fileName, name);
        }

        private SyntaxPartBase ParseIncludeSwitch(IExtendableSyntaxPart parent) {

            if (LookAhead(1, TokenKind.Plus)) {
                var result = new IoChecks();
                InitByTerminal(result, parent, TokenKind.IncludeSwitch);
                ContinueWith(result, TokenKind.Plus);
                result.Mode = IoCallCheck.EnableIoChecks;
                return result;
            }
            else if (LookAhead(1, TokenKind.Minus)) {
                var result = new IoChecks();
                InitByTerminal(result, parent, TokenKind.IncludeSwitch);
                ContinueWith(result, TokenKind.Minus);
                result.Mode = IoCallCheck.DisableIoChecks;
                return result;
            }
            else if (LookAhead(1, TokenKind.Identifier) || LookAhead(1, TokenKind.QuotedString)) {
                return ParseIncludeFileName(ContinueWith(TokenKind.IncludeSwitch));
            }
            else {
                var result = new IoChecks();
                InitByTerminal(result, parent, TokenKind.IncludeSwitch);
                ErrorAndSkip(result, CompilerDirectiveParserErrors.InvalidIoChecksDirective, new[] { TokenKind.Plus, TokenKind.Minus });
                return result;
            }
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidImportedDataDirective, new[] { TokenKind.Plus, TokenKind.Minus });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective, new[] { TokenKind.Plus, TokenKind.Minus });
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
                extension = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidExtensionDirective, new[] { TokenKind.Identifier });
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
                var mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidDebugInfoDirective, new[] { TokenKind.Plus, TokenKind.Minus, TokenKind.QuotedString });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidAssertDirective, new int[] { TokenKind.Plus, TokenKind.Minus });
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
                mode = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidBoolEvalDirective, new[] { TokenKind.Plus, TokenKind.Minus });
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
                    alignSwitch = ErrorAndSkip(null, CompilerDirectiveParserErrors.InvalidAlignDirective, new[] { TokenKind.Plus, TokenKind.Minus });
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
