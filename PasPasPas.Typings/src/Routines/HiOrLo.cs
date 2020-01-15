using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     mode
    /// </summary>
    public enum HiLoMode {

        /// <summary>
        ///     undefined mode
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     <pre>hi</pre>
        /// </summary>
        Hi = 1,


        /// <summary>
        ///     <pre>lo</pre>
        /// </summary>
        Lo = 2

    }

    /// <summary>
    ///     <c>hi</c> function
    /// </summary>
    public class HiOrLo : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     create a new hi or lo function
        /// </summary>
        /// <param name="mode"></param>
        public HiOrLo(HiLoMode mode) {
            if (mode == HiLoMode.Undefined)
                throw new ArgumentOutOfRangeException(nameof(mode));

            Mode = mode;
        }

        /// <summary>
        ///     hi or low
        /// </summary>
        private bool Lo
            => Mode == HiLoMode.Lo;

        /// <summary>
        ///     name of the function
        /// </summary>
        public override string Name
            => Lo ? "Lo" : "Hi";

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     mode
        /// </summary>
        public HiLoMode Mode { get; }

        /// <summary>
        ///     procedure kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId {
            get {
                if (Mode == HiLoMode.Hi)
                    return IntrinsicRoutineId.Hi;
                else if (Mode == HiLoMode.Lo)
                    return IntrinsicRoutineId.Lo;
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     check parameter types
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(IOldTypeReference parameter) {
            if (parameter.IsIntegral())
                return true;

            if (IsSubrangeType(parameter.TypeId, out var subrangeType))
                return subrangeType.BaseType.TypeKind.IsIntegral();

            return false;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IOldTypeReference ExecuteCall(IOldTypeReference parameter) {

            if (parameter.IsIntegral())
                if (Lo)
                    return Integers.Lo(parameter);
                else
                    return Integers.Hi(parameter);

            if (parameter.IsSubrangeValue(out var value))
                return ExecuteCall(value.Value);

            return RuntimeException();
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IOldTypeReference ResolveCall(IOldTypeReference parameter)
            => MakeTypeInstanceReference(KnownTypeIds.ByteType);
    }
}
