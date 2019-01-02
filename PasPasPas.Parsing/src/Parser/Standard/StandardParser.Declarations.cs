using PasPasPas.Parsing.SyntaxTree;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.Parser.Standard {
    public partial class StandardParser {

        #region ParseStructType

        /// <summary>
        ///     parse a structured type
        /// </summary>
        /// <returns></returns>
        [Rule("StructType", "[ 'packed' ] StructTypePart")]
        public StructTypeSymbol ParseStructType() {
            var packed = ContinueWith(TokenKind.Packed);
            var part = ParseStructTypePart();
            return new StructTypeSymbol(packed, part);
        }

        #endregion
        #region ParseStructTypePart

        [Rule("StructTypePart", "ArrayType | SetType | FileType | ClassDecl")]
        private StructTypePart ParseStructTypePart() {

            if (Match(TokenKind.Array))
                return new StructTypePart(ParseArrayType());

            if (Match(TokenKind.Set))
                return new StructTypePart(ParseSetDefinition());

            if (Match(TokenKind.File))
                return new StructTypePart(ParseFileType());

            if (Match(TokenKind.Class, TokenKind.Interface, TokenKind.Record, TokenKind.ObjectKeyword, TokenKind.DispInterface))
                return new StructTypePart(ParseClassDeclaration());

            return null;
        }

        #endregion
        #region SetDefinition

        /// <summary>
        ///     parse a set definition
        /// </summary>
        /// <returns></returns>

        [Rule("SetDef", "'set' 'of' TypeSpecification")]
        public SetDefinitionSymbol ParseSetDefinition() {
            var setSymbol = ContinueWithOrMissing(TokenKind.Set);
            var ofSymbol = ContinueWithOrMissing(TokenKind.Of);
            var typeDefinition = ParseTypeSpecification();
            return new SetDefinitionSymbol(setSymbol, ofSymbol, typeDefinition);
        }

        #endregion
        #region ParseSimpleType

        /// <summary>
        ///     parse a simple type
        /// </summary>
        /// <param name="constDeclaration"></param>
        /// <param name="varDeclaration"></param>
        /// <returns></returns>

        [Rule("SimpleType", "EnumType | (ConstExpression [ '..' ConstExpression ]) | ([ 'type' ] GenericNamespaceName {'.' GenericNamespaceName })")]
        public SimpleTypeSymbol ParseSimpleType(bool constDeclaration = false, bool varDeclaration = false) {

            if (Match(TokenKind.OpenParen)) {
                return new SimpleTypeSymbol(ParseEnumType());
            }

            var newType = default(Terminal);
            var typeOf = default(Terminal);

            if (!varDeclaration) {
                newType = ContinueWith(TokenKind.TypeKeyword);

                if (newType != default)
                    typeOf = ContinueWith(TokenKind.Of);
            }
            else {
                if (Match(TokenKind.TypeKeyword)) {
                    Unexpected();
                }
            }

            if (newType != default || (MatchIdentifier(TokenKind.ShortString, TokenKind.StringKeyword, TokenKind.WideString, TokenKind.UnicodeString, TokenKind.AnsiString) && (!LookAhead(1, TokenKind.DotDot)))) {
                using (var list = GetList<GenericNamespaceNameSymbol>()) {
                    var item = default(GenericNamespaceNameSymbol);
                    var openParen = default(Terminal);
                    var closeParen = default(Terminal);
                    var codePage = default(ConstantExpressionSymbol);

                    if (newType != default && Match(TokenKind.AnsiString)) {
                        AddToList(list, ParseGenericNamespaceName(false, false, false));
                        openParen = ContinueWith(TokenKind.OpenParen);
                        if (openParen != default) {
                            codePage = ParseConstantExpression(false, false, false);
                            closeParen = ContinueWith(TokenKind.CloseParen);
                        }
                    }
                    else {
                        do {
                            item = AddToList(list, ParseGenericNamespaceName(false, false, true));
                        } while (item != default && item.Dot != default);
                    }

                    return new SimpleTypeSymbol(newType, typeOf, GetFixedArray(list), openParen, codePage, closeParen);
                }
            }

            var subrangeStart = ParseConstantExpression(false, constDeclaration);
            var subrangeEnd = default(ConstantExpressionSymbol);
            var dotDot = ContinueWith(TokenKind.DotDot);
            if (dotDot != default) {
                subrangeEnd = ParseConstantExpression(false, constDeclaration);
            }

            return new SimpleTypeSymbol(newType, typeOf, subrangeStart, dotDot, subrangeEnd);
        }

        #endregion

    }
}
