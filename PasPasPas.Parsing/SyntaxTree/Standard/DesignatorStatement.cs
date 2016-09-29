namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     designator
    /// </summary>
    public class DesignatorStatement : SyntaxPartBase {

        /// <summary>
        ///     <c>@</c> is used as prefix
        /// </summary>
        public bool AddressOf { get; internal set; }

        /// <summary>
        ///     inherited
        /// </summary>
        public bool Inherited { get; set; }

        /// <summary>
        ///     name
        /// </summary>
        public TypeName Name { get; set; }

    }
}