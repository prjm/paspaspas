#nullable disable
using System.Collections;
using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Other {

    /// <summary>
    ///     base class for signatures
    /// </summary>
    internal abstract class SignatureBase : ISignature {

        public abstract ITypeSymbol this[int index] { get; }

        protected SignatureBase(ITypeSymbol returnType)
            => ReturnType = returnType;

        public ITypeSymbol ReturnType { get; private set; }

        public abstract int Count { get; }

        public bool HasConstantParameters {
            get {
                for (var i = 0; i < Count; i++)
                    if (!this[i].IsConstant())
                        return false;
                return true;
            }
        }

        public bool Equals(ISignature other) {
            if (other.Count != Count)
                return false;

            for (var i = 0; i < Count; i++)
                if (!this[i].Equals(other[i]))
                    return false;

            return ReturnType.Equals(other.ReturnType);
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ISignature signature)
                return Equals(signature);
            return false;
        }

        public override int GetHashCode() {
            var result = ReturnType.GetHashCode();

            for (var i = 0; i < Count; i++)
                result = unchecked(result * 31 + this[i].GetHashCode());

            return result;
        }

        public abstract IEnumerator<ITypeSymbol> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}