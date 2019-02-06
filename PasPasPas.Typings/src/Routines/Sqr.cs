﻿using PasPasPas.Globals.Runtime;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <pre>sqr</pre> routine
    /// </summary>
    public class Sqr : IntrinsicRoutine, IUnaryRoutine {


        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => "Sqr";

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter)
            => parameter.IsNumerical();

        /// <summary>
        ///     execute the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {
            if (parameter.IsIntegralValue(out var _))
                return Integers.Multiply(parameter, parameter);

            if (parameter.IsRealValue(out var _))
                return RealNumbers.Multiply(parameter, parameter);

            if (parameter.IsSubrangeValue(out var value))
                return ExecuteCall(value.Value);

            return RuntimeException();
        }

        /// <summary>
        ///     resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(parameter.TypeId);
    }
}