using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     index for routines
    /// </summary>
    public class RoutineIndex {

        /// <summary>
        ///     create a new routine index
        /// </summary>
        /// <param name="routine"></param>
        /// <param name="index"></param>
        public RoutineIndex(IRoutine routine, int index) {
            Routine = routine;
            Index = index;
        }

        /// <summary>
        ///     routine
        /// </summary>
        public IRoutine Routine { get; }

        /// <summary>
        ///     index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     defining type
        /// </summary>
        public int DefiningType
            => Routine.DefiningType;

        /// <summary>
        ///     test if this a class routine
        /// </summary>
        public bool IsClassItem
            => Routine.Parameters[Index].IsClassItem;

        /// <summary>
        ///     create a signature
        /// </summary>
        /// <returns></returns>
        public Signature CreateSignature(IRuntimeValueFactory runtime)
            => Routine.Parameters[Index].CreateSignature(runtime);

        /// <summary>
        ///     routine name
        /// </summary>
        public string Name
            => Routine.Name;
    }
}
