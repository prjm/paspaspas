#nullable disable
namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///  class for an ifdef statement
    /// </summary>
    public class IfDefCondition : ICondition {

        /// <summary>
        ///     test if the condition matches
        /// </summary>
        public bool Matches { get; set; }

        /// <summary>
        ///     Symbol name
        /// </summary>
        public string SymbolName { get; internal set; }
    }
}