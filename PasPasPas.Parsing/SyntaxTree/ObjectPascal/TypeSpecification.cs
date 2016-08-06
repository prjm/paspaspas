using PasPasPas.Api;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     a type specification
    /// </summary>
    public class TypeSpecification : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public TypeSpecification(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     pointer type
        /// </summary>
        public PointerType PointerType { get; internal set; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureType ProcedureType { get; internal set; }

        /// <summary>
        ///     simple type
        /// </summary>
        public SimpleType SimpleType { get; internal set; }

        /// <summary>
        ///     string type
        /// </summary>
        public StringType StringType { get; internal set; }

        /// <summary>
        ///     structured type
        /// </summary>
        public StructType StructuredType { get; internal set; }

        /// <summary>
        ///     format type specification 
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Part(ProcedureType);
            result.Part(PointerType);
            result.Part(SimpleType);
            result.Part(StringType);
            result.Part(StructuredType);
        }
    }
}