using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     special type
    /// </summary>
    public class MetaType : TypeSpecificationBase, IExpressionTarget {

        /// <summary>
        ///     type kind
        /// </summary>
        public MetaTypeKind Kind { get; set; }
            = MetaTypeKind.Undefined;

        /// <summary>
        ///     type name
        /// </summary>
        /// <remarks>used for class of xx</remarks>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     subexpression
        /// </summary>
        /// <remarks>used for string length / string codepage</remarks>
        public ExpressionBase Value { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                if (Value != null)
                    yield return Value;
            }
        }

        /// <summary>
        ///     convert a string type kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static MetaTypeKind ConvertKind(int kind) {
            switch (kind) {
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
                default:
                    return MetaTypeKind.Undefined;
            }
        }
    }
}
