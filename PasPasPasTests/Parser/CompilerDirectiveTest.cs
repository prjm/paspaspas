using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.Parser;
using System;
using System.Linq;

namespace PasPasPasTests.Parser {

    [TestClass]
    public class CompilerDirectiveTest : ParserTestBase {

        [TestMethod]
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
            RunCompilerDirective("A0", Alignment.Undefined, f, CompilerDirectiveParserErrors.InvalidAlignDirective);
        }

        [TestMethod]
        public void TestApptype() {
            RunCompilerDirective("", AppType.Undefined, () => CompilerOptions.ApplicationType.Value);
            RunCompilerDirective("APPTYPE GUI", AppType.Gui, () => CompilerOptions.ApplicationType.Value);
            RunCompilerDirective("APPTYPE CONSOLE", AppType.Console, () => CompilerOptions.ApplicationType.Value);
        }

        [TestMethod]
        public void TestMessage() {
            RunCompilerDirective("", true, () => true);
            RunCompilerDirective("MESSAGE 'X'", true, () => true);
            RunCompilerDirective("MESSAGE Hint 'X' ", true, () => true);
            RunCompilerDirective("MESSAGE Warn 'X' ", true, () => true);
            RunCompilerDirective("MESSAGE Error 'X'", true, () => true);
            RunCompilerDirective("MESSAGE Fatal 'x'", true, () => true);
        }

        [TestMethod]
        public void TestMemStackSize() {
            RunCompilerDirective("", 0L, () => CompilerOptions.MinimumStackMemSize.Value);
            RunCompilerDirective("", 0L, () => CompilerOptions.MaximumStackMemSize.Value);
            RunCompilerDirective("M 100, 200", 200L, () => CompilerOptions.MaximumStackMemSize.Value);
            RunCompilerDirective("M 100, 200", 100L, () => CompilerOptions.MinimumStackMemSize.Value);
            RunCompilerDirective("MINSTACKSIZE 300", 300L, () => CompilerOptions.MinimumStackMemSize.Value);
            RunCompilerDirective("MAXSTACKSIZE 400", 400L, () => CompilerOptions.MaximumStackMemSize.Value);
        }

        [TestMethod]
        public void TestBoolEvalSwitch() {
            RunCompilerDirective("", BooleanEvaluation.Undefined, () => CompilerOptions.BoolEval.Value);
            RunCompilerDirective("B+", BooleanEvaluation.CompleteEvaluation, () => CompilerOptions.BoolEval.Value);
            RunCompilerDirective("B-", BooleanEvaluation.ShortEvaluation, () => CompilerOptions.BoolEval.Value);
            RunCompilerDirective("BOOLEVAL ON", BooleanEvaluation.CompleteEvaluation, () => CompilerOptions.BoolEval.Value);
            RunCompilerDirective("BOOLEVAL OFF", BooleanEvaluation.ShortEvaluation, () => CompilerOptions.BoolEval.Value);
        }

        [TestMethod]
        public void TestCodeAlignParameter() {
            RunCompilerDirective("", CodeAlignment.Undefined, () => CompilerOptions.CodeAlign.Value);
            RunCompilerDirective("CODEALIGN 1", CodeAlignment.OneByte, () => CompilerOptions.CodeAlign.Value);
            RunCompilerDirective("CODEALIGN 2", CodeAlignment.TwoByte, () => CompilerOptions.CodeAlign.Value);
            RunCompilerDirective("CODEALIGN 4", CodeAlignment.FourByte, () => CompilerOptions.CodeAlign.Value);
            RunCompilerDirective("CODEALIGN 8", CodeAlignment.EightByte, () => CompilerOptions.CodeAlign.Value);
            RunCompilerDirective("CODEALIGN 16", CodeAlignment.SixteenByte, () => CompilerOptions.CodeAlign.Value);
        }

        [TestMethod]
        public void TestDefine() {
            RunCompilerDirective("", false, () => ConditionalCompilation.IsSymbolDefined("TESTSYM"));
            RunCompilerDirective("DEFINE TESTSYM", true, () => ConditionalCompilation.IsSymbolDefined("TESTSYM"));
            RunCompilerDirective("DEFINE TESTsYM", true, () => ConditionalCompilation.IsSymbolDefined("TESTSYM"));
            RunCompilerDirective("DEFINE TESTsYM | DEFINE X", false, () => ConditionalCompilation.IsSymbolDefined("TESTSYM"));
            RunCompilerDirective("DEFINE TESTSYM § DEFINE X", true, () => ConditionalCompilation.IsSymbolDefined("TESTSYM"));
            RunCompilerDirective("", false, () => ConditionalCompilation.IsSymbolDefined("TESTSYM"));
            RunCompilerDirective("DEFINE TESTSYM § UNDEF TESTSYM", false, () => ConditionalCompilation.IsSymbolDefined("TESTSYM"));
            RunCompilerDirective("DEFINE TESTSYM § UNDEF TESTSYM § DEFINE TESTSYM", true, () => ConditionalCompilation.IsSymbolDefined("TESTSYM"));
            RunCompilerDirective("UNDEF PASPASPAS_TEST | DEFINE X", true, () => ConditionalCompilation.IsSymbolDefined("PASPASPAS_TEST"));
            RunCompilerDirective("UNDEF PASPASPAS_TEST § DEFINE X", false, () => ConditionalCompilation.IsSymbolDefined("PASPASPAS_TEST"));

            RunCompilerDirective("IFDEF TESTSYM § DEFINE A § ENDIF", false, () => ConditionalCompilation.IsSymbolDefined("A"));
            RunCompilerDirective("IFDEF TESTSYM § ENDIF § DEFINE A ", true, () => ConditionalCompilation.IsSymbolDefined("A"));
            RunCompilerDirective("IFDEF TESTSYM § IFDEF Q § DEFINE A § ENDIF § ENDIF", false, () => ConditionalCompilation.IsSymbolDefined("A"));
            RunCompilerDirective("IFDEF PASPASPAS_TEST § DEFINE A § ENDIF", true, () => ConditionalCompilation.IsSymbolDefined("A"));

            RunCompilerDirective("IFDEF TESTSYM $ DEFINE B § ELSE § DEFINE A § ENDIF", true, () => ConditionalCompilation.IsSymbolDefined("A"));
            RunCompilerDirective("IFDEF TESTSYM $ DEFINE A § ELSE § DEFINE B § ENDIF", false, () => ConditionalCompilation.IsSymbolDefined("A"));
            RunCompilerDirective("IFDEF TESTSYM $ DEFINE B § ELSE § DEFINE A § ENDIF", false, () => ConditionalCompilation.IsSymbolDefined("B"));
            RunCompilerDirective("IFDEF TESTSYM $ DEFINE A § ELSE § DEFINE B § ENDIF", true, () => ConditionalCompilation.IsSymbolDefined("B"));

            RunCompilerDirective("IFNDEF TESTSYM § DEFINE A § ENDIF", true, () => ConditionalCompilation.IsSymbolDefined("A"));
            RunCompilerDirective("IFNDEF TESTSYM § IFNDEF Q § DEFINE A § ENDIF § ENDIF", true, () => ConditionalCompilation.IsSymbolDefined("A"));
            RunCompilerDirective("IFNDEF PASPASPAS_TEST § DEFINE A § ENDIF", false, () => ConditionalCompilation.IsSymbolDefined("A"));

        }

        [TestMethod]
        public void TestAssertions() {
            RunCompilerDirective("", AssertionMode.Undefined, () => CompilerOptions.Assertions.Value);
            RunCompilerDirective("C+", AssertionMode.EnableAssertions, () => CompilerOptions.Assertions.Value);
            RunCompilerDirective("C-", AssertionMode.DisableAssertions, () => CompilerOptions.Assertions.Value);
            RunCompilerDirective("ASSERTIONS ON", AssertionMode.EnableAssertions, () => CompilerOptions.Assertions.Value);
            RunCompilerDirective("ASSERTIONS OFF", AssertionMode.DisableAssertions, () => CompilerOptions.Assertions.Value);
        }


        [TestMethod]
        public void TestDebugInfo() {
            RunCompilerDirective("", DebugInformation.Undefined, () => CompilerOptions.DebugInfo.Value);
            RunCompilerDirective("D+", DebugInformation.IncludeDebugInformation, () => CompilerOptions.DebugInfo.Value);
            RunCompilerDirective("D-", DebugInformation.NoDebugInfo, () => CompilerOptions.DebugInfo.Value);
            RunCompilerDirective("DEBUGINFO ON", DebugInformation.IncludeDebugInformation, () => CompilerOptions.DebugInfo.Value);
            RunCompilerDirective("DEBUGINFO OFF", DebugInformation.NoDebugInfo, () => CompilerOptions.DebugInfo.Value);
        }

        [TestMethod]
        public void TestDenyPackageUnit() {
            RunCompilerDirective("", DenyUnitInPackages.Undefined, () => ConditionalCompilation.DenyInPackages.Value);
            RunCompilerDirective("DENYPACKAGEUNIT ON", DenyUnitInPackages.DenyUnit, () => ConditionalCompilation.DenyInPackages.Value);
            RunCompilerDirective("DENYPACKAGEUNIT OFF", DenyUnitInPackages.AllowUnit, () => ConditionalCompilation.DenyInPackages.Value);
        }

        [TestMethod]
        public void TestDescription() {
            RunCompilerDirective("", null, () => Meta.Description.Value);
            RunCompilerDirective("DESCRIPTION 'dummy'", "dummy", () => Meta.Description.Value);
            RunCompilerDirective("D 'dummy1'", "dummy1", () => Meta.Description.Value);
        }

        [TestMethod]
        public void TestDesigntimeOnly() {
            RunCompilerDirective("", DesignOnlyUnit.Undefined, () => ConditionalCompilation.DesignOnly.Value);
            RunCompilerDirective("DESIGNONLY ON", DesignOnlyUnit.InDesignTimeOnly, () => ConditionalCompilation.DesignOnly.Value);
            RunCompilerDirective("DESIGNONLY OFF", DesignOnlyUnit.Alltimes, () => ConditionalCompilation.DesignOnly.Value);
        }

        [TestMethod]
        public void TestExtensionsSwitch() {
            RunCompilerDirective("", null, () => Meta.FileExtension.Value);
            RunCompilerDirective("EXTENSION ddd", "ddd", () => Meta.FileExtension.Value);
            RunCompilerDirective("E ddd'", "ddd", () => Meta.FileExtension.Value);
        }

        [TestMethod]
        public void TestObjExportAll() {
            RunCompilerDirective("", ExportCppObjects.Undefined, () => CompilerOptions.ExportCppObjects.Value);
            RunCompilerDirective("OBJEXPORTALL ON", ExportCppObjects.ExportAll, () => CompilerOptions.ExportCppObjects.Value);
            RunCompilerDirective("OBJEXPORTALL OFF", ExportCppObjects.DoNotExportAll, () => CompilerOptions.ExportCppObjects.Value);
        }

        [TestMethod]
        public void TestExtendedCompatibility() {
            RunCompilerDirective("", ExtendedCompatiblityMode.Undefined, () => CompilerOptions.ExtendedCompatibility.Value);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY ON", ExtendedCompatiblityMode.Enabled, () => CompilerOptions.ExtendedCompatibility.Value);
            RunCompilerDirective("EXTENDEDCOMPATIBILITY OFF", ExtendedCompatiblityMode.Disabled, () => CompilerOptions.ExtendedCompatibility.Value);
        }

        [TestMethod]
        public void TestExtendedSyntax() {
            RunCompilerDirective("", ExtendedSyntax.Undefined, () => CompilerOptions.UseExtendedSyntax.Value);
            RunCompilerDirective("X+", ExtendedSyntax.UseExtendedSyntax, () => CompilerOptions.UseExtendedSyntax.Value);
            RunCompilerDirective("X-", ExtendedSyntax.NoExtendedSyntax, () => CompilerOptions.UseExtendedSyntax.Value);
            RunCompilerDirective("EXTENDEDSYNTAX ON", ExtendedSyntax.UseExtendedSyntax, () => CompilerOptions.UseExtendedSyntax.Value);
            RunCompilerDirective("EXTENDEDSYNTAX OFF", ExtendedSyntax.NoExtendedSyntax, () => CompilerOptions.UseExtendedSyntax.Value);
        }

        [TestMethod]
        public void TestExternalSymbol() {
            RunCompilerDirective("", 0, () => Meta.ExternalSymbols.Count);
            RunCompilerDirective("EXTERNALSYM dummy", true, () => Meta.ExternalSymbols.Any(t => string.Equals(t.IdentifierName, "dummy")));
            RunCompilerDirective("EXTERNALSYM dummy 'a'", true, () => Meta.ExternalSymbols.Any(t => string.Equals(t.IdentifierName, "dummy")));
            RunCompilerDirective("EXTERNALSYM dummy 'a' 'q'", true, () => Meta.ExternalSymbols.Any(t => string.Equals(t.IdentifierName, "dummy")));
        }

        [TestMethod]
        public void TestExcessPrecision() {
            RunCompilerDirective("", ExcessPrecisionForResults.Undefined, () => CompilerOptions.ExcessPrecision.Value);
            RunCompilerDirective("EXCESSPRECISION  ON", ExcessPrecisionForResults.EnableExcess, () => CompilerOptions.ExcessPrecision.Value);
            RunCompilerDirective("EXCESSPRECISION  OFF", ExcessPrecisionForResults.DisableExcess, () => CompilerOptions.ExcessPrecision.Value);
        }

        [TestMethod]
        public void TestHighCharUnicode() {
            RunCompilerDirective("", HighCharsUnicode.Undefined, () => CompilerOptions.HighCharUnicode.Value);
            RunCompilerDirective("HIGHCHARUNICODE OFF", HighCharsUnicode.DisableHighChars, () => CompilerOptions.HighCharUnicode.Value);
            RunCompilerDirective("HIGHCHARUNICODE ON", HighCharsUnicode.EnableHighChars, () => CompilerOptions.HighCharUnicode.Value);
        }

        [TestMethod]
        public void TestHints() {
            RunCompilerDirective("", CompilerHints.Undefined, () => CompilerOptions.Hints.Value);
            RunCompilerDirective("HINTS ON", CompilerHints.EnableHints, () => CompilerOptions.Hints.Value);
            RunCompilerDirective("HINTS OFF", CompilerHints.DisableHints, () => CompilerOptions.Hints.Value);
        }

        [TestMethod]
        public void TestHppEmit() {
            RunCompilerDirective("", 0, () => Meta.HeaderStrings.Count);
            RunCompilerDirective("HPPEMIT 'dummy'", true, () => Meta.HeaderStrings.Any(t => string.Equals(t.Value, "'dummy'")));
            RunCompilerDirective("HPPEMIT END 'dummy'", true, () => Meta.HeaderStrings.Any(t => string.Equals(t.Value, "'dummy'")));
            RunCompilerDirective("HPPEMIT LINKUNIT", true, () => Meta.HeaderStrings.Any(t => t.Mode == HppEmitMode.LinkUnit));
            RunCompilerDirective("HPPEMIT OPENNAMESPACE", true, () => Meta.HeaderStrings.Any(t => t.Mode == HppEmitMode.OpenNamespace));
            RunCompilerDirective("HPPEMIT CLOSENAMESPACE", true, () => Meta.HeaderStrings.Any(t => t.Mode == HppEmitMode.CloseNamespace));
            RunCompilerDirective("HPPEMIT NOUSINGNAMESPACE", true, () => Meta.HeaderStrings.Any(t => t.Mode == HppEmitMode.NoUsingNamespace));
        }

        [TestMethod]
        public void TestImageBase() {
            RunCompilerDirective("IMAGEBASE $40000000 ", 0x40000000, () => CompilerOptions.ImageBase.Value);
            RunCompilerDirective("IMAGEBASE 40000000 ", 40000000, () => CompilerOptions.ImageBase.Value);
        }

        [TestMethod]
        public void TestImplicitBuild() {
            RunCompilerDirective("", ImplicitBuildUnit.Undefined, () => CompilerOptions.ImplicitBuild.Value);
            RunCompilerDirective("IMPLICITBUILD ON", ImplicitBuildUnit.EnableImplicitBuild, () => CompilerOptions.ImplicitBuild.Value);
            RunCompilerDirective("IMPLICITBUILD OFF", ImplicitBuildUnit.DisableImplicitBuild, () => CompilerOptions.ImplicitBuild.Value);
        }

        [TestMethod]
        public void TestImportUnitData() {
            RunCompilerDirective("", ImportGlobalUnitData.Undefined, () => CompilerOptions.ImportedData.Value);
            RunCompilerDirective("G+", ImportGlobalUnitData.DoImport, () => CompilerOptions.ImportedData.Value);
            RunCompilerDirective("G-", ImportGlobalUnitData.NoImport, () => CompilerOptions.ImportedData.Value);
            RunCompilerDirective("IMPORTEDDATA  ON", ImportGlobalUnitData.DoImport, () => CompilerOptions.ImportedData.Value);
            RunCompilerDirective("IMPORTEDDATA OFF", ImportGlobalUnitData.NoImport, () => CompilerOptions.ImportedData.Value);
        }

        [TestMethod]
        public void TestInclude() {
            RunCompilerDirective("", false, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
            RunCompilerDirective("INCLUDE 'DUMMY.INC'", true, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
            RunCompilerDirective("", false, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
            RunCompilerDirective("I 'DUMMY.INC'", true, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
            RunCompilerDirective("I DUMMY.INC", true, () => ConditionalCompilation.IsSymbolDefined("DUMMY_INC"));
        }

        [TestMethod]
        public void TestIoChecks() {
            RunCompilerDirective("", IoCallChecks.Undefined, () => CompilerOptions.IoChecks.Value);
            RunCompilerDirective("I+", IoCallChecks.EnableIoChecks, () => CompilerOptions.IoChecks.Value);
            RunCompilerDirective("I-", IoCallChecks.DisableIoChecks, () => CompilerOptions.IoChecks.Value);
            RunCompilerDirective("IOCHECKS  ON", IoCallChecks.EnableIoChecks, () => CompilerOptions.IoChecks.Value);
            RunCompilerDirective("IOCHECKS OFF", IoCallChecks.DisableIoChecks, () => CompilerOptions.IoChecks.Value);
        }

        [TestMethod]
        public void TestLocalSymbols() {
            RunCompilerDirective("", LocalDebugSymbols.Undefined, () => CompilerOptions.LocalSymbols.Value);
            RunCompilerDirective("L+", LocalDebugSymbols.EnableLocalSymbols, () => CompilerOptions.LocalSymbols.Value);
            RunCompilerDirective("L-", LocalDebugSymbols.DisableLocalSymbols, () => CompilerOptions.LocalSymbols.Value);
            RunCompilerDirective("LOCALSYMBOLS  ON", LocalDebugSymbols.EnableLocalSymbols, () => CompilerOptions.LocalSymbols.Value);
            RunCompilerDirective("LOCALSYMBOLS OFF", LocalDebugSymbols.DisableLocalSymbols, () => CompilerOptions.LocalSymbols.Value);
        }

        [TestMethod]
        public void TestLongStrings() {
            RunCompilerDirective("", LongStringTypes.Undefined, () => CompilerOptions.LongStrings.Value);
            RunCompilerDirective("H+", LongStringTypes.EnableLongStrings, () => CompilerOptions.LongStrings.Value);
            RunCompilerDirective("H-", LongStringTypes.DisableLongStrings, () => CompilerOptions.LongStrings.Value);
            RunCompilerDirective("LONGSTRINGS  ON", LongStringTypes.EnableLongStrings, () => CompilerOptions.LongStrings.Value);
            RunCompilerDirective("LONGSTRINGS  OFF", LongStringTypes.DisableLongStrings, () => CompilerOptions.LongStrings.Value);
        }

        [TestMethod]
        public void TestOpenStrings() {
            RunCompilerDirective("", OpenStringTypes.Undefined, () => CompilerOptions.OpenStrings.Value);
            RunCompilerDirective("P+", OpenStringTypes.EnableOpenStrings, () => CompilerOptions.OpenStrings.Value);
            RunCompilerDirective("P-", OpenStringTypes.DisableOpenStrings, () => CompilerOptions.OpenStrings.Value);
            RunCompilerDirective("OPENSTRINGS  ON", OpenStringTypes.EnableOpenStrings, () => CompilerOptions.OpenStrings.Value);
            RunCompilerDirective("OPENSTRINGS  OFF", OpenStringTypes.DisableOpenStrings, () => CompilerOptions.OpenStrings.Value);
        }

        [TestMethod]
        public void TestOptimization() {
            RunCompilerDirective("", CompilerOptmization.Undefined, () => CompilerOptions.Optimization.Value);
            RunCompilerDirective("O+", CompilerOptmization.EnableOptimization, () => CompilerOptions.Optimization.Value);
            RunCompilerDirective("O-", CompilerOptmization.DisableOptimization, () => CompilerOptions.Optimization.Value);
            RunCompilerDirective("OPTIMIZATION  ON", CompilerOptmization.EnableOptimization, () => CompilerOptions.Optimization.Value);
            RunCompilerDirective("OPTIMIZATION  OFF", CompilerOptmization.DisableOptimization, () => CompilerOptions.Optimization.Value);
        }

        [TestMethod]
        public void TestOverflow() {
            RunCompilerDirective("", RuntimeOverflowChecks.Undefined, () => CompilerOptions.CheckOverflows.Value);
            RunCompilerDirective("Q+", RuntimeOverflowChecks.EnableChecks, () => CompilerOptions.CheckOverflows.Value);
            RunCompilerDirective("Q-", RuntimeOverflowChecks.DisableChecks, () => CompilerOptions.CheckOverflows.Value);
            RunCompilerDirective("OVERFLOWCHECKS ON", RuntimeOverflowChecks.EnableChecks, () => CompilerOptions.CheckOverflows.Value);
            RunCompilerDirective("OVERFLOWCHECKS OFF", RuntimeOverflowChecks.DisableChecks, () => CompilerOptions.CheckOverflows.Value);
        }

        [TestMethod]
        public void TestSaveDivide() {
            RunCompilerDirective("", FDivSafeDivide.Undefined, () => CompilerOptions.SafeDivide.Value);
            RunCompilerDirective("U+", FDivSafeDivide.EnableSafeDivide, () => CompilerOptions.SafeDivide.Value);
            RunCompilerDirective("U-", FDivSafeDivide.DisableSafeDivide, () => CompilerOptions.SafeDivide.Value);
            RunCompilerDirective("SAFEDIVIDE  ON", FDivSafeDivide.EnableSafeDivide, () => CompilerOptions.SafeDivide.Value);
            RunCompilerDirective("SAFEDIVIDE  OFF", FDivSafeDivide.DisableSafeDivide, () => CompilerOptions.SafeDivide.Value);
        }

        [TestMethod]
        public void TestRangeChecks() {
            RunCompilerDirective("", RuntimeRangeChecks.Undefined, () => CompilerOptions.RangeChecks.Value);
            RunCompilerDirective("R+", RuntimeRangeChecks.EnableRangeChecks, () => CompilerOptions.RangeChecks.Value);
            RunCompilerDirective("R-", RuntimeRangeChecks.DisableRangeChecks, () => CompilerOptions.RangeChecks.Value);
            RunCompilerDirective("RANGECHECKS ON", RuntimeRangeChecks.EnableRangeChecks, () => CompilerOptions.RangeChecks.Value);
            RunCompilerDirective("RANGECHECKS OFF", RuntimeRangeChecks.DisableRangeChecks, () => CompilerOptions.RangeChecks.Value);
        }

        [TestMethod]
        public void TestStackFrames() {
            RunCompilerDirective("", StackFrameGeneration.Undefined, () => CompilerOptions.StackFrames.Value);
            RunCompilerDirective("W+", StackFrameGeneration.EnableFrames, () => CompilerOptions.StackFrames.Value);
            RunCompilerDirective("W-", StackFrameGeneration.DisableFrames, () => CompilerOptions.StackFrames.Value);
            RunCompilerDirective("STACKFRAMES  ON", StackFrameGeneration.EnableFrames, () => CompilerOptions.StackFrames.Value);
            RunCompilerDirective("STACKFRAMES  OFF", StackFrameGeneration.DisableFrames, () => CompilerOptions.StackFrames.Value);
        }

        [TestMethod]
        public void TestZeroBasedStrings() {
            RunCompilerDirective("", FirstCharIndex.Undefined, () => CompilerOptions.IndexOfFirstCharInString.Value);
            RunCompilerDirective("ZEROBASEDSTRINGS  ON", FirstCharIndex.IsZero, () => CompilerOptions.IndexOfFirstCharInString.Value);
            RunCompilerDirective("ZEROBASEDSTRINGS  OFF", FirstCharIndex.IsOne, () => CompilerOptions.IndexOfFirstCharInString.Value);
        }

        [TestMethod]
        public void TestWritableConst() {
            RunCompilerDirective("", ConstantValues.Undefined, () => CompilerOptions.WritableConstants.Value);
            RunCompilerDirective("J-", ConstantValues.Constant, () => CompilerOptions.WritableConstants.Value);
            RunCompilerDirective("J+", ConstantValues.Writable, () => CompilerOptions.WritableConstants.Value);
            RunCompilerDirective("WRITEABLECONST  OFF", ConstantValues.Constant, () => CompilerOptions.WritableConstants.Value);
            RunCompilerDirective("WRITEABLECONST  ON", ConstantValues.Writable, () => CompilerOptions.WritableConstants.Value);
        }

        [TestMethod]
        public void TestWeakLinkRtti() {
            RunCompilerDirective("", RttiLinkMode.Undefined, () => CompilerOptions.WeakLinkRtti.Value);
            RunCompilerDirective("WEAKLINKRTTI  ON", RttiLinkMode.LinkWeakRtti, () => CompilerOptions.WeakLinkRtti.Value);
            RunCompilerDirective("WEAKLINKRTTI  OFF", RttiLinkMode.LinkFullRtti, () => CompilerOptions.WeakLinkRtti.Value);
        }

        [TestMethod]
        public void TestWeakPackageUnit() {
            RunCompilerDirective("", WeakPackaging.Undefined, () => CompilerOptions.WeakPackageUnit.Value);
            RunCompilerDirective("WEAKPACKAGEUNIT ON", WeakPackaging.Enable, () => CompilerOptions.WeakPackageUnit.Value);
            RunCompilerDirective("WEAKPACKAGEUNIT OFF", WeakPackaging.Disable, () => CompilerOptions.WeakPackageUnit.Value);
        }

        [TestMethod]
        public void TestWarnings() {
            RunCompilerDirective("", CompilerWarnings.Undefined, () => CompilerOptions.Warnings.Value);
            RunCompilerDirective("WARNINGS  ON", CompilerWarnings.Enable, () => CompilerOptions.Warnings.Value);
            RunCompilerDirective("WARNINGS  OFF", CompilerWarnings.Disable, () => CompilerOptions.Warnings.Value);
        }

        [TestMethod]
        public void TestWarn() {
            RunCompilerDirective("", WarningMode.Undefined, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
            RunCompilerDirective("WARN SYMBOL_DEPRECATED ON", WarningMode.On, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
            RunCompilerDirective("WARN SYMBOL_DEPRECATED OFF", WarningMode.Off, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
            RunCompilerDirective("WARN SYMBOL_DEPRECATED ERROR", WarningMode.Error, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
            RunCompilerDirective("WARN SYMBOL_DEPRECATED DEFAULT", WarningMode.Undefined, () => Warnings.GetModeByIdentifier("SYMBOL_DEPRECATED"));
        }

        [TestMethod]
        public void TestVarStringChecks() {
            RunCompilerDirective("", ShortVarStringChecks.Undefined, () => CompilerOptions.VarStringChecks.Value);
            RunCompilerDirective("V+", ShortVarStringChecks.EnableChecks, () => CompilerOptions.VarStringChecks.Value);
            RunCompilerDirective("V-", ShortVarStringChecks.DisableChecks, () => CompilerOptions.VarStringChecks.Value);
            RunCompilerDirective("VARSTRINGCHECKS ON", ShortVarStringChecks.EnableChecks, () => CompilerOptions.VarStringChecks.Value);
            RunCompilerDirective("VARSTRINGCHECKS  OFF", ShortVarStringChecks.DisableChecks, () => CompilerOptions.VarStringChecks.Value);
        }

        [TestMethod]
        public void TestTypeCheckedPointers() {
            RunCompilerDirective("", TypeCheckedPointers.Undefined, () => CompilerOptions.TypedPointers.Value);
            RunCompilerDirective("T+", TypeCheckedPointers.Enable, () => CompilerOptions.TypedPointers.Value);
            RunCompilerDirective("T-", TypeCheckedPointers.Disable, () => CompilerOptions.TypedPointers.Value);
            RunCompilerDirective("TYPEDADDRESS ON", TypeCheckedPointers.Enable, () => CompilerOptions.TypedPointers.Value);
            RunCompilerDirective("TYPEDADDRESS OFF", TypeCheckedPointers.Disable, () => CompilerOptions.TypedPointers.Value);
        }

        [TestMethod]
        public void TestSymbolDefinitionInfo() {
            RunCompilerDirective("", SymbolDefinitionInfo.Undefined, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("Y+", SymbolDefinitionInfo.Enable, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("Y-", SymbolDefinitionInfo.Disable, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("YD", SymbolDefinitionInfo.Enable, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("DEFINITIONINFO OFF", SymbolDefinitionInfo.Disable, () => CompilerOptions.SymbolDefinitions.Value);
            RunCompilerDirective("DEFINITIONINFO ON", SymbolDefinitionInfo.Enable, () => CompilerOptions.SymbolDefinitions.Value);
        }

        [TestMethod]
        public void TestSymbolReferenceInfo() {
            RunCompilerDirective("", SymbolReferenceInfo.Undefined, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("Y-", SymbolReferenceInfo.Disable, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("Y+", SymbolReferenceInfo.Enable, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("YD", SymbolReferenceInfo.Disable, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("REFERENCEINFO ON", SymbolReferenceInfo.Enable, () => CompilerOptions.SymbolReferences.Value);
            RunCompilerDirective("REFERENCEINFO OFF", SymbolReferenceInfo.Disable, () => CompilerOptions.SymbolReferences.Value);
        }

        [TestMethod]
        public void TestStrongLinking() {
            RunCompilerDirective("", StrongTypeLinking.Undefined, () => CompilerOptions.LinkAllTypes.Value);
            RunCompilerDirective("STRONGLINKTYPES ON", StrongTypeLinking.Enable, () => CompilerOptions.LinkAllTypes.Value);
            RunCompilerDirective("STRONGLINKTYPES OFF", StrongTypeLinking.Disable, () => CompilerOptions.LinkAllTypes.Value);
        }

        [TestMethod]
        public void TestScopedEnums() {
            RunCompilerDirective("", RequireScopedEnums.Undefined, () => CompilerOptions.ScopedEnums.Value);
            RunCompilerDirective("SCOPEDENUMS ON", RequireScopedEnums.Enable, () => CompilerOptions.ScopedEnums.Value);
            RunCompilerDirective("SCOPEDENUMS OFF", RequireScopedEnums.Disable, () => CompilerOptions.ScopedEnums.Value);
        }

        [TestMethod]
        public void TestTypeInfo() {
            RunCompilerDirective("", RttiForPublishedProperties.Undefined, () => CompilerOptions.PublishedRtti.Value);
            RunCompilerDirective("M+", RttiForPublishedProperties.Enable, () => CompilerOptions.PublishedRtti.Value);
            RunCompilerDirective("M-", RttiForPublishedProperties.Disable, () => CompilerOptions.PublishedRtti.Value);
            RunCompilerDirective("TYPEINFO ON", RttiForPublishedProperties.Enable, () => CompilerOptions.PublishedRtti.Value);
            RunCompilerDirective("TYPEINFO OFF", RttiForPublishedProperties.Disable, () => CompilerOptions.PublishedRtti.Value);
        }

        [TestMethod]
        public void TestRunOnly() {
            RunCompilerDirective("", RuntimePackageMode.Undefined, () => CompilerOptions.RuntimeOnlyPackage.Value);
            RunCompilerDirective("RUNONLY OFF", RuntimePackageMode.Standard, () => CompilerOptions.RuntimeOnlyPackage.Value);
            RunCompilerDirective("RUNONLY ON", RuntimePackageMode.RuntimeOnly, () => CompilerOptions.RuntimeOnlyPackage.Value);
        }

        [TestMethod]
        public void TestLinkedFiles() {
            RunCompilerDirective("", false, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("L link.dll", true, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("LINK 'link.dll'", true, () => Meta.LinkedFiles.Any(t => string.Equals(t.TargetPath.Path, "link.dll", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestRegion() {
            RunCompilerDirective("", 0, () => Meta.Regions.Count);
            RunCompilerDirective("REGION", 1, () => Meta.Regions.Count);
            RunCompilerDirective("REGION 'XXX' ", "XXX", () => Meta.Regions.Peek());
            RunCompilerDirective("REGION § ENDREGION", 0, () => Meta.Regions.Count);
        }

        [TestMethod]
        public void TestRealCompatibility() {
            RunCompilerDirective("", Real48.Undefined, () => CompilerOptions.RealCompatiblity.Value);
            RunCompilerDirective("REALCOMPATIBILITY ON", Real48.EnableCompatibility, () => CompilerOptions.RealCompatiblity.Value);
            RunCompilerDirective("REALCOMPATIBILITY OFF", Real48.DisableCompatibility, () => CompilerOptions.RealCompatiblity.Value);
        }

        [TestMethod]
        public void TestPointerMath() {
            RunCompilerDirective("", PointerManipulation.Undefined, () => CompilerOptions.PointerMath.Value);
            RunCompilerDirective("POINTERMATH ON", PointerManipulation.EnablePointerMath, () => CompilerOptions.PointerMath.Value);
            RunCompilerDirective("POINTERMATH OFF", PointerManipulation.DisablePointerMath, () => CompilerOptions.PointerMath.Value);
        }

        [TestMethod]
        public void TestOldTypeLayout() {
            RunCompilerDirective("", OldRecordTypes.Undefined, () => CompilerOptions.OldTypeLayout.Value);
            RunCompilerDirective("OLDTYPELAYOUT  ON", OldRecordTypes.EnableOldRecordPacking, () => CompilerOptions.OldTypeLayout.Value);
            RunCompilerDirective("OLDTYPELAYOUT  OFF", OldRecordTypes.DisableOldRecordPacking, () => CompilerOptions.OldTypeLayout.Value);
        }

        [TestMethod]
        public void TestNoDefine() {
            RunCompilerDirective("", false, () => Meta.NoDefines.Any());
            RunCompilerDirective("NODEFINE TDEMO", true, () => Meta.NoDefines.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("NODEFINE TMIMOA", false, () => Meta.NoDefines.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("NODEFINE TMIMOA FUZZ", true, () => Meta.NoDefines.Any(t => t.UnionTypeName.StartsWith("fuz", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void TestObjTypeName() {
            RunCompilerDirective("", false, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("OBJTYPENAME tdemo", true, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("OBJTYPENAME tmemo", false, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("OBJTYPENAME tdemo 'Bul'", true, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("OBJTYPENAME tdemo 'Ntdemo'", true, () => Meta.ObjectFileTypeNames.Any(t => t.TypeName.StartsWith("TDemo", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void TestNoInclude() {
            RunCompilerDirective("", false, () => Meta.NoIncludes.Any(t => t.StartsWith("Winapi", StringComparison.OrdinalIgnoreCase)));
            RunCompilerDirective("NOINCLUDE WinApi.Messages", true, () => Meta.NoIncludes.Any(t => t.StartsWith("Winapi", StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
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

        [TestMethod]
        public void TestMethodInfo() {
            RunCompilerDirective("", MethodInfoRtti.Undefined, () => CompilerOptions.MethodInfo.Value);
            RunCompilerDirective("METHODINFO ON", MethodInfoRtti.EnableMethodInfo, () => CompilerOptions.MethodInfo.Value);
            RunCompilerDirective("METHODINFO OFF", MethodInfoRtti.DisableMethodInfo, () => CompilerOptions.MethodInfo.Value);
        }

        [TestMethod]
        public void TestLibMeta() {
            RunCompilerDirective("", null, () => Meta.LibPrefix.Value);
            RunCompilerDirective("", null, () => Meta.LibSuffix.Value);
            RunCompilerDirective("", null, () => Meta.LibVersion.Value);
            RunCompilerDirective("LIBPREFIX 'P'", "P", () => Meta.LibPrefix.Value);
            RunCompilerDirective("LIBSUFFIX 'M'", "M", () => Meta.LibSuffix.Value);
            RunCompilerDirective("LIBVERSION 'V'", "V", () => Meta.LibVersion.Value);
        }

        [TestMethod]
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
