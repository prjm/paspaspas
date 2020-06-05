#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     block target
    /// </summary>
    public interface IBlockTarget {

        /// <summary>
        ///     block
        /// </summary>
        StatementBase Block { get; set; }

    }
}