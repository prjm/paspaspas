using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     switch to toggle assertions
    /// </summary>
    public class AssertSwitch : SyntaxPartBase {

        /// <summary>
        ///     assertion mode
        /// </summary>
        public AssertionMode Assertions { get; set; }
    }
}
