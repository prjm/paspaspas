using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a type signature
    /// </summary>
    public struct Signature : IEquatable<Signature> {

        private int[] data;

        /// <summary>
        ///     create a new type signature
        /// </summary>
        /// <param name="type"></param>
        public Signature(int type)
            => data = new int[1] { type };


        /// <summary>
        ///     create a new signature
        /// </summary>
        /// <param name="type1">first type</param>
        /// <param name="type2">second type</param>
        public Signature(int type1, int type2)
            => data = new int[2] { type1, type2 };

        /// <summary>
        ///     check if this signature consinsts of a single type
        ///     and compare its type id
        /// </summary>
        /// <param name="typeId">type id to compare</param>
        /// <returns><c>true</c> if this signature only uses this type</returns>
        public bool EqualsType(int typeId)
            => data.Length == 1 && data[0] == typeId;


        /// <summary>
        ///     create a new type signature
        /// </summary>
        /// <param name="typeIds"></param>
        public Signature(params int[] typeIds) {
            data = new int[typeIds.Length];
            Array.Copy(typeIds, data, typeIds.Length);
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
    }
}
