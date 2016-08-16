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
            RunCompilerDirective("", null, () => Meta.FileExtension.Value);
            RunCompilerDirective("EXTENSION ddd", "ddd", () => Meta.FileExtension.Value);
            RunCompilerDirective("E ddd'", "ddd", () => Meta.FileExtension.Value);
        }

        [Fact]
        public void TestObjExportAll() {
            RunCompilerDirective("", ExportCppObjects.Undefined, () => CompilerOptions.ExportCppObjects.Value);
            RunCompilerDirective("OBJEXPORTALL ON", ExportCppObjects.ExportAll, () => CompilerOptions.ExportCppObjects.Value);
            RunCompilerDirective("OBJEXPORTALL OFF", ExportCppObjects.DoNotExportAll, () => CompilerOptions.ExportCppObjects.Value);
        }

        [Fact]
        public void TestExtendedCompatibility() {
            RunCompilerDirective("", ExtendedCompatiblityMode.Undefined, () => CompilerOptions.ExtendedCompatibility.Value);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY ON", ExtendedCompatiblityMode.Enabled, () => CompilerOptions.ExtendedCompatibility.Value);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY OFF", ExtendedCompatiblityMode.Disabled, () => CompilerOptions.ExtendedCompatibility.Value);
        }

        [Fact]
        public void TestExtendedSyntax() {
            RunCompilerDirective("", ExtendedSyntax.Undefined, () => CompilerOptions.UseExtendedSyntax.Value);
            RunCompilerDirective("X+", ExtendedSyntax.UseExtendedSyntax, () => CompilerOptions.UseExtendedSyntax.Value);
            RunCompilerDirective("X-", ExtendedSyntax.NoExtendedSyntax, () => CompilerOptions.UseExtendedSyntax.Value);
            RunCompilerDirective("EXTENDEDSYNTAX ON", ExtendedSyntax.UseExtendedSyntax, () => CompilerOptions.UseExtendedSyntax.Value);
            RunCompilerDirective("EXTENDEDSYNTAX OFF", ExtendedSyntax.NoExtendedSyntax, () => CompilerOptions.UseExtendedSyntax.Value);
        }

        [Fact]
        public void TestExternalSymbol() {
            RunCompilerDirective("", 0, () => Meta.ExternalSymbols.Count);
            RunCompilerDirective("EXTERNALSYM dummy", true, () => Meta.ExternalSymbols.Any(t => string.Equals(t.IdentifierName, "dummy")));
            RunCompilerDirective("EXTERNALSYM dummy 'a'", true, () => Meta.ExternalSymbols.Any(t => string.Equals(t.IdentifierName, "dummy")));
            RunCompilerDirective("EXTERNALSYM dummy 'a' 'q'", true, () => Meta.ExternalSymbols.Any(t => string.Equals(t.IdentifierName, "dummy")));
        }

        [Fact]
        public void TestExcessPrecision() {
            RunCompilerDirective("", ExcessPrecisionForResults.Undefined, () => CompilerOptions.ExcessPrecision.Value);
            RunCompilerDirective("EXCESSPRECISION  ON", ExcessPrecisionForResults.EnableExcess, () => CompilerOptions.ExcessPrecision.Value);
            RunCompilerDirective("EXCESSPRECISION  OFF", ExcessPrecisionForResults.DisableExcess, () => CompilerOptions.ExcessPrecision.Value);
        }

        [Fact]
        public void TestHighCharUnicode() {
            RunCompilerDirective("", HighCharsUnicode.Undefined, () => CompilerOptions.HighCharUnicode.Value);
            RunCompilerDirective("HIGHCHARUNICODE OFF", HighCharsUnicode.DisableHighChars, () => CompilerOptions.HighCharUnicode.Value);
            RunCompilerDirective("HIGHCHARUNICODE ON", HighCharsUnicode.EnableHighChars, () => CompilerOptions.HighCharUnicode.Value);
        }

        [Fact]
        public void TestHints() {
            RunCompilerDirective("", CompilerHints.Undefined, () => CompilerOptions.Hints.Value);
            RunCompilerDirective("HINTS ON", CompilerHints.EnableHints, () => CompilerOptions.Hints.Value);
            RunCompilerDirective("HINTS OFF", CompilerHints.DisableHints, () => CompilerOptions.Hints.Value);
        }

        [Fact]
        public void TestHppEmit() {
            RunCompilerDirective("", 0, () => Meta.HeaderStrings.Count);
            RunCompilerDirective("HPPEMIT 'dummy'", true, () => Meta.HeaderStrings.Any(t => string.Equals(t.Value, "'dummy'")));
            RunCompilerDirective("HPPEMIT END 'dummy'", true, () => Meta.HeaderStrings.Any(t => string.Equals(t.Value, "'dummy'")));
            RunCompilerDirective("HPPEMIT LINKUNIT", true, () => Meta.HeaderStrings.Any(t => t.Mode == HppEmitMode.LinkUnit));
            RunCompilerDirective("HPPEMIT OPENNAMESPACE", true, () => Meta.HeaderStrings.Any(t => t.Mode == HppEmitMode.OpenNamespace));
            RunCompilerDirective("HPPEMIT CLOSENAMESPACE", true, () => Meta.HeaderStrings.Any(t => t.Mode == HppEmitMode.CloseNamespace));
            RunCompilerDirective("HPPEMIT NOUSINGNAMESPACE", true, () => Meta.HeaderStrings.Any(t => t.Mode == HppEmitMode.NoUsingNamespace));
        }

        [Fact]
        public void TestImageBase() {
            RunCompilerDirective("IMAGEBASE $40000000 ", 0x40000000, () => CompilerOptions.ImageBase.Value);
            RunCompilerDirective("IMAGEBASE 40000000 ", 40000000, () => CompilerOptions.ImageBase.Value);
        }

        [Fact]
        public void TestImplicitBuild() {
            RunCompilerDirective("", ImplicitBuildUnit.Undefined, () => CompilerOptions.ImplicitBuild.Value);
            RunCompilerDirective("IMPLICITBUILD ON", ImplicitBuildUnit.EnableImplicitBuild, () => CompilerOptions.ImplicitBuild.Value);
            RunCompilerDirective("IMPLICITBUILD OFF", ImplicitBuildUnit.DisableImplicitBuild, () => CompilerOptions.ImplicitBuild.Value);
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
            RunCompilerDirective("", false, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
            RunCompilerDirective("INCLUDE 'DUMMY.INC'", true, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
            RunCompilerDirective("", false, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
            RunCompilerDirective("I 'DUMMY.INC'", true, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
            RunCompilerDirective("I DUMMY.INC", true, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
        }

        [Fact]
        public void TestIoChecks() {
            RunCompilerDirective("", IoCallChecks.Undefined, () => CompilerOptions.IoChecks.Value);
            RunCompilerDirective("I+", IoCallChecks.EnableIoChecks, () => CompilerOptions.IoChecks.Value);
            RunCompilerDirective("I-", IoCallChecks.DisableIoChecks, () => CompilerOptions.IoChecks.Value);
            RunCompilerDirective("IOCHECKS  ON", IoCallChecks.EnableIoChecks, () => CompilerOptions.IoChecks.Value);
            RunCompilerDirective("IOCHECKS OFF", IoCallChecks.DisableIoChecks, () => CompilerOptions.IoChecks.Value);
        }

        [Fact]
        public void TestLocalSymbols() {
            RunCompilerDirective("", LocalDebugSymbols.Undefined, () => CompilerOptions.LocalSymbols.Value);
            RunCompilerDirective("L+", LocalDebugSymbols.EnableLocalSymbols, () => CompilerOptions.LocalSymbols.Value);
            RunCompilerDirective("L-", LocalDebugSymbols.DisableLocalSymbols, () => CompilerOptions.LocalSymbols.Value);
            RunCompilerDirective("LOCALSYMBOLS  ON", LocalDebugSymbols.EnableLocalSymbols, () => CompilerOptions.LocalSymbols.Value);
            RunCompilerDirective("LOCALSYMBOLS OFF", LocalDebugSymbols.DisableLocalSymbols, () => CompilerOptions.LocalSymbols.Value);
        }

        [Fact]
        public void TestLongStrings() {
            RunCompilerDirective("", LongStringTypes.Undefined, () => CompilerOptions.LongStrings.Value);
            RunCompilerDirective("H+", LongStringTypes.EnableLongStrings, () => CompilerOptions.LongStrings.Value);
            RunCompilerDirective("H-", LongStringTypes.DisableLongStrings, () => CompilerOptions.LongStrings.Value);
            RunCompilerDirective("LONGSTRINGS  ON", LongStringTypes.EnableLongStrings, () => CompilerOptions.LongStrings.Value);
            RunCompilerDirective("LONGSTRINGS  OFF", LongStringTypes.DisableLongStrings, () => CompilerOptions.LongStrings.Value);
        }

        [Fact]
        public void TestOpenStrings() {
            RunCompilerDirective("", OpenStringTypes.Undefined, () => CompilerOptions.OpenStrings.Value);
            RunCompilerDirective("P+", OpenStringTypes.EnableOpenStrings, () => CompilerOptions.OpenStrings.Value);
            RunCompilerDirective("P-", OpenStringTypes.DisableOpenStrings, () => CompilerOptions.OpenStrings.Value);
            RunCompilerDirective("OPENSTRINGS  ON", OpenStringTypes.EnableOpenStrings, () => CompilerOptions.OpenStrings.Value);
            RunCompilerDirective("OPENSTRINGS  OFF", OpenStringTypes.DisableOpenStrings, () => CompilerOptions.OpenStrings.Value);
        }

        [Fact]
        public void TestOptimization() {
            RunCompilerDirective("", CompilerOptmization.Undefined, () => CompilerOptions.Optimization.Value);
            RunCompilerDirective("O+", CompilerOptmization.EnableOptimization, () => CompilerOptions.Optimization.Value);
            RunCompilerDirective("O-", CompilerOptmization.DisableOptimization, () => CompilerOptions.Optimization.Value);
            RunCompilerDirective("OPTIMIZATION  ON", CompilerOptmization.EnableOptimization, () => CompilerOptions.Optimization.Value);
            RunCompilerDirective("OPTIMIZATION  OFF", CompilerOptmization.DisableOptimization, () => CompilerOptions.Optimization.Value);
        }

        [Fact]
        public void TestOverflow() {
            RunCompilerDirective("", RuntimeOverflowChecks.Undefined, () => CompilerOptions.CheckOverflows.Value);
            RunCompilerDirective("Q+", RuntimeOverflowChecks.EnableChecks, () => CompilerOptions.CheckOverflows.Value);
            RunCompilerDirective("Q-", RuntimeOverflowChecks.DisableChecks, () => CompilerOptions.CheckOverflows.Value);
            RunCompilerDirective("OVERFLOWCHECKS ON", RuntimeOverflowChecks.EnableChecks, () => CompilerOptions.CheckOverflows.Value);
            RunCompilerDirective("OVERFLOWCHECKS OFF", RuntimeOverflowChecks.DisableChecks, () => CompilerOptions.CheckOverflows.Value);
        }

        [Fact]
        public void TestSaveDivide() {
            RunCompilerDirective("", FDivSafeDivide.Undefined, () => CompilerOptions.SafeDivide.Value);
            RunCompilerDirective("U+", FDivSafeDivide.EnableSafeDivide, () => CompilerOptions.SafeDivide.Value);
            RunCompilerDirective("U-", FDivSafeDivide.DisableSafeDivide, () => CompilerOptions.SafeDivide.Value);
            RunCompilerDirective("SAFEDIVIDE  ON", FDivSafeDivide.EnableSafeDivide, () => CompilerOptions.SafeDivide.Value);
            RunCompilerDirective("SAFEDIVIDE  OFF", FDivSafeDivide.DisableSafeDivide, () => CompilerOptions.SafeDivide.Value);
        }

        [Fact]
        public void TestRangeChecks() {
            RunCompilerDirective("", RuntimeRangeChecks.Undefined, () => CompilerOptions.RangeChecks.Value);
            RunCompilerDirective("R+", RuntimeRangeChecks.EnableRangeChecks, () => CompilerOptions.RangeChecks.Value);
            RunCompilerDirective("R-", RuntimeRangeChecks.DisableRangeChecks, () => CompilerOptions.RangeChecks.Value);
            RunCompilerDirective("RANGECHECKS ON", RuntimeRangeChecks.EnableRangeChecks, () => CompilerOptions.RangeChecks.Value);
            RunCompilerDirective("RANGECHECKS OFF", RuntimeRangeChecks.DisableRangeChecks, () => CompilerOptions.RangeChecks.Value);
        }

        [Fact]
        public void TestStackFrames() {
            RunCompilerDirective("", StackFrameGeneration.Undefined, () => CompilerOptions.StackFrames.Value);
            RunCompilerDirective("W+", StackFrameGeneration.EnableFrames, () => CompilerOptions.StackFrames.Value);
            RunCompilerDirective("W-", StackFrameGeneration.DisableFrames, () => CompilerOptions.StackFrames.Value);
            RunCompilerDirective("STACKFRAMES  ON", StackFrameGeneration.EnableFrames, () => CompilerOptions.StackFrames.Value);
            RunCompilerDirective("STACKFRAMES  OFF", StackFrameGeneration.DisableFrames, () => CompilerOptions.StackFrames.Value);
        }

        [Fact]
        public void TestZeroBasedStrings() {
            RunCompilerDirective("", FirstCharIndex.Undefined, () => CompilerOptions.IndexOfFirstCharInString.Value);
            RunCompilerDirective("ZEROBASEDSTRINGS  ON", FirstCharIndex.IsZero, () => CompilerOptions.IndexOfFirstCharInString.Value);
            RunCompilerDirective("ZEROBASEDSTRINGS  OFF", FirstCharIndex.IsOne, () => CompilerOptions.IndexOfFirstCharInString.Value);
        }

        [Fact]
        public void TestWritableConst() {
            RunCompilerDirective("", ConstantValues.Undefined, () => CompilerOptions.WritableConstants.Value);
            RunCompilerDirective("J-", ConstantValues.Constant, () => CompilerOptions.WritableConstants.Value);
            RunCompilerDirective("J+", ConstantValues.Writable, () => CompilerOptions.WritableConstants.Value);
            RunCompilerDirective("WRITEABLECONST  OFF", ConstantValues.Constant, () => CompilerOptions.WritableConstants.Value);
            RunCompilerDirective("WRITEABLECONST  ON", ConstantValues.Writable, () => CompilerOptions.WritableConstants.Value);
        }

        [Fact]
        public void TestWeakLinkRtti() {
            RunCompilerDirective("", RttiLinkMode.Undefined, () => CompilerOptions.WeakLinkRtti.Value);
            RunCompilerDirective("WEAKLINKRTTI  ON", RttiLinkMode.LinkWeakRtti, () => CompilerOptions.WeakLinkRtti.Value);
            RunCompilerDirective("WEAKLINKRTTI  OFF", RttiLinkMode.LinkFullRtti, () => CompilerOptions.WeakLinkRtti.Value);
        }

        [Fact]
        public void TestWeakPackageUnit() {
            RunCompilerDirective("", WeakPackaging.Undefined, () => CompilerOptions.WeakPackageUnit.Value);
            RunCompilerDirective("WEAKPACKAGEUNIT ON", WeakPackaging.Enable, () => CompilerOptions.WeakPackageUnit.Value);
            RunCompilerDirective("WEAKPACKAGEUNIT OFF", WeakPackaging.Disable, () => CompilerOptions.WeakPackageUnit.Value);
        }

        [Fact]
        public void TestWarnings() {
            RunCompilerDirective("", CompilerWarnings.Undefined, () => CompilerOptions.Warnings.Value);
            RunCompilerDirective("WARNINGS  ON", CompilerWarnings.Enable, () => CompilerOptions.Warnings.Value);
            RunCompilerDirective("WARNINGS  OFF", CompilerWarnings.Disable, () => CompilerOptions.Warnings.Value);
        }

        [Fact]
        public void TestWarn() {
            RunCompilerDirective("", WarningMode.Undefined, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
            RunCompilerDirective("WARN SYMBOL_DEPRECATED ON", WarningMode.On, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
            RunCompilerDirective("WARN SYMBOL_DEPRECATED OFF", WarningMode.Off, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
            RunCompilerDirective("WARN SYMBOL_DEPRECATED ERROR", WarningMode.Error, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
            RunCompilerDirective("WARN SYMBOL_DEPRECATED DEFAULT", WarningMode.Undefined, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
        }

        [Fact]
        public void TestVarStringChecks() {
            RunCompilerDirective("", ShortVarStringChecks.Undefined, () => CompilerOptions.VarStringChecks.Value);
            RunCompilerDirective("V+", ShortVarStringChecks.EnableChecks, () => CompilerOptions.VarStringChecks.Value);
            RunCompilerDirective("V-", ShortVarStringChecks.DisableChecks, () => CompilerOptions.VarStringChecks.Value);
            RunCompilerDirective("VARSTRINGCHECKS ON", ShortVarStringChecks.EnableChecks, () => CompilerOptions.VarStringChecks.Value);
            RunCompilerDirective("VARSTRINGCHECKS  OFF", ShortVarStringChecks.DisableChecks, () => CompilerOptions.VarStringChecks.Value);
        }

        [Fact]
        public void TestTypeCheckedPointers() {
            RunCompilerDirective("", TypeCheckedPointers.Undefined, () => CompilerOptions.TypedPointers.Value);
            RunCompilerDirective("T+", TypeCheckedPointers.Enable, () => CompilerOptions.TypedPointers.Value);
            RunCompilerDirective("T-", TypeCheckedPointers.Disable, () => CompilerOptions.TypedPointers.Value);
            RunCompilerDirective("TYPEDADDRESS ON", TypeCheckedPointers.Enable, () => CompilerOptions.TypedPointers.Value);
            RunCompilerDirective("TYPEDADDRESS OFF", TypeCheckedPointers.Disable, () => CompilerOptions.TypedPointers.Value);
        }

        [Fact]
        public void TestSymbolDefinitionInfo() {
            RunCompilerDirective("", SymbolDefinitionInfo.Undefined, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("Y+", SymbolDefinitionInfo.Enable, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("Y-", SymbolDefinitionInfo.Disable, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("YD", SymbolDefinitionInfo.Enable, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("DEFINITIONINFO OFF", SymbolDefinitionInfo.Disable, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("DEFINITIONINFO ON", SymbolDefinitionInfo.Enable, () => CompilerOptions.SymbolDefinitions.Value);
        }

        [Fact]
        public void TestSymbolReferenceInfo() {
            RunCompilerDirective("", SymbolReferenceInfo.Undefined, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("Y-", SymbolReferenceInfo.Disable, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("Y+", SymbolReferenceInfo.Enable, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("YD", SymbolReferenceInfo.Disable, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("REFERENCEINFO ON", SymbolReferenceInfo.Enable, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("REFERENCEINFO OFF", SymbolReferenceInfo.Disable, () => CompilerOptions.SymbolReferences.Value);
        }

        [Fact]
        public void TestStrongLinking() {
            RunCompilerDirective("", StrongTypeLinking.Undefined, () => CompilerOptions.LinkAllTypes.Value);
            RunCompilerDirective("STRONGLINKTYPES ON", StrongTypeLinking.Enable, () => CompilerOptions.LinkAllTypes.Value);
            RunCompilerDirective("STRONGLINKTYPES OFF", StrongTypeLinking.Disable, () => CompilerOptions.LinkAllTypes.Value);
        }

        [Fact]
        public void TestScopedEnums() {
            RunCompilerDirective("", RequireScopedEnums.Undefined, () => CompilerOptions.ScopedEnums.Value);
            RunCompilerDirective("SCOPEDENUMS ON", RequireScopedEnums.Enable, () => CompilerOptions.ScopedEnums.Value);
            RunCompilerDirective("SCOPEDENUMS OFF", RequireScopedEnums.Disable, () => CompilerOptions.ScopedEnums.Value);
        }

        [Fact]
        public void TestTypeInfo() {
            RunCompilerDirective("", RttiForPublishedProperties.Undefined, () => CompilerOptions.PublishedRtti.Value);
            RunCompilerDirective("M+", RttiForPublishedProperties.Enable, () => CompilerOptions.PublishedRtti.Value);
            RunCompilerDirective("M-", RttiForPublishedProperties.Disable, () => CompilerOptions.PublishedRtti.Value);
            RunCompilerDirective("TYPEINFO ON", RttiForPublishedProperties.Enable, () => CompilerOptions.PublishedRtti.Value);
            RunCompilerDirective("TYPEINFO OFF", RttiForPublishedProperties.Disable, () => CompilerOptions.PublishedRtti.Value);
        }

        [Fact]
        public void TestRunOnly() {
            RunCompilerDirective("", RuntimePackageMode.Undefined, () => CompilerOptions.RuntimeOnlyPackage.Value);
            RunCompilerDirective("RUNONLY OFF", RuntimePackageMode.Standard, () => CompilerOptions.RuntimeOnlyPackage.Value);
            RunCompilerDirective("RUNONLY ON", RuntimePackageMode.RuntimeOnly, () => CompilerOptions.RuntimeOnlyPackage.Value);
        }

        [Fact]
        public void TestLinkedFiles() {
            RunCompilerDirective("", false, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("L link.dll", true, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("LINK 'link.dll'", true, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void TestLegacyIfEnd() {
            RunCompilerDirective("", EndIfMode.Undefined, () => CompilerOptions.LegacyIfEnd.Value);
            RunCompilerDirective("LEGACYIFEND ON", EndIfMode.LegacyIfEnd, () => CompilerOptions.LegacyIfEnd.Value);
            RunCompilerDirective("LEGACYIFEND OFF", EndIfMode.Standard, () => CompilerOptions.LegacyIfEnd.Value);
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
            RunCompilerDirective("", Real48.Undefined, () => CompilerOptions.RealCompatiblity.Value);
            RunCompilerDirective("REALCOMPATIBILITY ON", Real48.EnableCompatibility, () => CompilerOptions.RealCompatiblity.Value);
            RunCompilerDirective("REALCOMPATIBILITY OFF", Real48.DisableCompatibility, () => CompilerOptions.RealCompatiblity.Value);
        }

        [Fact]
        public void TestPointerMath() {
            RunCompilerDirective("", PointerManipulation.Undefined, () => CompilerOptions.PointerMath.Value);
            RunCompilerDirective("POINTERMATH ON", PointerManipulation.EnablePointerMath, () => CompilerOptions.PointerMath.Value);
            RunCompilerDirective("POINTERMATH OFF", PointerManipulation.DisablePointerMath, () => CompilerOptions.PointerMath.Value);
        }

        [Fact]
        public void TestOldTypeLayout() {
            RunCompilerDirective("", OldRecordTypes.Undefined, () => CompilerOptions.OldTypeLayout.Value);
            RunCompilerDirective("OLDTYPELAYOUT  ON", OldRecordTypes.EnableOldRecordPacking, () => CompilerOptions.OldTypeLayout.Value);
            RunCompilerDirective("OLDTYPELAYOUT  OFF", OldRecordTypes.DisableOldRecordPacking, () => CompilerOptions.OldTypeLayout.Value);
        }

        [Fact]
        public void TestNoDefine() {
            RunCompilerDirective("", false, () => Meta.NoDefines.Any());
            RunCompilerDirective("NODEFINE TDEMO", true, () => Meta.NoDefines.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("NODEFINE TMIMOA", false, () => Meta.NoDefines.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("NODEFINE TMIMOA FUZZ", true, () => Meta.NoDefines.Any(t => t.UnionTypeName.StartsWith("fuz", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void TestObjTypeName() {
            RunCompilerDirective("", false, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("OBJTYPENAME tdemo", true, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("OBJTYPENAME tmemo", false, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("OBJTYPENAME tdemo 'Bul'", true, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("OBJTYPENAME tdemo 'Ntdemo'", true, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void TestNoInclude() {
            RunCompilerDirective("", false, () => Meta.NoIncludes.Any(t => t.StartsWith("Winapi", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("NOINCLUDE WinApi.Messages", true, () => Meta.NoIncludes.Any(t => t.StartsWith("Winapi", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void TestMinEnumSize() {
            RunCompilerDirective("", EnumSize.Undefined, () => CompilerOptions.MinumEnumSize.Value);
            RunCompilerDirective("Z+", EnumSize.FourByte, () => CompilerOptions.MinumEnumSize.Value);
            RunCompilerDirective("Z-", EnumSize.OneByte, () => CompilerOptions.MinumEnumSize.Value);
            RunCompilerDirective("Z1", EnumSize.OneByte, () => CompilerOptions.MinumEnumSize.Value);
            RunCompilerDirective("Z2", EnumSize.TwoByte, () => CompilerOptions.MinumEnumSize.Value);
            RunCompilerDirective("Z4", EnumSize.FourByte, () => CompilerOptions.MinumEnumSize.Value);
            RunCompilerDirective("MINENUMSIZE 1", EnumSize.OneByte, () => CompilerOptions.MinumEnumSize.Value);
            RunCompilerDirective("MINENUMSIZE 2", EnumSize.TwoByte, () => CompilerOptions.MinumEnumSize.Value);
            RunCompilerDirective("MINENUMSIZE 4", EnumSize.FourByte, () => CompilerOptions.MinumEnumSize.Value);
        }

        [Fact]
        public void TestMethodInfo() {
            RunCompilerDirective("", MethodInfoRtti.Undefined, () => CompilerOptions.MethodInfo.Value);
            RunCompilerDirective("METHODINFO ON", MethodInfoRtti.EnableMethodInfo, () => CompilerOptions.MethodInfo.Value);
            RunCompilerDirective("METHODINFO OFF", MethodInfoRtti.DisableMethodInfo, () => CompilerOptions.MethodInfo.Value);
        }

        [Fact]
        public void TestLibMeta() {
            RunCompilerDirective("", null, () => Meta.LibPrefix.Value);
            RunCompilerDirective("", null, () => Meta.LibSuffix.Value);
            RunCompilerDirective("", null, () => Meta.LibVersion.Value);
            RunCompilerDirective("LIBPREFIX 'P'", "P", () => Meta.LibPrefix.Value);
            RunCompilerDirective("LIBSUFFIX 'M'", "M", () => Meta.LibSuffix.Value);
            RunCompilerDirective("LIBVERSION 'V'", "V", () => Meta.LibVersion.Value);
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
