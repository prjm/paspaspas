using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     syntax tree element to change alignment
    /// </summary>
    public class AlignSwitch : SyntaxPartBase {

        /// <summary>
        ///     new align setting
        /// </summary>
        public Alignment AlignValue { get; set; }

    }
}
