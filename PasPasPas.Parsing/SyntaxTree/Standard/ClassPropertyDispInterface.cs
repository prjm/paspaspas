namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property accessor for disp interfaces
    /// </summary>
    public class ClassPropertyDispInterface : SyntaxPartBase {

        /// <summary>
        ///     Disp id directive
        /// </summary>
        public DispIdDirective DispId { get; set; }

        /// <summary>
        ///     readonly
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        ///    write only
        /// </summary>
        public bool WriteOnly { get; set; }

    }
}