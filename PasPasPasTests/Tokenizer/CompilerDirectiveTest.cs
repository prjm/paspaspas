using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Api.Options;

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

    }
}
