namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     simple type definiion
    /// </summary>
    public class SimpleType : SyntaxPartBase {

        /// <summary>
        ///     enumeration
        /// </summary>
        public EnumTypeDefinition EnumType { get; set; }

        /// <summary>
        ///     generic postfix
        /// </summary>
        public GenericTypeSuffix GenericPostfix { get; set; }

        /// <summary>
        ///     <c>true</c> for a new type definition
        /// </summary>
        public bool NewType { get; internal set; }

        /// <summary>
        ///     subrange start
        /// </summary>
        public ConstantExpression SubrangeEnd { get; set; }

        /// <summary>
        ///     subrange end
        /// </summary>
        public ConstantExpression SubrangeStart { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public NamespaceName TypeId { get; set; }
    }
}