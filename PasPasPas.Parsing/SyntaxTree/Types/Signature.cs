using System;
using PasPasPas.Global.Runtime;

namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     a type signature
    /// </summary>
    public class Signature : IEquatable<Signature> {

        private IValue[] data;

        /// <summary>
        ///     get the number of parameters in this signature
        /// </summary>
        public int Length
            => data.Length;

        /// <summary>
        ///     create a new unary signature
        /// </summary>
        /// <param name="value"></param>
        public Signature(IValue value)
            => data = new IValue[] { value };

        /// <summary>
        ///     create a new signature
        /// </summary>
        /// <param name="value1">first type</param>
        /// <param name="value2">second type</param>
        public Signature(IValue value1, IValue value2)
            => data = new IValue[] { value1, value2 };

        /// <summary>
        ///     create a new type signature
        /// </summary>
        /// <param name="values">values</param>
        public Signature(params IValue[] values) {
            data = new IValue[values.Length];
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
        ///     get the hash code for this signatur
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;
            for (var i = 0; i < data.Length; i++)
                result = result * 31 + data[i].GetHashCode();
            return result;
        }

        /// <summary>
        ///     a value by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public IValue this[int index]
            => data[index];

    }
}
