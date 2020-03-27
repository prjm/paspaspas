using System.Collections.Generic;
using System.Linq;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     signature without arguments
    /// </summary>
    internal class Signature0 : SignatureBase, ISignature {

        /// <summary>
        ///     create a new signature
        /// </summary>
        /// <param name="returnType"></param>
        public Signature0(ITypeSymbol returnType) : base(returnType) { }

        public override ITypeSymbol this[int index]
            => throw new System.ArgumentOutOfRangeException(nameof(index));

        public override int Count
            => 0;

        public override IEnumerator<ITypeSymbol> GetEnumerator()
            => Enumerable.Empty<ITypeSymbol>().GetEnumerator();
    }
}