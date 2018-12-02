namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     conditional branch
    /// </summary>
    public class ConditionBranch : IConditionBranch {

        /// <summary>
        ///     create a new branch
        /// </summary>
        /// <param name="root"></param>
        /// <param name="condition"></param>
        public ConditionBranch(IConditionRoot root, ICondition condition) {
            Parent = root;
            Condition = condition;
        }

        /// <summary>
        ///     condition
        /// </summary>
        public ICondition Condition { get; }

        /// <summary>
        ///     test if the condition matches
        /// </summary>
        public bool Matches
            => Condition != null ? Condition.Matches : false;

        /// <summary>
        ///     parent condition branch set
        /// </summary>
        public IConditionRoot Parent { get; }

        /// <summary>
        ///     parent branch
        /// </summary>
        public IConditionBranch ParentBranch
            => Parent?.Parent;
    }
}