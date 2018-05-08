using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formal parameter definition
    /// </summary>
    public class ParameterTypeDefinition : AbstractSyntaxPartBase, ITypeTarget, IExpressionTarget {

        /// <summary>
        ///     parameter type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     parameter definitions
        /// </summary>
        public ISyntaxPartList<ParameterDefinition> Parameters { get; }

        /// <summary>
        ///     creates a new set of parameter definitions
        /// </summary>
        public ParameterTypeDefinition()
            => Parameters = new SyntaxPartCollection<ParameterDefinition>(this);

        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var parameter in Parameters)
                    yield return parameter;
                if (TypeValue != null)
                    yield return TypeValue;
                if (Value != null)
                    yield return Value;
            }
        }

        /// <summary>
        ///     default value
        /// </summary>
        public IExpression Value { get; set; }

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