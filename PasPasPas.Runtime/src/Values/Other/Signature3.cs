#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     signature with three arguments
    /// </summary>
    internal class Signature3 : SignatureBase {

        /// <summary>
        ///     first argument
        /// </summary>
        private readonly ITypeSymbol arg1;

        /// <summary>
        ///     second argument
        /// </summary>
        private readonly ITypeSymbol arg2;

        /// <summary>
        ///     third argument
        /// </summary>
        private readonly ITypeSymbol arg3;


        /// <summary>
        ///     create a new signature
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="argument1"></param>
        /// <param name="argument2"></param>
        /// <param name="argument3"></param>
        internal Signature3(ITypeSymbol returnType, ITypeSymbol argument1, ITypeSymbol argument2, ITypeSymbol argument3) : base(returnType) {
            arg1 = argument1;
            arg2 = argument2;
            arg3 = argument3;
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
                    case 2:
                        return arg3;
                    default:
                        throw new System.ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        /// <summary>
        ///     two arguments
        /// </summary>
        public override int Count
            => 3;

        /// <summary>
        ///     get an enumerator
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<ITypeSymbol> GetEnumerator() {
            yield return arg1;
            yield return arg2;
            yield return arg3;
        }
    }
}
