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
        public ISyntaxPartCollection<ParameterDefinition> Parameters { get; }

        /// <summary>
        ///     creates a new set of parameter definitions
        /// </summary>
        public ParameterTypeDefinition()
            => Parameters = new SyntaxPartCollection<ParameterDefinition>();

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
            AcceptPart(this, Parameters, visitor);
            AcceptPart(this, TypeValue, visitor);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }

    }
}