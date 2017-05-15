using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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
        ///     subexpression
        /// </summary>
        /// <remarks>used for string length / string codepage</remarks>
        public IExpression Value { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
                foreach (GenericNameFragment fragment in Fragments)
                    yield return fragment;
            }
        }

        /// <summary>
        ///     creates a new meta typedefinition
        /// </summary>
        public MetaType()
            => Fragments = new SyntaxPartCollection<GenericNameFragment>(this);

        /// <summary>
        ///     name fragements
        /// </summary>
        public ISyntaxPartList<GenericNameFragment> Fragments;

        /// <summary>
        ///     add a afragment
        /// </summary>
        /// <param name="fragment"></param>
        public void AddFragment(GenericNameFragment fragment)
            => Fragments.Add(fragment);

        /// <summary>
        ///     convert a string type kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        [System.Obsolete]
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
