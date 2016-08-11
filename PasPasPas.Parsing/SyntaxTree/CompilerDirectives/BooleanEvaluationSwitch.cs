using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     boolean evaluation switch
    /// </summary>
    public class BooleanEvaluationSwitch : SyntaxPartBase {

        /// <summary>
        ///     boolean evaluation mode
        /// </summary>
        public BooleanEvaluation BoolEval { get; set; }
    }
}
