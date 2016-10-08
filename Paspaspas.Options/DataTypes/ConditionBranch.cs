namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     conidtional branch
    /// </summary>
    public class ConditionBranch : IConditionBranch {

        private ICondition condition;
        private IConditionRoot root;

        /// <summary>
        ///     create a new branch
        /// </summary>
        /// <param name="root"></param>
        /// <param name="condition"></param>
        public ConditionBranch(IConditionRoot root, ICondition condition) {
            this.root = root;
            this.condition = condition;
        }

        /// <summary>
        ///     condition
        /// </summary>
        public ICondition Condition
            => condition;

        /// <summary>
        ///     test if the condition matches
        /// </summary>
        public bool Matches
            => condition != null ? condition.Matches : false;

        /// <summary>
        ///     parent condition branch set
        /// </summary>
        public IConditionRoot Parent
            => root;

        /// <summary>
        ///     parent branch
        /// </summary>      
        public IConditionBranch ParentBranch
            => root != null ? root.Parent : null;
    }
}