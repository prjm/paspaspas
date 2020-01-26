using System;
using System.Collections.Immutable;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     a type signature
    /// </summary>
    public class Signature : IEquatable<Signature> {

        private readonly ImmutableArray<ITypeSymbol> InputTypes;

        /// <summary>
        ///     get the number of parameters in this signature
        /// </summary>
        public int Length
            => InputTypes.Length;

        /// <summary>
        ///     test if all input parameters are constant values
        /// </summary>
        public bool IsConstant {
            get {
                return false;
                /*
                for (var i = 0; i < InputTypes.Length; i++)
                    if (InputTypes[i] == null || !InputTypes[i].IsConstant())
                        return false;
                return true;
                */
            }
        }

        /// <summary>
        ///     return type
        /// </summary>
        public ITypeSymbol ReturnType { get; }

        /// <summary>
        ///     create a new type signature
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="inputTypes">values</param>
        public Signature(ITypeSymbol returnType, ImmutableArray<ITypeSymbol> inputTypes) {
            ReturnType = returnType;
            InputTypes = inputTypes;
        }

        /// <summary>
        ///     test two signatures for equality
        /// </summary>
        /// <param name="other">signature to compare</param>
        /// <returns></returns>
        public bool Equals(Signature other) {
            if (other.InputTypes.Length != InputTypes.Length)
                return false;

            for (var i = 0; i < other.InputTypes.Length; i++) {
                if (InputTypes[i] != other.InputTypes[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     compare to another signature
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is Signature signature)
                return Equals(signature);

            return false;
        }

        /// <summary>
        ///     get the hash code for this signature
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            unchecked {
                for (var i = 0; i < InputTypes.Length; i++)
                    result = result * 31 + InputTypes[i].GetHashCode();
                return result;
            }
        }

        /// <summary>
        ///     a value by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ITypeSymbol this[int index]
            => InputTypes[index];

    }
}
