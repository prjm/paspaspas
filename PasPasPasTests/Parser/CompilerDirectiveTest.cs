using PasPasPas.Options.Bundles;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using System;
using System.Linq;
using Xunit;

namespace PasPasPasTests.Parser {

    public class CompilerDirectiveTest : ParserTestBase {

        [Fact]
        public void TestAlign() {
            Func<object> f = () => CompilerOptions.Align.Value;
            RunCompilerDirective("", Alignment.Undefined, f);
            RunCompilerDirective("A+", Alignment.QuadWord, f);
            RunCompilerDirective("A-", Alignment.Unaligned, f);
            RunCompilerDirective("A1", Alignment.Unaligned, f);
            RunCompilerDirective("A2", Alignment.Word, f);
            RunCompilerDirective("A4", Alignment.DoubleWord, f);
            RunCompilerDirective("A8", Alignment.QuadWord, f);
            RunCompilerDirective("A16", Alignment.DoubleQuadWord, f);
            RunCompilerDirective("ALIGN ON", Alignment.QuadWord, f);
            RunCompilerDirective("ALIGN OFF", Alignment.Unaligned, f);
            RunCompilerDirective("ALIGN 1", Alignment.Unaligned, f);
            RunCompilerDirective("ALIGN 2", Alignment.Word, f);
            RunCompilerDirective("ALIGN 4", Alignment.DoubleWord, f);
            RunCompilerDirective("ALIGN 8", Alignment.QuadWord, f);
            RunCompilerDirective("ALIGN 16", Alignment.DoubleQuadWord, f);
            RunCompilerDirective("ALIGN KAPUTT", Alignment.Undefined, f, CompilerDirectiveParserErrors.InvalidAlignDirective);
            RunCompilerDirective("ALIGN 17", Alignment.Undefined, f, CompilerDirectiveParserErrors.InvalidAlignDirective);
            RunCompilerDirective("A 0", Alignment.Undefined, f, CompilerDirectiveParserErrors.InvalidAlignDirective);
            RunCompilerDirective("ALIGN", Alignment.Undefined, f, CompilerDirectiveParserErrors.InvalidAlignDirective);
            RunCompilerDirective("A", Alignment.Undefined, f, CompilerDirectiveParserErrors.InvalidAlignDirective);
        }

        [Fact]
        public void TestApptype() {
            Func<object> f = () => CompilerOptions.ApplicationType.Value;
            RunCompilerDirective("", AppType.Undefined, f);
            RunCompilerDirective("APPTYPE GUI", AppType.Gui, f);
            RunCompilerDirective("APPTYPE CONSOLE", AppType.Console, f);
            RunCompilerDirective("APPTYPE 17", AppType.Undefined, f, CompilerDirectiveParserErrors.InvalidApplicationType);
            RunCompilerDirective("APPTYPE UNDIFINED", AppType.Undefined, f, CompilerDirectiveParserErrors.InvalidApplicationType);
            RunCompilerDirective("APPTYPE", AppType.Undefined, f, CompilerDirectiveParserErrors.InvalidApplicationType);
        }

        [Fact]
        public void TestMessage() {
            RunCompilerDirective("", true, () => true);
            RunCompilerDirective("MESSAGE 'X'", true, () => true);
            RunCompilerDirective("MESSAGE Hint 'X' ", true, () => true);
            RunCompilerDirective("MESSAGE Warn 'X' ", true, () => true);
            RunCompilerDirective("MESSAGE Error 'X'", true, () => true);
            RunCompilerDirective("MESSAGE Fatal 'x'", true, () => true);
        }

        [Fact]
        public void TestMemStackSize() {
            RunCompilerDirective("", 0L, () => CompilerOptions.MinimumStackMemSize.Value);
            RunCompilerDirective("", 0L, () => CompilerOptions.MaximumStackMemSize.Value);
            RunCompilerDirective("M 100, 200", 200L, () => CompilerOptions.MaximumStackMemSize.Value);
            RunCompilerDirective("M 100, 200", 100L, () => CompilerOptions.MinimumStackMemSize.Value);
            RunCompilerDirective("MINSTACKSIZE 300", 300L, () => CompilerOptions.MinimumStackMemSize.Value);
            RunCompilerDirective("MAXSTACKSIZE 400", 400L, () => CompilerOptions.MaximumStackMemSize.Value);
        }

        [Fact]
        public void TestBoolEvalSwitch() {
            Func<object> f = () => CompilerOptions.BoolEval.Value;
            RunCompilerDirective("", BooleanEvaluation.Undefined, f);
            RunCompilerDirective("B+", BooleanEvaluation.CompleteEvaluation, f);
            RunCompilerDirective("B-", BooleanEvaluation.ShortEvaluation, f);
            RunCompilerDirective("BOOLEVAL ON", BooleanEvaluation.CompleteEvaluation, f);
            RunCompilerDirective("BOOLEVAL OFF", BooleanEvaluation.ShortEvaluation, f);
            RunCompilerDirective("BOOLEVAL FUZZBUFF", BooleanEvaluation.Undefined, f, CompilerDirectiveParserErrors.InvalidBoolEvalDirective);
            RunCompilerDirective("BOOLEVAL", BooleanEvaluation.Undefined, f, CompilerDirectiveParserErrors.InvalidBoolEvalDirective);
        }

        [Fact]
        public void TestCodeAlignParameter() {
            Func<object> f = () => CompilerOptions.CodeAlign.Value;
            RunCompilerDirective("", CodeAlignment.Undefined, f);
            RunCompilerDirective("CODEALIGN 1", CodeAlignment.OneByte, f);
            RunCompilerDirective("CODEALIGN 2", CodeAlignment.TwoByte, f);
            RunCompilerDirective("CODEALIGN 4", CodeAlignment.FourByte, f);
            RunCompilerDirective("CODEALIGN 8", CodeAlignment.EightByte, f);
            RunCompilerDirective("CODEALIGN 16", CodeAlignment.SixteenByte, f);
            RunCompilerDirective("CODEALIGN 17", CodeAlignment.Undefined, f, CompilerDirectiveParserErrors.InvalidCodeAlignDirective);
            RunCompilerDirective("CODEALIGN FUZBUZ", CodeAlignment.Undefined, f, CompilerDirectiveParserErrors.InvalidCodeAlignDirective);
            RunCompilerDirective("CODEALIGN", CodeAlignment.Undefined, f, CompilerDirectiveParserErrors.InvalidCodeAlignDirective);
        }

        [Fact]
        public void TestDefine() {
            Func<object> f = () => ConditionalCompilation.IsSymbolDefined("TESTSYM");
            Func<object> g = () => ConditionalCompilation.IsSymbolDefined("PASPASPAS_TEST");
            Func<object> h = () => ConditionalCompilation.IsSymbolDefined("A");
            Func<object> b = () => ConditionalCompilation.IsSymbolDefined("B");

            RunCompilerDirective("", false, f);
            RunCompilerDirective("DEFINE TESTSYM", true, f);
            RunCompilerDirective("DEFINE TESTsYM", true, f);
            RunCompilerDirective("DEFINE TESTsYM | DEFINE X", false, f);
            RunCompilerDirective("DEFINE TESTSYM § DEFINE X", true, f);
            RunCompilerDirective("", false, f);
            RunCompilerDirective("DEFINE TESTSYM § UNDEF TESTSYM", false, f);
            RunCompilerDirective("DEFINE TESTSYM § UNDEF TESTSYM § DEFINE TESTSYM", true, f);
            RunCompilerDirective("UNDEF PASPASPAS_TEST | DEFINE X", true, g);
            RunCompilerDirective("UNDEF PASPASPAS_TEST § DEFINE X", false, g);
            RunCompilerDirective("IFDEF TESTSYM § DEFINE A § ENDIF", false, h);
            RunCompilerDirective("IFDEF TESTSYM § ENDIF § DEFINE A ", true, h);
            RunCompilerDirective("IFDEF TESTSYM § IFDEF Q § DEFINE A § ENDIF § ENDIF", false, h);
            RunCompilerDirective("IFDEF PASPASPAS_TEST § DEFINE A § ENDIF", true, h);
            RunCompilerDirective("IFDEF TESTSYM § DEFINE B § ELSE § DEFINE A § ENDIF", true, h);
            RunCompilerDirective("IFDEF TESTSYM § DEFINE A § ELSE § DEFINE B § ENDIF", false, h);
            RunCompilerDirective("IFDEF TESTSYM § DEFINE B § ELSE § DEFINE A § ENDIF", false, b);
            RunCompilerDirective("IFDEF TESTSYM § DEFINE A § ELSE § DEFINE B § ENDIF", true, b);
            RunCompilerDirective("IFNDEF TESTSYM § DEFINE A § ENDIF", true, h);
            RunCompilerDirective("IFNDEF TESTSYM § IFNDEF Q § DEFINE A § ENDIF § ENDIF", true, h);
            RunCompilerDirective("IFNDEF PASPASPAS_TEST § DEFINE A § ENDIF", false, h);

            RunCompilerDirective("ENDIF", false, f, CompilerDirectiveParserErrors.EndIfWithoutIf);
            RunCompilerDirective("ELSE", false, f, CompilerDirectiveParserErrors.ElseIfWithoutIf);
            RunCompilerDirective("IFDEF", false, f, CompilerDirectiveParserErrors.InvalidIfDefDirective);
            RunCompilerDirective("IFNDEF", false, f, CompilerDirectiveParserErrors.InvalidIfNDefDirective);
            RunCompilerDirective("IFDEF X | DEFINE Z ", false, f, OptionSet.PendingCondition);
            RunCompilerDirective("IFNDEF X | DEFINE Z ", false, f, OptionSet.PendingCondition);
            RunCompilerDirective("IFDEF X § ELSE | DEFINE Z ", false, f, OptionSet.PendingCondition);
        }

        [Fact]
        public void TestAssertions() {
            Func<object> f = () => CompilerOptions.Assertions.Value;
            RunCompilerDirective("", AssertionMode.Undefined, f);
            RunCompilerDirective("C+", AssertionMode.EnableAssertions, f);
            RunCompilerDirective("C-", AssertionMode.DisableAssertions, f);
            RunCompilerDirective("ASSERTIONS ON", AssertionMode.EnableAssertions, f);
            RunCompilerDirective("ASSERTIONS OFF", AssertionMode.DisableAssertions, f);
            RunCompilerDirective("ASSERTIONS FUZBAZ", AssertionMode.Undefined, f, CompilerDirectiveParserErrors.InvalidAssertDirective);
            RunCompilerDirective("ASSERTIONS", AssertionMode.Undefined, f, CompilerDirectiveParserErrors.InvalidAssertDirective);
            RunCompilerDirective("C", AssertionMode.Undefined, f, CompilerDirectiveParserErrors.InvalidAssertDirective);
        }


        [Fact]
        public void TestDebugInfo() {
            Func<object> f = () => CompilerOptions.DebugInfo.Value;
            RunCompilerDirective("", DebugInformation.Undefined, f);
            RunCompilerDirective("D+", DebugInformation.IncludeDebugInformation, f);
            RunCompilerDirective("D-", DebugInformation.NoDebugInfo, f);
            RunCompilerDirective("DEBUGINFO ON", DebugInformation.IncludeDebugInformation, f);
            RunCompilerDirective("DEBUGINFO OFF", DebugInformation.NoDebugInfo, f);
            RunCompilerDirective("DEBUGINFO FUZZ", DebugInformation.Undefined, f, CompilerDirectiveParserErrors.InvalidDebugInfoDirective);
            RunCompilerDirective("DEBUGINFO", DebugInformation.Undefined, f, CompilerDirectiveParserErrors.InvalidDebugInfoDirective);
            RunCompilerDirective("D", DebugInformation.Undefined, f, CompilerDirectiveParserErrors.InvalidDebugInfoDirective);
        }

        [Fact]
        public void TestDenyPackageUnit() {
            Func<object> f = () => ConditionalCompilation.DenyInPackages.Value;
            RunCompilerDirective("", DenyUnitInPackages.Undefined, f);
            RunCompilerDirective("DENYPACKAGEUNIT ON", DenyUnitInPackages.DenyUnit, f);
            RunCompilerDirective("DENYPACKAGEUNIT OFF", DenyUnitInPackages.AllowUnit, f);
            RunCompilerDirective("DENYPACKAGEUNIT KAPUTT", DenyUnitInPackages.Undefined, f, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective);
            RunCompilerDirective("DENYPACKAGEUNIT", DenyUnitInPackages.Undefined, f, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective);
        }

        [Fact]
        public void TestDescription() {
            Func<object> f = () => Meta.Description.Value;
            RunCompilerDirective("", null, f);
            RunCompilerDirective("DESCRIPTION 'dummy'", "dummy", f);
            RunCompilerDirective("D 'dummy1'", "dummy1", f);
            RunCompilerDirective("DESCRIPTION KAPUTT", null, f, CompilerDirectiveParserErrors.InvalidDescriptionDirective);
            RunCompilerDirective("DESCRIPTION", null, f, CompilerDirectiveParserErrors.InvalidDescriptionDirective);
            RunCompilerDirective("D KAPUTT", null, f, CompilerDirectiveParserErrors.InvalidDebugInfoDirective);
            RunCompilerDirective("D", null, f, CompilerDirectiveParserErrors.InvalidDebugInfoDirective);
        }

        [Fact]
        public void TestDesigntimeOnly() {
            Func<object> f = () => ConditionalCompilation.DesignOnly.Value;
            RunCompilerDirective("", DesignOnlyUnit.Undefined, f);
            RunCompilerDirective("DESIGNONLY ON", DesignOnlyUnit.InDesignTimeOnly, f);
            RunCompilerDirective("DESIGNONLY OFF", DesignOnlyUnit.Alltimes, f);
            RunCompilerDirective("DESIGNONLY KAPUTT", DesignOnlyUnit.Undefined, f, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective);
            RunCompilerDirective("DESIGNONLY", DesignOnlyUnit.Undefined, f, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective);
        }

        [Fact]
        public void TestExtensionsSwitch() {
            Func<object> f = () => Meta.FileExtension.Value;
            RunCompilerDirective("", null, f);
            RunCompilerDirective("EXTENSION ddd", "ddd", f);
            RunCompilerDirective("E ddd", "ddd", f);
            RunCompilerDirective("E +", null, f, CompilerDirectiveParserErrors.InvalidExtensionDirective);
            RunCompilerDirective("E", null, f, CompilerDirectiveParserErrors.InvalidExtensionDirective);
            RunCompilerDirective("EXTENSION +", null, f, CompilerDirectiveParserErrors.InvalidExtensionDirective);
            RunCompilerDirective("EXTENSION", null, f, CompilerDirectiveParserErrors.InvalidExtensionDirective);
        }

        [Fact]
        public void TestObjExportAll() {
            Func<object> f = () => CompilerOptions.ExportCppObjects.Value;
            RunCompilerDirective("", ExportCppObjects.Undefined, f);
            RunCompilerDirective("OBJEXPORTALL ON", ExportCppObjects.ExportAll, f);
            RunCompilerDirective("OBJEXPORTALL OFF", ExportCppObjects.DoNotExportAll, f);
            RunCompilerDirective("OBJEXPORTALL KAPUTT", ExportCppObjects.Undefined, f, CompilerDirectiveParserErrors.InvalidObjectExportDirective);
            RunCompilerDirective("OBJEXPORTALL", ExportCppObjects.Undefined, f, CompilerDirectiveParserErrors.InvalidObjectExportDirective);
        }

        [Fact]
        public void TestExtendedCompatibility() {
            Func<object> f = () => CompilerOptions.ExtendedCompatibility.Value;
            RunCompilerDirective("", ExtendedCompatiblityMode.Undefined, f);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY ON", ExtendedCompatiblityMode.Enabled, f);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY OFF", ExtendedCompatiblityMode.Disabled, f);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY KAPUTT", ExtendedCompatiblityMode.Undefined, f, CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY", ExtendedCompatiblityMode.Undefined, f, CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective);
        }

        [Fact]
        public void TestExtendedSyntax() {
            Func<object> f = () => CompilerOptions.UseExtendedSyntax.Value;
            RunCompilerDirective("", ExtendedSyntax.Undefined, f);
            RunCompilerDirective("X+", ExtendedSyntax.UseExtendedSyntax, f);
            RunCompilerDirective("X-", ExtendedSyntax.NoExtendedSyntax, f);
            RunCompilerDirective("X A", ExtendedSyntax.Undefined, f, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective);
            RunCompilerDirective("X", ExtendedSyntax.Undefined, f, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective);
            RunCompilerDirective("EXTENDEDSYNTAX ON", ExtendedSyntax.UseExtendedSyntax, f);
            RunCompilerDirective("EXTENDEDSYNTAX OFF", ExtendedSyntax.NoExtendedSyntax, f);
            RunCompilerDirective("EXTENDEDSYNTAX KAPUTT", ExtendedSyntax.Undefined, f, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective);
            RunCompilerDirective("EXTENDEDSYNTAX", ExtendedSyntax.Undefined, f, CompilerDirectiveParserErrors.InvalidExtendedSyntaxDirective);
        }

        [Fact]
        public void TestExternalSymbol() {
            Func<object> c = () => Meta.ExternalSymbols.Count;
            Func<object> f = () => Meta.ExternalSymbols.Any(t => string.Equals(t.IdentifierName, "dummy"));
            RunCompilerDirective("", 0, c);
            RunCompilerDirective("EXTERNALSYM dummy", true, f);
            RunCompilerDirective("EXTERNALSYM dummy 'a'", true, f);
            RunCompilerDirective("EXTERNALSYM dummy 'a' 'q'", true, f);
            RunCompilerDirective("EXTERNALSYM+", 0, c, CompilerDirectiveParserErrors.InvalidExternalSymbolDirective);
            RunCompilerDirective("EXTERNALSYM", 0, c, CompilerDirectiveParserErrors.InvalidExternalSymbolDirective);
        }

        [Fact]
        public void TestExcessPrecision() {
            Func<object> f = () => CompilerOptions.ExcessPrecision.Value;
            RunCompilerDirective("", ExcessPrecisionForResults.Undefined, f);
            RunCompilerDirective("EXCESSPRECISION  ON", ExcessPrecisionForResults.EnableExcess, f);
            RunCompilerDirective("EXCESSPRECISION  OFF", ExcessPrecisionForResults.DisableExcess, f);
            RunCompilerDirective("EXCESSPRECISION  KAPUTT", ExcessPrecisionForResults.Undefined, f, CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective);
            RunCompilerDirective("EXCESSPRECISION  ", ExcessPrecisionForResults.Undefined, f, CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective);
        }

        [Fact]
        public void TestHighCharUnicode() {
            Func<object> f = () => CompilerOptions.HighCharUnicode.Value;
            RunCompilerDirective("", HighCharsUnicode.Undefined, f);
            RunCompilerDirective("HIGHCHARUNICODE OFF", HighCharsUnicode.DisableHighChars, f);
            RunCompilerDirective("HIGHCHARUNICODE ON", HighCharsUnicode.EnableHighChars, f);
            RunCompilerDirective("HIGHCHARUNICODE KAPUTT", HighCharsUnicode.Undefined, f, CompilerDirectiveParserErrors.InvalidHighCharUnicodeDirective);
            RunCompilerDirective("HIGHCHARUNICODE ", HighCharsUnicode.Undefined, f, CompilerDirectiveParserErrors.InvalidHighCharUnicodeDirective);
        }

        [Fact]
        public void TestHints() {
            Func<object> f = () => CompilerOptions.Hints.Value;
            RunCompilerDirective("", CompilerHints.Undefined, f);
            RunCompilerDirective("HINTS ON", CompilerHints.EnableHints, f);
            RunCompilerDirective("HINTS OFF", CompilerHints.DisableHints, f);
            RunCompilerDirective("HINTS KAPUTT", CompilerHints.Undefined, f, CompilerDirectiveParserErrors.InvalidHintsDirective);
            RunCompilerDirective("HINTS ", CompilerHints.Undefined, f, CompilerDirectiveParserErrors.InvalidHintsDirective);
        }

        [Fact]
        public void TestHppEmit() {
            Func<object> c = () => Meta.HeaderStrings.Count;
            Func<object> f = () => Meta.HeaderStrings.Any(t => string.Equals(t.Value, "'dummy'"));
            Func<HppEmitMode, Func<object>> g = x => () => Meta.HeaderStrings.Any(t => t.Mode == x);
            RunCompilerDirective("", 0, c);
            RunCompilerDirective("HPPEMIT 'dummy'", true, f);
            RunCompilerDirective("HPPEMIT END 'dummy'", true, f);
            RunCompilerDirective("HPPEMIT LINKUNIT", true, g(HppEmitMode.LinkUnit));
            RunCompilerDirective("HPPEMIT OPENNAMESPACE", true, g(HppEmitMode.OpenNamespace));
            RunCompilerDirective("HPPEMIT CLOSENAMESPACE", true, g(HppEmitMode.CloseNamespace));
            RunCompilerDirective("HPPEMIT NOUSINGNAMESPACE", true, g(HppEmitMode.NoUsingNamespace));
            RunCompilerDirective("HPPEMIT KAPUTT", false, f, CompilerDirectiveParserErrors.InvalidHppEmitDirective);
            RunCompilerDirective("HPPEMIT END KAPUTT", false, f, CompilerDirectiveParserErrors.InvalidHppEmitDirective);
            RunCompilerDirective("HPPEMIT ", false, f, CompilerDirectiveParserErrors.InvalidHppEmitDirective);
        }

        [Fact]
        public void TestImageBase() {
            Func<object> f = () => CompilerOptions.ImageBase.Value;
            RunCompilerDirective("", 0, f);
            RunCompilerDirective("IMAGEBASE $40000000 ", 0x40000000, f);
            RunCompilerDirective("IMAGEBASE 40000000 ", 40000000, f);
            RunCompilerDirective("IMAGEBASE KAPUTT ", 0, f, CompilerDirectiveParserErrors.InvalidImageBaseDirective);
            RunCompilerDirective("IMAGEBASE ", 0, f, CompilerDirectiveParserErrors.InvalidImageBaseDirective);
        }

        [Fact]
        public void TestImplicitBuild() {
            Func<object> f = () => CompilerOptions.ImplicitBuild.Value;
            RunCompilerDirective("", ImplicitBuildUnit.Undefined, f);
            RunCompilerDirective("IMPLICITBUILD ON", ImplicitBuildUnit.EnableImplicitBuild, f);
            RunCompilerDirective("IMPLICITBUILD OFF", ImplicitBuildUnit.DisableImplicitBuild, f);
            RunCompilerDirective("IMPLICITBUILD KAPUTT", ImplicitBuildUnit.Undefined, f, CompilerDirectiveParserErrors.InvalidImplicitBuildDirective);
            RunCompilerDirective("IMPLICITBUILD ", ImplicitBuildUnit.Undefined, f, CompilerDirectiveParserErrors.InvalidImplicitBuildDirective);
        }

        [Fact]
        public void TestImportUnitData() {
            RunCompilerDirective("", ImportGlobalUnitData.Undefined, () => CompilerOptions.ImportedData.Value);
            RunCompilerDirective("G+", ImportGlobalUnitData.DoImport, () => CompilerOptions.ImportedData.Value);
            RunCompilerDirective("G-", ImportGlobalUnitData.NoImport, () => CompilerOptions.ImportedData.Value);
            RunCompilerDirective("IMPORTEDDATA  ON", ImportGlobalUnitData.DoImport, () => CompilerOptions.ImportedData.Value);
            RunCompilerDirective("IMPORTEDDATA OFF", ImportGlobalUnitData.NoImport, () => CompilerOptions.ImportedData.Value);
        }

        [Fact]
        public void TestInclude() {
            Func<object> f = () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC");
            RunCompilerDirective("", false, f);
            RunCompilerDirective("INCLUDE 'DUMMY.INC'", true, f);
            RunCompilerDirective("", false, f);
            RunCompilerDirective("I 'DUMMY.INC'", true, f);
            RunCompilerDirective("I DUMMY.INC", true, f);
        }

        [Fact]
        public void TestIoChecks() {
            Func<object> f = () => CompilerOptions.IoChecks.Value;
            RunCompilerDirective("", IoCallChecks.Undefined, f);
            RunCompilerDirective("I+", IoCallChecks.EnableIoChecks, f);
            RunCompilerDirective("I-", IoCallChecks.DisableIoChecks, f);
            RunCompilerDirective("I 3", IoCallChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
            RunCompilerDirective("I", IoCallChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
            RunCompilerDirective("IOCHECKS  ON", IoCallChecks.EnableIoChecks, f);
            RunCompilerDirective("IOCHECKS OFF", IoCallChecks.DisableIoChecks, f);
            RunCompilerDirective("IOCHECKS KAPUTT", IoCallChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
            RunCompilerDirective("IOCHECKS ", IoCallChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
        }

        [Fact]
        public void TestLocalSymbols() {
            Func<object> f = () => CompilerOptions.LocalSymbols.Value;
            RunCompilerDirective("", LocalDebugSymbols.Undefined, f);
            RunCompilerDirective("L+", LocalDebugSymbols.EnableLocalSymbols, f);
            RunCompilerDirective("L-", LocalDebugSymbols.DisableLocalSymbols, f);
            RunCompilerDirective("LOCALSYMBOLS  ON", LocalDebugSymbols.EnableLocalSymbols, f);
            RunCompilerDirective("LOCALSYMBOLS OFF", LocalDebugSymbols.DisableLocalSymbols, f);
            RunCompilerDirective("LOCALSYMBOLS KAPUTT", LocalDebugSymbols.Undefined, f, CompilerDirectiveParserErrors.InvalidLocalSymbolsDirective);
            RunCompilerDirective("LOCALSYMBOLS ", LocalDebugSymbols.Undefined, f, CompilerDirectiveParserErrors.InvalidLocalSymbolsDirective);
        }

        [Fact]
        public void TestLongStrings() {
            Func<object> f = () => CompilerOptions.LongStrings.Value;
            RunCompilerDirective("", LongStringTypes.Undefined, f);
            RunCompilerDirective("H+", LongStringTypes.EnableLongStrings, f);
            RunCompilerDirective("H-", LongStringTypes.DisableLongStrings, f);
            RunCompilerDirective("H 3", LongStringTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective);
            RunCompilerDirective("H", LongStringTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective);
            RunCompilerDirective("LONGSTRINGS  ON", LongStringTypes.EnableLongStrings, f);
            RunCompilerDirective("LONGSTRINGS  OFF", LongStringTypes.DisableLongStrings, f);
            RunCompilerDirective("LONGSTRINGS  KAPUTT", LongStringTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective);
            RunCompilerDirective("LONGSTRINGS  ", LongStringTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidLongStringSwitchDirective);
        }

        [Fact]
        public void TestOpenStrings() {
            Func<object> f = () => CompilerOptions.OpenStrings.Value;
            RunCompilerDirective("", OpenStringTypes.Undefined, f);
            RunCompilerDirective("P+", OpenStringTypes.EnableOpenStrings, f);
            RunCompilerDirective("P-", OpenStringTypes.DisableOpenStrings, f);
            RunCompilerDirective("P 3", OpenStringTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidOpenStringsDirective);
            RunCompilerDirective("P", OpenStringTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidOpenStringsDirective);
            RunCompilerDirective("OPENSTRINGS  ON", OpenStringTypes.EnableOpenStrings, f);
            RunCompilerDirective("OPENSTRINGS  OFF", OpenStringTypes.DisableOpenStrings, f);
            RunCompilerDirective("OPENSTRINGS  KAPUTT", OpenStringTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidOpenStringsDirective);
            RunCompilerDirective("OPENSTRINGS  ", OpenStringTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidOpenStringsDirective);
        }

        [Fact]
        public void TestOptimization() {
            Func<object> f = () => CompilerOptions.Optimization.Value;
            RunCompilerDirective("", CompilerOptmization.Undefined, f);
            RunCompilerDirective("O+", CompilerOptmization.EnableOptimization, f);
            RunCompilerDirective("O-", CompilerOptmization.DisableOptimization, f);
            RunCompilerDirective("O 4", CompilerOptmization.Undefined, f, CompilerDirectiveParserErrors.InvalidOptimizationDirective);
            RunCompilerDirective("O ", CompilerOptmization.Undefined, f, CompilerDirectiveParserErrors.InvalidOptimizationDirective);
            RunCompilerDirective("OPTIMIZATION  ON", CompilerOptmization.EnableOptimization, f);
            RunCompilerDirective("OPTIMIZATION  OFF", CompilerOptmization.DisableOptimization, f);
            RunCompilerDirective("OPTIMIZATION  KAPUTT", CompilerOptmization.Undefined, f, CompilerDirectiveParserErrors.InvalidOptimizationDirective);
            RunCompilerDirective("OPTIMIZATION  ", CompilerOptmization.Undefined, f, CompilerDirectiveParserErrors.InvalidOptimizationDirective);
        }

        [Fact]
        public void TestOverflow() {
            Func<object> f = () => CompilerOptions.CheckOverflows.Value;
            RunCompilerDirective("", RuntimeOverflowChecks.Undefined, f);
            RunCompilerDirective("Q+", RuntimeOverflowChecks.EnableChecks, f);
            RunCompilerDirective("Q-", RuntimeOverflowChecks.DisableChecks, f);
            RunCompilerDirective("Q 4", RuntimeOverflowChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective);
            RunCompilerDirective("Q ", RuntimeOverflowChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective);
            RunCompilerDirective("OVERFLOWCHECKS ON", RuntimeOverflowChecks.EnableChecks, f);
            RunCompilerDirective("OVERFLOWCHECKS OFF", RuntimeOverflowChecks.DisableChecks, f);
            RunCompilerDirective("OVERFLOWCHECKS KAPUTT", RuntimeOverflowChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective);
            RunCompilerDirective("OVERFLOWCHECKS ", RuntimeOverflowChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective);
        }

        [Fact]
        public void TestSaveDivide() {
            Func<object> f = () => CompilerOptions.SafeDivide.Value;
            RunCompilerDirective("", FDivSafeDivide.Undefined, f);
            RunCompilerDirective("U+", FDivSafeDivide.EnableSafeDivide, f);
            RunCompilerDirective("U-", FDivSafeDivide.DisableSafeDivide, f);
            RunCompilerDirective("U 3", FDivSafeDivide.Undefined, f, CompilerDirectiveParserErrors.InvalidSafeDivide);
            RunCompilerDirective("U", FDivSafeDivide.Undefined, f, CompilerDirectiveParserErrors.InvalidSafeDivide);
            RunCompilerDirective("SAFEDIVIDE  ON", FDivSafeDivide.EnableSafeDivide, f);
            RunCompilerDirective("SAFEDIVIDE  OFF", FDivSafeDivide.DisableSafeDivide, f);
            RunCompilerDirective("SAFEDIVIDE  KAPUTT", FDivSafeDivide.Undefined, f, CompilerDirectiveParserErrors.InvalidSafeDivide);
            RunCompilerDirective("SAFEDIVIDE  ", FDivSafeDivide.Undefined, f, CompilerDirectiveParserErrors.InvalidSafeDivide);
        }

        [Fact]
        public void TestRangeChecks() {
            Func<object> f = () => CompilerOptions.RangeChecks.Value;
            RunCompilerDirective("", RuntimeRangeChecks.Undefined, f);
            RunCompilerDirective("R+", RuntimeRangeChecks.EnableRangeChecks, f);
            RunCompilerDirective("R-", RuntimeRangeChecks.DisableRangeChecks, f);
            RunCompilerDirective("RANGECHECKS ON", RuntimeRangeChecks.EnableRangeChecks, f);
            RunCompilerDirective("RANGECHECKS OFF", RuntimeRangeChecks.DisableRangeChecks, f);
            RunCompilerDirective("RANGECHECKS KAPUTT", RuntimeRangeChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidRangeCheckDirective);
            RunCompilerDirective("RANGECHECKS ", RuntimeRangeChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidRangeCheckDirective);
        }

        [Fact]
        public void TestStackFrames() {
            Func<object> f = () => CompilerOptions.StackFrames.Value;
            RunCompilerDirective("", StackFrameGeneration.Undefined, f);
            RunCompilerDirective("W+", StackFrameGeneration.EnableFrames, f);
            RunCompilerDirective("W-", StackFrameGeneration.DisableFrames, f);
            RunCompilerDirective("W 3", StackFrameGeneration.Undefined, f, CompilerDirectiveParserErrors.InvalidStackFramesDirective);
            RunCompilerDirective("W", StackFrameGeneration.Undefined, f, CompilerDirectiveParserErrors.InvalidStackFramesDirective);
            RunCompilerDirective("STACKFRAMES  ON", StackFrameGeneration.EnableFrames, f);
            RunCompilerDirective("STACKFRAMES  OFF", StackFrameGeneration.DisableFrames, f);
            RunCompilerDirective("STACKFRAMES  KAPUTT", StackFrameGeneration.Undefined, f, CompilerDirectiveParserErrors.InvalidStackFramesDirective);
            RunCompilerDirective("STACKFRAMES  ", StackFrameGeneration.Undefined, f, CompilerDirectiveParserErrors.InvalidStackFramesDirective);
        }

        [Fact]
        public void TestZeroBasedStrings() {
            Func<object> f = () => CompilerOptions.IndexOfFirstCharInString.Value;
            RunCompilerDirective("", FirstCharIndex.Undefined, f);
            RunCompilerDirective("ZEROBASEDSTRINGS  ON", FirstCharIndex.IsZero, f);
            RunCompilerDirective("ZEROBASEDSTRINGS  OFF", FirstCharIndex.IsOne, f);
            RunCompilerDirective("ZEROBASEDSTRINGS  KAPUTT", FirstCharIndex.Undefined, f, CompilerDirectiveParserErrors.InvalidZeroBasedStringsDirective);
            RunCompilerDirective("ZEROBASEDSTRINGS  ", FirstCharIndex.Undefined, f, CompilerDirectiveParserErrors.InvalidZeroBasedStringsDirective);
        }

        [Fact]
        public void TestWritableConst() {
            Func<object> f = () => CompilerOptions.WritableConstants.Value;
            RunCompilerDirective("", ConstantValues.Undefined, f);
            RunCompilerDirective("J-", ConstantValues.Constant, f);
            RunCompilerDirective("J+", ConstantValues.Writable, f);
            RunCompilerDirective("J 3", ConstantValues.Undefined, f, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective);
            RunCompilerDirective("J", ConstantValues.Undefined, f, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective);
            RunCompilerDirective("WRITEABLECONST  OFF", ConstantValues.Constant, f);
            RunCompilerDirective("WRITEABLECONST  ON", ConstantValues.Writable, f);
            RunCompilerDirective("WRITEABLECONST  KAPUTT", ConstantValues.Undefined, f, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective);
            RunCompilerDirective("WRITEABLECONST  ", ConstantValues.Undefined, f, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective);
        }

        [Fact]
        public void TestWeakLinkRtti() {
            Func<object> f = () => CompilerOptions.WeakLinkRtti.Value;
            RunCompilerDirective("", RttiLinkMode.Undefined, f);
            RunCompilerDirective("WEAKLINKRTTI  ON", RttiLinkMode.LinkWeakRtti, f);
            RunCompilerDirective("WEAKLINKRTTI  OFF", RttiLinkMode.LinkFullRtti, f);
            RunCompilerDirective("WEAKLINKRTTI  KAPUTT", RttiLinkMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWeakLinkRttiDirective);
            RunCompilerDirective("WEAKLINKRTTI  ", RttiLinkMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWeakLinkRttiDirective);
        }

        [Fact]
        public void TestWeakPackageUnit() {
            RunCompilerDirective("", WeakPackaging.Undefined, () => CompilerOptions.WeakPackageUnit.Value);
            RunCompilerDirective("WEAKPACKAGEUNIT ON", WeakPackaging.Enable, () => CompilerOptions.WeakPackageUnit.Value);
            RunCompilerDirective("WEAKPACKAGEUNIT OFF", WeakPackaging.Disable, () => CompilerOptions.WeakPackageUnit.Value);
        }

        [Fact]
        public void TestWarnings() {
            Func<object> f = () => CompilerOptions.Warnings.Value;
            RunCompilerDirective("", CompilerWarnings.Undefined, f);
            RunCompilerDirective("WARNINGS  ON", CompilerWarnings.Enable, f);
            RunCompilerDirective("WARNINGS  OFF", CompilerWarnings.Disable, f);
            RunCompilerDirective("WARNINGS  KAPUTT", CompilerWarnings.Undefined, f, CompilerDirectiveParserErrors.InvalidWarningsDirective);
            RunCompilerDirective("WARNINGS  ", CompilerWarnings.Undefined, f, CompilerDirectiveParserErrors.InvalidWarningsDirective);
        }

        [Fact]
        public void TestWarn() {
            Func<object> f = () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED");
            RunCompilerDirective("", WarningMode.Undefined, f);
            RunCompilerDirective("WARN SYMBOL_DEPRECATED ON", WarningMode.On, f);
            RunCompilerDirective("WARN SYMBOL_DEPRECATED OFF", WarningMode.Off, f);
            RunCompilerDirective("WARN SYMBOL_DEPRECATED ERROR", WarningMode.Error, f);
            RunCompilerDirective("WARN SYMBOL_DEPRECATED DEFAULT", WarningMode.Undefined, f);
            RunCompilerDirective("WARN SYMBOL_DEPRECATED KAPUTT", WarningMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWarnDirective);
            RunCompilerDirective("WARN SYMBOL_DEPRECATED ", WarningMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWarnDirective);
            RunCompilerDirective("WARN KAPUTT ", WarningMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWarnDirective);
            RunCompilerDirective("WARN KAPUTT ERROR ", WarningMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWarnDirective);
            RunCompilerDirective("WARN KAPUTT KAPUTT ", WarningMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWarnDirective);
            RunCompilerDirective("WARN  ", WarningMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWarnDirective);
        }

        [Fact]
        public void TestVarStringChecks() {
            Func<object> f = () => CompilerOptions.VarStringChecks.Value;
            RunCompilerDirective("", ShortVarStringChecks.Undefined, f);
            RunCompilerDirective("V+", ShortVarStringChecks.EnableChecks, f);
            RunCompilerDirective("V-", ShortVarStringChecks.DisableChecks, f);
            RunCompilerDirective("V 3", ShortVarStringChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidStringCheckDirective);
            RunCompilerDirective("V", ShortVarStringChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidStringCheckDirective);
            RunCompilerDirective("VARSTRINGCHECKS ON", ShortVarStringChecks.EnableChecks, f);
            RunCompilerDirective("VARSTRINGCHECKS  OFF", ShortVarStringChecks.DisableChecks, f);
            RunCompilerDirective("VARSTRINGCHECKS  KAPUTT", ShortVarStringChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidStringCheckDirective);
            RunCompilerDirective("VARSTRINGCHECKS  ", ShortVarStringChecks.Undefined, f, CompilerDirectiveParserErrors.InvalidStringCheckDirective);
        }

        [Fact]
        public void TestTypeCheckedPointers() {
            Func<object> f = () => CompilerOptions.TypedPointers.Value;
            RunCompilerDirective("", TypeCheckedPointers.Undefined, f);
            RunCompilerDirective("T+", TypeCheckedPointers.Enable, f);
            RunCompilerDirective("T-", TypeCheckedPointers.Disable, f);
            RunCompilerDirective("T 3", TypeCheckedPointers.Undefined, f, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective);
            RunCompilerDirective("T", TypeCheckedPointers.Undefined, f, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective);
            RunCompilerDirective("TYPEDADDRESS ON", TypeCheckedPointers.Enable, f);
            RunCompilerDirective("TYPEDADDRESS OFF", TypeCheckedPointers.Disable, f);
            RunCompilerDirective("TYPEDADDRESS KAPUTT", TypeCheckedPointers.Undefined, f, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective);
            RunCompilerDirective("TYPEDADDRESS ", TypeCheckedPointers.Undefined, f, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective);
        }

        [Fact]
        public void TestSymbolDefinitionInfo() {
            Func<object> f = () => CompilerOptions.SymbolDefinitions.Value;
            RunCompilerDirective("", SymbolDefinitionInfo.Undefined, f);
            RunCompilerDirective("Y+", SymbolDefinitionInfo.Enable, f);
            RunCompilerDirective("Y-", SymbolDefinitionInfo.Disable, f);
            RunCompilerDirective("YD", SymbolDefinitionInfo.Enable, f);
            RunCompilerDirective("Y 3", SymbolDefinitionInfo.Undefined, f, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective);
            RunCompilerDirective("Y", SymbolDefinitionInfo.Undefined, f, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective);
            RunCompilerDirective("DEFINITIONINFO OFF", SymbolDefinitionInfo.Disable, f);
            RunCompilerDirective("DEFINITIONINFO ON", SymbolDefinitionInfo.Enable, f);
            RunCompilerDirective("DEFINITIONINFO KAPUTT", SymbolDefinitionInfo.Undefined, f, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective);
            RunCompilerDirective("DEFINITIONINFO ", SymbolDefinitionInfo.Undefined, f, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective);
        }

        [Fact]
        public void TestSymbolReferenceInfo() {
            Func<object> f = () => CompilerOptions.SymbolReferences.Value;
            RunCompilerDirective("", SymbolReferenceInfo.Undefined, f);
            RunCompilerDirective("Y-", SymbolReferenceInfo.Disable, f);
            RunCompilerDirective("Y+", SymbolReferenceInfo.Enable, f);
            RunCompilerDirective("YD", SymbolReferenceInfo.Disable, f);
            RunCompilerDirective("Y 3", SymbolReferenceInfo.Undefined, f, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective);
            RunCompilerDirective("Y", SymbolReferenceInfo.Undefined, f, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective);
            RunCompilerDirective("REFERENCEINFO ON", SymbolReferenceInfo.Enable, f);
            RunCompilerDirective("REFERENCEINFO OFF", SymbolReferenceInfo.Disable, f);
            RunCompilerDirective("REFERENCEINFO KAPUTT", SymbolReferenceInfo.Undefined, f, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective);
            RunCompilerDirective("REFERENCEINFO ", SymbolReferenceInfo.Undefined, f, CompilerDirectiveParserErrors.InvalidDefinitionInfoDirective);
        }

        [Fact]
        public void TestStrongLinking() {
            Func<object> f = () => CompilerOptions.LinkAllTypes.Value;
            RunCompilerDirective("", StrongTypeLinking.Undefined, f);
            RunCompilerDirective("STRONGLINKTYPES ON", StrongTypeLinking.Enable, f);
            RunCompilerDirective("STRONGLINKTYPES OFF", StrongTypeLinking.Disable, f);
        }

        [Fact]
        public void TestScopedEnums() {
            Func<object> f = () => CompilerOptions.ScopedEnums.Value;
            RunCompilerDirective("", RequireScopedEnums.Undefined, f);
            RunCompilerDirective("SCOPEDENUMS ON", RequireScopedEnums.Enable, f);
            RunCompilerDirective("SCOPEDENUMS OFF", RequireScopedEnums.Disable, f);
            RunCompilerDirective("SCOPEDENUMS KAPUTT", RequireScopedEnums.Undefined, f, CompilerDirectiveParserErrors.InvalidScopedEnumsDirective);
            RunCompilerDirective("SCOPEDENUMS ", RequireScopedEnums.Undefined, f, CompilerDirectiveParserErrors.InvalidScopedEnumsDirective);
        }

        [Fact]
        public void TestTypeInfo() {
            Func<object> f = () => CompilerOptions.PublishedRtti.Value;
            RunCompilerDirective("", RttiForPublishedProperties.Undefined, f);
            RunCompilerDirective("M+", RttiForPublishedProperties.Enable, f);
            RunCompilerDirective("M-", RttiForPublishedProperties.Disable, f);
            RunCompilerDirective("M X", RttiForPublishedProperties.Undefined, f, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective);
            RunCompilerDirective("M", RttiForPublishedProperties.Undefined, f, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective);
            RunCompilerDirective("TYPEINFO ON", RttiForPublishedProperties.Enable, f);
            RunCompilerDirective("TYPEINFO OFF", RttiForPublishedProperties.Disable, f);
            RunCompilerDirective("TYPEINFO KAPUTT", RttiForPublishedProperties.Undefined, f, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective);
            RunCompilerDirective("TYPEINFO ", RttiForPublishedProperties.Undefined, f, CompilerDirectiveParserErrors.InvalidPublishedRttiDirective);
        }

        [Fact]
        public void TestRunOnly() {
            Func<object> f = () => CompilerOptions.RuntimeOnlyPackage.Value;
            RunCompilerDirective("", RuntimePackageMode.Undefined, f);
            RunCompilerDirective("RUNONLY OFF", RuntimePackageMode.Standard, f);
            RunCompilerDirective("RUNONLY ON", RuntimePackageMode.RuntimeOnly, f);
            RunCompilerDirective("RUNONLY KAPUTT", RuntimePackageMode.Undefined, f, CompilerDirectiveParserErrors.InvalidRunOnlyDirective);
            RunCompilerDirective("RUNONLY ", RuntimePackageMode.Undefined, f, CompilerDirectiveParserErrors.InvalidRunOnlyDirective);
        }

        [Fact]
        public void TestLinkedFiles() {
            RunCompilerDirective("", false, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("L link.dll", true, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("LINK 'link.dll'", true, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void TestLegacyIfEnd() {
            Func<object> f = () => CompilerOptions.LegacyIfEnd.Value;
            RunCompilerDirective("", EndIfMode.Undefined, () => CompilerOptions.LegacyIfEnd.Value);
            RunCompilerDirective("LEGACYIFEND ON", EndIfMode.LegacyIfEnd, f);
            RunCompilerDirective("LEGACYIFEND OFF", EndIfMode.Standard, f);
            RunCompilerDirective("LEGACYIFEND KAPUTT", EndIfMode.Undefined, f, CompilerDirectiveParserErrors.InvalidLegacyIfEndDirective);
            RunCompilerDirective("LEGACYIFEND ", EndIfMode.Undefined, f, CompilerDirectiveParserErrors.InvalidLegacyIfEndDirective);
        }

        private void TestIfOpt(char opt) {
            RunCompilerDirective("IFOPT " + opt + "+ § DEFINE TT § ENDIF", false, () => ConditionalCompilation.Conditionals.Any(t => string.Equals(t.Name, "TT", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective(opt + "+ § IFOPT " + opt + "+ § DEFINE TT § ENDIF", true, () => ConditionalCompilation.Conditionals.Any(t => string.Equals(t.Name, "TT", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective(opt + "+ § IFOPT " + opt + "- § DEFINE TT § ENDIF", false, () => ConditionalCompilation.Conditionals.Any(t => string.Equals(t.Name, "TT", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective(opt + "- § IFOPT " + opt + "- § DEFINE TT § ENDIF", true, () => ConditionalCompilation.Conditionals.Any(t => string.Equals(t.Name, "TT", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective(opt + "- § IFOPT " + opt + "+ § DEFINE TT § ENDIF", false, () => ConditionalCompilation.Conditionals.Any(t => string.Equals(t.Name, "TT", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void TestIfOpt() {
            TestIfOpt('A');
            TestIfOpt('B');
            TestIfOpt('C');
            TestIfOpt('D');
            TestIfOpt('G');
            TestIfOpt('I');
            TestIfOpt('J');
            TestIfOpt('H');
            TestIfOpt('L');
            TestIfOpt('M');
            TestIfOpt('O');
            TestIfOpt('P');
            TestIfOpt('Q');
            TestIfOpt('R');
            TestIfOpt('T');
            TestIfOpt('U');
            TestIfOpt('V');
            TestIfOpt('W');
            TestIfOpt('X');
            TestIfOpt('Y');
            TestIfOpt('Z');
        }

        [Fact]
        public void TestRtti() {
            RunCompilerDirective("", RttiGenerationMode.Undefined, () => CompilerOptions.Rtti.Mode);

            RunCompilerDirective("RTTI INHERIT", //
                Tuple.Create(RttiGenerationMode.Inherit, new RttiForVisibility()), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods));

            RunCompilerDirective("RTTI INHERIT METHODS([])", //
                Tuple.Create(RttiGenerationMode.Inherit, new RttiForVisibility()), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods));

            RunCompilerDirective("RTTI INHERIT METHODS([vcPrivate])", //
                Tuple.Create(RttiGenerationMode.Inherit, new RttiForVisibility() { ForPrivate = true }), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods));

            RunCompilerDirective("RTTI INHERIT METHODS([vcPrivate, vcProtected])", //
                Tuple.Create(RttiGenerationMode.Inherit, new RttiForVisibility() { ForPrivate = true, ForProtected = true }), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods));

            RunCompilerDirective("RTTI INHERIT METHODS([vcPrivate, vcProtected, vcPublic])", //
                Tuple.Create(RttiGenerationMode.Inherit, new RttiForVisibility() { ForPrivate = true, ForProtected = true, ForPublic = true }), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods));

            RunCompilerDirective("RTTI INHERIT METHODS([vcPrivate, vcProtected, vcPublic, vcPublished])", //
                Tuple.Create(RttiGenerationMode.Inherit, new RttiForVisibility() { ForPrivate = true, ForProtected = true, ForPublic = true, ForPublished = true }), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods));

            RunCompilerDirective("RTTI EXPLICIT METHODS([vcPrivate]) PROPERTIES([vcPrivate])", //
                Tuple.Create(RttiGenerationMode.Explicit, new RttiForVisibility() { ForPrivate = true }, new RttiForVisibility() { ForPrivate = true }), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods, CompilerOptions.Rtti.Properties));

            RunCompilerDirective("RTTI EXPLICIT METHODS([vcPrivate]) PROPERTIES([vcPrivate]) FIELDS([vcPublic])", //
                Tuple.Create(RttiGenerationMode.Explicit, new RttiForVisibility() { ForPrivate = true }, new RttiForVisibility() { ForPrivate = true }, new RttiForVisibility() { ForPublic = true }), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods, CompilerOptions.Rtti.Properties, CompilerOptions.Rtti.Fields));

            RunCompilerDirective("RTTI EXPLICIT METHODS([]) PROPERTIES([]) FIELDS([])", //
                Tuple.Create(RttiGenerationMode.Explicit, new RttiForVisibility(), new RttiForVisibility(), new RttiForVisibility()), //
                () => Tuple.Create(CompilerOptions.Rtti.Mode, CompilerOptions.Rtti.Methods, CompilerOptions.Rtti.Properties, CompilerOptions.Rtti.Fields));
        }

        [Fact]
        public void TestResourceReference() {
            RunCompilerDirective("R Res ", true, () => Meta.ResourceReferences.Any(t => t.TargetPath.Path.EndsWith("res.res", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("R Res.Res ", true, () => Meta.ResourceReferences.Any(t => t.TargetPath.Path.EndsWith("res.res", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("R Res.Res Res.rc", true, () => Meta.ResourceReferences.Any(t => t.RcFile.EndsWith("res.rc", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("R *.Res ", true, () => Meta.ResourceReferences.Any(t => t.TargetPath.Path.EndsWith("test_0.res", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("RESOURCE Res ", true, () => Meta.ResourceReferences.Any(t => t.TargetPath.Path.EndsWith("res.res", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("RESOURCE Res.Res ", true, () => Meta.ResourceReferences.Any(t => t.TargetPath.Path.EndsWith("res.res", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("RESOURCE *.Res ", true, () => Meta.ResourceReferences.Any(t => t.TargetPath.Path.EndsWith("test_0.res", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("RESOURCE Res.Res Res.rc", true, () => Meta.ResourceReferences.Any(t => t.RcFile.EndsWith("res.rc", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void TestRegion() {
            RunCompilerDirective("", 0, () => Meta.Regions.Count);
            RunCompilerDirective("REGION", 1, () => Meta.Regions.Count);
            RunCompilerDirective("REGION 'XXX' ", "XXX", () => Meta.Regions.Peek());
            RunCompilerDirective("REGION § ENDREGION", 0, () => Meta.Regions.Count);
        }

        [Fact]
        public void TestRealCompatibility() {
            Func<object> f = () => CompilerOptions.RealCompatiblity.Value;
            RunCompilerDirective("", Real48.Undefined, f);
            RunCompilerDirective("REALCOMPATIBILITY ON", Real48.EnableCompatibility, f);
            RunCompilerDirective("REALCOMPATIBILITY OFF", Real48.DisableCompatibility, f);
            RunCompilerDirective("REALCOMPATIBILITY KAPUTT", Real48.Undefined, f, CompilerDirectiveParserErrors.InvalidRealCompatibilityMode);
            RunCompilerDirective("REALCOMPATIBILITY UNDEFINED", Real48.Undefined, f, CompilerDirectiveParserErrors.InvalidRealCompatibilityMode);
        }

        [Fact]
        public void TestPointerMath() {
            Func<object> f = () => CompilerOptions.PointerMath.Value;
            RunCompilerDirective("", PointerManipulation.Undefined, f);
            RunCompilerDirective("POINTERMATH ON", PointerManipulation.EnablePointerMath, f);
            RunCompilerDirective("POINTERMATH OFF", PointerManipulation.DisablePointerMath, f);
            RunCompilerDirective("POINTERMATH KAPUTT", PointerManipulation.Undefined, f, CompilerDirectiveParserErrors.InvalidPointerMathDirective);
            RunCompilerDirective("POINTERMATH ", PointerManipulation.Undefined, f, CompilerDirectiveParserErrors.InvalidPointerMathDirective);
        }

        [Fact]
        public void TestOldTypeLayout() {
            Func<object> f = () => CompilerOptions.OldTypeLayout.Value;
            RunCompilerDirective("", OldRecordTypes.Undefined, f);
            RunCompilerDirective("OLDTYPELAYOUT  ON", OldRecordTypes.EnableOldRecordPacking, f);
            RunCompilerDirective("OLDTYPELAYOUT  OFF", OldRecordTypes.DisableOldRecordPacking, f);
            RunCompilerDirective("OLDTYPELAYOUT  KAPUTT", OldRecordTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidOldTypeLayoutDirective);
            RunCompilerDirective("OLDTYPELAYOUT  ", OldRecordTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidOldTypeLayoutDirective);
        }

        [Fact]
        public void TestNoDefine() {
            Func<object> f = () => Meta.NoDefines.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase));
            Func<object> g = () => Meta.NoDefines.Any(t => t.HppName.StartsWith("baz", StringComparison.OrdinalIgnoreCase));
            Func<object> h = () => Meta.NoDefines.Any(t => t.UnionTypeName.StartsWith("fuz", StringComparison.OrdinalIgnoreCase));

            RunCompilerDirective("", false, f);
            RunCompilerDirective("NODEFINE TDEMO", true, f);
            RunCompilerDirective("NODEFINE TMIMOA", false, f);
            RunCompilerDirective("NODEFINE TMIMOA 'BAZ' ", true, g);
            RunCompilerDirective("NODEFINE TMIMOA 'BAZ' 'FUZZ'", true, h);
            RunCompilerDirective("NODEFINE 3 ", false, f, CompilerDirectiveParserErrors.InvalidNoDefineDirective);
            RunCompilerDirective("NODEFINE ", false, f, CompilerDirectiveParserErrors.InvalidNoDefineDirective);
        }

        [Fact]
        public void TestObjTypeName() {
            Func<object> f = () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase));
            RunCompilerDirective("", false, f);
            RunCompilerDirective("OBJTYPENAME tdemo", true, f);
            RunCompilerDirective("OBJTYPENAME tmemo", false, f);
            RunCompilerDirective("OBJTYPENAME tdemo 'Bul'", true, f);
            RunCompilerDirective("OBJTYPENAME tdemo 'Ntdemo'", true, f);
            RunCompilerDirective("OBJTYPENAME tdemo 'Xtdemo'", false, f, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
            RunCompilerDirective("OBJTYPENAME tdemo ''", false, f, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
            RunCompilerDirective("OBJTYPENAME ''", false, f, CompilerDirectiveParserErrors.InvalidObjTypeDirective);
        }

        [Fact]
        public void TestNoInclude() {
            Func<object> f = () => Meta.NoIncludes.Any(t => t.StartsWith("Winapi", StringComparison.OrdinalIgnoreCase));
            RunCompilerDirective("", false, f);
            RunCompilerDirective("NOINCLUDE WinApi.Messages", true, f);
            RunCompilerDirective("NOINCLUDE 3", false, f, CompilerDirectiveParserErrors.InvalidNoIncludeDirective);
            RunCompilerDirective("NOINCLUDE ", false, f, CompilerDirectiveParserErrors.InvalidNoIncludeDirective);
        }

        [Fact]
        public void TestMinEnumSize() {
            Func<object> f = () => CompilerOptions.MinumEnumSize.Value;
            RunCompilerDirective("", EnumSize.Undefined, f);
            RunCompilerDirective("Z+", EnumSize.FourByte, f);
            RunCompilerDirective("Z-", EnumSize.OneByte, f);
            RunCompilerDirective("Z1", EnumSize.OneByte, f);
            RunCompilerDirective("Z2", EnumSize.TwoByte, f);
            RunCompilerDirective("Z4", EnumSize.FourByte, f);
            RunCompilerDirective("Z 4", EnumSize.Undefined, f, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective);
            RunCompilerDirective("Z", EnumSize.Undefined, f, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective);
            RunCompilerDirective("MINENUMSIZE 1", EnumSize.OneByte, f);
            RunCompilerDirective("MINENUMSIZE 2", EnumSize.TwoByte, f);
            RunCompilerDirective("MINENUMSIZE 4", EnumSize.FourByte, f);
            RunCompilerDirective("MINENUMSIZE KAPUTT", EnumSize.Undefined, f, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective);
            RunCompilerDirective("MINENUMSIZE ", EnumSize.Undefined, f, CompilerDirectiveParserErrors.InvalidMinEnumSizeDirective);
        }

        [Fact]
        public void TestMethodInfo() {
            Func<object> f = () => CompilerOptions.MethodInfo.Value;
            RunCompilerDirective("", MethodInfoRtti.Undefined, f);
            RunCompilerDirective("METHODINFO ON", MethodInfoRtti.EnableMethodInfo, f);
            RunCompilerDirective("METHODINFO OFF", MethodInfoRtti.DisableMethodInfo, f);
            RunCompilerDirective("METHODINFO KAPUTT", MethodInfoRtti.Undefined, f, CompilerDirectiveParserErrors.InvalidMethodInfoDirective);
            RunCompilerDirective("METHODINFO ", MethodInfoRtti.Undefined, f, CompilerDirectiveParserErrors.InvalidMethodInfoDirective);
        }

        [Fact]
        public void TestLibMeta() {
            Func<object> f = () => Meta.LibPrefix.Value;
            Func<object> g = () => Meta.LibPrefix.Value;
            Func<object> h = () => Meta.LibVersion.Value;
            RunCompilerDirective("", null, f);
            RunCompilerDirective("", null, g);
            RunCompilerDirective("", null, h);
            RunCompilerDirective("LIBPREFIX 'P'", "P", f);
            RunCompilerDirective("LIBSUFFIX 'M'", "M", g);
            RunCompilerDirective("LIBVERSION 'V'", "V", h);
            RunCompilerDirective("LIBPREFIX KAPUTT", null, f, CompilerDirectiveParserErrors.InvalidLibDirective);
            RunCompilerDirective("LIBSUFFIX KAPUTT", null, g, CompilerDirectiveParserErrors.InvalidLibDirective);
            RunCompilerDirective("LIBVERSION KAPUTT", null, h, CompilerDirectiveParserErrors.InvalidLibDirective);
            RunCompilerDirective("LIBPREFIX ", null, f, CompilerDirectiveParserErrors.InvalidLibDirective);
            RunCompilerDirective("LIBSUFFIX ", null, g, CompilerDirectiveParserErrors.InvalidLibDirective);
            RunCompilerDirective("LIBVERSION ", null, h, CompilerDirectiveParserErrors.InvalidLibDirective);
        }

        [Fact]
        public void TestPeVersions() {
            RunCompilerDirective("", 0, () => Meta.PEOsVersion.MajorVersion.Value);
            RunCompilerDirective("SETPEOSVERSION 1.43", 1, () => Meta.PEOsVersion.MajorVersion.Value);
            RunCompilerDirective("SETPEOSVERSION 1.44", 44, () => Meta.PEOsVersion.MinorVersion.Value);
            RunCompilerDirective("", 0, () => Meta.PESubsystemVersion.MajorVersion.Value);
            RunCompilerDirective("SETPESUBSYSVERSION 2.43", 2, () => Meta.PESubsystemVersion.MajorVersion.Value);
            RunCompilerDirective("SETPESUBSYSVERSION 2.44", 44, () => Meta.PESubsystemVersion.MinorVersion.Value);
            RunCompilerDirective("", 0, () => Meta.PEUserVersion.MajorVersion.Value);
            RunCompilerDirective("SETPEUSERVERSION 4.43", 4, () => Meta.PEUserVersion.MajorVersion.Value);
            RunCompilerDirective("SETPEUSERVERSION 4.44", 44, () => Meta.PEUserVersion.MinorVersion.Value);

        }

    }
}
