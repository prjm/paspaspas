using Microsoft.VisualStudio.TestTools.UnitTesting;
using PasPasPas.Api.Options;

namespace PasPasPasTests.Tokenizer {

    [TestClass]
    public class CompilerDirectiveTest : ParserTestBase {

        [TestMethod]
        public void TestAlign() {
            RunCompilerDirective("", Alignment.Undefined, () => TestOptions.Align.Value);
            RunCompilerDirective("A+", Alignment.QuadWord, () => TestOptions.Align.Value);
            RunCompilerDirective("A-", Alignment.Unaligned, () => TestOptions.Align.Value);
            RunCompilerDirective("A1", Alignment.Unaligned, () => TestOptions.Align.Value);
            RunCompilerDirective("A2", Alignment.Word, () => TestOptions.Align.Value);
            RunCompilerDirective("A4", Alignment.DoubleWord, () => TestOptions.Align.Value);
            RunCompilerDirective("A8", Alignment.QuadWord, () => TestOptions.Align.Value);
            RunCompilerDirective("A16", Alignment.DoubleQuadWord, () => TestOptions.Align.Value);
            RunCompilerDirective("ALIGN ON", Alignment.QuadWord, () => TestOptions.Align.Value);
            RunCompilerDirective("ALIGN OFF", Alignment.Unaligned, () => TestOptions.Align.Value);
            RunCompilerDirective("ALIGN 1", Alignment.Unaligned, () => TestOptions.Align.Value);
            RunCompilerDirective("ALIGN 2", Alignment.Word, () => TestOptions.Align.Value);
            RunCompilerDirective("ALIGN 4", Alignment.DoubleWord, () => TestOptions.Align.Value);
            RunCompilerDirective("ALIGN 8", Alignment.QuadWord, () => TestOptions.Align.Value);
            RunCompilerDirective("ALIGN 16", Alignment.DoubleQuadWord, () => TestOptions.Align.Value);
        }

        [TestMethod]
        public void TestApptype() {
            RunCompilerDirective("", AppType.Undefined, () => TestOptions.ApplicationType.Value);
            RunCompilerDirective("APPTYPE GUI", AppType.Gui, () => TestOptions.ApplicationType.Value);
            RunCompilerDirective("APPTYPE CONSOLE", AppType.Console, () => TestOptions.ApplicationType.Value);
        }

        [TestMethod]
        public void TestBoolEvalSwitch() {
            RunCompilerDirective("", BooleanEvaluation.Undefined, () => TestOptions.BoolEval.Value);
            RunCompilerDirective("B+", BooleanEvaluation.CompleteEvaluation, () => TestOptions.BoolEval.Value);
            RunCompilerDirective("B-", BooleanEvaluation.ShortEvaluation, () => TestOptions.BoolEval.Value);
            RunCompilerDirective("BOOLEVAL ON", BooleanEvaluation.CompleteEvaluation, () => TestOptions.BoolEval.Value);
            RunCompilerDirective("BOOLEVAL OFF", BooleanEvaluation.ShortEvaluation, () => TestOptions.BoolEval.Value);
        }

    }
}
