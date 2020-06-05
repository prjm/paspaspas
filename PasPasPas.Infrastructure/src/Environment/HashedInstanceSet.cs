#nullable disable
using System;

namespace PasPasPas.Infrastructure.Environment {

    internal static class Helper {

        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

        public static int GetPrime(int min) {
            for (var i = 0; i < primes.Length; i++)
                if (primes[i] >= min) return primes[i];

            for (var i = min | 1; i < int.MaxValue; i += 2)
                if (IsPrime(i) && (i - 1) % 101 != 0)
                    return i;

            return min;
        }

        public static bool IsPrime(int candidate) {
            if ((candidate & 1) != 0) {
                var limit = (int)Math.Sqrt(candidate);
                for (var divisor = 3; divisor <= limit; divisor += 2)
                    if (candidate % divisor == 0)
                        return false;

                return true;
            }
            return candidate == 2;
        }
    }

    /// <summary>
    ///     base class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HashedInstanceSet<T> {

        /// <summary>
        ///     helper delegate to check equality
        /// </summary>
        /// <typeparam name="TToCompare"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public delegate bool CheckEquality<TToCompare>(in T left, in TToCompare right);

        /// <summary>
        ///     check equality to span
        /// </summary>
        /// <typeparam name="TToCompare"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public delegate bool CheckSpanEquality<TToCompare>(in T left, in ReadOnlySpan<TToCompare> right);

        private struct Entry {
            internal int hashCode;
            internal int next;
            internal T data;
        }

        private int[] buckets;
        private Entry[] slots;
        private int lastIndex;
        private readonly CheckEquality<T> checkEquality;
        private const int Lower31BitMask = 0x7FFFFFFF;

        /// <summary>
        ///     create a new hashed instance set
        /// </summary>
        protected HashedInstanceSet() {
            var initSize = 97;
            buckets = new int[initSize];
            slots = new Entry[initSize];
            lastIndex = 0;
            Count = 0;
            checkEquality = new CheckEquality<T>(Equals);
        }

        /// <summary>
        ///     item count
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     add an item manually
        /// </summary>
        /// <param name="item"></param>
        public bool Add(T item) {
            var hashCode = GetHashCode(item) & Lower31BitMask;
            var bucket = (hashCode % buckets.Length) & Lower31BitMask;

            for (var i = buckets[hashCode % buckets.Length] - 1; i >= 0; i = slots[i].next)
                if (slots[i].hashCode == hashCode && Equals(slots[i].data, item))
                    return false;

            if (lastIndex == slots.Length) {
                Enlarge();
                bucket = hashCode % buckets.Length;
            }

            var index = lastIndex;
            lastIndex++;
            slots[index].hashCode = hashCode;
            slots[index].data = item;
            slots[index].next = buckets[bucket] - 1;
            buckets[bucket] = index + 1;
            Count++;
            return true;
        }

        private void Enlarge() {
            var newSize = Helper.GetPrime(2 * buckets.Length);
            var newSlots = new Entry[newSize];
            Array.Copy(slots, 0, newSlots, 0, lastIndex);

            var newBuckets = new int[newSize];
            for (var i = 0; i < lastIndex; i++) {
                var bucket = newSlots[i].hashCode % newSize;
                newSlots[i].next = newBuckets[bucket] - 1;
                newBuckets[bucket] = i + 1;
            }
            slots = newSlots;
            buckets = newBuckets;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected abstract int GetHashCode(in T item);

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        protected abstract bool Equals(in T left, in T right);

        /// <summary>
        ///     check if an item is contained in this  set
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(in T item) {
            var hashCode = GetHashCode(item);
            return TryGetValue(hashCode, item, out _, checkEquality);
        }

        /// <summary>
        ///     try to get a value
        /// </summary>
        /// <typeparam name="TLookup"></typeparam>
        /// <param name="hashCode"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="equality"></param>
        /// <returns></returns>
        protected bool TryGetValue<TLookup>(int hashCode, in TLookup input, out T output, CheckEquality<TLookup> equality) {
            hashCode &= Lower31BitMask;
            for (var i = buckets[hashCode % buckets.Length] - 1; i >= 0; i = slots[i].next) {
                if (slots[i].hashCode == hashCode && equality(slots[i].data, input)) {
                    output = slots[i].data;
                    return true;
                }
            }

            output = default;
            return false;
        }

        /// <summary>
        ///     try to get a span value
        /// </summary>
        /// <typeparam name="TLookupSpan"></typeparam>
        /// <param name="hashCode"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="equality"></param>
        /// <returns></returns>
        protected bool TryToGetSpanValue<TLookupSpan>(int hashCode, in ReadOnlySpan<TLookupSpan> input, out T output, CheckSpanEquality<TLookupSpan> equality) {
            hashCode &= Lower31BitMask;
            for (var i = buckets[hashCode % buckets.Length] - 1; i >= 0; i = slots[i].next) {
                if (slots[i].hashCode == hashCode && equality(slots[i].data, input)) {
                    output = slots[i].data;
                    return true;
                }
            }

            output = default;
            return false;
        }

        /// <summary>
        ///     clear all items
        /// </summary>
        public void Clear() {
            Array.Clear(slots, 0, lastIndex);
            Array.Clear(buckets, 0, buckets.Length);
            lastIndex = 0;
            Count = 0;
        }
    }

}
