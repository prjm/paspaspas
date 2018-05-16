namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     conditional branch
    /// </summary>
    public interface IConditionBranch {

        /// <summary>
        ///     parent condition
        /// </summary>
        IConditionRoot Parent { get; }

        /// <summary>
        ///     parent branch
        /// </summary>
        IConditionBranch ParentBranch { get; }

        /// <summary>
        ///     condition itself
        /// </summary>
        ICondition Condition { get; }

        /// <summary>
        ///     tests if the condition matches
        /// </summary>
        bool Matches { get; }

    }
}