using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     <c>concat</c> routine
    /// </summary>
    public class Concat : IntrinsicRoutine, IConstantVariadicRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public override string Name
            => KnownNames.Concat;

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine kind
        /// </summary>
        public RoutineKind Kind
            => RoutineKind.Function;

        /// <summary>
        ///     <c>concat</c> routine id
        /// </summary>
        public override IntrinsicRoutineId RoutineId
            => IntrinsicRoutineId.Concat;

        /// <summary>
        ///     check type parameters
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool CheckParameter(ISignature signature) {
            if (signature.Count < 1)
                return false;

            foreach (var parameter in signature) {

                if (parameter.HasTextType())
                    continue;

                if (parameter.TypeDefinition is ISubrangeType subrangeType && subrangeType.SubrangeOfType.IsTextType())
                    continue;

                return false;
            }

            return true;
        }

        /// <summary>
        ///     execute a call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IValue ExecuteCall(ISignature signature) {
            if (signature.Count < 1)
                return Strings.Invalid;

            var result = Runtime.Strings.EmptyString;

            if (result is ISubrangeValue subrange)
                result = subrange.WrappedValue;

            foreach (var parameter in signature) {

                if (parameter.HasTextType())
                    result = Strings.Concat(result, parameter as IValue);

                else if (parameter is ISubrangeValue subrangeValue && subrangeValue.WrappedValue.HasTextType())
                    result = Strings.Concat(result, subrangeValue);

                else
                    return Strings.Invalid;
            }

            return result;
        }

        /// <summary>
        ///     resolve call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public IIntrinsicInvocationResult ResolveCall(ISignature signature) {
            if (signature.Count == 1) {
                if (signature[0] is ISubrangeType subrangeType)
                    return MakeResult(subrangeType.SubrangeOfType.Reference, subrangeType.SubrangeOfType.Reference);
                else
                    return MakeResult(signature[0], signature[0]);
            }

            var useUnicode = false;

            foreach (var parameter in signature) {
                ITypeDefinition baseType;

                if (parameter.TypeDefinition is ISubrangeType subrangeType)
                    baseType = subrangeType.SubrangeOfType;
                else
                    baseType = parameter.TypeDefinition;

                useUnicode |= baseType is IStringType stringType && //
                    (stringType.Kind == StringTypeKind.UnicodeString || stringType.Kind == StringTypeKind.WideStringType);

                useUnicode |= baseType is ICharType charType && charType.Kind == CharTypeKind.WideChar;

            }

            if (useUnicode)
                return MakeResult(TypeRegistry.SystemUnit.UnicodeStringType.Reference, signature);
            else
                return MakeResult(TypeRegistry.SystemUnit.AnsiStringType.Reference, signature);

        }
    }
}
