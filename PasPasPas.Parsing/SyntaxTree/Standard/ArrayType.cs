namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayType : SyntaxPartBase {

        /// <summary>
        ///     true if the array is of type <c>array of const</c>
        /// </summary>
        public bool ArrayOfConst { get; set; }

        /// <summary>
        ///     array type specification
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

    }
}