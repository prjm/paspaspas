using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Api.Options;
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
            //RunCompilerDirective("", 0, () => Meta.ExternalSymbols.Count);
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

    }
}
