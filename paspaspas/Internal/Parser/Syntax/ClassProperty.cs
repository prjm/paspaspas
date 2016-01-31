using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     property declaration
    /// </summary>
    public class ClassProperty : ComposedPart<ClassPropertySpecifier> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassProperty(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     property access index
        /// </summary>
        public FormalParameters ArrayIndex { get; internal set; }

        /// <summary>
        ///     index of the property
        /// </summary>
        public ExpressionBase PropertyIndex { get; internal set; }

        /// <summary>
        ///     property name
        /// </summary>
        public PascalIdentifier PropertyName { get; internal set; }

        /// <summary>
        ///     property type
        /// </summary>
        public NamespaceName TypeName { get; internal set; }

        /// <summary>
        ///     format property
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            result.Keyword("property");
            result.Space();
            PropertyName.ToFormatter(result);

            if (ArrayIndex != null) {
                result.Space();
                result.Punct("[");
                result.Space();
                ArrayIndex.ToFormatter(result);
                result.Space();
                result.Punct("]");
                result.Space();
            }

            if (TypeName != null) {
                result.Space();
                result.Punct(":");
                result.Space();
                TypeName.ToFormatter(result);
                result.Space();
            }

            if (PropertyIndex != null) {
                result.Space();
                result.Keyword("index");
                result.Space();
                PropertyIndex.ToFormatter(result);
                result.Space();
            }

            FlattenToPascal(result, x => x.Space());
            result.Punct(";");
            result.NewLine();
        }
    }
}