using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     property accessor kind
    /// </summary>
    public class StructurePropertyAccessor : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     accessor kind
        /// </summary>
        public StructurePropertyAccessorKind Kind { get; set; }

        /// <summary>
        ///     accessor member name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     expression for disp ids
        /// </summary>
        public ExpressionBase Value { get; set; }

        /// <summary>
        ///     map accessor lind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns>mapped kind</returns>
        public static StructurePropertyAccessorKind MapKind(int kind) {

            switch (kind) {

                case TokenKind.Read:
                    return StructurePropertyAccessorKind.Read;

                case TokenKind.Write:
                    return StructurePropertyAccessorKind.Write;

                case TokenKind.Add:
                    return StructurePropertyAccessorKind.Add;

                case TokenKind.Remove:
                    return StructurePropertyAccessorKind.Remove;

                default:
                    return StructurePropertyAccessorKind.Remove;


            }
        }

        /// <summary>
        ///     enumerate party
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
            }
        }
    }
}
