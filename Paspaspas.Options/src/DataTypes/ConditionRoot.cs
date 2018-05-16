using System.Collections.Generic;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     root for conditional branches
    /// </summary>
    public class ConditionRoot : IConditionRoot {

        private IConditionBranch parent;

        /// <summary>
        ///     create a new condition root
        /// </summary>
        /// <param name="parentBranch">parent branch</param>
        public ConditionRoot(IConditionBranch parentBranch) {
            parent = parentBranch;
        }

        /// <summary>
        ///     conditions
        /// </summary>
        public IList<IConditionBranch> Conditions { get; }
            = new List<IConditionBranch>();

        /// <summary>
        ///     parent branch
        /// </summary>
        public IConditionBranch Parent
             => parent;

    }
}