using System;
using System.Text;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Infrastructure.ObjectPooling {

    /// <summary>
    ///     hashed strings
    /// </summary>
    public class HashedStrings :
        HashedInstanceSet<string>,
        ILookupByHash<StringBuilder, string> {

        private const int FnvOffsetBias = unchecked((int)2166136261);
        private const int FnvPrime = 16777619;

        /// <summary>
        ///     maximal string length
        /// </summary>
        public const int MaxStringLength = 300;

        private readonly CheckEquality<StringBuilder> checkEquality
            = new CheckEquality<StringBuilder>(EqualsStringBuilder);
        private readonly CheckSpanEquality<byte> checkSpan
            = new CheckSpanEquality<byte>(EqualsByteSpan);

        private static string Decode(in Span<byte> data)
#if DESKTOP
            => Encoding.Unicode.Decode(data);
#else
            => Encoding.Unicode.GetString(data);
#endif

        private static void Decode(in Span<byte> input, in Span<char> output)
#if DESKTOP
            => Encoding.Unicode.GetCharsBySpan(input, output);
#else
            => Encoding.Unicode.GetChars(input, output);
#endif

        /// <summary>
        ///     add an item
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Add(StringBuilder input) {
            var result = input.ToString();

            if (result.Length > MaxStringLength)
                return result;

            Add(result);
            return result;
        }

        /// <summary>
        ///     add an string manually
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Add(in Span<byte> input) {
            var result = Decode(input);

            if (result.Length > MaxStringLength)
                return result;

            Add(result);
            return result;
        }


        /// <summary>
        ///     add a string by a string builder
        /// </summary>
        /// <param name="input"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool TryGetValue(StringBuilder input, out string target) {
            if (input.Length > MaxStringLength) {
                target = default;
                return false;
            }

            var hashCode = GetHashCode(input);
            return TryGetValue(hashCode, input, out target, checkEquality);
        }

        /// <summary>
        ///     try to get a cached span
        /// </summary>
        /// <param name="input"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool TryGetValue(in Span<byte> input, out string target) {
            if (2 * input.Length > MaxStringLength) {
                target = default;
                return false;
            }

            var hashCode = GetHashCode(input);
            return TryToGetSpanValue(hashCode, input, out target, checkSpan);
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        protected override bool Equals(in string left, in string right)
            => StringComparer.Ordinal.Equals(left, right);


        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        protected static bool EqualsStringBuilder(in string l, in StringBuilder r) {
            if (l.Length != r.Length)
                return false;

            for (var i = 0; i < l.Length; i++)
                if (l[i] != r[i])
                    return false;

            return true;
        }

        /// <summary>
        ///     check if a byte span equals a string
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        protected static bool EqualsByteSpan(in string l, in Span<byte> r) {
            if (l.Length != r.Length * 2)
                return false;

            Span<char> b = stackalloc char[1];
            for (var i = 0; i < r.Length; i += 2) {
                Decode(r.Slice(i, 2), b);
                if (b[0] != l[i / 2])
                    return false;
            }

            return true;
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override int GetHashCode(in string item) {
            var hashCode = FnvOffsetBias;
            var start = 0;
            var end = item.Length;

            for (var i = start; i < end; i++)
                hashCode = unchecked((hashCode ^ item[i]) * FnvPrime);

            return hashCode;
        }

        private static int GetHashCode(StringBuilder text) {
            var hashCode = FnvOffsetBias;
            var start = 0;
            var end = text.Length;

            for (var i = start; i < end; i++) {
                hashCode = unchecked((hashCode ^ text[i]) * FnvPrime);
            }

            return hashCode;
        }

        private static int GetHashCode(in Span<byte> text) {
            var hashCode = FnvOffsetBias;
            var start = 0;
            var end = text.Length;
            Span<char> b = stackalloc char[1];

            for (var i = start; i < end; i += 2) {
                Decode(text.Slice(i, 2), b);
                hashCode = unchecked((hashCode ^ b[0]) * FnvPrime);
            }

            return hashCode;
        }

    }
}
