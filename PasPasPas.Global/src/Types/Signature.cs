using System;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     a type signature
    /// </summary>
    public class Signature : IEquatable<Signature> {

        private ITypeReference[] data;

        /// <summary>
        ///     get the number of parameters in this signature
        /// </summary>
        public int Length
            => data.Length;

        /// <summary>
        ///     test if all input parameters are constant values
        /// </summary>
        public bool IsConstant {
            get {
                for (var i = 0; i < data.Length; i++)
                    if (data[i] == null || (!data[i].IsConstant()))
                        return false;
                return true;
            }
        }

        /// <summary>
        ///     create a new unary signature
        /// </summary>
        /// <param name="value"></param>
        public Signature(ITypeReference value)
            => data = new ITypeReference[] { value };

        /// <summary>
        ///     create a new signature
        /// </summary>
        /// <param name="value1">first type</param>
        /// <param name="value2">second type</param>
        public Signature(ITypeReference value1, ITypeReference value2)
            => data = new ITypeReference[] { value1, value2 };

        /// <summary>
        ///     create a new type signature
        /// </summary>
        /// <param name="values">values</param>
        public Signature(params ITypeReference[] values) {
            data = new ITypeReference[values.Length];
            Array.Copy(values, data, values.Length);
        }

        /// <summary>
        ///     test two signatures for equality
        /// </summary>
        /// <param name="other">signature to compare</param>
        /// <returns></returns>
        public bool Equals(Signature other) {
            if (other.data.Length != data.Length)
                return false;

            for (var i = 0; i < other.data.Length; i++) {
                if (data[i] != other.data[i])
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
                for (var i = 0; i < data.Length; i++)
                    result = result * 31 + data[i].GetHashCode();
                return result;
            }
        }

        /// <summary>
        ///     a value by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ITypeReference this[int index]
            => data[index];

    }
}
