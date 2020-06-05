#nullable disable
using System.Collections.Generic;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     a set of conditional compilation directives
    /// </summary>
    public interface IConditionRoot {

        /// <summary>
        ///     parent condition
        /// </summary>
        IConditionBranch Parent { get; }

        /// <summary>
        ///     conditions
        /// </summary>
        IList<IConditionBranch> Conditions { get; }

    }
}