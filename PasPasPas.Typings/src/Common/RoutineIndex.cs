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
        public RoutineIndex(IRoutineGroup routine, int index) {
            Routine = routine;
            Index = index;
        }

        /// <summary>
        ///     routine
        /// </summary>
        public IRoutineGroup Routine { get; }

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
            => Routine.Items[Index].IsClassItem;

        /// <summary>
        ///     parameters
        /// </summary>
        public IRoutine Parameters
            => Routine.Items[Index];

        /// <summary>
        ///     create a signature
        /// </summary>
        /// <param name="registeredTypes"></param>
        /// <returns></returns>
        public Signature CreateSignature(ITypeRegistry registeredTypes)
            => Routine.Items[Index].CreateSignature(registeredTypes);

        /// <summary>
        ///     routine name
        /// </summary>
        public string Name
            => Routine.Name;

    }
}
