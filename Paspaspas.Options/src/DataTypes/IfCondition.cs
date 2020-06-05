#nullable disable
namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     <c>$if</c> condition
    /// </summary>
    public class IfCondition : ICondition {

        /// <summary>
        ///     <c>true</c> if the condition matche
        /// </summary>
        public bool Matches { get; set; }

    }
}
