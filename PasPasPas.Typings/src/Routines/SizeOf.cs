﻿using PasPasPas.Globals.Runtime;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <pre>SizeOf</pre> routine
    /// </summary>
    public class SizeOf : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine name <pre>SizeOf</pre>
        /// </summary>
        public override string Name
            => "SizeOf";

        /// <summary>
        ///     check parameters
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter)
            => true;

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {
            var type = TypeRegistry.GetTypeByIdOrUndefinedType(parameter.TypeId);
            return Integers.ToScaledIntegerValue(type.TypeSizeInBytes);
        }

        /// <summary>
        ///     resolve type definition
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => ExecuteCall(parameter);
    }
}