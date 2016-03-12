using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     class property specifier
    /// </summary>
    public class ClassPropertySpecifier : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassPropertySpecifier(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     default property
        /// </summary>
        public bool IsDefault { get; internal set; }

        /// <summary>
        ///     default property expression
        /// </summary>
        public Expression DefaultProperty { get; internal set; }

        /// <summary>
        ///     dispinterface
        /// </summary>
        public ClassPropertyDispInterface PropertyDispInterface { get; internal set; }

        /// <summary>
        ///     read write accessor
        /// </summary>
        public ClassPropertyReadWrite PropertyReadWrite { get; internal set; }

        /// <summary>
        ///     stored property
        /// </summary>
        public bool IsStored { get; internal set; }

        /// <summary>
        ///     stored property expression
        /// </summary>
        public Expression StoredProperty { get; internal set; }

        /// <summary>
        ///     no default
        /// </summary>
        public bool NoDefault { get; internal set; }

        /// <summary>
        ///     implementing type name
        /// </summary>
        public NamespaceName ImplementsTypeId { get; internal set; }

        /// <summary>
        ///     format property specifier
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            if (PropertyReadWrite != null) {
                PropertyReadWrite.ToFormatter(result);
                return;
            }

            if (PropertyDispInterface != null) {
                PropertyDispInterface.ToFormatter(result);
                return;
            }

            if (IsDefault) {
                result.Keyword("default");
                result.Space();
                if (DefaultProperty != null)
                    DefaultProperty.ToFormatter(result);
                result.Punct(";");
                return;
            }

            if (IsStored) {
                result.Keyword("stored");
                result.Space();
                StoredProperty.ToFormatter(result);
                result.Punct(";");
                return;
            }

            if (NoDefault) {
                result.Keyword("nodefault");
                result.Punct(";");
                return;
            }

            result.Keyword("implements");
            result.Space();
            ImplementsTypeId.ToFormatter(result);
        }
    }
}