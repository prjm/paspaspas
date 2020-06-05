#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     common interface for array required expressions
    /// </summary>
    public interface IRequiresArrayExpression {

        /// <summary>
        ///     <c>true if an array is required</c>
        /// </summary>
        bool RequiresArray { get; set; }
    }
}
