namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     interface definition
    /// </summary>
    public class InterfaceDefinition : SyntaxPartBase {

        /// <summary>
        ///     <c>true</c> if dispinterface
        /// </summary>
        public bool DispInterface { get; set; }

        /// <summary>
        ///     guid declaration
        /// </summary>
        public InterfaceGuid Guid { get; set; }

        /// <summary>
        ///     interface items
        /// </summary>
        public InterfaceItems Items { get; set; }

        /// <summary>
        ///     parent interface
        /// </summary>
        public ParentClass ParentInterface { get; set; }

    }
}