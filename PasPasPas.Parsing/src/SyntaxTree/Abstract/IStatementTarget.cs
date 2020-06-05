#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Abstract {
    /// <summary>
    ///     statement target
    /// </summary>
    public interface IStatementTarget {

        /// <summary>
        ///     list of statements
        /// </summary>
        BlockOfStatements Statements { get; }

    }
}
