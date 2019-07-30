using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     condition for ifopt
    /// </summary>
    public class IfOptCondition : ICondition {

        /// <summary>
        ///     <c>true</c> if the condition matches
        /// </summary>
        public bool Matches { get; set; }


        /// <summary>
        ///     required switch state
        /// </summary>
        public SwitchInfo RequiredCondition { get; set; }

        /// <summary>
        ///     switch name
        /// </summary>
        public string SwitchName { get; set; }
    }
}