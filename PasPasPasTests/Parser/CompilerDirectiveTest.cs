using System;
using System.Linq;
using PasPasPas.Options.Bundles;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using Xunit;

namespace PasPasPasTests.Parser {

    public class CompilerDirectiveTest : ParserTestBase {

        [Fact]
        public void TestAlign() {
            object f(OptionSet o) => o.CompilerOptions.Align.Value;
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
            object f(OptionSet o) => o.CompilerOptions.ApplicationType.Value;
            RunCompilerDirective("", AppType.Undefined, f);
            RunCompilerDirective("APPTYPE GUI", AppType.Gui, f);
            RunCompilerDirective("APPTYPE CONSOLE", AppType.Console, f);
            RunCompilerDirective("APPTYPE 17", AppType.Undefined, f, CompilerDirectiveParserErrors.InvalidApplicationType);
            RunCompilerDirective("APPTYPE UNDIFINED", AppType.Undefined, f, CompilerDirectiveParserErrors.InvalidApplicationType);
            RunCompilerDirective("APPTYPE", AppType.Undefined, f, CompilerDirectiveParserErrors.InvalidApplicationType);
        }

        [Fact]
        public void TestMessage() {
            object f(OptionSet o) => true;
            RunCompilerDirective("", true, f);
            RunCompilerDirective("MESSAGE 'X'", true, f, ParserBase.UserGeneratedMessage);
            RunCompilerDirective("MESSAGE Hint 'X' ", true, f, ParserBase.UserGeneratedMessage);
            RunCompilerDirective("MESSAGE Warn 'X' ", true, f, ParserBase.UserGeneratedMessage);
            RunCompilerDirective("MESSAGE Error 'X'", true, f, ParserBase.UserGeneratedMessage);
            RunCompilerDirective("MESSAGE Fatal 'x'", true, f, ParserBase.UserGeneratedMessage);
            RunCompilerDirective("MESSAGE hint KAPUTT ", true, f, CompilerDirectiveParserErrors.InvalidMessageDirective);
            RunCompilerDirective("MESSAGE KAPUTT ", true, f, CompilerDirectiveParserErrors.InvalidMessageDirective);
            RunCompilerDirective("MESSAGE ", true, f, CompilerDirectiveParserErrors.InvalidMessageDirective);
        }

        [Fact]
        public void TestMemStackSize() {
            object mi(OptionSet o) => o.CompilerOptions.MinimumStackMemorySize.Value;
            object ma(OptionSet o) => o.CompilerOptions.MaximumStackMemorySize.Value;
            RunCompilerDirective("", 0UL, ma);
            RunCompilerDirective("", 0UL, mi);
            RunCompilerDirective("M 100, 200", 200UL, ma);
            RunCompilerDirective("M 100, 200", 100UL, mi);
            RunCompilerDirective("MINSTACKSIZE 300", 300UL, mi);
            RunCompilerDirective("MAXSTACKSIZE 400", 400UL, ma);
            RunCompilerDirective("MAXSTACKSIZE KAPUTT", 0UL, ma, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective);
            RunCompilerDirective("MAXSTACKSIZE ", 0UL, ma, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective);
            RunCompilerDirective("MINSTACKSIZE KAPUTT", 0UL, ma, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective);
            RunCompilerDirective("MINSTACKSIZE ", 0UL, ma, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective);
            RunCompilerDirective("M 1, KAPUTT ", 0UL, ma, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective);
            RunCompilerDirective("M 1 KAPUTT ", 0UL, ma, CompilerDirectiveParserErrors.InvalidStackMemorySizeDirective);
        }

        [Fact]
        public void TestBoolEvalSwitch() {
            object f(OptionSet o) => o.CompilerOptions.BoolEval.Value;
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
            object f(OptionSet o) => o.CompilerOptions.CodeAlign.Value;
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
            object f(OptionSet o) => o.ConditionalCompilation.IsSymbolDefined("TESTSYM");
            object g(OptionSet o) => o.ConditionalCompilation.IsSymbolDefined("PASPASPAS_TEST");
            object h(OptionSet o) => o.ConditionalCompilation.IsSymbolDefined("A");
            object b(OptionSet o) => o.ConditionalCompilation.IsSymbolDefined("B");
            object c(OptionSet o) => o.ConditionalCompilation.IsSymbolDefined("32BIT");

            RunCompilerDirective("", false, f);
            RunCompilerDirective("DEFINE 32BIT", true, c);
            RunCompilerDirective("IFDEF TESTSYM § IFDEF 32BIT § ELSE § DEFINE A  § ENDIF § ENDIF", false, h);
            RunCompilerDirective("IFDEF TESTSYM § DEFINE B § ELSE § DEFINE A § ENDIF", true, h);
            RunCompilerDirective("IFDEF TESTSYM § IFDEF TESTSYM § DEFINE A § ELSE § DEFINE A § ENDIF § ENDIF", false, h);
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
            object f(OptionSet o) => o.CompilerOptions.Assertions.Value;
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
            object f(OptionSet o) => o.CompilerOptions.DebugInfo.Value;
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
            object f(OptionSet o) => o.ConditionalCompilation.DenyInPackages.Value;
            RunCompilerDirective("", DenyUnitInPackage.Undefined, f);
            RunCompilerDirective("DENYPACKAGEUNIT ON", DenyUnitInPackage.DenyUnit, f);
            RunCompilerDirective("DENYPACKAGEUNIT OFF", DenyUnitInPackage.AllowUnit, f);
            RunCompilerDirective("DENYPACKAGEUNIT KAPUTT", DenyUnitInPackage.Undefined, f, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective);
            RunCompilerDirective("DENYPACKAGEUNIT", DenyUnitInPackage.Undefined, f, CompilerDirectiveParserErrors.InvalidDenyPackageUnitDirective);
        }

        [Fact]
        public void TestDescription() {
            object f(OptionSet o) => o.Meta.Description.Value;
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
            object f(OptionSet o) => o.ConditionalCompilation.DesignOnly.Value;
            RunCompilerDirective("", DesignOnlyUnit.Undefined, f);
            RunCompilerDirective("DESIGNONLY ON", DesignOnlyUnit.InDesignTimeOnly, f);
            RunCompilerDirective("DESIGNONLY OFF", DesignOnlyUnit.AllTimes, f);
            RunCompilerDirective("DESIGNONLY KAPUTT", DesignOnlyUnit.Undefined, f, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective);
            RunCompilerDirective("DESIGNONLY", DesignOnlyUnit.Undefined, f, CompilerDirectiveParserErrors.InvalidDesignTimeOnlyDirective);
        }

        [Fact]
        public void TestExtensionsSwitch() {
            object f(OptionSet o) => o.Meta.FileExtension.Value;
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
            object f(OptionSet o) => o.CompilerOptions.ExportCppObjects.Value;
            RunCompilerDirective("", ExportCppObjects.Undefined, f);
            RunCompilerDirective("OBJEXPORTALL ON", ExportCppObjects.ExportAll, f);
            RunCompilerDirective("OBJEXPORTALL OFF", ExportCppObjects.DoNotExportAll, f);
            RunCompilerDirective("OBJEXPORTALL KAPUTT", ExportCppObjects.Undefined, f, CompilerDirectiveParserErrors.InvalidObjectExportDirective);
            RunCompilerDirective("OBJEXPORTALL", ExportCppObjects.Undefined, f, CompilerDirectiveParserErrors.InvalidObjectExportDirective);
        }

        [Fact]
        public void TestExtendedCompatibility() {
            object f(OptionSet o) => o.CompilerOptions.ExtendedCompatibility.Value;
            RunCompilerDirective("", ExtendedCompatibilityMode.Undefined, f);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY ON", ExtendedCompatibilityMode.Enabled, f);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY OFF", ExtendedCompatibilityMode.Disabled, f);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY KAPUTT", ExtendedCompatibilityMode.Undefined, f, CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY", ExtendedCompatibilityMode.Undefined, f, CompilerDirectiveParserErrors.InvalidExtendedCompatibilityDirective);
        }

        [Fact]
        public void TestExtendedSyntax() {
            object f(OptionSet o) => o.CompilerOptions.UseExtendedSyntax.Value;
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
            object c(OptionSet o) => o.Meta.ExternalSymbols.Count;
            object f(OptionSet o) => o.Meta.ExternalSymbols.Any(t => string.Equals(t.IdentifierName, "dummy", StringComparison.Ordinal));
            RunCompilerDirective("", 0, c);
            RunCompilerDirective("EXTERNALSYM dummy", true, f);
            RunCompilerDirective("EXTERNALSYM dummy 'a'", true, f);
            RunCompilerDirective("EXTERNALSYM dummy 'a' 'q'", true, f);
            RunCompilerDirective("EXTERNALSYM+", 0, c, CompilerDirectiveParserErrors.InvalidExternalSymbolDirective);
            RunCompilerDirective("EXTERNALSYM", 0, c, CompilerDirectiveParserErrors.InvalidExternalSymbolDirective);
        }

        [Fact]
        public void TestExcessPrecision() {
            object f(OptionSet o) => o.CompilerOptions.ExcessPrecision.Value;
            RunCompilerDirective("", ExcessPrecisionForResult.Undefined, f);
            RunCompilerDirective("EXCESSPRECISION  ON", ExcessPrecisionForResult.EnableExcess, f);
            RunCompilerDirective("EXCESSPRECISION  OFF", ExcessPrecisionForResult.DisableExcess, f);
            RunCompilerDirective("EXCESSPRECISION  KAPUTT", ExcessPrecisionForResult.Undefined, f, CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective);
            RunCompilerDirective("EXCESSPRECISION  ", ExcessPrecisionForResult.Undefined, f, CompilerDirectiveParserErrors.InvalidExcessPrecisionDirective);
        }

        [Fact]
        public void TestHighCharUnicode() {
            object f(OptionSet o) => o.CompilerOptions.HighCharUnicode.Value;
            RunCompilerDirective("", HighCharsUnicode.Undefined, f);
            RunCompilerDirective("HIGHCHARUNICODE OFF", HighCharsUnicode.DisableHighChars, f);
            RunCompilerDirective("HIGHCHARUNICODE ON", HighCharsUnicode.EnableHighChars, f);
            RunCompilerDirective("HIGHCHARUNICODE KAPUTT", HighCharsUnicode.Undefined, f, CompilerDirectiveParserErrors.InvalidHighCharUnicodeDirective);
            RunCompilerDirective("HIGHCHARUNICODE ", HighCharsUnicode.Undefined, f, CompilerDirectiveParserErrors.InvalidHighCharUnicodeDirective);
        }

        [Fact]
        public void TestHints() {
            object f(OptionSet o) => o.CompilerOptions.Hints.Value;
            RunCompilerDirective("", CompilerHint.Undefined, f);
            RunCompilerDirective("HINTS ON", CompilerHint.EnableHints, f);
            RunCompilerDirective("HINTS OFF", CompilerHint.DisableHints, f);
            RunCompilerDirective("HINTS KAPUTT", CompilerHint.Undefined, f, CompilerDirectiveParserErrors.InvalidHintsDirective);
            RunCompilerDirective("HINTS ", CompilerHint.Undefined, f, CompilerDirectiveParserErrors.InvalidHintsDirective);
        }

        [Fact]
        public void TestHppEmit() {
            object c(OptionSet o) => o.Meta.HeaderStrings.Count;
            object f(OptionSet o) => o.Meta.HeaderStrings.Any(t => string.Equals(t.Value, "'dummy'", StringComparison.InvariantCulture));
            Func<OptionSet, object> g(HppEmitMode x) => (OptionSet o) => o.Meta.HeaderStrings.Any(t => t.Mode == x);
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
            object f(OptionSet o) => o.CompilerOptions.ImageBase.Value;
            RunCompilerDirective("", 0UL, f);
            RunCompilerDirective("IMAGEBASE $40000000 ", 0x40000000UL, f);
            RunCompilerDirective("IMAGEBASE 40000000 ", 40000000UL, f);
            RunCompilerDirective("IMAGEBASE KAPUTT ", 0UL, f, CompilerDirectiveParserErrors.InvalidImageBaseDirective);
            RunCompilerDirective("IMAGEBASE ", 0UL, f, CompilerDirectiveParserErrors.InvalidImageBaseDirective);
        }

        [Fact]
        public void TestImplicitBuild() {
            object f(OptionSet o) => o.CompilerOptions.ImplicitBuild.Value;
            RunCompilerDirective("", ImplicitBuildUnit.Undefined, f);
            RunCompilerDirective("IMPLICITBUILD ON", ImplicitBuildUnit.EnableImplicitBuild, f);
            RunCompilerDirective("IMPLICITBUILD OFF", ImplicitBuildUnit.DisableImplicitBuild, f);
            RunCompilerDirective("IMPLICITBUILD KAPUTT", ImplicitBuildUnit.Undefined, f, CompilerDirectiveParserErrors.InvalidImplicitBuildDirective);
            RunCompilerDirective("IMPLICITBUILD ", ImplicitBuildUnit.Undefined, f, CompilerDirectiveParserErrors.InvalidImplicitBuildDirective);
        }

        [Fact]
        public void TestImportUnitData() {
            object f(OptionSet o) => o.CompilerOptions.ImportedData.Value;
            RunCompilerDirective("", ImportGlobalUnitData.Undefined, f);
            RunCompilerDirective("G+", ImportGlobalUnitData.DoImport, f);
            RunCompilerDirective("G-", ImportGlobalUnitData.NoImport, f);
            RunCompilerDirective("G 3", ImportGlobalUnitData.Undefined, f, CompilerDirectiveParserErrors.InvalidImportedDataDirective);
            RunCompilerDirective("G", ImportGlobalUnitData.Undefined, f, CompilerDirectiveParserErrors.InvalidImportedDataDirective);

            RunCompilerDirective("IMPORTEDDATA  ON", ImportGlobalUnitData.DoImport, f);
            RunCompilerDirective("IMPORTEDDATA OFF", ImportGlobalUnitData.NoImport, f);
            RunCompilerDirective("IMPORTEDDATA  KAPUTT", ImportGlobalUnitData.Undefined, f, CompilerDirectiveParserErrors.InvalidImportedDataDirective);
            RunCompilerDirective("IMPORTEDDATA ", ImportGlobalUnitData.Undefined, f, CompilerDirectiveParserErrors.InvalidImportedDataDirective);
        }

        [Fact]
        public void TestInclude() {
            object f(OptionSet o) => o.ConditionalCompilation.IsSymbolDefined("DUMMY_INC");
            RunCompilerDirective("", false, f);
            RunCompilerDirective("INCLUDE 'DUMMY.INC'", true, f);
            RunCompilerDirective("INCLUDE 3", false, f, CompilerDirectiveParserErrors.InvalidIncludeDirective, CompilerDirectiveParserErrors.InvalidFileName);
            RunCompilerDirective("INCLUDE ", false, f, CompilerDirectiveParserErrors.InvalidIncludeDirective, CompilerDirectiveParserErrors.InvalidFileName);
            RunCompilerDirective("", false, f);
            RunCompilerDirective("I 'DUMMY.INC'", true, f);
            RunCompilerDirective("I DUMMY.INC", true, f);
            RunCompilerDirective("I 3", false, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
            RunCompilerDirective("I ", false, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
        }

        [Fact]
        public void TestIoChecks() {
            object f(OptionSet o) => o.CompilerOptions.IoChecks.Value;
            RunCompilerDirective("", IoCallCheck.Undefined, f);
            RunCompilerDirective("I+", IoCallCheck.EnableIoChecks, f);
            RunCompilerDirective("I-", IoCallCheck.DisableIoChecks, f);
            RunCompilerDirective("I 3", IoCallCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
            RunCompilerDirective("I", IoCallCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
            RunCompilerDirective("IOCHECKS  ON", IoCallCheck.EnableIoChecks, f);
            RunCompilerDirective("IOCHECKS OFF", IoCallCheck.DisableIoChecks, f);
            RunCompilerDirective("IOCHECKS KAPUTT", IoCallCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
            RunCompilerDirective("IOCHECKS ", IoCallCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidIoChecksDirective);
        }

        [Fact]
        public void TestLocalSymbols() {
            object f(OptionSet o) => o.CompilerOptions.LocalSymbols.Value;
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
            object f(OptionSet o) => o.CompilerOptions.LongStrings.Value;
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
            object f(OptionSet o) => o.CompilerOptions.OpenStrings.Value;
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
            object f(OptionSet o) => o.CompilerOptions.Optimization.Value;
            RunCompilerDirective("", CompilerOptimization.Undefined, f);
            RunCompilerDirective("O+", CompilerOptimization.EnableOptimization, f);
            RunCompilerDirective("O-", CompilerOptimization.DisableOptimization, f);
            RunCompilerDirective("O 4", CompilerOptimization.Undefined, f, CompilerDirectiveParserErrors.InvalidOptimizationDirective);
            RunCompilerDirective("O ", CompilerOptimization.Undefined, f, CompilerDirectiveParserErrors.InvalidOptimizationDirective);
            RunCompilerDirective("OPTIMIZATION  ON", CompilerOptimization.EnableOptimization, f);
            RunCompilerDirective("OPTIMIZATION  OFF", CompilerOptimization.DisableOptimization, f);
            RunCompilerDirective("OPTIMIZATION  KAPUTT", CompilerOptimization.Undefined, f, CompilerDirectiveParserErrors.InvalidOptimizationDirective);
            RunCompilerDirective("OPTIMIZATION  ", CompilerOptimization.Undefined, f, CompilerDirectiveParserErrors.InvalidOptimizationDirective);
        }

        [Fact]
        public void TestOverflow() {
            object f(OptionSet o) => o.CompilerOptions.CheckOverflows.Value;
            RunCompilerDirective("", RuntimeOverflowCheck.Undefined, f);
            RunCompilerDirective("Q+", RuntimeOverflowCheck.EnableChecks, f);
            RunCompilerDirective("Q-", RuntimeOverflowCheck.DisableChecks, f);
            RunCompilerDirective("Q 4", RuntimeOverflowCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective);
            RunCompilerDirective("Q ", RuntimeOverflowCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective);
            RunCompilerDirective("OVERFLOWCHECKS ON", RuntimeOverflowCheck.EnableChecks, f);
            RunCompilerDirective("OVERFLOWCHECKS OFF", RuntimeOverflowCheck.DisableChecks, f);
            RunCompilerDirective("OVERFLOWCHECKS KAPUTT", RuntimeOverflowCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective);
            RunCompilerDirective("OVERFLOWCHECKS ", RuntimeOverflowCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidOverflowCheckDirective);
        }

        [Fact]
        public void TestSaveDivide() {
            object f(OptionSet o) => o.CompilerOptions.SafeDivide.Value;
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
            object f(OptionSet o) => o.CompilerOptions.RangeChecks.Value;
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
            object f(OptionSet o) => o.CompilerOptions.StackFrames.Value;
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
            object f(OptionSet o) => o.CompilerOptions.IndexOfFirstCharInString.Value;
            RunCompilerDirective("", FirstCharIndex.Undefined, f);
            RunCompilerDirective("ZEROBASEDSTRINGS  ON", FirstCharIndex.IsZero, f);
            RunCompilerDirective("ZEROBASEDSTRINGS  OFF", FirstCharIndex.IsOne, f);
            RunCompilerDirective("ZEROBASEDSTRINGS  KAPUTT", FirstCharIndex.Undefined, f, CompilerDirectiveParserErrors.InvalidZeroBasedStringsDirective);
            RunCompilerDirective("ZEROBASEDSTRINGS  ", FirstCharIndex.Undefined, f, CompilerDirectiveParserErrors.InvalidZeroBasedStringsDirective);
        }

        [Fact]
        public void TestWritableConst() {
            object f(OptionSet o) => o.CompilerOptions.WritableConstants.Value;
            RunCompilerDirective("", ConstantValue.Undefined, f);
            RunCompilerDirective("J-", ConstantValue.Constant, f);
            RunCompilerDirective("J+", ConstantValue.Writable, f);
            RunCompilerDirective("J 3", ConstantValue.Undefined, f, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective);
            RunCompilerDirective("J", ConstantValue.Undefined, f, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective);
            RunCompilerDirective("WRITEABLECONST  OFF", ConstantValue.Constant, f);
            RunCompilerDirective("WRITEABLECONST  ON", ConstantValue.Writable, f);
            RunCompilerDirective("WRITEABLECONST  KAPUTT", ConstantValue.Undefined, f, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective);
            RunCompilerDirective("WRITEABLECONST  ", ConstantValue.Undefined, f, CompilerDirectiveParserErrors.InvalidWritableConstantsDirective);
        }

        [Fact]
        public void TestWeakLinkRtti() {
            object f(OptionSet o) => o.CompilerOptions.WeakLinkRtti.Value;
            RunCompilerDirective("", RttiLinkMode.Undefined, f);
            RunCompilerDirective("WEAKLINKRTTI  ON", RttiLinkMode.LinkWeakRtti, f);
            RunCompilerDirective("WEAKLINKRTTI  OFF", RttiLinkMode.LinkFullRtti, f);
            RunCompilerDirective("WEAKLINKRTTI  KAPUTT", RttiLinkMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWeakLinkRttiDirective);
            RunCompilerDirective("WEAKLINKRTTI  ", RttiLinkMode.Undefined, f, CompilerDirectiveParserErrors.InvalidWeakLinkRttiDirective);
        }

        [Fact]
        public void TestWeakPackageUnit() {
            object f(OptionSet o) => o.CompilerOptions.WeakPackageUnit.Value;
            RunCompilerDirective("", WeakPackaging.Undefined, f);
            RunCompilerDirective("WEAKPACKAGEUNIT ON", WeakPackaging.Enable, f);
            RunCompilerDirective("WEAKPACKAGEUNIT OFF", WeakPackaging.Disable, f);
            RunCompilerDirective("WEAKPACKAGEUNIT KAPUTT", WeakPackaging.Undefined, f, CompilerDirectiveParserErrors.InvalidWeakPackageUnitDirective);
            RunCompilerDirective("WEAKPACKAGEUNIT", WeakPackaging.Undefined, f, CompilerDirectiveParserErrors.InvalidWeakPackageUnitDirective);
        }

        [Fact]
        public void TestWarnings() {
            object f(OptionSet o) => o.CompilerOptions.Warnings.Value;
            RunCompilerDirective("", CompilerWarning.Undefined, f);
            RunCompilerDirective("WARNINGS  ON", CompilerWarning.Enable, f);
            RunCompilerDirective("WARNINGS  OFF", CompilerWarning.Disable, f);
            RunCompilerDirective("WARNINGS  KAPUTT", CompilerWarning.Undefined, f, CompilerDirectiveParserErrors.InvalidWarningsDirective);
            RunCompilerDirective("WARNINGS  ", CompilerWarning.Undefined, f, CompilerDirectiveParserErrors.InvalidWarningsDirective);
        }

        [Fact]
        public void TestWarn() {
            object f(OptionSet o) => o.Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED");
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
            object f(OptionSet o) => o.CompilerOptions.VarStringChecks.Value;
            RunCompilerDirective("", ShortVarStringCheck.Undefined, f);
            RunCompilerDirective("V+", ShortVarStringCheck.EnableChecks, f);
            RunCompilerDirective("V-", ShortVarStringCheck.DisableChecks, f);
            RunCompilerDirective("V 3", ShortVarStringCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidStringCheckDirective);
            RunCompilerDirective("V", ShortVarStringCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidStringCheckDirective);
            RunCompilerDirective("VARSTRINGCHECKS ON", ShortVarStringCheck.EnableChecks, f);
            RunCompilerDirective("VARSTRINGCHECKS  OFF", ShortVarStringCheck.DisableChecks, f);
            RunCompilerDirective("VARSTRINGCHECKS  KAPUTT", ShortVarStringCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidStringCheckDirective);
            RunCompilerDirective("VARSTRINGCHECKS  ", ShortVarStringCheck.Undefined, f, CompilerDirectiveParserErrors.InvalidStringCheckDirective);
        }

        [Fact]
        public void TestTypeCheckedPointers() {
            object f(OptionSet o) => o.CompilerOptions.TypedPointers.Value;
            RunCompilerDirective("", UsePointersWithTypeChecking.Undefined, f);
            RunCompilerDirective("T+", UsePointersWithTypeChecking.Enable, f);
            RunCompilerDirective("T-", UsePointersWithTypeChecking.Disable, f);
            RunCompilerDirective("T 3", UsePointersWithTypeChecking.Undefined, f, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective);
            RunCompilerDirective("T", UsePointersWithTypeChecking.Undefined, f, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective);
            RunCompilerDirective("TYPEDADDRESS ON", UsePointersWithTypeChecking.Enable, f);
            RunCompilerDirective("TYPEDADDRESS OFF", UsePointersWithTypeChecking.Disable, f);
            RunCompilerDirective("TYPEDADDRESS KAPUTT", UsePointersWithTypeChecking.Undefined, f, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective);
            RunCompilerDirective("TYPEDADDRESS ", UsePointersWithTypeChecking.Undefined, f, CompilerDirectiveParserErrors.InvalidTypeCheckedPointersDirective);
        }

        [Fact]
        public void TestSymbolDefinitionInfo() {
            object f(OptionSet o) => o.CompilerOptions.SymbolDefinitions.Value;
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
            object f(OptionSet o) => o.CompilerOptions.SymbolReferences.Value;
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
            object f(OptionSet o) => o.CompilerOptions.LinkAllTypes.Value;
            RunCompilerDirective("", StrongTypeLinking.Undefined, f);
            RunCompilerDirective("STRONGLINKTYPES ON", StrongTypeLinking.Enable, f);
            RunCompilerDirective("STRONGLINKTYPES OFF", StrongTypeLinking.Disable, f);
        }

        [Fact]
        public void TestScopedEnums() {
            object f(OptionSet o) => o.CompilerOptions.ScopedEnums.Value;
            RunCompilerDirective("", RequireScopedEnums.Undefined, f);
            RunCompilerDirective("SCOPEDENUMS ON", RequireScopedEnums.Enable, f);
            RunCompilerDirective("SCOPEDENUMS OFF", RequireScopedEnums.Disable, f);
            RunCompilerDirective("SCOPEDENUMS KAPUTT", RequireScopedEnums.Undefined, f, CompilerDirectiveParserErrors.InvalidScopedEnumsDirective);
            RunCompilerDirective("SCOPEDENUMS ", RequireScopedEnums.Undefined, f, CompilerDirectiveParserErrors.InvalidScopedEnumsDirective);
        }

        [Fact]
        public void TestTypeInfo() {
            object f(OptionSet o) => o.CompilerOptions.PublishedRtti.Value;
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
            object f(OptionSet o) => o.CompilerOptions.RuntimeOnlyPackage.Value;
            RunCompilerDirective("", RuntimePackageMode.Undefined, f);
            RunCompilerDirective("RUNONLY OFF", RuntimePackageMode.Standard, f);
            RunCompilerDirective("RUNONLY ON", RuntimePackageMode.RuntimeOnly, f);
            RunCompilerDirective("RUNONLY KAPUTT", RuntimePackageMode.Undefined, f, CompilerDirectiveParserErrors.InvalidRunOnlyDirective);
            RunCompilerDirective("RUNONLY ", RuntimePackageMode.Undefined, f, CompilerDirectiveParserErrors.InvalidRunOnlyDirective);
        }

        [Fact]
        public void TestLinkedFiles() {
            object f(OptionSet o) => o.Meta.LinkedFiles.Where(t => t.TargetPath != null).Select(t => t.TargetPath).Any(t => t.FileName.IndexOf("link.dll", StringComparison.OrdinalIgnoreCase) >= 0);
            RunCompilerDirective("", false, f);
            RunCompilerDirective("L link.dll", true, f);
            RunCompilerDirective("LINK 'link.dll'", true, f);
            RunCompilerDirective("LINK 3", false, f, CompilerDirectiveParserErrors.InvalidLinkDirective, CompilerDirectiveParserErrors.InvalidFileName);
            RunCompilerDirective("LINK ", false, f, CompilerDirectiveParserErrors.InvalidLinkDirective, CompilerDirectiveParserErrors.InvalidFileName);
        }

        [Fact]
        public void TestLegacyIfEnd() {
            object f(OptionSet o) => o.CompilerOptions.LegacyIfEnd.Value;
            RunCompilerDirective("", EndIfMode.Undefined, (OptionSet o) => o.CompilerOptions.LegacyIfEnd.Value);
            RunCompilerDirective("LEGACYIFEND ON", EndIfMode.LegacyIfEnd, f);
            RunCompilerDirective("LEGACYIFEND OFF", EndIfMode.Standard, f);
            RunCompilerDirective("LEGACYIFEND KAPUTT", EndIfMode.Undefined, f, CompilerDirectiveParserErrors.InvalidLegacyIfEndDirective);
            RunCompilerDirective("LEGACYIFEND ", EndIfMode.Undefined, f, CompilerDirectiveParserErrors.InvalidLegacyIfEndDirective);
        }

        private void TestIfOptHelper(char opt) {
            object f(OptionSet o) => o.ConditionalCompilation.Conditionals.Any(t => string.Equals(t.Name, "TT", StringComparison.OrdinalIgnoreCase));
            RunCompilerDirective("IFOPT " + opt + "+ § DEFINE TT § ENDIF", false, f);
            RunCompilerDirective(opt + "+ § IFOPT " + opt + "+ § DEFINE TT § ENDIF", true, f);
            RunCompilerDirective(opt + "+ § IFOPT " + opt + "- § DEFINE TT § ENDIF", false, f);
            RunCompilerDirective(opt + "- § IFOPT " + opt + "- § DEFINE TT § ENDIF", true, f);
            RunCompilerDirective(opt + "- § IFOPT " + opt + "+ § DEFINE TT § ENDIF", false, f);
        }

        [Fact]
        public void TestIfOpt() {
            TestIfOptHelper('A');
            TestIfOptHelper('B');
            TestIfOptHelper('C');
            TestIfOptHelper('D');
            TestIfOptHelper('G');
            TestIfOptHelper('I');
            TestIfOptHelper('J');
            TestIfOptHelper('H');
            TestIfOptHelper('L');
            TestIfOptHelper('M');
            TestIfOptHelper('O');
            TestIfOptHelper('P');
            TestIfOptHelper('Q');
            TestIfOptHelper('R');
            TestIfOptHelper('T');
            TestIfOptHelper('U');
            TestIfOptHelper('V');
            TestIfOptHelper('W');
            TestIfOptHelper('X');
            TestIfOptHelper('Y');
            TestIfOptHelper('Z');
        }

        //[Fact(Skip = "options changed")]
        internal void TestRtti(OptionSet o1) {

            var i = RttiGenerationMode.Inherit;
            var e = RttiGenerationMode.Explicit;
            var u = RttiGenerationMode.Undefined;
            object[] p(RttiGenerationMode _) => new object[] { _, new RttiForVisibility() };
            object[] q(RttiGenerationMode _) => new object[] { _, new RttiForVisibility() { ForPrivate = true } };
            object[] r(RttiGenerationMode _) => new object[] { _, new RttiForVisibility() { ForPrivate = true, ForProtected = true } };
            object[] s(RttiGenerationMode _) => new object[] { _, new RttiForVisibility() { ForPrivate = true, ForProtected = true, ForPublic = true } };
            object[] t(RttiGenerationMode _) => new object[] { _, new RttiForVisibility() { ForPrivate = true, ForProtected = true, ForPublic = true, ForPublished = true } };
            RttiForVisibility[] l(OptionSet o) => new[] { o.CompilerOptions.Rtti.Methods, o.CompilerOptions.Rtti.Fields, o.CompilerOptions.Rtti.Properties };
            var k = new[] { "METHODS", "FIELDS", "PROPERTIES" };
            object m(OptionSet o) => o.CompilerOptions.Rtti.Mode;
            Func<OptionSet, object> n(RttiForVisibility _) => (OptionSet o2) => new object[] { o2.CompilerOptions.Rtti.Mode, _ };

            RunCompilerDirective("", RttiGenerationMode.Undefined, m);

            for (var idx = 0; idx < k.Length; idx++) {
                var _ = l(o1)[idx];
                RunCompilerDirective("RTTI INHERIT", p(i), n(_));
                RunCompilerDirective("RTTI INHERIT " + k[idx] + "([])", p(i), n(_));
                RunCompilerDirective("RTTI INHERIT " + k[idx] + "([vcPrivate])", q(i), n(_));
                RunCompilerDirective("RTTI INHERIT " + k[idx] + "([vcPrivate, vcProtected])", r(i), n(_));
                RunCompilerDirective("RTTI INHERIT " + k[idx] + "([vcPrivate, vcProtected, vcPublic])", s(i), n(_));
                RunCompilerDirective("RTTI INHERIT " + k[idx] + "([vcPrivate, vcProtected, vcPublic, vcPublished])", t(i), n(_));

                RunCompilerDirective("RTTI EXPLICIT", p(e), n(_));
                RunCompilerDirective("RTTI EXPLICIT " + k[idx] + "([])", p(e), n(_));
                RunCompilerDirective("RTTI EXPLICIT " + k[idx] + "([vcPrivate])", q(e), n(_));
                RunCompilerDirective("RTTI EXPLICIT " + k[idx] + "([vcPrivate, vcProtected])", r(e), n(_));
                RunCompilerDirective("RTTI EXPLICIT " + k[idx] + "([vcPrivate, vcProtected, vcPublic])", s(e), n(_));
                RunCompilerDirective("RTTI EXPLICIT " + k[idx] + "([vcPrivate, vcProtected, vcPublic, vcPublished])", t(e), n(_));
            }

            RunCompilerDirective("RTTI INHERIT METHODS([KAPUTT", p(u), n(l(o1)[0]), CompilerDirectiveParserErrors.InvalidRttiDirective);
            RunCompilerDirective("RTTI EXPLICIT METHODS([KAPUTT", p(u), n(l(o1)[0]), CompilerDirectiveParserErrors.InvalidRttiDirective);
            RunCompilerDirective("RTTI INHERIT METHODS(KAPUTT", p(u), n(l(o1)[0]), CompilerDirectiveParserErrors.InvalidRttiDirective);
            RunCompilerDirective("RTTI EXPLICIT METHODS(KAPUTT", p(u), n(l(o1)[0]), CompilerDirectiveParserErrors.InvalidRttiDirective);
            RunCompilerDirective("RTTI INHERIT METHODS KAPUTT", p(u), n(l(o1)[0]), CompilerDirectiveParserErrors.InvalidRttiDirective);
            RunCompilerDirective("RTTI EXPLICIT METHODS KAPUTT", p(u), n(l(o1)[0]), CompilerDirectiveParserErrors.InvalidRttiDirective);
            RunCompilerDirective("RTTI KAPUTT", p(u), n(l(o1)[0]), CompilerDirectiveParserErrors.InvalidRttiDirective);
            RunCompilerDirective("RTTI ", p(u), n(l(o1)[0]), CompilerDirectiveParserErrors.InvalidRttiDirective);
        }

        [Fact]
        public void TestResourceReference() {

            object f(OptionSet o) => o.Meta.ResourceReferences.Any(t => t.TargetPath.Path.EndsWith("res.res", StringComparison.OrdinalIgnoreCase));
            object g(OptionSet o) => o.Meta.ResourceReferences.Any(t => t.RcFile.EndsWith("res.rc", StringComparison.OrdinalIgnoreCase));
            object h(OptionSet o) => o.Meta.ResourceReferences.Any(t => t.TargetPath.Path.EndsWith("test_0.res", StringComparison.OrdinalIgnoreCase));

            RunCompilerDirective("R Res ", true, f);
            RunCompilerDirective("R Res.Res ", true, f);
            RunCompilerDirective("R Res.Res Res.rc", true, g);
            RunCompilerDirective("R *.Res ", true, h);
            RunCompilerDirective("R 3 ", false, h, CompilerDirectiveParserErrors.InvalidResourceDirective, CompilerDirectiveParserErrors.InvalidFileName);
            RunCompilerDirective("R  ", false, h, CompilerDirectiveParserErrors.InvalidResourceDirective, CompilerDirectiveParserErrors.InvalidFileName);
            RunCompilerDirective("RESOURCE Res ", true, f);
            RunCompilerDirective("RESOURCE Res.Res ", true, f);
            RunCompilerDirective("RESOURCE *.Res ", true, h);
            RunCompilerDirective("RESOURCE Res.Res Res.rc", true, g);
            RunCompilerDirective("RESOURCE 3 ", false, h, CompilerDirectiveParserErrors.InvalidResourceDirective, CompilerDirectiveParserErrors.InvalidFileName);
            RunCompilerDirective("RESOURCE  ", false, h, CompilerDirectiveParserErrors.InvalidResourceDirective, CompilerDirectiveParserErrors.InvalidFileName);
        }

        [Fact]
        public void TestRegion() {
            object f(OptionSet o) => o.Meta.Regions.Count;
            RunCompilerDirective("", 0, f);
            RunCompilerDirective("REGION", 0, f, CompilerDirectiveParserErrors.InvalidRegionDirective);
            RunCompilerDirective("REGION 'XXX' | DEFINE Q  ", 0, f, OptionSet.PendingRegion);
            RunCompilerDirective("REGION 'XXX' § ENDREGION", 0, f);
            RunCompilerDirective("ENDREGION", 0, f, CompilerDirectiveParserErrors.EndRegionWithoutRegion);
        }

        [Fact]
        public void TestRealCompatibility() {
            object f(OptionSet o) => o.CompilerOptions.RealCompatibility.Value;
            RunCompilerDirective("", Real48.Undefined, f);
            RunCompilerDirective("REALCOMPATIBILITY ON", Real48.EnableCompatibility, f);
            RunCompilerDirective("REALCOMPATIBILITY OFF", Real48.DisableCompatibility, f);
            RunCompilerDirective("REALCOMPATIBILITY KAPUTT", Real48.Undefined, f, CompilerDirectiveParserErrors.InvalidRealCompatibilityMode);
            RunCompilerDirective("REALCOMPATIBILITY UNDEFINED", Real48.Undefined, f, CompilerDirectiveParserErrors.InvalidRealCompatibilityMode);
        }

        [Fact]
        public void TestPointerMath() {
            object f(OptionSet o) => o.CompilerOptions.PointerMath.Value;
            RunCompilerDirective("", PointerManipulation.Undefined, f);
            RunCompilerDirective("POINTERMATH ON", PointerManipulation.EnablePointerMath, f);
            RunCompilerDirective("POINTERMATH OFF", PointerManipulation.DisablePointerMath, f);
            RunCompilerDirective("POINTERMATH KAPUTT", PointerManipulation.Undefined, f, CompilerDirectiveParserErrors.InvalidPointerMathDirective);
            RunCompilerDirective("POINTERMATH ", PointerManipulation.Undefined, f, CompilerDirectiveParserErrors.InvalidPointerMathDirective);
        }

        [Fact]
        public void TestOldTypeLayout() {
            object f(OptionSet o) => o.CompilerOptions.OldTypeLayout.Value;
            RunCompilerDirective("", OldRecordTypes.Undefined, f);
            RunCompilerDirective("OLDTYPELAYOUT  ON", OldRecordTypes.EnableOldRecordPacking, f);
            RunCompilerDirective("OLDTYPELAYOUT  OFF", OldRecordTypes.DisableOldRecordPacking, f);
            RunCompilerDirective("OLDTYPELAYOUT  KAPUTT", OldRecordTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidOldTypeLayoutDirective);
            RunCompilerDirective("OLDTYPELAYOUT  ", OldRecordTypes.Undefined, f, CompilerDirectiveParserErrors.InvalidOldTypeLayoutDirective);
        }

        [Fact]
        public void TestNoDefine() {
            object f(OptionSet o) => o.Meta.NoDefines.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase));
            object g(OptionSet o) => o.Meta.NoDefines.Any(t => t.HppName.StartsWith("baz", StringComparison.OrdinalIgnoreCase));
            object h(OptionSet o) => o.Meta.NoDefines.Any(t => t.UnionTypeName.StartsWith("fuz", StringComparison.OrdinalIgnoreCase));

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
            object f(OptionSet o) => o.Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase));
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
            object f(OptionSet o) => o.Meta.NoIncludes.Any(t => t.StartsWith("Winapi", StringComparison.OrdinalIgnoreCase));
            RunCompilerDirective("", false, f);
            RunCompilerDirective("NOINCLUDE WinApi.Messages", true, f);
            RunCompilerDirective("NOINCLUDE 3", false, f, CompilerDirectiveParserErrors.InvalidNoIncludeDirective);
            RunCompilerDirective("NOINCLUDE ", false, f, CompilerDirectiveParserErrors.InvalidNoIncludeDirective);
        }

        [Fact]
        public void TestMinEnumSize() {
            object f(OptionSet o) => o.CompilerOptions.MinimumEnumSize.Value;
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
            object f(OptionSet o) => o.CompilerOptions.MethodInfo.Value;
            RunCompilerDirective("", MethodInfoRtti.Undefined, f);
            RunCompilerDirective("METHODINFO ON", MethodInfoRtti.EnableMethodInfo, f);
            RunCompilerDirective("METHODINFO OFF", MethodInfoRtti.DisableMethodInfo, f);
            RunCompilerDirective("METHODINFO KAPUTT", MethodInfoRtti.Undefined, f, CompilerDirectiveParserErrors.InvalidMethodInfoDirective);
            RunCompilerDirective("METHODINFO ", MethodInfoRtti.Undefined, f, CompilerDirectiveParserErrors.InvalidMethodInfoDirective);
        }

        [Fact]
        public void TestLibMeta() {
            object f(OptionSet o) => o.Meta.LibPrefix.Value;
            object g(OptionSet o) => o.Meta.LibSuffix.Value;
            object h(OptionSet o) => o.Meta.LibVersion.Value;
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
            Func<OptionSet, object> ma = (OptionSet o) => o.Meta.PEOsVersion.MajorVersion.Value;
            Func<OptionSet, object> mi = (OptionSet o) => o.Meta.PEOsVersion.MinorVersion.Value;
            RunCompilerDirective("", 0, ma);
            RunCompilerDirective("", 0, mi);
            RunCompilerDirective("SETPEOSVERSION 1.43", 1, ma);
            RunCompilerDirective("SETPEOSVERSION 1.44", 44, mi);
            RunCompilerDirective("SETPEOSVERSION 1", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            RunCompilerDirective("SETPEOSVERSION KAPUTT", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            RunCompilerDirective("SETPEOSVERSION ", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);

            ma = (OptionSet o) => o.Meta.PESubsystemVersion.MajorVersion.Value;
            mi = (OptionSet o) => o.Meta.PESubsystemVersion.MinorVersion.Value;
            RunCompilerDirective("", 0, ma);
            RunCompilerDirective("", 0, mi);
            RunCompilerDirective("SETPESUBSYSVERSION 2.43", 2, ma);
            RunCompilerDirective("SETPESUBSYSVERSION 2.44", 44, mi);
            RunCompilerDirective("SETPESUBSYSVERSION 1", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            RunCompilerDirective("SETPESUBSYSVERSION KAPUTT", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            RunCompilerDirective("SETPESUBSYSVERSION ", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);


            ma = (OptionSet o) => o.Meta.PEUserVersion.MajorVersion.Value;
            mi = (OptionSet o) => o.Meta.PEUserVersion.MinorVersion.Value;
            RunCompilerDirective("", 0, ma);
            RunCompilerDirective("", 0, mi);
            RunCompilerDirective("SETPEUSERVERSION 4.43", 4, ma);
            RunCompilerDirective("SETPEUSERVERSION 4.44", 44, mi);
            RunCompilerDirective("SETPEUSERVERSION 1", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            RunCompilerDirective("SETPEUSERVERSION KAPUTT", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
            RunCompilerDirective("SETPEUSERVERSION ", 0, ma, CompilerDirectiveParserErrors.InvalidPEVersionDirective);
        }

    }
}
