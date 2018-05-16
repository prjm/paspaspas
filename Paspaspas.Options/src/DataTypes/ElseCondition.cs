namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     condition for <c>#ELSE</c>
    /// </summary>
    public class ElseCondition : ICondition {

        /// <summary>
        ///     <c>true</c> if the condition matche
        /// </summary>
        public bool Matches { get; set; }
    }
}