using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     interface for expressions
    /// </summary>
    public interface IExpression : ISyntaxPart, ITypedSyntaxNode, IConstantValueNode {

        /// <summary>
        ///     literal value
        /// </summary>
        object LiteralValue { get; }


    }
}