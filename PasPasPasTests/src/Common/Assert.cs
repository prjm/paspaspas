#nullable disable
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SharpFloat.FloatingPoint;
using A = Xunit.Assert;

namespace PasPasPasTests.Common {

    /// <summary>
    ///     common assertion library / alias
    /// </summary>
    public static class Assert {

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreEqual(object expected, object actual)
            => A.Equal(expected, actual);

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreEqual(string expected, string actual, StringComparer comp = default) {
            if (comp == default)
                comp = StringComparer.OrdinalIgnoreCase;

            A.Equal(expected, actual, comp);
            return true;
        }

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreEqual(in ExtF80 expected, in ExtF80 actual)
            => A.Equal(expected, actual);

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreEqual(double expected, double actual)
            => A.Equal(expected, actual, 10);

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="o"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsTrue(bool o)
            => A.True(o);

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="o"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsFalse(bool o)
            => A.False(o);

        /// <summary>
        ///     check inequality
        /// </summary>
        /// <param name="notExpected"></param>
        /// <param name="actual"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreNotEqual(object notExpected, object actual)
            => A.NotEqual(notExpected, actual);

        /// <summary>
        ///     check that an object is not null
        /// </summary>
        /// <param name="o"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNull(object o)
            => A.NotNull(o);

        /// <summary>
        ///     check that an object is null
        /// </summary>
        /// <param name="o"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNull(object o)
            => A.Null(o);

        /// <summary>
        ///     check that an exception is thrown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testCode"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Throws<T>(Action testCode) where T : Exception
            => A.Throws<T>(testCode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AreEqualSequences<T>(IEnumerable<T> expected, IEnumerable<T> actual)
            => A.Equal(expected, actual);

    }
}