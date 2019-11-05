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
            => "Concat";

        /// <summary>
        ///     constant routine
        /// </summary>
        public bool IsConstant
            => true;

        /// <summary>
        ///     routine kind
        /// </summary>
        public ProcedureKind Kind
            => ProcedureKind.Function;

        /// <summary>
        ///     check type parameters
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public bool CheckParameter(Signature signature) {
            if (signature.Length < 1)
                return false;

            for (var i = 0; i < signature.Length; i++) {
                if (signature[i].IsTextual())
                    continue;

                if (IsSubrangeType(signature[i].TypeId, out var subrangeType) && subrangeType.BaseType.TypeKind.IsTextual())
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
        public ITypeReference ExecuteCall(Signature signature) {
            if (signature.Length < 1)
                return RuntimeException();

            var result = signature[0];

            if (result.IsSubrangeValue(out var value))
                result = value.Value;

            for (var i = 1; i < signature.Length; i++) {

                if (signature[i].IsTextual())
                    result = Strings.Concat(result, signature[i]);

                else if (signature[i].IsSubrangeValue(out value))
                    result = Strings.Concat(result, value.Value);

                else
                    return RuntimeException();
            }

            return result;
        }

        /// <summary>
        ///     resolve call
        /// </summary>
        /// <param name="signature"></param>
        /// <returns></returns>
        public ITypeReference ResolveCall(Signature signature) {
            if (signature.Length == 1) {
                if (IsSubrangeType(signature[0].TypeId, out var subrangeType))
                    return MakeTypeInstanceReference(subrangeType.BaseTypeId);
                else
                    return MakeTypeInstanceReference(signature[0].TypeId);
            }

            var useUnicode = false;

            for (var i = 0; i < signature.Length; i++) {
                if (IsSubrangeType(signature[i].TypeId, out var subrangeType))
                    useUnicode |= subrangeType.BaseType.TypeKind.IsUnicodeText();
                else
                    useUnicode |= signature[i].IsUnicodeText();
            }

            if (useUnicode)
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.UnicodeStringType);
            else
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.AnsiStringType);

        }
    }
}
