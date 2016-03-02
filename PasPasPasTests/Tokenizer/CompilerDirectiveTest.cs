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

    }
}
