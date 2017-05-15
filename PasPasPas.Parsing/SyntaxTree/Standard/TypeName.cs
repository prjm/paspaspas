using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type name / reference to a type
    /// </summary>
    public class TypeName : StandardSyntaxTreeBase {

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

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
