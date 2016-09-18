namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pointer type specification
    /// </summary>
    public class PointerType : SyntaxPartBase {

        /// <summary>
        ///     true if a generic pointer type is found
        /// </summary>
        public bool GenericPointer { get; set; }
            = false;

        /// <summary>
        ///     type specification for non generic pointers
        /// </summary>
        public TypeSpecification TypeSpecification { get; set; }

    }
}