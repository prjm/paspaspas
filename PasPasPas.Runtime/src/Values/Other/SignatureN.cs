using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     variadic signature
    /// </summary>
    internal class SignatureN : SignatureBase {
        private readonly ImmutableArray<ITypeSymbol> parameters;

        public SignatureN(ITypeSymbol returnType, ISignature signature) : base(returnType) {
            var parameterBuilder = ImmutableArray.CreateBuilder<ITypeSymbol>((int)signature.Count);
            parameterBuilder.AddRange(signature);
            parameters = parameterBuilder.ToImmutableArray();
        }

        public override ITypeSymbol this[int index]
            => parameters[index];

        public override int Count
            => parameters.Length;

        /// <summary>
        ///     enumerate all items
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<ITypeSymbol> GetEnumerator()
            => (parameters as IEnumerable<ITypeSymbol>).GetEnumerator();
    }
}
