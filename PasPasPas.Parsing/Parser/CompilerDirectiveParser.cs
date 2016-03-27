using PasPasPas.Api;
using System.Collections.Generic;
using System;
using PasPasPas.Options.DataTypes;
using System.Globalization;
using PasPasPas.Options.Bundles;
using PasPasPas.Infrastructure.Service;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.Tokenizer;
using PasPasPas.Internal.Tokenizer;

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
        ///     parser input
        /// </summary>
        public StackedFileReader Input
        {
            get
            {
                return ((StandardTokenizer)BaseTokenizer).Input = Input;
            }
            set
            {
                ((StandardTokenizer)BaseTokenizer).Input = value;
            }
        }

        private static HashSet<int> switches
            = new HashSet<int>() {
                PascalToken.AlignSwitch, PascalToken.AlignSwitch1,PascalToken.AlignSwitch2,PascalToken.AlignSwitch4,PascalToken.AlignSwitch8,PascalToken.AlignSwitch16,
                PascalToken.BoolEvalSwitch,
                PascalToken.AssertSwitch,
                PascalToken.DebugInfoOrDescriptionSwitch,
                PascalToken.ExtensionSwitch,
                PascalToken.ExtendedSyntaxSwitch,
                PascalToken.ImportedDataSwitch,
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

            return false;
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

            return false;
        }

        private void ParseLongIncludeSwitch() {
            var includeToken = Require(PascalToken.Identifier, PascalToken.QuotedString);
            string filename = includeToken.Value;

            if (includeToken.Kind == PascalToken.QuotedString) {
                filename = QuotedStringTokenValue.Unwrap(includeToken.Value);
            }

            string sourcePath = Input.CurrentFile.Path ?? string.Empty;
            string targetPath = Meta.IncludePathResolver.ResolvePath(sourcePath, filename);

            // resolve path to import file
            // append file in impotfilereader
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

            return false;
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
