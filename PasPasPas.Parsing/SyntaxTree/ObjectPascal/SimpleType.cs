using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     simple type definiion
    /// </summary>
    public class SimpleType : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public SimpleType(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     enumeration
        /// </summary>
        public EnumTypeDefinition EnumType { get; internal set; }

        /// <summary>
        ///     generic postfix
        /// </summary>
        public GenericTypesuffix GenericPostfix { get; internal set; }

        /// <summary>
        ///     <c>true</c> for a new type definition
        /// </summary>
        public bool NewType { get; internal set; }

        /// <summary>
        ///     subrange start
        /// </summary>
        public ConstantExpression SubrangeEnd { get; internal set; }

        /// <summary>
        ///     subrange end
        /// </summary>
        public ConstantExpression SubrangeStart { get; internal set; }

        /// <summary>
        ///     type id
        /// </summary>
        public NamespaceName TypeId { get; internal set; }

        /// <summary>
        ///     format type
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (EnumType != null) {
                result.Part(EnumType);
                return;
            }

            if (SubrangeStart != null) {
                result.Part(SubrangeStart);
                if (SubrangeEnd != null) {
                    result.Operator("..");
                    result.Part(SubrangeEnd);
                }
                return;
            }

            if (NewType)
                result.Keyword("type").Space();
            result.Part(TypeId);
            result.Part(GenericPostfix);
        }
    }
}