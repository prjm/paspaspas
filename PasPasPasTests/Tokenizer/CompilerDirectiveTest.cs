using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Options.DataTypes;
using System.Collections.Generic;
using System.Linq;

namespace PasPasPasTests.Tokenizer {

    [TestClass]
    public class CompilerDirectiveTest : ParserTestBase {

        [TestMethod]
        public void TestAlign() {
            RunCompilerDirective("", Alignment.Undefined, () => CompilerOptions.Align.Value);
            RunCompilerDirective("A+", Alignment.QuadWord, () => CompilerOptions.Align.Value);
            RunCompilerDirective("A-", Alignment.Unaligned, () => CompilerOptions.Align.Value);
            RunCompilerDirective("A1", Alignment.Unaligned, () => CompilerOptions.Align.Value);
            RunCompilerDirective("A2", Alignment.Word, () => CompilerOptions.Align.Value);
            RunCompilerDirective("A4", Alignment.DoubleWord, () => CompilerOptions.Align.Value);
            RunCompilerDirective("A8", Alignment.QuadWord, () => CompilerOptions.Align.Value);
            RunCompilerDirective("A16", Alignment.DoubleQuadWord, () => CompilerOptions.Align.Value);
            RunCompilerDirective("ALIGN ON", Alignment.QuadWord, () => CompilerOptions.Align.Value);
            RunCompilerDirective("ALIGN OFF", Alignment.Unaligned, () => CompilerOptions.Align.Value);
            RunCompilerDirective("ALIGN 1", Alignment.Unaligned, () => CompilerOptions.Align.Value);
            RunCompilerDirective("ALIGN 2", Alignment.Word, () => CompilerOptions.Align.Value);
            RunCompilerDirective("ALIGN 4", Alignment.DoubleWord, () => CompilerOptions.Align.Value);
            RunCompilerDirective("ALIGN 8", Alignment.QuadWord, () => CompilerOptions.Align.Value);
            RunCompilerDirective("ALIGN 16", Alignment.DoubleQuadWord, () => CompilerOptions.Align.Value);
        }

        [TestMethod]
        public void TestApptype() {
            RunCompilerDirective("", AppType.Undefined, () => CompilerOptions.ApplicationType.Value);
            RunCompilerDirective("APPTYPE GUI", AppType.Gui, () => CompilerOptions.ApplicationType.Value);
            RunCompilerDirective("APPTYPE CONSOLE", AppType.Console, () => CompilerOptions.ApplicationType.Value);
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
            RunCompilerDirective("", ConstantValues.Undefined, () => CompilerOptions.WriteableConstants.Value);
            RunCompilerDirective("J-", ConstantValues.Constant, () => CompilerOptions.WriteableConstants.Value);
            RunCompilerDirective("J+", ConstantValues.Writeable, () => CompilerOptions.WriteableConstants.Value);
            RunCompilerDirective("WRITEABLECONST  OFF", ConstantValues.Constant, () => CompilerOptions.WriteableConstants.Value);
            RunCompilerDirective("WRITEABLECONST  ON", ConstantValues.Writeable, () => CompilerOptions.WriteableConstants.Value);
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
        public void TestLibMeta() {
            RunCompilerDirective("", null, () => Meta.LibPrefix.Value);
            RunCompilerDirective("", null, () => Meta.LibSuffix.Value);
            RunCompilerDirective("", null, () => Meta.LibVersion.Value);
            RunCompilerDirective("LIBPREFIX 'P'", "P", () => Meta.LibPrefix.Value);
            RunCompilerDirective("LIBSUFFIX 'M'", "M", () => Meta.LibSuffix.Value);
            RunCompilerDirective("LIBVERSION 'V'", "V", () => Meta.LibVersion.Value);
        }

    }
}
