#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.src.Utils;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     signature with two arguments
    /// </summary>
    internal class Signature2 : SignatureBase {

        /// <summary>
        ///     first argument
        /// </summary>
        private readonly ITypeSymbol arg1;

        /// <summary>
        ///     second argument
        /// </summary>
        private readonly ITypeSymbol arg2;


        /// <summary>
        ///     create a new signature
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        public Signature2(ITypeSymbol returnType, ITypeSymbol argument1, ITypeSymbol argument2) : base(returnType) {
            arg1 = argument1;
            arg2 = argument2;
        }

        /// <summary>
        ///     access argument by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override ITypeSymbol this[int index] {
            get {
                switch (index) {
                    case 0:
                        return arg1;
                    case 1:
                        return arg2;
                    default:
                        throw new System.ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        /// <summary>
        ///     two arguments
        /// </summary>
        public override int Count
            => 2;

        /// <summary>
        ///     get an enumerator
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<ITypeSymbol> GetEnumerator()
            => new PairEnumerator<ITypeSymbol>(arg1, arg2);
    }
}
