#nullable disable
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for variables
    /// </summary>
    public interface IVariable : INamedTypeSymbol {

        /// <summary>
        ///     visibility
        /// </summary>
        MemberVisibility Visibility { get; }

        /// <summary>
        ///     variable kind
        /// </summary>
        VariableKind Kind { get; }

        /// <summary>
        ///     flags
        /// </summary>
        VariableFlags Flags { get; }

    }

    /// <summary>
    ///     helper functions for routines
    /// </summary>
    public static class VariableFlagsHelper {

        /// <summary>
        ///     test if the variable is a class routine
        /// </summary>
        /// <param name="variable"></param>
        /// <returns></returns>
        public static bool IsClassItem(this IVariable variable)
            => (variable.Flags & VariableFlags.ClassItem) == VariableFlags.ClassItem;
    }

}