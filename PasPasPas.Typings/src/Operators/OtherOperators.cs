using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     address-of operator
    /// </summary>
    public class AddressOfOperator : OperatorBase {

        /// <summary>
        ///     create a new address-of operator
        /// </summary>
        public AddressOfOperator() : base(DefinedOperators.AtOperator, 1) {
        }

        /// <summary>
        ///     operator name
        /// </summary>
        public override string Name
            => "@";

        /// <summary>
        ///     evaluate this operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeReference EvaluateUnaryOperator(Signature input) {
            var operand = input[0];
            return Runtime.MakePointerValue(operand);
        }
    }

    /// <summary>
    ///     other operators
    /// </summary>
    public static class OtherOperators {

        /// <summary>
        ///     register operator
        /// </summary>
        /// <param name="registeredTypes"></param>
        internal static void RegisterOperators(RegisteredTypes registeredTypes)
            => registeredTypes.RegisterOperator(new AddressOfOperator());
    }
}
