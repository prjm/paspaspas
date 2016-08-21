using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     type checked pointers directive
    /// </summary>
    public class TypedPointers : SyntaxPartBase {

        /// <summary>
        ///     switch mode
        /// </summary>
        public TypeCheckedPointers Mode { get; set; }
    }
}
