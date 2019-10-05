using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     class for a parameter group
    /// </summary>
    public class ParameterGroup : IParameterGroup {

        /// <summary>
        ///     result type
        /// </summary>
        public ITypeReference ResultType { get; set; }

        /// <summary>
        ///     routine parameters
        /// </summary>
        public IList<IVariable> Parameters { get; private set; }

        /// <summary>
        ///     class item
        /// </summary>
        public bool IsClassItem { get; set; }

        /// <summary>
        ///     add a parameter definition
        /// </summary>
        /// <param name="completeName"></param>
        /// <returns></returns>
        public Variable AddParameter(string completeName) {
            if (Parameters == null)
                Parameters = new List<IVariable>();

            var result = new Variable {
                Name = completeName
            };

            Parameters.Add(result);
            return result;
        }

        /// <summary>
        ///     get a parameter by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IVariable this[int index]
            => Parameters[index];

        /// <summary>
        ///     check if this parameter group matches a signature
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="typeRegistry">type registry</param>
        /// <returns></returns>
        public bool Matches(ITypeRegistry typeRegistry, Signature signature) {
            var paramCount = Parameters == null ? 0 : Parameters.Count;

            if (paramCount != signature.Length)
                return false;

            var match = true;

            for (var i = 0; Parameters != null && i < Parameters.Count; i++) {
                var parameter = Parameters[i];
                var sourceType = typeRegistry.GetTypeByIdOrUndefinedType(signature[i].TypeId);
                match = match && typeRegistry.GetTypeByIdOrUndefinedType(parameter.SymbolType.TypeId).CanBeAssignedFrom(sourceType);

                if (!match)
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     create the signature for this parameters
        /// </summary>
        /// <returns></returns>
        public Signature CreateSignature(ITypeRegistry runtime) {
            if (Parameters == default || Parameters.Count < 1)
                return new Signature(ResultType, ImmutableArray<ITypeReference>.Empty);

            using (var list = runtime.ListPools.GetList<ITypeReference>()) {
                var values = new ITypeReference[Parameters.Count];
                for (var i = 0; i < Parameters.Count; i++)
                    values[i] = Parameters[i].SymbolType ?? runtime.Runtime.Types.MakeErrorTypeReference();

                return new Signature(ResultType, runtime.ListPools.GetFixedArray(list));
            }
        }
    }
}