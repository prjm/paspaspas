using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method directive
    /// </summary>
    public class MethodDirective : AbstractSyntaxPartBase, IExpressionTarget {

        /// <summary>
        ///     kind
        /// </summary>
        public MethodDirectiveKind Kind { get; set; }

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     directive specifiers
        /// </summary>
        public ISyntaxPartCollection<MethodDirectiveSpecifier> Specifiers { get; }

        /// <summary>
        ///     create a new method directive
        /// </summary>
        public MethodDirective()
            => Specifiers = new SyntaxPartCollection<MethodDirectiveSpecifier>(this);

        /// <summary>
        ///     enumerate parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
                foreach (var specifier in Specifiers)
                    yield return specifier;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
