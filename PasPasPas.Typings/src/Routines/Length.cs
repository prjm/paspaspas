using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>length</c> of strings or arrays
    /// </summary>
    public class Length : IntrinsicRoutine, IUnaryRoutine {

        /// <summary>
        ///     routine name <c>Length</c>
        /// </summary>
        public override string Name
            => "Length";

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     check parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CheckParameter(ITypeReference parameter) {

            if (parameter.IsString())
                return true;

            if (parameter.IsChar())
                return true;

            return false;
        }

        /// <summary>
        ///     get the length of a string value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ExecuteCall(ITypeReference parameter) {

            if (parameter.IsStringValue(out var stringValue))
                return Integers.ToScaledIntegerValue(stringValue.NumberOfCharElements);

            if (parameter.IsChar())
                return Integers.ToScaledIntegerValue(1);

            var x = $"{3}";

            return RuntimeException();
        }

        /// <summary>
        ///   resolve the call
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(ITypeReference parameter)
            => MakeTypeInstanceReference(KnownTypeIds.IntegerType);
    }
}
