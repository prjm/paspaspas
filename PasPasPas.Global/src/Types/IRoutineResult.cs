using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Globals {

    /// <summary>
    ///     result of invoking a routine
    /// </summary>
    public interface IRoutineResult : ITypeSymbol {

        /// <summary>
        ///     get the called routine
        /// </summary>
        IRoutineGroup Routine { get; }
    }
}








