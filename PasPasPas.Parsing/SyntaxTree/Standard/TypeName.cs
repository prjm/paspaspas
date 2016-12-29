using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type name / reference to a type
    /// </summary>
    public class TypeName : SyntaxPartBase {

        /// <summary>
        ///     string type
        /// </summary>
        public int StringType { get; set; }
            = TokenKind.Undefined;

        /// <summary>
        ///     map typ name kind
        /// </summary>
        /// <returns></returns>
        public MetaTypeKind MapTypeKind() {
            switch (StringType) {
                case TokenKind.String:
                    return MetaTypeKind.String;

                case TokenKind.AnsiString:
                    return MetaTypeKind.AnsiString;

                case TokenKind.ShortString:
                    return MetaTypeKind.ShortString;

                case TokenKind.WideString:
                    return MetaTypeKind.WideString;

                case TokenKind.UnicodeString:
                    return MetaTypeKind.UnicodeString;
            }

            return MetaTypeKind.NamedType;
        }
    }
}
