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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreEqual(object expected, object actual)
            => A.Equal(expected, actual);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreEqual(string expected, string actual, StringComparer comp = default) {
            if (comp == default)
                comp = StringComparer.OrdinalIgnoreCase;

            A.Equal(expected, actual, comp);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreEqual(in ExtF80 expected, in ExtF80 actual)
            => A.Equal(expected, actual);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreEqual(double expected, double actual)
            => A.Equal(expected, actual, 10);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsTrue(bool o)
            => A.True(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsFalse(bool o)
            => A.False(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AreNotEqual(object notExpected, object actual)
            => A.NotEqual(notExpected, actual);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotNull(object o)
            => A.NotNull(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNull(object o)
            => A.Null(o);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Throws<T>(Action testCode) where T : Exception
            => A.Throws<T>(testCode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AreEqualSequences<T>(IEnumerable<T> expected, IEnumerable<T> actual)
            => A.Equal(expected, actual);

    }
}