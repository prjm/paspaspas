using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method specifiers
    /// </summary>
    public class MethodDirectiveSpecifier : AbstractSyntaxPart, IExpressionTarget {

        /// <summary>
        ///     directive kind
        /// </summary>
        public MethodDirectiveSpecifierKind Kind { get; set; }

        /// <summary>
        ///     specified constraints
        /// </summary>
        public IList<IExpression> Constraints { get; }
            = new List<IExpression>();

        /// <summary>
        ///     values
        /// </summary>
        public IExpression Value {
            get {
                return Constraints.LastOrDefault();
            }

            set {
                Constraints.Add(value);
            }
        }

        /// <summary>
        ///     map directive kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static MethodDirectiveSpecifierKind MapKind(int kind) {
            switch (kind) {

                case TokenKind.Name:
                    return MethodDirectiveSpecifierKind.Name;

                case TokenKind.Index:
                    return MethodDirectiveSpecifierKind.Index;

                case TokenKind.Delayed:
                    return MethodDirectiveSpecifierKind.Delayed;

                case TokenKind.Dependency:
                    return MethodDirectiveSpecifierKind.Dependency;

                default:
                    return MethodDirectiveSpecifierKind.Unknown;
            }
        }

        /// <summary>
        ///     expression parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ExpressionBase part in Constraints)
                    yield return part;
            }
        }
    }
}