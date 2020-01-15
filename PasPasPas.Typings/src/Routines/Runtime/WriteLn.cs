using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines.Runtime {

    /// <summary>
    ///     <c>writeln</c> routine
    /// </summary>
    public class WriteLn : IntrinsicRoutine, IVariadicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.WriteLn;

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Procedure;

        /// <summary>
        ///     dynamic routine
        /// </summary>
        public bool IsConstant
            => false;

        /// <summary>
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.WriteLn;

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool CheckParameter(Signature signature) {
            if (signature.Length < 1)
                return true;

            for (var i = 0; i < signature.Length; i++)
                if (!IsWriteLnType(signature[i]))
                    return false;

            return true;
        }

        private bool IsWriteLnType(IOldTypeReference typeReference) {
            var typeKind = typeReference.TypeKind;

            if (typeKind.IsString())
                return true;

            if (typeKind.IsChar())
                return true;

            return false;
        }

        internal override void CreateParameters() {
            var p = new Routine(this, RoutineKind.Procedure, TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.NoType));
            Items.Add(p);
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IOldTypeReference ResolveCall(Signature signature) {
            var kind = MakeTypeInstanceReference(KnownTypeIds.NoType);
            return Types.MakeInvocationResultFromIntrinsic(this, new Routine(this, RoutineKind.Procedure, kind));
        }
    }
}
