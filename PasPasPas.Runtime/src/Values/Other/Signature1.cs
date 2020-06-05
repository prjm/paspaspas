#nullable disable
using System;
using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     unary signature
    /// </summary>
    internal class Signature1 : SignatureBase, ISignature {

        private readonly ITypeSymbol parameter;

        public Signature1(ITypeSymbol returnType, ITypeSymbol parameter) : base(returnType) {
            this.parameter = parameter;
        }

        public override ITypeSymbol this[int index]
            => index == 0 ? parameter : throw new ArgumentOutOfRangeException(nameof(index));

        public override int Count
            => 1;

        public override IEnumerator<ITypeSymbol> GetEnumerator()
            => new SingleEnumerator<ITypeSymbol>(parameter);
    }
}